using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using LinkedIn.Models;
using LinkedIn.Services.Models.Groups;
using LinkedIn.Services.UserSessionUtils;
using Microsoft.AspNet.Identity;

namespace LinkedIn.Services.Controllers
{
    [RoutePrefix("api")]
    [SessionAuthorize]
    public class GroupsController:BaseApiController
    {
        // create , delete ,change , get and get all // get all for current user or get by id

        [HttpGet]
        [AllowAnonymous]
        [Route("groups/all")]
        public async Task<IHttpActionResult> GetAll()
        {
            var groups = await this.Data.Groups
                .All()
                .OrderBy(g=>g.Name)
                .Select(GroupViewModel.Create).ToListAsync();

            if (!groups.Any())
            {
                return this.Ok("No groups yet.");
            }

            return  this.Ok(groups);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("group/{id}")]
        public async Task<IHttpActionResult> GetById(string id)
        {
            var group = await this.Data.Groups
                .All()
                .Where(g=>g.Id==new Guid(id))
                .Select(GroupViewModel.Create).ToListAsync();

            var groupResult = group.FirstOrDefault();

            if (groupResult == null)
            {
                return this.BadRequest("Invalid group id");
            }

            return this.Ok(groupResult);
        }

        [HttpGet]
        [Route("user/groups/all")]
        public async Task<IHttpActionResult> GroupsForCurrentUser()
        {

            var userId = this.User.Identity.GetUserId();

            var groupsForCurrentUser =await this.Data.Groups
                .All()
                .Where(g => g.Users
                    .Any(u => u.Id == userId))
                .Select(GroupViewModel.Create)
                .ToListAsync();

            if (!groupsForCurrentUser.Any())
            {
                return this.Ok("No groups where found for the current user");
            }
            return this.Ok(GroupsForCurrentUser());

        }

        [HttpGet]
        [Route("user/group/{id}")]
        public async Task<IHttpActionResult> GroupForCurrentUserById(string id)
        {

            var userId = this.User.Identity.GetUserId();

            var groupForCurrentUserById = await this.Data.Groups
                .All()
                .Where(g => g.Users
                    .Any(u => u.Id == userId) && g.Id== new Guid(id))
                .Select(GroupViewModel.Create)
                .ToListAsync();

            var resultGroup = groupForCurrentUserById.FirstOrDefault();

            if (resultGroup==null)
            {
                return this.BadRequest("Group id is not correct or you are not allowed to view it.");
            }
            return this.Ok(GroupsForCurrentUser());

        }

        [HttpPost]
        [Route("user/group/create")]
        public async Task<IHttpActionResult> GroupForCurrentUserById(GroupBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            var currentUser =await this.Data.Users.All().Where(u => u.Id == userId).ToListAsync();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null)
            {
                return this.BadRequest("Invalid input data");
            }

            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var group = new Group()
            {
                Name = model.Name,
                CreatedOn = model.CreatedOn,
                Description = model.Description ?? null,
                Users = currentUser
            };
            this.Data.Groups.Add(group);
            await this.Data.SaveChangesAsync();

            return StatusCode(HttpStatusCode.Created);
        }


        [HttpPut]
        [Route("user/group/{id}/edit")]
        public async Task<IHttpActionResult> EditGroupForUserById(string id,EditGroupBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (model == null)
            {
                return this.BadRequest("Invalid input data");
            }

            var groupForCurrentUser = await this.Data.Groups.All()
                .Where(g => g.Users.Any(u => u.Id == userId) && g.Id == new Guid(id))
                .ToListAsync();

            var groupResult = groupForCurrentUser.FirstOrDefault();

            if (groupResult == null)
            {
                return this.BadRequest("Group id is not correct or you are not allowed to view it.");
            }

            groupResult.Description = model.Description ?? groupResult.Description;
            groupResult.Name = model.Name ?? groupResult.Name;
            
            await this.Data.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }


        [HttpDelete]
        [Route("user/group/{id}/delete")]
        public async Task<IHttpActionResult> DeleteGroupForUserById(string id)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            var groupForCurrentUser = await this.Data.Groups.All()
               .Where(g => g.Users.Any(u => u.Id == userId) && g.Id == new Guid(id))
               .ToListAsync();

            var groupToDelete = groupForCurrentUser.FirstOrDefault();

            if (groupToDelete == null)
            {
                return this.BadRequest("Group id is not correct or you are not allowed to delete it.");
            }
            this.Data.Groups.Delete(groupToDelete);
            await this.Data.SaveChangesAsync();

            return this.Ok("deleted successfully");
        } 

    }
}