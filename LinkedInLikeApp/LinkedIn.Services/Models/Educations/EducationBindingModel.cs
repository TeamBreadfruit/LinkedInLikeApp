namespace LinkedIn.Services.Models.Educations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EducationBindingModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Location { get; set; }
       
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string DegreeName { get; set; }

        [Required]
        public double Grade { get; set; }
    }
}