namespace LinkedIn.Services.Models.Messages
{
    using System;
    using System.Linq.Expressions;

    using LinkedIn.Models;

    public class MessageViewModel
    {
        public static Expression<Func<Message, MessageViewModel>> DataModel
        {
            get
            {
                return message => new MessageViewModel
                {
                    Id = message.Id,
                    Title = message.Title,
                    Content = message.Content,
                    SendOn = message.SendOn,
                    FromUserId = message.FromUserId,
                    FromUserName = message.FromUser.UserName,
                    ToUserId = message.ToUserId,
                    ToUserName = message.ToUser != null ? message.ToUser.UserName : null,
                    ToGroupId = message.GroupId,
                    ToGroupName = message.Group != null ? message.Group.Name : null
                };
            }
        }
 
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
