using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Job
    {
        private ICollection<UserJob> users;

        public Job()
        {
            this.users = new HashSet<UserJob>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual ICollection<UserJob> Users { get { return this.users; } set { this.users = value; } }
    }
}
