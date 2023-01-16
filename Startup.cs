using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobOffers.Startup))]
namespace JobOffers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
