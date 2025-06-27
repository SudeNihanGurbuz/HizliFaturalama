namespace HizliFaturalama.ViewModels
{
    public class InvoiceItemViewModel
    {
        public string Name { get; set; }
        public string Unit { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
