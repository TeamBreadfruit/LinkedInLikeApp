using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Education
    {
        private ICollection<UserEducation> users;

        public Education()
        {
            this.users = new HashSet<UserEducation>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public virtual ICollection<UserEducation> Users { get { return this.users; } set { this.users = value; } }
    }
}
