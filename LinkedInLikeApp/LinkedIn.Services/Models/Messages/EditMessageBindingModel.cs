namespace LinkedIn.Services.Models.Messages
{
    using System.ComponentModel.DataAnnotations;

    public class EditMessageBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Content { get; set; }
    }
}