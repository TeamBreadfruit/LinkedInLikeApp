namespace LinkedIn.Services.Models.Messages
{
    using System.ComponentModel.DataAnnotations;

    public class AddMessageBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [MinLength(3)]
        public string Content { get; set; }

        public string ToUserId { get; set; }

        public string GroupId { get; set; }
    }
}