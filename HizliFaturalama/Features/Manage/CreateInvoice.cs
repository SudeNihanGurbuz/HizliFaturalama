using HizliFaturalama.Models;
using HizliFaturalama.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Manage
{
    public class CreateInvoice
    {
        public record Command (InvoiceCreateVm model): IRequest<int>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Command, int>
        {
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var ctx = await context.CreateDbContextAsync(cancellationToken);
                if (ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }

                var req = request.model;

                var Invoice = new Invoice()
                {
                    InvoiceId = Guid.NewGuid().ToString(),
                    CustomerId = req.CustomerId,
                    InvoiceDate = DateTime.Now,
                    TotalAmount = req.Total,
                    RecordDate = DateTime.Now,
                    InvoiceNumber = req.InvoiceNumber,
                    UserId = req.UserId
                };

                var items = new List<InvoiceLine>();
                foreach (var item in req.Items)
                {
                    InvoiceLine line = new InvoiceLine()
                    {
                        InvoiceLineId = Guid.NewGuid().ToString(),
                        InvoiceId = Invoice.InvoiceId,
                        ItemId = Guid.NewGuid().ToString(),
                        ItemName = item.ItemName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        RecordDate = DateTime.Now,
                        Unit = item.Unit,
                        UserId = req.UserId
                    };
                    items.Add(line);
                }
                ctx.Invoices.Add(Invoice);
                ctx.InvoiceLines.AddRange(items);
                return ctx.SaveChanges();
            }
        }
    }
}
