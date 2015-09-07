namespace LinkedIn.Services.Models.Companies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using LinkedIn.Models;
    using LinkedIn.Services.Models.Jobs;

    public class CompanyViewModel
    {
        public static Expression<Func<Company, CompanyViewModel>> Create
        {
            get
            {
                return g => new CompanyViewModel()
                {
                    Name = g.Name,
                    CreatedOn = g.CreatedOn,
                    Email = g.Name,
                    OwnerName = g.Owner.Name,
                    Jobs = g.Jobs.Select(j=> new JobCompanyViewModel()
                    {
                        Name = j.Name
                    })
                };
            }
        }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OwnerName { get; set; }

        public IEnumerable<JobCompanyViewModel> Jobs { get; set; } 
    }
}