namespace LinkedIn.Services.Models.Companies
{
    using System.ComponentModel.DataAnnotations;

    public class CompanyBindingModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email")]
        public string Email { get; set; }

    }
}