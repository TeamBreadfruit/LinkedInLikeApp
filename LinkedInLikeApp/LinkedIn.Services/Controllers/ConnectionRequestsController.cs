namespace LinkedIn.Services.Controllers
{
    using System.Web.Http;

    using LinkedIn.Services.UserSessionUtils;

    [SessionAuthorize]
    [RoutePrefix("api/users/{id}/ConnectionRequests")]
    public class ConnectionRequestsController : BaseApiController
    {

    }
}
