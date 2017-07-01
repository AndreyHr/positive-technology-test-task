using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransitProvider;
using CoreLogic;
using Client1;
using WebApiProvider;
using System.Threading;
using Interfaces;
using Newtonsoft.Json;
using StructureMap;

namespace Client2
{//Переполнение? хранить ли все числа ?
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(c => {
                c.For<ISettingsProvider>().Add<SimpleSettingsProvider>();
                c.For<IChannelProvider>().Add<MassTransitProvider.MassTransitProvider>().Named("output");
                c.For<IChannelProvider>().Add<RestProvider>().Named("input");
            });
            
          
            var processor = new FibbonachiProcessor(container.GetInstance<IChannelProvider>("input"),container.GetInstance<IChannelProvider>("output"));
            
            try
            {
                processor.Start();  
            }
            catch (Exception e)
            {
                Logger.Log.Fatal(JsonConvert.SerializeObject(e));
                throw;
            } 
            Console.Read();

        }
    }
}
