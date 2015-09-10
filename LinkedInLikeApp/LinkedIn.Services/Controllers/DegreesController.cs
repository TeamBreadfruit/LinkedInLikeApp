namespace LinkedIn.Services.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Data.Entity;

    using LinkedIn.Services.Models.Educations;

    [RoutePrefix("api")]
    public class DegreesController :BaseApiController
    {
        [Route("degrees/all")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllDegrees()
        {
            var degrees =await this.Data.Degrees.All().Select(d => new DegreeViewModel()
            {
                Name = d.Name
            }).ToListAsync();
            return this.Ok(degrees);
        }
    }
}