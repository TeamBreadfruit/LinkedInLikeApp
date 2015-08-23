namespace LinkedIn.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ConnectionRequest
    {
        public ConnectionRequest()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        public ConnectionRequestStatus ConnectionRequestStatus { get; set; }

        [Required]
        [ForeignKey("FromUser")]
        public string FromUserId { get; set; }

        public virtual ApplicationUser FromUser { get; set; }

        [Required]
        [ForeignKey("ToUser")]
        public string ToUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }
    }
}
