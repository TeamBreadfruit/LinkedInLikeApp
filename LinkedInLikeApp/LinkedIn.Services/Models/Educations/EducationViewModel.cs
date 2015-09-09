namespace LinkedIn.Services.Models.Educations
{
    using System;
    using System.Linq.Expressions;

    public class EducationViewModel
    {
        public static Expression<Func<LinkedIn.Models.Education, EducationViewModel>> Create
        {
            get
            {
                return a => new EducationViewModel()
                {
                    Id = a.Id,
                    Name = a.Name,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    Location = a.Location,
                    Degree = new DegreeViewModel()
                    {
                        Name = a.Degree.Name
                    },
                    Grade = a.Grade
                };
            }
        }
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DegreeViewModel Degree { get; set; }

        public double Grade { get; set; }
    }
}