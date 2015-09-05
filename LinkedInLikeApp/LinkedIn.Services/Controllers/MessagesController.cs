namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LinkedIn.Models;
    using LinkedIn.Services.Models.Messages;
    using LinkedIn.Services.UserSessionUtils;

    using Microsoft.AspNet.Identity;

    [SessionAuthorize]
    [RoutePrefix("api")]
    public class MessagesController : BaseApiController
    {
        private const int PageSize = 20;

        [HttpGet]
        [Route("user/messages/sent/{page?}")]
        public async Task<IHttpActionResult> GetMessagesFromUser(int page = 1)
        {
            int currentPage = page - 1;
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            currentPage = currentPage < 0 ? 0 : currentPage;

            var messages = await this.Data.Messages.All()
                .Where(m => m.FromUserId == userId).OrderBy(m => m.SendOn)
                .Skip(currentPage * PageSize)
                .Take(PageSize)
                .Select(MessageViewModel.DataModel)
                .ToListAsync();

            return this.Ok(messages);
        }

        [HttpGet]
        [Route("user/messages/received/{page?}")]
        public async Task<IHttpActionResult> GetMessagesForUser(int page = 1)
        {
            int currentPage = page - 1;
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            currentPage = currentPage < 0 ? 0 : currentPage;

            var messages = await this.Data.Messages.All()
                .Where(m => m.ToUserId == userId)
                .OrderBy(m => m.SendOn)
                .Skip(currentPage * PageSize)
                .Take(PageSize)
                .Select(MessageViewModel.DataModel)
                .ToListAsync();

            return this.Ok(messages);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("group/{id}/messages/{page?}")]
        public async Task<IHttpActionResult> GetMessagesForGroup(string id, int page = 1)
        {
            int currentPage = page - 1;
            var group = await this.Data.Groups.All().FirstOrDefaultAsync(g => g.Id == new Guid(id));
            if (group == null)
            {
                return this.BadRequest("Group id is incorrect");
            }

            currentPage = currentPage < 0 ? 0 : currentPage;

            var messages = await this.Data.Messages.All()
                .Where(m => m.Group == group)
                .OrderBy(m => m.SendOn)
                .Skip(currentPage * PageSize)
                .Take(PageSize)
                .Select(MessageViewModel.DataModel)
                .ToListAsync();

            return this.Ok(messages);
        }

        [HttpGet]
        [Route("messages/{id}")]
        public async Task<IHttpActionResult> GetMessage(string id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var messages = await this.Data.Messages.All()
                .Where(m => m.Id == new Guid(id) && (m.GroupId != null || m.FromUserId == userId || m.ToUserId == userId))
                .Select(MessageViewModel.DataModel)
                .ToListAsync();

            var resultMessage = messages.FirstOrDefault();
            
            if (resultMessage == null)
            {
                return this.BadRequest("Message id is incorrect or you are not allowed to view this message");
            }

            return this.Ok(resultMessage);
        }

        [HttpPut]
        [Route("messages/{id}/edit")]
        public async Task<IHttpActionResult> PutMessage(string id, EditMessageBindingModel message)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var existingMessasge = await this.Data.Messages.All()
                .FirstOrDefaultAsync(m => m.Id == new Guid(id));
            if (existingMessasge == null)
            {
                return this.BadRequest("Message id is invalid.");
            }

            if (existingMessasge.FromUserId != userId)
            {
                return this.BadRequest("You cant edit this message. You are not the author.");
            }

            existingMessasge.Title = message.Title;
            existingMessasge.Content = message.Content;

            try
            {
                var rowsAffected = await this.Data.SaveChangesAsync();
                if (rowsAffected != 1)
                {
                    return this.BadRequest("Something went wrong");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return this.BadRequest("Something went wrong. Try again.");
            }

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("messages/create")]
        public async Task<IHttpActionResult> PostMessage(AddMessageBindingModel message)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (message.ToUserId != null && message.GroupId != null)
            {
                return this.BadRequest("You cant send a message to a user and a group at the same time");
            }
            else if (message.GroupId != null)
            {
                var userInGroup = await this.Data.Groups.All()
                    .FirstOrDefaultAsync(g => g.Id == new Guid(message.GroupId) && g.Users.Any(u => u.Id == userId));

                if (userInGroup == null)
                {
                    return this.BadRequest("You have no rights to post in this group or the group does not exist");
                }
            }
            else
            {
                var toUser = await this.Data.Users.All().FirstOrDefaultAsync(u => u.Id == message.ToUserId);

                if (toUser == null)
                {
                    return this.BadRequest("Incorrect receiver id. You cant send a message to no one.");
                }
            }

            var msgToSend = new Message()
            {
                Title = message.Title,
                Content = message.Content,
                SendOn = DateTime.Now,
                FromUserId = userId,
                GroupId = message.GroupId != null ? new Guid(message.GroupId) : (Guid?)null,
                ToUserId = message.ToUserId
            };

            this.Data.Messages.Add(msgToSend);
            var rowsAffected = await this.Data.SaveChangesAsync();
            if (rowsAffected != 1)
            {
                return this.BadRequest("Something went wrong");
            }

            return this.StatusCode(HttpStatusCode.Created);
        }

        // DELETE: api/Messages/5
        [Route("messages/{id}/delete")]
        public async Task<IHttpActionResult> DeleteMessage(Guid id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            Message message = await this.Data.Messages.All()
                .FirstOrDefaultAsync(m => m.Id == id && (m.FromUserId == userId || m.ToUserId == userId));
            if (message == null)
            {
                return this.BadRequest("Message id is incorrect or you are not allowed to delete this message");
            }

            this.Data.Messages.Delete(message);
            int rowsAffected = await this.Data.SaveChangesAsync();
            if (rowsAffected != 1)
            {
                return this.BadRequest("Something went wrong");
            }

            return this.Ok(message);
        }
    }
}