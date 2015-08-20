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
        }
    }
}
