using Microsoft.Owin;

[assembly: OwinStartup(typeof(LinkedIn.Services.Startup))]

namespace LinkedIn.Services
{
    using System.Threading.Tasks;
    using System.Web.Cors;
    using System.Web.Http;

    using Owin;
    using Microsoft.Owin;

    using Microsoft.Owin.Cors;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(new CorsOptions()
            {
                PolicyProvider = new CorsPolicyProvider()
                {
                    PolicyResolver = request =>
                    {
                        if (request.Path.StartsWithSegments(new PathString(TokenEndpointPath)))
                        {
                            return Task.FromResult(new CorsPolicy { AllowAnyOrigin = true });
                        }

                        return Task.FromResult<CorsPolicy>(null);
                    }
                }
            });

            this.ConfigureAuth(app);

            GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }
    }
}
