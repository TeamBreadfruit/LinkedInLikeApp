using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace LinkedIn.Services.Models.Groups
{
    public class GroupViewModel
    {
        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public static Expression<Func<LinkedIn.Models.Group, GroupViewModel>> Create
        {
            get
            {
                return g => new GroupViewModel()
                {
                    Name = g.Name,
                    CreatedOn = g.CreatedOn,
                    Description = g.Description
                };
            }
        }
    }
}