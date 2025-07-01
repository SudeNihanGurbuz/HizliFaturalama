//using HızlıFatura.Features.Manage;
//using HızlıFatura.Models;
using HizliFaturalama.Models.Model;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;

namespace HizliFaturalama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterModel model)
        {

            if (!ModelState.IsValid) {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return UnprocessableEntity($"<div class='alert alert-danger'>{string.Join("<br>", errors)}</div>");
            }
            
            var existingUserByUsername = await _userManager.FindByNameAsync(model.Username);
            if (existingUserByUsername != null)
            {
                return UnprocessableEntity("<div class='alert alert-danger'>Bu kullanıcı adı zaten alınmış.</div>");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
            if (existingUserByEmail != null)
            {
                return UnprocessableEntity("<div class='alert alert-danger'>Bu e-posta adresi zaten kayıtlı.</div>");
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var login = new LoginModel()
                {
                    Password = model.Password,
                    Username = model.Username,
                };
                return await Login(login);

            }
            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest($"<div class='alert alert-danger'>{string.Join("<br>", identityErrors)}</div>");
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.
                    SelectMany(v=>v.Errors).
                    Select(e => e.ErrorMessage).ToList();
                return BadRequest($"<div class='alert alert-danger'>{string.Join("<br>", errors)}</div>");
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var claims = new[]
                {
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                Response.Cookies.Append("jwt_token", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(30)
                });
               

                // HTMX destekli redirect yanıtı
                Response.Headers.Add("HX-Redirect", "/Muhasebe/Muhasebe/CreateInvoice");
                return Ok("<div class='alert alert-success'>Giriş başarılı! Yönlendiriliyorsunuz...</div>");

            }

            return Unauthorized("<div class='alert alert-danger'>Yanlış kullanıcı adı veya şifre</div>");
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {

            Response.Cookies.Delete("jwt_token");
            return Redirect("/index.html"); // index sayfası wwwroot'un altında olduğu için yönlendirmeyi bu şekilde yaparız

        }

    }
}