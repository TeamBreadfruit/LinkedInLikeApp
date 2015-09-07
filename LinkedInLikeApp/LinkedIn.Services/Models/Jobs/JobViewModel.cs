namespace LinkedIn.Services.Models.Jobs
{
    using LinkedIn.Models;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;

    public class JobViewModel
    {
        public static Expression<Func<Job, JobViewModel>> DataModel
        {
            get
            {
                return job => new JobViewModel
                {
                    Name = job.Name,
                    Company = job.Company.Name,
                    Users = job.Users.Select(u => new JobUserViewModel()
                    {
                        Description = u.Description,
                        StartDate = u.StartDate,
                        EndDate = u.EndDate,
                        UserId = u.UserId,
                        Username = u.User.UserName
                    })
                };
            }
        }

        public string Name { get; set; }

        public string Company { get; set; }

        public IEnumerable<JobUserViewModel> Users { get; set; }
    }
}