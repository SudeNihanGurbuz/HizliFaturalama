using System.ComponentModel.DataAnnotations;

namespace HizliFaturalama.ViewModels
{
    public class UpdateCustomerVm
    {
        public string CustomerId { get; set; }
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Title { get; set; }        
        
        [Required]
        public string TaxNumber { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
