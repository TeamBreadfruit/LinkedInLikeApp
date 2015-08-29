namespace LinkedIn.Services.UserSessionUtils
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    
    using LinkedIn.Data;
    using LinkedIn.Data.Contracts;

    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public SessionAuthorizeAttribute(ILinkedInData data)
        {
            this.Data = data;
        }

        public SessionAuthorizeAttribute() 
            : this(new LinkedInData())
        {
        }

        protected ILinkedInData Data { get; private set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            var requestProperties = actionContext.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(requestProperties);
            if (userSessionManager.ReValidateSession())
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Session token expried or not valid.");
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}