using HizliFaturalama.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Get
{
    public class GetCustomers
    {
        public record Query(String UserId) : IRequest<List<Customer>>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Query, List<Customer>>
        {
            public async Task<List<Customer>> Handle(Query request, CancellationToken cancellationToken)
            {

                var ctx = await context.CreateDbContextAsync(cancellationToken);

                if (ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }
                var customers = ctx.Customers
                   .Where(x => x.UserId == request.UserId && x.IsActive == true);
                if (customers == null)
                {
                    throw new ArgumentNullException(nameof(customers));
                }

                return await customers.ToListAsync(cancellationToken);
            }
        }
    }
}
