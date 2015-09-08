using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using LinkedIn.Models;
using LinkedIn.Services.UserSessionUtils;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using LinkedIn.Services.Models;
using LinkedIn.Services.Models.Skills;

namespace LinkedIn.Services.Controllers
{
    [SessionAuthorize]
    [RoutePrefix("api")]
    public class SkillsController : BaseApiController
    {

        // GET: api/Skills

        [Route("user/skill/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSkillsForUser()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var result = await this.Data.Skills.All()
                .Where(e => e.Users.Any(u => u.Id == userId))
                .OrderBy(e => e.Description)
                .Select(SkillViewModel.Create).ToListAsync();
            if (result == null)
            {
                return this.Ok("No such skill.");
            }

            return this.Ok(result);
        }

        // GET: api/Skills/5

        [Route("user/skill/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> UserSkillById(string id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var result = await this.Data.Skills.All()
                .Where(e => e.Users.Any(u => u.Id == userId) && e.Id == new Guid(id))
                .Select(SkillViewModel.Create).ToListAsync();
            if (result.Count == 0)
            {
                return this.BadRequest("Invalid id ");
            }

            return this.Ok(result);
        }

        // PUT: api/Skills/5
        [HttpPut]
        [Route("user/skill/{id}/edit")]
        public async Task<IHttpActionResult> EditSkill(Guid id, EditSkillBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            if (model == null)
            {
                return this.BadRequest("Invalid input data");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var skillsToCurrentUser = await this.Data.Skills.All()
                .Where(s => s.Users
                    .Any(u => u.Id == userId))
                .ToListAsync();
            if (skillsToCurrentUser.Count == 0)
            {
                return this.Unauthorized();
            }
            if (skillsToCurrentUser.All(s => s.Id != id))
            {
                return this.Unauthorized();
            }
            var result = await this.Data.Skills
                .All()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (result == null)
            {
                return this.BadRequest("Invalid skill id");
            }


            result.Name = model.Name ?? result.Name;
            result.Description = model.Description ?? result.Description;

            await this.Data.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Skills
        [HttpPost]
        [Route("user/skill/create")]
        public async Task<IHttpActionResult> PostSkill(SkillBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            ApplicationUser currentUser = await this.Data.Users.All().FirstOrDefaultAsync(us => us.Id == userId);


            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            if (model == null)
            {
                return this.BadRequest("Invalid input datta");
            }
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            await this.Data.SaveChangesAsync();
            Skill skill = new Skill()
            {
                Name = model.Name,
                Description = model.Description,
                Users = new List<ApplicationUser>()
                {
                    currentUser
                },
            };
            this.Data.Skills.Add(skill);
            await this.Data.SaveChangesAsync();
            return StatusCode(HttpStatusCode.Created);
        }

        // DELETE: api/Skills/5
        [HttpDelete]
        [Route("user/skill/{id}/delete")]
        public async Task<IHttpActionResult> EditSkill(Guid id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var skillsToCurrentUser = await this.Data.Skills.All()
                .Where(s => s.Users
                .Any(u => u.Id == userId))
                .ToListAsync();

            if (skillsToCurrentUser == null)
            {
                return BadRequest("Skill id is incorrect or you are not allowed to delete it");
            }
            if (skillsToCurrentUser.All(s => s.Id != id))
            {
                return this.Unauthorized();
            }
            var skillToDelete = this.Data.Skills.All().FirstOrDefaultAsync(s => s.Id == id);

            this.Data.Skills.Delete(skillToDelete);
            await this.Data.SaveChangesAsync();
            return this.Ok("deleted successfully");

        }

    }
}