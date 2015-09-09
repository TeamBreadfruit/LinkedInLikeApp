namespace LinkedIn.Services.Models.Skills
{
    using System.ComponentModel.DataAnnotations;

    public class SkillBindingModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}