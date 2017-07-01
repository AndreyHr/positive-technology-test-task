 
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace WebApiProvider
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {
            
                var config = new HttpConfiguration();
                config.Services.Replace(typeof(IAssembliesResolver), new AssembliesResolver());
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                app.UseWebApi(config);
            
        }
    }
}
