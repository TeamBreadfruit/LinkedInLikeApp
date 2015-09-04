namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using System.Data.Entity;
    using System.Net;
    using System.Threading.Tasks;

    using LinkedIn.Models;
    using LinkedIn.Services.Models.Companies;
    using LinkedIn.Services.UserSessionUtils;

    using Microsoft.AspNet.Identity;

    [RoutePrefix("api")]
    [SessionAuthorize]
    public class CompaniesController :BaseApiController
    {
        [Route("companies/all")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAll()
        {
            var companies = await this.Data.Companies
                .All()
                .OrderBy(c => c.Name)
                .Select(CompanyViewModel.Create).ToListAsync();

            if (!companies.Any())
            {
                return this.Ok("No companies yet.");
            }

            return this.Ok(companies);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("company/{id}")]
        public async Task<IHttpActionResult> GetById(string id)
        {
            var company = await this.Data.Companies
                .All()
                .Where(c => c.Id == new Guid(id))
                .Select(CompanyViewModel.Create).ToListAsync();

            var companyResult = company.FirstOrDefault();

            if (companyResult == null)
            {
                return this.BadRequest("Invalid company id");
            }

            return this.Ok(companyResult);
        }

        [HttpGet]
        [Route("user/companies/all")]
        public async Task<IHttpActionResult> CompaniesForCurrentUser()
        {

            var userId = this.User.Identity.GetUserId();

            var companiesForCurrentUser = await this.Data.Companies
                .All()
                .Where(c => c.OwnerId== userId)
                .Select(CompanyViewModel.Create)
                .ToListAsync();

            if (!companiesForCurrentUser.Any())
            {
                return this.Ok("No companies where found for the current user");
            }
            return this.Ok(companiesForCurrentUser);

        }

        [HttpGet]
        [Route("user/company/{id}")]
        public async Task<IHttpActionResult> CompanypForCurrentUserById(string id)
        {

            var userId = this.User.Identity.GetUserId();

            var companyForCurrentUserById = await this.Data.Companies
                .All()
                .Where(c=>c.OwnerId==userId)
                .Where(c=>c.Id==new Guid(id))
                .Select(CompanyViewModel.Create)
                .ToListAsync();

            var resultCompany = companyForCurrentUserById.FirstOrDefault();

            if (resultCompany == null)
            {
                return this.BadRequest("Company id is not correct or you are not allowed to view it.");
            }
            return this.Ok(resultCompany);

        }

        [HttpPost]
        [Route("user/company/create")]
        public async Task<IHttpActionResult> PostCompany(CompanyBindingModel model)
        {
            var userId = this.User.Identity.GetUserId();
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

            var company = new Company()
            {
                Name = model.Name,
                CreatedOn = DateTime.Now,
                Email = model.Email,
                OwnerId = userId
            };
            this.Data.Companies.Add(company);
            await this.Data.SaveChangesAsync();

            return this.StatusCode(HttpStatusCode.Created);
        }


        [HttpPut]
        [Route("user/company/{id}/edit")]
        public async Task<IHttpActionResult> EditCompany(string id, EditCompanyBindingModel model)
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

            var company =await this.Data.Companies.All()
                .Where(c=>c.Id==new Guid(id) && c.OwnerId==userId)
                .FirstOrDefaultAsync();

            if (company == null)
            {
                return this.BadRequest("Company id is not correct or you are not allowed to edit it.");
            }

            company.Email = model.Email ?? company.Email;
            company.Name = model.Name ?? company.Name;

            await this.Data.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpDelete]
        [Route("user/company/{id}/delete")]
        public async Task<IHttpActionResult> DeleteCompany(string id)
        {
            var userId = this.User.Identity.GetUserId();

            if (userId == null)
            {
                return this.BadRequest("Invalid session token.");
            }
            var companyToDelete = await this.Data.Companies.All()
                 .Where(c => c.Id == new Guid(id) && c.OwnerId == userId)
                 .FirstOrDefaultAsync();


            if (companyToDelete == null)
            {
                return this.BadRequest("Company id is not correct or you are not allowed to delete it.");
            }
            this.Data.Companies.Delete(companyToDelete);
            await this.Data.SaveChangesAsync();

            return this.Ok("deleted successfully");
        } 

    }
}