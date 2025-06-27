using HizliFaturalama.Models;
using HizliFaturalama.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class UpdateCustomer
    {

        public record Command(UpdateCustomerVm model) : IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);

                var customer =await ctx.Customers.FirstOrDefaultAsync(i=>i.CustomerId == request.model.CustomerId);

                var updatedCustomer = request.model;

                if (customer != null) {
                    customer.UserName = updatedCustomer.UserName;
                    customer.Email = updatedCustomer.Email;
                    customer.Address = updatedCustomer.Address;
                    customer.PhoneNumber = updatedCustomer.PhoneNumber;
                    customer.TaxNumber = updatedCustomer.TaxNumber;
                    customer.Title = updatedCustomer.Title;

                    int result = await ctx.SaveChangesAsync();
                    return result;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
