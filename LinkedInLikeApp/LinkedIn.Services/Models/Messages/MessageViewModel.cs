using LinkedIn.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinkedIn.Services.Models.Messages
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public string FromUserId { get; set; }

        public string FromUserName { get; set; }

        public string ToUserId { get; set; }

        public string ToUserName { get; set; }

        public Guid? ToGroupId { get; set; }

        public string ToGroupName { get; set; }
    }
}