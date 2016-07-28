using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Loyung.Startup))]
namespace Loyung
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
