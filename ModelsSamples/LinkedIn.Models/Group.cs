using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Group
    {
        private ICollection<Message> messages;
        private ICollection<ApplicationUser> users;

        public Group()
        {
            this.messages = new HashSet<Message>();
            this.users = new HashSet<ApplicationUser>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Message> Messages { get { return this.messages; } set { this.messages = value; } }
        public virtual ICollection<ApplicationUser> Users { get { return this.users; } set { this.users = value; } }
    }
}
