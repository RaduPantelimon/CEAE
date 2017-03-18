using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CEAE.Startup))]
namespace CEAE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
