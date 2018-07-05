using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OopRestaurant201807.Startup))]
namespace OopRestaurant201807
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
