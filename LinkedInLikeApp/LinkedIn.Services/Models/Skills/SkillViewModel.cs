namespace LinkedIn.Services.Models.Skills
{
    using System;
    using System.Linq.Expressions;

    public class SkillViewModel
    {
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

        public string Name { get; set; }

        public string Description { get; set; }
    }
}