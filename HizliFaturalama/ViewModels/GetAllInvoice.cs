using HizliFaturalama.Models;

namespace HizliFaturalama.ViewModels
{
    public class GetAllInvoice
    {
        public string CustomerName { get; set; }
        public List<Invoice> Invoices { get; set; }
        
    }
}
