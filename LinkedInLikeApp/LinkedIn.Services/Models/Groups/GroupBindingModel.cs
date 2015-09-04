namespace LinkedIn.Services.Models.Groups
{
    using System.ComponentModel.DataAnnotations;

    public class GroupBindingModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

    }
}