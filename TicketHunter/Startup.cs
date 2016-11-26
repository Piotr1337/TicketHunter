using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TicketHunter.Startup))]
namespace TicketHunter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
