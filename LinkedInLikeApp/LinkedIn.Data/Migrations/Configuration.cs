namespace LinkedIn.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LinkedIn.Data.LinkedInContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "LinkedIn.Data.LinkedInContext";
        }

        protected override void Seed(LinkedIn.Data.LinkedInContext context)
        {
            if (!context.Degrees.Any())
            {
                var degreeList = new List<Degree>()
                {
                    new Degree() {Name = "High School"},
                    new Degree() {Name = "Associate's Degree"},
                    new Degree() {Name = "Bachelor's Degree"},
                    new Degree(){ Name = "Master's Degree"},
                    new Degree(){ Name = "Master's of Business Administration (M.B.A)"},
                    new Degree(){ Name = "Juris Doctor (J.D.)"},
                    new Degree(){ Name = "Doctor of Medicine (M.D.)"},
                    new Degree(){ Name = "Doctor of Philosophy (Ph.D.)"},
                    new Degree(){ Name = "Engineer's Degree"},
                    new Degree(){ Name = "Other"},
                };
                context.Degrees.AddRange(degreeList);
                context.SaveChanges();
            }
        }
    }
}
