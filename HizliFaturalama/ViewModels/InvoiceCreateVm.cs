using HizliFaturalama.Models;

namespace HizliFaturalama.ViewModels
{
    public class InvoiceCreateVm

    {
        public string InvoiceNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public List<InvoiceLine> Items { get; set; }
    }
}
