namespace LinkedIn.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public Guid GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
