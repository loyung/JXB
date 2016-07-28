using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JXB.Startup))]
namespace JXB
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
