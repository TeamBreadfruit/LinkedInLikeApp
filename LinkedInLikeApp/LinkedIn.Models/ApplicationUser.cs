namespace LinkedIn.Models
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Job> jobs;
        private ICollection<Skill> skills;
        private ICollection<Education> educations;
        private ICollection<Group> groups;
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> connections;
        private ICollection<Company> companies;

        private ICollection<ConnectionRequest> connectionRequests;

        public ApplicationUser()
        {
            this.jobs = new HashSet<Job>();
            this.skills = new HashSet<Skill>();
            this.educations = new HashSet<Education>();
            this.groups = new HashSet<Group>();
            this.messages = new HashSet<Message>();
            this.connections = new HashSet<ApplicationUser>();
            this.companies = new HashSet<Company>();
            this.connectionRequests = new HashSet<ConnectionRequest>();
        }

        public virtual ICollection<Job> Jobs
        {
            get { return this.jobs; } 
            set { this.jobs = value; }
        }

        public virtual ICollection<Skill> Skills
        {
            get { return this.skills; } 
            set { this.skills = value; }
        }

        public virtual ICollection<Education> Educations
        {
            get { return this.educations; } 
            set { this.educations = value; }
        }

        public virtual ICollection<Group> Groups
        {
            get { return this.groups; } 
            set { this.groups = value; }
        }

        public virtual ICollection<Message> Messages
        {
            get { return this.messages; } 
            set { this.messages = value; }
        }

        public virtual ICollection<ApplicationUser> Connections
        {
            get { return this.connections; } 
            set { this.connections = value; }
        }

        public virtual ICollection<Company> Companies
        {
            get { return this.companies; } 
            set { this.companies = value; }
        }

        public virtual ICollection<ConnectionRequest> ConnectionRequests
        {
            get { return this.connectionRequests; }
            set { this.connectionRequests = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }
    }
}
