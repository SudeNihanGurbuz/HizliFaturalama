using HizliFaturalama.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Get
{
    public class GetAllInvoices
    {
        public record Query(String UserId) : IRequest<List<Invoice>>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Query, List<Invoice>>
        {
            public async Task<List<Invoice>> Handle(Query request, CancellationToken cancellationToken)
            {

                var ctx = await context.CreateDbContextAsync(cancellationToken);
                if (ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }

                var Invoices = ctx.Invoices.Where(x=>x.UserId == request.UserId);
                if(Invoices ==null)
                {
                    throw new ArgumentNullException(nameof(Invoices));
                }

                return await Invoices.ToListAsync(cancellationToken);
            }
        }
    }
}
