using HizliFaturalama.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class DeleteInvoice
    {

        public record Command(String InvoiceId) : IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);
                if (ctx != null)
                {
                    var Invoice = ctx.Invoices.FirstOrDefault(x => x.InvoiceId == request.InvoiceId);
                    var InvoiceLines = ctx.InvoiceLines.Where(x => x.InvoiceId == request.InvoiceId);

                    ctx.Invoices.Remove(Invoice);
                    ctx.InvoiceLines.RemoveRange(InvoiceLines);
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
