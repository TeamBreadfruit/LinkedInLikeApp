namespace LinkedIn.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        public virtual ApplicationUser FromUser { get; set; }

        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }

        public Guid? GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
