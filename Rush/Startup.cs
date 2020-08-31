using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rush.Startup))]
namespace Rush

{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
