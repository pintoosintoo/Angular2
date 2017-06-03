using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ang2Demo.Startup))]
namespace Ang2Demo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
