using System.ComponentModel.DataAnnotations;

namespace LinkedIn.Services.Models
{
    public class SkillBindingModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}