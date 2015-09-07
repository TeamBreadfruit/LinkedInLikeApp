namespace LinkedIn.Services.Models.Educations
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class EducationBindingModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(10)]
        public string Name { get; set; }

        public string Location { get; set; }
       
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Guid DegreeId { get; set; }

        public string DegreeDescription { get; set; }

        [Required]
        public double Grade { get; set; }
    }
}