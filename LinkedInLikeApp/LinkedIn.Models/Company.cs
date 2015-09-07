namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Company
    {
        private ICollection<Job> jobs;

        public Company()
        {
            this.Id = Guid.NewGuid();
            this.jobs = new HashSet<Job>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime CreatedOn { get; set; }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public virtual ICollection<Job> Jobs
        {
            get { return this.jobs; } 
            set { this.jobs = value; }
        }
    }
}
