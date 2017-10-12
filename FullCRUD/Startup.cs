using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FullCRUD.Startup))]
namespace FullCRUD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
