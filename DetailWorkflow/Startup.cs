using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DetailWorkflow.Startup))]
namespace DetailWorkflow
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
