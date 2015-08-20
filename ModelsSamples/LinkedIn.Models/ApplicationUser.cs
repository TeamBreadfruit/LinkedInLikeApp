using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class ApplicationUser : IdentityUser
    {
        private ICollection<UserJob> jobs;
        private ICollection<Skill> skills;
        private ICollection<UserEducation> educations;
        private ICollection<Group> groups;
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> connections;
        private ICollection<Company> companies;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }

        public ApplicationUser()
        {
            this.jobs = new HashSet<UserJob>();
            this.skills = new HashSet<Skill>();
            this.educations = new HashSet<UserEducation>();
            this.groups = new HashSet<Group>();
            this.messages = new HashSet<Message>();
            this.connections = new HashSet<ApplicationUser>();
            this.companies = new HashSet<Company>();
        }

        public virtual ICollection<UserJob> Jobs { get { return this.jobs; } set { this.jobs = value; } }
        public virtual ICollection<Skill> Skills { get { return this.skills; } set { this.skills = value; } }
        public virtual ICollection<UserEducation> Educations { get { return this.educations; } set { this.educations = value; } }
        public virtual ICollection<Group> Groups { get { return this.groups; } set { this.groups = value; } }
        public virtual ICollection<Message> Messages { get { return this.messages; } set { this.messages = value; } }
        public virtual ICollection<ApplicationUser> Connections { get { return this.connections; } set { this.connections = value; } }
        public virtual ICollection<Company> Companies { get { return this.companies; } set { this.companies = value; } }
    }
}
