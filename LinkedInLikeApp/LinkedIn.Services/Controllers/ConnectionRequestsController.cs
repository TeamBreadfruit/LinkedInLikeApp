namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LinkedIn.Data;
    using LinkedIn.Models;
    using LinkedIn.Services.Models.ConnectionRequests;
    using LinkedIn.Services.UserSessionUtils;

    using Microsoft.AspNet.Identity;

    [SessionAuthorize]
    [RoutePrefix("api")]
    public class ConnectionRequestsController : BaseApiController
    {
        public ConnectionRequestsController()
            : base(new LinkedInData())
        {
        }
        
        [HttpGet]
        [Route("ConnectionRequests")]
        public async Task<IHttpActionResult> GetConnectionRequests()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var connectionRequests = await this.Data.ConnectionRequests.All()
                .Where(cr => cr.ToUserId == userId && cr.ConnectionRequestStatus == ConnectionRequestStatus.Pending)
                .Select(ConnectionRequestViewModel.Create)
                .ToListAsync();

            return this.Ok(connectionRequests);
        }

        [HttpPost]
        [Route("ConnectionRequests/Create")]
        public async Task<IHttpActionResult> PostConnectionRequests(ConnectionRequestBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var loggedUserUsername = this.User.Identity.GetUserName();
            if (model.Username == loggedUserUsername)
            {
                return this.BadRequest("You can nnot send connection request to yourself.");
            }

            var toUserId = await this.Data.Users.All()
                .Where(u => u.UserName == model.Username)
                .Select(u => u.Id)
                .FirstOrDefaultAsync();

            var loggedUser = await this.Data.Users.All()
                .FirstOrDefaultAsync(u => u.Id == userId);

            var existingConnection = loggedUser.Connections
                .FirstOrDefault(c => c.UserName == model.Username);

            if (existingConnection != null)
            {
                return this.BadRequest(string.Format("User '{0}' is already in your connection.", model.Username));
            }

            var newConnectionRequest = new ConnectionRequest
            {
                ConnectionRequestStatus = ConnectionRequestStatus.Pending,
                FromUserId = userId,
                ToUserId = toUserId
            };

            this.Data.ConnectionRequests.Add(newConnectionRequest);
            await this.Data.SaveChangesAsync();

            return this.Ok(string.Format("Connection request to user: '{0}' successfully sent!", model.Username));
        }

        [HttpPut]
        [Route("ConnectionRequests/{id}/Approve")]
        public async Task<IHttpActionResult> ApproveConnectionRequests(string id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            
            Guid connectionRequestGuid = new Guid(id);
            var connectionRequestInDb = await this.Data.ConnectionRequests.All()
                .FirstOrDefaultAsync(
                    cr => cr.Id == connectionRequestGuid &&
                    cr.ConnectionRequestStatus == ConnectionRequestStatus.Pending);

            if (connectionRequestInDb == null)
            {
                return this.BadRequest("Connection request already approved/rejected or is not existing.");
            }

            if (connectionRequestInDb.ToUserId != userId)
            {
                return this.BadRequest("You can not approve connection request that is not sent to you.");
            }

            var loggedUser = await this.Data.Users.All()
                .FirstOrDefaultAsync(u => u.Id == userId);
            var newConnection = await this.Data.Users.All()
                .FirstOrDefaultAsync(u => u.Id == connectionRequestInDb.FromUserId);

            connectionRequestInDb.ConnectionRequestStatus = ConnectionRequestStatus.Approved;
            loggedUser.Connections.Add(newConnection);
            await this.Data.SaveChangesAsync();

            return this.Ok(string.Format(
                "Connection request to user: '{0}' successfully approved!",
                connectionRequestInDb.FromUser.UserName));
        }

        [HttpPut]
        [Route("ConnectionRequests/{id}/Reject")]
        public async Task<IHttpActionResult> RejectConnectionRequests(string id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            Guid connectionRequestGuid = new Guid(id);
            var connectionRequestInDb = await this.Data.ConnectionRequests.All()
                .FirstOrDefaultAsync(
                    cr => cr.Id == connectionRequestGuid && 
                    cr.ConnectionRequestStatus == ConnectionRequestStatus.Pending);

            if (connectionRequestInDb == null)
            {
                return this.BadRequest("Connection request already approved/rejected or is not existing.");
            }

            if (connectionRequestInDb.ToUserId != userId)
            {
                return this.BadRequest("You can not reject connection request that is not sent to you.");
            }

            connectionRequestInDb.ConnectionRequestStatus = ConnectionRequestStatus.Rejected;
            await this.Data.SaveChangesAsync();

            return this.Ok(string.Format(
                "Connection request to user: '{0}' successfully rejected!",
                connectionRequestInDb.FromUser.UserName));
        }
    }
}
