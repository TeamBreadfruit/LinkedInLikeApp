namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Education
    {
        private ICollection<ApplicationUser> users;

        public Education()
        {
            this.Id = Guid.NewGuid();
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public virtual Degree Degree { get; set; }

        public double Grade { get; set; }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
