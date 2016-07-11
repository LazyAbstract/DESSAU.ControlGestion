using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DESSAU.ControlGestion.Web.Startup))]
namespace DESSAU.ControlGestion.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
