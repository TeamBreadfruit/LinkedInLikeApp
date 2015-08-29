using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using LinkedIn.Data;
using LinkedIn.Models;
using LinkedIn.Services.Models.Messages;
using Microsoft.AspNet.Identity;
using LinkedIn.Services.UserSessionUtils;

namespace LinkedIn.Services.Controllers
{
    [SessionAuthorize]
    [RoutePrefix("api")]
    public class MessagesController : BaseApiController
    {
        private const int PAGE_SIZE = 20;

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

            var messages = await this.Data.Messages.All().Where(m => m.FromUserId == userId).OrderBy(m => m.SendOn).Select(m => new MessageViewModel() { 
                Id = m.Id,
                Title = m.Title,
                Content = m.Content,
                SendOn = m.SendOn,
                FromUserId = m.FromUserId,
                FromUserName = m.FromUser.UserName,
                ToUserId = m.ToUserId,
                ToUserName = m.ToUser != null ? m.ToUser.UserName : null,
                ToGroupId = m.GroupId,
                ToGroupName = m.Group != null ? m.Group.Name : null
            })
                .Skip(currentPage * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();

            return Ok(messages);
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

            var messages = await this.Data.Messages.All().Where(m => m.ToUserId == userId).OrderBy(m => m.SendOn).Select(m => new MessageViewModel()
            {
                Id = m.Id,
                Title = m.Title,
                Content = m.Content,
                SendOn = m.SendOn,
                FromUserId = m.FromUserId,
                FromUserName = m.FromUser.UserName,
                ToUserId = m.ToUserId,
                ToUserName = m.ToUser != null ? m.ToUser.UserName : null,
                ToGroupId = m.GroupId,
                ToGroupName = m.Group != null ? m.Group.Name : null
            })
                .Skip(currentPage * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();

            return Ok(messages);
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
                return BadRequest("Group id is incorrect");
            }

            currentPage = currentPage < 0 ? 0 : currentPage;

            var messages = await this.Data.Messages.All().Where(m => m.Group == group).OrderBy(m => m.SendOn).Select(m => new MessageViewModel()
            {
                Id = m.Id,
                Title = m.Title,
                Content = m.Content,
                SendOn = m.SendOn,
                FromUserId = m.FromUserId,
                FromUserName = m.FromUser.UserName,
                ToUserId = m.ToUserId,
                ToUserName = m.ToUser != null ? m.ToUser.UserName : null,
                ToGroupId = m.GroupId,
                ToGroupName = m.Group != null ? m.Group.Name : null
            })
                .Skip(currentPage * PAGE_SIZE)
                .Take(PAGE_SIZE)
                .ToListAsync();

            return Ok(messages);
        }

        [HttpGet]
        [Route("messages/{id}")]
        public async Task<IHttpActionResult> GetMessage(Guid id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            Message message = await this.Data.Messages.All().FirstOrDefaultAsync(m => m.Id == id && (m.GroupId != null || m.FromUserId == userId || m.ToUserId == userId));
            if (message == null)
            {
                return BadRequest("Message id is incorrect or you are not allowed to view this message");
            }

            return Ok(new MessageViewModel()
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
            });
        }

        [HttpPut]
        [Route("messages/{id}/edit")]
        public async Task<IHttpActionResult> PutMessage(string id, EditMessageBindingModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var existingMessasge = await this.Data.Messages.All().FirstOrDefaultAsync(m => m.Id == new Guid(id));
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
                    return BadRequest("Something went wrong");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Something went wrong. Try again.");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Route("messages/create")]
        public async Task<IHttpActionResult> PostMessage(AddMessageBindingModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            if (message.ToUserId != null && message.GroupId != null)
            {
                return BadRequest("You cant send a message to a user and a group at the same time");
            }
            else if (message.GroupId != null)
            {
                var userInGroup = await this.Data.Groups.All().FirstOrDefaultAsync(g => g.Id == new Guid(message.GroupId) && g.Users.Any(u => u.Id == userId));

                if (userInGroup == null)
                {
                    return BadRequest("You have no rights to post in this group or the group does not exist");
                }
            }
            else
            {
                var toUser = await this.Data.Users.All().FirstOrDefaultAsync(u => u.Id == message.ToUserId);

                if (toUser == null)
                {
                    return BadRequest("Incorrect receiver id. You cant send a message to no one.");
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
                return BadRequest("Something went wrong");
            }

            return StatusCode(HttpStatusCode.Created);
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

            Message message = await this.Data.Messages.All().FirstOrDefaultAsync(m => m.Id == id && (m.FromUserId == userId || m.ToUserId == userId));
            if (message == null)
            {
                return BadRequest("Message id is incorrect or you are not allowed to delete this message");
            }

            this.Data.Messages.Delete(message);
            int rowsAffected = await this.Data.SaveChangesAsync();
            if (rowsAffected != 1)
            {
                return BadRequest("Something went wrong");
            }

            return Ok(message);
        }
    }
}