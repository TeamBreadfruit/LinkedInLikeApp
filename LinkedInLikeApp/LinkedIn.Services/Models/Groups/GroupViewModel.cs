namespace LinkedIn.Services.Models.Groups
{
    using System;
    using System.Linq.Expressions;

    public class GroupViewModel
    {
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

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }
    }
}