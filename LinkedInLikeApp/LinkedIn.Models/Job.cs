namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Job
    {
        private ICollection<ApplicationUser> users;

        public Job()
        {
            this.Id = Guid.NewGuid();
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
