using System.ComponentModel.DataAnnotations;

namespace HizliFaturalama.ViewModels
{
    public class CreateCustomerVm
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string TaxNumber { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

    }
}
