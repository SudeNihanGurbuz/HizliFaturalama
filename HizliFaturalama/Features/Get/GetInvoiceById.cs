using Azure.Core;
using HizliFaturalama.Models;
using HizliFaturalama.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HizliFaturalama.Features.Get
{
    public class GetInvoiceById
    {
        public record Query (String InvoiceId) :IRequest<ShowInvoice>;
        public class Handler(IDbContextFactory<HizliFaturaDbContext> context) : IRequestHandler<Query, ShowInvoice>
        {

            public async Task<ShowInvoice> Handle(Query request, CancellationToken cancellationToken)
            {

                var ctx = await context.CreateDbContextAsync(cancellationToken);

                if(ctx == null)
                {
                    throw new ArgumentNullException(nameof(ctx));
                }

                var invoice = ctx.Invoices.FirstOrDefault(x => x.InvoiceId == request.InvoiceId);

                var invoicelines = ctx.InvoiceLines.Where(x=>x.InvoiceId == request.InvoiceId).ToList();

                var customer = ctx.Customers.FirstOrDefault(x => x.CustomerId == invoice.CustomerId);
                var showInvoice = new ShowInvoice() {
                    CustomerName = customer.UserName,
                    InvoiceNo = invoice.InvoiceNumber,
                    RecordDate = invoice.RecordDate,
                    TotalAmount = invoice.TotalAmount,
                    invoiceLines = invoicelines
                };
                return showInvoice;
            }
        }
    }
}
