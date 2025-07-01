using HizliFaturalama.Features.Manage;
using HizliFaturalama.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MediatR;
using HizliFaturalama.Features.Get;
using HizliFaturalama.Models;
namespace HizliFaturalama.Areas.Muhasebe.Controllers
{
    [Authorize]
    [Area("Muhasebe")]
    public class MuhasebeController(IMediator MediatR) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> CreateInvoice()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetCustomers.Query(userId);
            var customers = MediatR.Send(query);

            if(customers != null)
            {
                return View(await customers);
            }
            else return View();
        }
        [HttpPost]
        [Route("Muhasebe/CreateInvoicePost")]
        public async Task<IActionResult> CreateInvoicePost(string InvoiceNumber,string CustomerId, string CustomerName, decimal Total, List<InvoiceLine> Items)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            InvoiceCreateVm vm = new InvoiceCreateVm();
            vm.InvoiceNumber = InvoiceNumber;   
            vm.CustomerName = CustomerName;
            vm.Total = Total;
            vm.Items = Items;
            vm.UserId = userId;
            vm.CustomerId = CustomerId;

            var command = new CreateInvoice.Command(vm);
            var response = MediatR.Send(command);

            if(response != null)
            {
                return RedirectToAction("GetAllInvoices");
            }
            else return RedirectToAction("CreateInvoice");
        }

        [HttpGet]
        public async Task<IActionResult> ManageCustomer()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var query = new GetCustomers.Query(userId);
            var customers = MediatR.Send(query);

            if (customers != null)
            {
                return View(await customers);
            }
            return View();
        }

        [HttpPost]
        [Route("Muhasebe/CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerVm model)
        {
            if (ModelState.IsValid) {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var command = new CreateCustomer.Command(model, userId);

                var response = await MediatR.Send(command);
                if(response == 1)
                {
                    Response.Headers.Add("HX-Redirect", "/Muhasebe/Muhasebe/ManageCustomer");
                    return Ok();
                }
                else if (response == 2)
                {
                    return BadRequest("<div class='bg-red-100 text-red-700 p-3 rounded-md'>Bu e-mail adresi ile kayıtlı müşteri bulunmaktadır. Yeni bir e-mail adresi giriniz.</div>");
                }
                else
                {
                    return View();
                }
            }
            return Ok();
        }

        [HttpPost]
        [Route("Muhasebe/UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomerAsync(UpdateCustomerVm model)
        {
            if (ModelState.IsValid) {
            
                var command = new UpdateCustomer.Command(model);
                var response = await MediatR.Send(command);
                if (response > 0) {
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Muhasebe/DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(String CustomerId)
        {
            if(CustomerId != null)
            {
                var command = new DeleteCustomer.Command(CustomerId);
                var result = await MediatR.Send(command);
                if (result > 0)
                {
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("Muhasebe/DeleteInvoice")]
        public async Task<IActionResult> DeleteInvoice(String InvoiceId)
        {
            if(InvoiceId != null)
            {
                var command = new DeleteInvoice.Command(InvoiceId);
                var result = await MediatR.Send(command);
                if (result > 0)
                {
                    Response.Headers.Add("HX-Refresh", "true");
                    return Ok();
                }
            }
            return BadRequest();
        }


        [HttpGet("/Muhasebe/GetInvoice/{id}")]
        public async Task<IActionResult> GetInvoice(string id)
        {
            var query = new GetInvoiceById.Query(id);    
            var response = await MediatR.Send(query);
            if(response is not null)
            {
                return View(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllInvoices()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetAllInvoices.Query(userId);
            var response = MediatR.Send(query);
            if (response != null)
            { 
                return View(await response);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
