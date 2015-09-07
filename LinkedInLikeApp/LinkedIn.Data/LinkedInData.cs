namespace LinkedIn.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    using LinkedIn.Data.Contracts;
    using LinkedIn.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Threading.Tasks;

    public class LinkedInData : ILinkedInData
    {
        private readonly DbContext context;

        private readonly IDictionary<Type, object> repositories;

        public LinkedInData()
            : this(new LinkedInContext())
        {
        }

        public LinkedInData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<IdentityRole> UserRoles
        {
            get { return this.GetRepository<IdentityRole>(); }
        }

        public IRepository<Company> Companies
        {
            get { return this.GetRepository<Company>(); }
        }

        public IRepository<ConnectionRequest> ConnectionRequests
        {
            get { return this.GetRepository<ConnectionRequest>(); }
        }

        public IRepository<Degree> Degrees
        {
            get { return this.GetRepository<Degree>(); }
        }

        public IRepository<Education> Educations
        {
            get { return this.GetRepository<Education>(); }
        }

        public IRepository<Group> Groups
        {
            get { return this.GetRepository<Group>(); }
        }

        public IRepository<Job> Jobs
        {
            get { return this.GetRepository<Job>(); }
        }

        public IRepository<UserJob> UserJobs
        {
            get { return this.GetRepository<UserJob>(); }
        }

        public IRepository<Message> Messages
        {
            get { return this.GetRepository<Message>(); }
        }

        public IRepository<Skill> Skills
        {
            get { return this.GetRepository<Skill>(); }
        }

        public IRepository<UserSession> UserSessions
        {
            get { return this.GetRepository<UserSession>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            if (!this.repositories.ContainsKey(typeof(T)))
            {
                var type = typeof(EfRepository<T>);
                this.repositories.Add(typeof(T), Activator.CreateInstance(type, this.context));
            }

            return (IRepository<T>)this.repositories[typeof(T)];
        }

        public Task<int> SaveChangesAsync()
        {
            return this.context.SaveChangesAsync();
        }
    }
}
