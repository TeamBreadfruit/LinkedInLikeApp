namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Skill
    {
        private ICollection<ApplicationUser> users;

        public Skill()
        {
            this.Id = Guid.NewGuid();
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
