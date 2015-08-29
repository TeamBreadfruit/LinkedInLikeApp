namespace LinkedIn.Data.Contracts
{
    using LinkedIn.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    public interface ILinkedInData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<IdentityRole> UserRoles { get; }

        IRepository<Company> Companies { get; }

        IRepository<Degree> Degrees { get; }

        IRepository<Education> Educations { get; }

        IRepository<Group> Groups { get; }

        IRepository<Job> Jobs { get; }

        IRepository<Message> Messages { get; }

        IRepository<Skill> Skills { get; }

        IRepository<UserSession> UserSessions { get; }

        int SaveChanges();
    }
}
