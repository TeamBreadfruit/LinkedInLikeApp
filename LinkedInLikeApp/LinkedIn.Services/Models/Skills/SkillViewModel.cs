

namespace LinkedIn.Services.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;
    using LinkedIn.Models;

    public class SkillViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public static Expression<Func<LinkedIn.Models.Skill, SkillViewModel>> Create
        {
            get
            {
                return a => new SkillViewModel()
                {
                    Name = a.Name,
                    Description = a.Description
                };
            }
        }
    }
}