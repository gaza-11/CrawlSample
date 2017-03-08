using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DisplayWebSite.Startup))]
namespace DisplayWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
