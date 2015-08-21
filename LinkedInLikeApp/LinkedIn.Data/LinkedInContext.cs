namespace LinkedIn.Data
{
    using System.Data.Entity;
    using LinkedIn.Models;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class LinkedInContext : IdentityDbContext<ApplicationUser>
    {
        public LinkedInContext()
            : base("name=LinkedInContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<LinkedInContext, Migrations.Configuration>());
        }
        
        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Degree> Degrees { get; set; }

        public virtual DbSet<Education> Educations { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<Message> Messages { get; set; }

        public virtual DbSet<Skill> Skills { get; set; }

        public static LinkedInContext Create()
        {
            return new LinkedInContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(x => x.Connections)
              .WithMany()
              .Map(m =>
              {
                  m.MapLeftKey("UserId");
                  m.MapRightKey("ConnectionId");
                  m.ToTable("UserConnections");
              });

            base.OnModelCreating(modelBuilder);
        }
    }
}