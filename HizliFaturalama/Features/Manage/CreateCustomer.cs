using HizliFaturalama.Models;
using HizliFaturalama.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class CreateCustomer
    {

        public record Command(CreateCustomerVm model) : IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);

                if (ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }

                var customer = request.model;
                if(ctx.Customers.Any(x=>x.Email == customer.Email.ToLower()))
                {
                    throw new InvalidOperationException("Bu e-mail adresi ile bir müşteri zaten kayıtlı.");
                }

                var newCustomer = new Customer()
                {
                    UserId = Guid.Parse("7122b34b-26d3-4363-9ffd-a4fc998c1df3").ToString(),
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
