

namespace LinkedIn.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using LinkedIn.Models;

    public class EducationViewModel
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DegreeViewModels Degree { get; set; }

        public double Grade { get; set; }

        public static Expression<Func<LinkedIn.Models.Education, EducationViewModel>> Create
        {
            get
            {
                return a => new EducationViewModel()
                {
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Location = a.Location,
                    Degree = new DegreeViewModels()
                    {
                        Name = a.Degree.Name,
                        Description = a.Degree.Description
                    },
                    Grade = a.Grade

                };
            }
        }
    }
}