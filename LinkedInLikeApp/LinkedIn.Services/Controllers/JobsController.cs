namespace LinkedIn.Services.Controllers
{
    using LinkedIn.Services.UserSessionUtils;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    using LinkedIn.Services.Models.Jobs;

    using Microsoft.AspNet.Identity;
    using LinkedIn.Models;

    [SessionAuthorize]
    [RoutePrefix("api")]
    public class JobsController : BaseApiController
    {
        private const int PageSize = 20;

        [HttpGet]
        [AllowAnonymous]
        [Route("job/{search}/{isIdSearch?}")]
        public async Task<IHttpActionResult> GetJob(string search, bool isIdSearch = true)
        {
            var jobQuerry = this.Data.Jobs.All();
            jobQuerry = isIdSearch ? jobQuerry.Where(j => j.Id == new Guid(search)) : jobQuerry.Where(j => j.Name == search);

            var job = await jobQuerry.Select(JobViewModel.DataModel).ToListAsync();

            return this.Ok(job);
        }

        [HttpPost]
        [Route("user/job/create")]
        public async Task<IHttpActionResult> PostUserJob(AddUserJobBindingModel job)
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

            Guid jobId = await getJobId(job);

            this.Data.UserJobs.Add(new UserJob()
            {
                JobId = jobId,
                UserId = userId,
                Description = job.Description,
                StartDate = job.StartDate,
                EndDate = job.EndDate
            });

            await this.Data.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.Created);
        }

        [HttpPost]
        [Route("company/{id}/job/create")]
        public async Task<IHttpActionResult> PostCompanyJob(string id, JobBindingModel job)
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

            var userCompany = await this.Data.Companies.All().FirstOrDefaultAsync(c => c.OwnerId == userId && c.Id == new Guid(id));
            if (userCompany == null)
            {
                return BadRequest("You're not the owner of this company or the company does not exist");
            }

            if (userCompany.Jobs.Any(j => j.Name == job.Name))
            {
                return BadRequest("That job already exist for that company");
            }

            this.Data.Jobs.Add(new Job()
            {
                Name = job.Name,
                Company = userCompany
            });

            await this.Data.SaveChangesAsync();
            return this.StatusCode(HttpStatusCode.Created);
        }

        [HttpDelete]
        [Route("company/{id}/job/{jobId}/delete")]
        public async Task<IHttpActionResult> DeleteCompanyJob(string id, string jobId)
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

            var userCompany = await this.Data.Companies.All().FirstOrDefaultAsync(c => c.OwnerId == userId && c.Id == new Guid(id));
            if (userCompany == null)
            {
                return BadRequest("You're not the owner of this company or the company does not exist");
            }

            var job = userCompany.Jobs.FirstOrDefault(j => j.Id == new Guid(jobId));
            if (job == null)
            {
                return BadRequest("Wrong job Id");
            }

            if (job.Users.Any())
            {
                return BadRequest("Can't delete this job because there are users connected to it");
            }

            this.Data.Jobs.Delete(job);
            await this.Data.SaveChangesAsync();

            return Ok(job);
        }

        [HttpPut]
        [Route("user-job/{id}/edit")]
        public async Task<IHttpActionResult> PutUserJob(string id, AddUserJobBindingModel job)
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

            var userJob = await this.Data.UserJobs.All().FirstOrDefaultAsync(uj => uj.Id == new Guid(id) && uj.UserId == userId);
            if (userJob == null)
            {
                return NotFound();
            }

            Guid newJobId = await getJobId(job);

            var oldJobId = userJob.JobId;

            userJob.JobId = newJobId;
            userJob.StartDate = job.StartDate;
            userJob.EndDate = job.EndDate;
            userJob.Description = job.Description;
            await this.Data.SaveChangesAsync();

            await removeUnusedJobAndCompany(oldJobId);

            return this.StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("user-job/{id}/delete")]
        public async Task<IHttpActionResult> DeleteUserJob(string id)
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

            var userJob = await this.Data.UserJobs.All().FirstOrDefaultAsync(j => j.Id == new Guid(id) && j.UserId == userId);
            if (userJob == null)
            {
                return NotFound();
            }

            var oldJobId = userJob.JobId;
            this.Data.UserJobs.Delete(userJob);
            await this.Data.SaveChangesAsync();

            await removeUnusedJobAndCompany(oldJobId);

            return Ok(userJob);
        }

        private async Task<Guid> getJobId(AddUserJobBindingModel job)
        {
            var existJob = await this.Data.Jobs.All().FirstOrDefaultAsync(j => j.Name == job.Name && j.Company.Name == job.CompanyName);
            Guid newJobId;
            if (existJob == null)
            {
                var existCompany = await this.Data.Companies.All().FirstOrDefaultAsync(c => c.Name == job.CompanyName);
                Guid newCompanyId;
                if (existCompany == null)
                {
                    var newCompany = new Company()
                    {
                        Name = job.CompanyName,
                        CreatedOn = DateTime.Now
                    };

                    this.Data.Companies.Add(newCompany);
                    await this.Data.SaveChangesAsync();
                    newCompanyId = newCompany.Id;
                }
                else
                {
                    newCompanyId = existCompany.Id;
                }

                var newJob = new Job()
                {
                    Name = job.Name,
                    CompanyId = newCompanyId
                };

                this.Data.Jobs.Add(newJob);
                await this.Data.SaveChangesAsync();
                newJobId = newJob.Id;
            }
            else
            {
                newJobId = existJob.Id;
            }
            return newJobId;
        }

        private async Task removeUnusedJobAndCompany(Guid oldJobId)
        {
            var oldJob = await this.Data.Jobs.All().FirstOrDefaultAsync(j => j.Id == oldJobId);
            if (!oldJob.Users.Any() && oldJob.Company.OwnerId == null)
            {
                var oldCompany = oldJob.Company;
                this.Data.Jobs.Delete(oldJob);
                await this.Data.SaveChangesAsync();
                if (!oldCompany.Jobs.Any())
                {
                    this.Data.Companies.Delete(oldCompany);
                    await this.Data.SaveChangesAsync();
                }
            }
        }
    }
}
