using HizliFaturalama.Models;
using HizliFaturalama.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class CreateCustomer
    {

        public record Command(CreateCustomerVm model, String UserId) : IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);

                if (ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }

                var customers = ctx.Customers.Where(x => x.UserId == request.UserId).ToList();
                if( customers.Any(x=>x.Email == request.model.Email.ToLower()))
                {
                    return 2; /*Kullanıcı e-maili zaten kayıtlı olduğu için*/
                }

                var customer = request.model;
                var newCustomer = new Customer()
                {
                    UserId = request.UserId,
                    UserName = customer.UserName,
                    Email = customer.Email,
                    TaxNumber = customer.TaxNumber,
                    PhoneNumber = customer.PhoneNumber,
                    RecordDate = DateTime.Now,
                    Title = customer.Title,
                    CustomerId = Guid.NewGuid().ToString(),
                    Address = customer.Address,
                    IsActive = true
                };
                ctx.Customers.Add(newCustomer);
                await ctx.SaveChangesAsync(cancellationToken);

                return 1;
            }
        }
    }
}
