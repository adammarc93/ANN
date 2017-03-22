using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ANN.Startup))]
namespace ANN
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
