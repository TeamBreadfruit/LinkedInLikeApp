using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedIn.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime SendOn { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
