using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AutomobilMng.Startup))]
namespace AutomobilMng
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
