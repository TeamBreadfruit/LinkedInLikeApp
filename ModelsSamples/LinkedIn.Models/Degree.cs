using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Degree
    {
        private ICollection<UserEducation> userEducations;

        public Degree()
        {
            this.userEducations = new HashSet<UserEducation>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserEducation> UserEducations { get { return this.userEducations; } set { this.userEducations = value; } }
    }
}
