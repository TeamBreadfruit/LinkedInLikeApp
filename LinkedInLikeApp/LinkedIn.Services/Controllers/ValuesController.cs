namespace LinkedIn.Services.Controllers
{
    using System.Linq;
    using System.Web.Http;

    public class ValuesController : BaseApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            return this.Ok(this.Data.Degrees.All().Count());
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
