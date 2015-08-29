using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedIn.Services.Models
{
    public class EducationBindingModel
    {
        [Required]
        public string Name { get; set; }

        public string Location { get; set; }
        [Required]

        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public Guid  DegreeId { get; set; }

        public string DegreeDescription { get; set; }

        [Required]
        public double Grade { get; set; }
    }
}