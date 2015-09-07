namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Job
    {
        private ICollection<UserJob> users;

        public Job()
        {
            this.Id = Guid.NewGuid();
            this.users = new HashSet<UserJob>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<UserJob> Users
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
