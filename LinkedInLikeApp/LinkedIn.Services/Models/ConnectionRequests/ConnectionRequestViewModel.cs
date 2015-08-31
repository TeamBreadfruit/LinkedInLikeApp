namespace LinkedIn.Services.Models.ConnectionRequests
{
    using System;
    using System.Linq.Expressions;

    using LinkedIn.Models;

    public class ConnectionRequestViewModel
    {
        public static Expression<Func<ConnectionRequest, ConnectionRequestViewModel>> Create
        {
            get
            {
                return x => new ConnectionRequestViewModel
                {
                    Id = x.Id.ToString(),
                    FromUser = x.FromUser.Name,
                    FromUserUsername = x.FromUser.UserName,
                    FromUserId = x.FromUserId
                };
            }
        }

        public string Id { get; set; }

        public string FromUser { get; set; }

        public string FromUserUsername { get; set; }

        public string FromUserId { get; set; }
    }
}