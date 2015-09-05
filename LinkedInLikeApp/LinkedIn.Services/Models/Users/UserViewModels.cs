namespace LinkedIn.Services.Models.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using LinkedIn.Models;
    using LinkedIn.Services.Models.Educations;
    using LinkedIn.Services.Models.Jobs;
    using LinkedIn.Services.Models.Skills;

    public class UserRegisterViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }
    }

    public class GetUserInfoViewModel
    {
        public static Expression<Func<ApplicationUser, GetUserInfoViewModel>> Create
        {
            get
            {
                return u => new GetUserInfoViewModel
                {
                    Username = u.UserName,
                    Name = u.Name,
                    Address = u.Address ?? "No address",
                    Website = u.Website ?? "No website",
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber ?? "No phone number",
                    Skills = u.Skills.Select(s => new SkillViewModel
                    {
                        Name = s.Name
                    }),
                    Educations = u.Educations.Select(e => new EducationViewModel
                    {
                        Name = e.Name,
                        Location = e.Location,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Degree = new DegreeViewModel
                        {
                            Name = e.Degree.Name,
                            Description = e.Degree.Description
                        }
                    }),
                    Jobs = u.Jobs.Select(j => new JobViewModel
                    {
                        Name = j.Name,
                        Description = j.Description,
                        Company = j.Company.Name,
                        StartDate = j.StartDate,
                        EndDate = j.EndDate
                    })
                };
            }
        }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public IEnumerable<SkillViewModel> Skills { get; set; }

        public IEnumerable<EducationViewModel> Educations { get; set; }

        public IEnumerable<JobViewModel> Jobs { get; set; }
    }
}
