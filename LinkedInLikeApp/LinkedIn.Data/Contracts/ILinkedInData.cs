namespace LinkedIn.Data.Contracts
{
    using LinkedIn.Models;

    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Threading.Tasks;

    public interface ILinkedInData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<IdentityRole> UserRoles { get; }

        IRepository<Company> Companies { get; }

        IRepository<ConnectionRequest> ConnectionRequests { get; }

        IRepository<Degree> Degrees { get; }

        IRepository<Education> Educations { get; }

        IRepository<Group> Groups { get; }

        IRepository<Job> Jobs { get; }

        IRepository<Message> Messages { get; }

        IRepository<Skill> Skills { get; }

        IRepository<UserSession> UserSessions { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
