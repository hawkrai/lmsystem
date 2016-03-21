using Microsoft.Owin;
using Owin;
using LMPlatform.UI;

namespace LMPlatform.UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}