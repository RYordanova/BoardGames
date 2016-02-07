using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoardGames.Web.Startup))]
namespace BoardGames.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
