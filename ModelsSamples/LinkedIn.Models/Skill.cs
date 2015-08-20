using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Skill
    {
        private ICollection<ApplicationUser> users;

        public Skill()
        {
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Users { get { return this.users; } set { this.users = value; } }
    }
}
