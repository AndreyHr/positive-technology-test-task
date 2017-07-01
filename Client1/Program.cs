using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using CoreLogic;
using Interfaces;
using Newtonsoft.Json;
using StructureMap;
using WebApiProvider;


namespace Client1
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(c => {
                c.For<ISettingsProvider>().Add<SimpleSettingsProvider>();
                c.For<IChannelProvider>().Add<MassTransitProvider.MassTransitProvider>().Named("input");
                c.For<IChannelProvider>().Add<RestProvider>().Named("output");
            });
            
          
            var processor = new FibbonachiProcessor(container.GetInstance<IChannelProvider>("input"),container.GetInstance<IChannelProvider>("output"));
            Console.WriteLine("Enter number of calculations");
            int number = 0;
            while (!int.TryParse( Console.ReadLine(),out number))
            {
                Console.WriteLine("Wrong output");
            }
            try
            {
                processor.Start(true,number);  
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
