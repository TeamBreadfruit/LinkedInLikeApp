using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using LinkedIn.Services.Models.Educations;
using Microsoft.AspNet.Identity;

namespace LinkedIn.Services.Controllers
{
    [RoutePrefix("api")]
    public class DegreesController :BaseApiController
    {
        [Route("degrees/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllDegrees()
        {

            var degrees =await this.Data.Degrees.All().Select(d => new DegreeViewModelWithId
            {
                Id = d.Id,
                Name = d.Name
            }).ToListAsync();

            return this.Ok(degrees);
        }
    }
}