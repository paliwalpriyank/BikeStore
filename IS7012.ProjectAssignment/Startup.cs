using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IS7012.ProjectAssignment.Startup))]
namespace IS7012.ProjectAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
