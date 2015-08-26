namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Degree
    {
        private ICollection<ApplicationUser> users;

        public Degree()
        {
            this.Id = Guid.NewGuid();
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> UserEducations
        {
            get { return this.users; } 
            set { this.users = value; }
        }
    }
}
