using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jornalero.web.Startup))]
namespace Jornalero.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
