namespace LinkedIn.Services.Controllers
{
    using System.Web.Http;

    using LinkedIn.Data;
    using LinkedIn.Data.Contracts;

    using Microsoft.AspNet.Identity;

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

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}