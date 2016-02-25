using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Interview.Startup))]
namespace Interview
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
