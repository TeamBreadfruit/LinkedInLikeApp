namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;

    using LinkedIn.Models;
    using LinkedIn.Services.Models.Educations;
    using LinkedIn.Services.UserSessionUtils;

    using Microsoft.AspNet.Identity;


    [SessionAuthorize]
    [RoutePrefix("api")]
    public class EducationsController : BaseApiController
    {
        [Route("user/education/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetEducationsForUser()
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var result = await this.Data.Educations.All()
                .Where(e => e.Users.Any(u => u.Id == userId))
                .OrderBy( e=> e.StartDate)
                .Select(EducationViewModel.Create).ToListAsync();
            if (result == null)
            {
                return this.Ok("No data about this user education.");
            }

            return this.Ok(result);
        }

        [Route("user/education/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> UserEducationById(string id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var result = await this.Data.Educations.All()
                .Where(e => e.Users.Any(u => u.Id == userId) && e.Id == new Guid(id))
                .Select(EducationViewModel.Create).ToListAsync();
            if (result.Count == 0)
            {
                return this.BadRequest("Invalid id ");
            }

            return this.Ok(result);
        }

        [HttpPost]
        [Route("user/education/create")]
        public async Task<IHttpActionResult> PostEducation(EducationBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();

            ApplicationUser currentUser = await this.Data.Users.All().FirstOrDefaultAsync(us => us.Id == userId);


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
                return this.BadRequest(this.ModelState);
            }
            var degree = await this.Data.Degrees.All().FirstOrDefaultAsync(d => d.Name==model.DegreeName);
            if (degree == null)
            {
                var errorMessage = "Invalid degree name.Posiible degree names: " +
                                    "Juris Doctor (J.D.) ," + "Doctor of Medicine (M.D.) ," +
                                    "Master's of Business Administration (M.B.A) ," +
                                    "Engineer's Degree ,"+"High School"+
                                    "Doctor of Philosophy (Ph.D.) ,"+
                                    "Associate's Degree ,"+
                                    "Master's Degree ," +
                                    "Bachelor's Degree ," +
                                    "Bachelor's Degree ,";
                return this.BadRequest(errorMessage);
            }

            degree.Description = model.DegreeDescription ?? null;
            await this.Data.SaveChangesAsync();
            Education education = new Education()
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Grade = model.Grade,
                Location = model.Location,
                Users = new List<ApplicationUser>()
                {
                    currentUser
                },
                DegreeId = degree.Id,
            };
            this.Data.Educations.Add(education);
            await this.Data.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.Created);
        }

        [HttpPut]
        [Route("user/education/{id}/edit")]
        public async Task<IHttpActionResult> EditEducation(Guid id, EditEducationBindingModel model)
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
                return this.BadRequest(this.ModelState);
            }

            var educationsToCurrentUser = await this.Data.Educations.All()
                .Where(e => e.Users
                    .Any(u => u.Id == userId))
                .ToListAsync();
            if (educationsToCurrentUser.Count == 0)
            {
                return this.Unauthorized();
            }
            if (educationsToCurrentUser.All(e => e.Id != id))
            {
                return this.Unauthorized();
            }

            var result = await this.Data.Educations
                .All()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                return this.BadRequest("Invalid education id");
            }
            var degree = await this.Data.Degrees.All().FirstOrDefaultAsync(d => d.Name == model.DegreeName);
            if (degree != null)
            {
                degree.Name = model.DegreeName ?? degree.Name;
                result.Degree = degree;
            }

            result.Name = model.Name ?? result.Name;
            result.StartDate = model.StartDate ?? result.StartDate;
            result.EndDate = model.EndDate ?? result.EndDate;
            result.Grade = model.Grade ?? result.Grade;
            result.Location = model.Location ?? result.Location;

            await this.Data.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("user/education/{id}/delete")]
        public async Task<IHttpActionResult> EditEducation(Guid id)
        {
            var userId = this.User.Identity.GetUserId();
            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }

            var educationsToCurrentUser = await this.Data.Educations.All()
                .Where(e => e.Users
                    .Any(u => u.Id == userId))
                .ToListAsync();

            if (educationsToCurrentUser == null)
            {
                return this.BadRequest("Education id is incorrect or you are not allowed to delete it");
            }

            if (educationsToCurrentUser.All(e => e.Id != id))
            {
                return this.Unauthorized();
            }

            var educationToDelete = this.Data.Educations.All().FirstOrDefaultAsync(e => e.Id == id);

            this.Data.Educations.Delete(educationToDelete);
            await this.Data.SaveChangesAsync();
            return this.Ok("deleted successfully");
        }
    }
}