using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LinkedIn.Services.Models.Education
{
    public class EditEducationBindingModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? DegreeId { get; set; }

        public string DegreeDescription { get; set; }

        public double? Grade { get; set; }
    }
}