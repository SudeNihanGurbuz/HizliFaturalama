using HizliFaturalama.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class DeleteCustomer
    {

        public record Command (String CustomerId)  :IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);
                if (ctx != null)
                {
                    var customer = ctx.Customers.FirstOrDefault(x => x.CustomerId == request.CustomerId);

                    customer.IsActive = false;
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
