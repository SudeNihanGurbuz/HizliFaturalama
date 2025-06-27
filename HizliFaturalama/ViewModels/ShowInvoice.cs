using HizliFaturalama.Models;

namespace HizliFaturalama.ViewModels
{
    public class ShowInvoice
    {
        public string InvoiceNo {  get; set; }
        public string CustomerName { get; set; }
        public DateTime RecordDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public List<InvoiceLine> invoiceLines { get; set; }
    }
}
