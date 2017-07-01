using Client1;
using Microsoft.Owin.Hosting;
using Owin;
using RestSharp;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using CoreLogic;
using Interfaces;
using Microsoft.Owin.Logging;
using Newtonsoft.Json;
using WebApiProvider;

namespace WebApiProvider
{
    public class Observe : IObserver
    {
        Action<Guid, long> action;
        public void Invoke(Guid id, long val)
        {
            action?.Invoke(id, val);
        }
        public void SetAction(Action<Guid, long> act)
        {
            action = act;
        }

    }


    public class RestProvider : IChannelProvider
    {
        private string _host;
        public void Send(Guid id,long val) {
            var client = new RestClient(_host);
            var request = new RestRequest("api/home/GetNextValue/", Method.GET);
            request.AddParameter("id", id);
            request.AddParameter("current", val);
            client.ExecuteAsync(request,response=> {
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Console.Write($"something happens: {response.ErrorMessage} ${response.StatusCode}");
                    Logger.Log.Error($"sending request error:{JsonConvert.SerializeObject(response)}");
                }
                    

            });
        }

       

        private IDisposable _server;
        private IContainer _container;
        public void Start() {
            try
            {
                _server=WebApp.Start(_host, app =>
                {
                    var config = new HttpConfiguration();
                    config.Routes.MapHttpRoute(
                        name: "DefaultApi",
                        routeTemplate: "api/{controller}/{id}",
                        defaults: new { id = RouteParameter.Optional }
                    );

                    var observer = new Observe();
                    observer.SetAction((id,val)=>OnDataRecived?.Invoke(id, val));
                    _container.Configure(x=>x.For<IObserver>().Use(observer));

                    config.DependencyResolver = new DependencyResolver(_container);
                   
                    app.UseWebApi(config);
                });
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Cannot start webserver {e.Message}"); 
                Console.ResetColor();
           
                Logger.Log.Fatal($"Error when starting server {e.Message}");
                Logger.Log.Fatal(JsonConvert.SerializeObject(e));
            }
           
        }
        public RestProvider(ISettingsProvider settings,IContainer container)
        {
            _container = container;
            _host = settings.GetSetting("web_api").ToString();  
        }
        public event Action<Guid, long> OnDataRecived;

       
    }
 
}
