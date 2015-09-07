namespace LinkedIn.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class UserJob
    {
        public UserJob()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
