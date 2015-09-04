﻿namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Linq;
    using System.Net;

    using LinkedIn.Models;
    using LinkedIn.Services.UserSessionUtils;
    using LinkedIn.Services.Models.Educations;

    using Microsoft.AspNet.Identity;


    [SessionAuthorize]
    [RoutePrefix("api")]
    public class EducationsController:BaseApiController
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

            var result =await this.Data.Educations.All()
                .Where(e => e.Users.Any(u => u.Id == userId))
                .OrderBy(e=>e.StartDate)
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
                .Where(e => e.Users.Any(u => u.Id == userId) && e.Id ==new Guid(id))
                .Select(EducationViewModel.Create).ToListAsync();
            if (result.Count==0)
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
                return this.BadRequest(ModelState);
            }
            var degree =await this.Data.Degrees.All().FirstOrDefaultAsync(d=>d.Id==model.DegreeId) ;
            if (degree == null)
            {
                return this.BadRequest("Invalid degree id");
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
                DegreeId = model.DegreeId,
            };
            this.Data.Educations.Add(education);
            await this.Data.SaveChangesAsync();
            return StatusCode(HttpStatusCode.Created);
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
                return this.BadRequest(ModelState);
            }

            var educationsToCurrentUser =await this.Data.Educations.All()
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
            var result =await this.Data.Educations
                .All()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (result == null)
            {
                return this.BadRequest("Invalid education id");
            }
            var degree =await this.Data.Degrees.All().FirstOrDefaultAsync(d => d.Id == model.DegreeId);
            if (degree != null)
            {
                degree.Description = model.DegreeDescription ?? null;
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

            var educationsToCurrentUser =await this.Data.Educations.All()
                .Where(e => e.Users
                    .Any(u => u.Id == userId))
                .ToListAsync();

            if (educationsToCurrentUser == null)
            {
                return BadRequest("Education id is incorrect or you are not allowed to delete it");
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