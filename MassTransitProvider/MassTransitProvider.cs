
using MassTransit;
using MassTransit.Context;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreLogic;
using GreenPipes;
using Interfaces;
using MassTransit.Log4NetIntegration;
using Newtonsoft.Json;

namespace MassTransitProvider
{
    public class Message
    {
        public long Val { get; set; } 
        public Guid Id { get; set; }
    }
    public class MassTransitProvider : IChannelProvider
    {
        private readonly ISettingsProvider _settings;

        private IBusControl _bus;
      
        private Guid _id;
        private string _queue;
        private string _host;
        public MassTransitProvider(ISettingsProvider settings)
        {
            _settings = settings;
        }

        public event Action<Guid, long> OnDataRecived;
      
        public async void Send(Guid id, long value)
        {
            if(_bus==null)  CreateBus();
            try
            {
                var endpoint = await _bus.GetSendEndpoint(new Uri(_host + _queue));
                await endpoint.Send<Message>(new Message() {Id = id, Val = value});
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Cant send message to  queue: {e.Message}");
                Console.ResetColor();
                Logger.Log.Error(JsonConvert.SerializeObject(e));
            }

        }

        public void Start()
        {
            CreateBus(true);
        }

        private void CreateBus(bool isNeedReciaver=false)
        {
            _queue = _settings.GetSetting("queue_name")?.ToString();
            _host = _settings.GetSetting("queue_host")?.ToString();
            var username=_settings.GetSetting("queue_user").ToString();
            var pass=_settings.GetSetting("queue_pass").ToString();
            try
            {
                _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(_host), h =>
                    {
                        h.Username(username);
                        h.Password(pass);
                    });
                    cfg.UseLog4Net();
                    if (isNeedReciaver)
                        cfg.ReceiveEndpoint(host, _queue, e =>
                        {
                            e.PurgeOnStartup = true;
                            e.Handler<Message>(context =>
                            {
                                return Task.Run(() => OnDataRecived?.Invoke(context.Message.Id, context.Message.Val));
                            });

                        });
                     
                });
                _bus.Start();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Cant start queue {e.Message}");
                Console.ResetColor();
                Logger.Log.Error(JsonConvert.SerializeObject(e));
            }
            
        }
    }
}
