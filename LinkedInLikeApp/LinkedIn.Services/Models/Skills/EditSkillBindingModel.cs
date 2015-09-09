namespace LinkedIn.Services.Models.Skills
{
    using System.ComponentModel.DataAnnotations;

    public class EditSkillBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}