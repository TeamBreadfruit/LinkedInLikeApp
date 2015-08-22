namespace LinkedIn.Services.Controllers
{
    using System.Web.Http;

    using LinkedIn.Data;
    using LinkedIn.Data.Contracts;

    public class BaseApiController : ApiController
    {
        public BaseApiController()
            : this(new LinkedInData())
        {
        }

        public BaseApiController(ILinkedInData data)
        {
            this.Data = data;
        }

        protected ILinkedInData Data { get; private set; }
    }
}