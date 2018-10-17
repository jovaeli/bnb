using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeautiesAndBarbers.Startup))]
namespace BeautiesAndBarbers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
