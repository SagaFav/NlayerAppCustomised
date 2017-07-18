using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Services.App_Start.SignalRInit))]
namespace Services.App_Start
{
    public class SignalRInit
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}