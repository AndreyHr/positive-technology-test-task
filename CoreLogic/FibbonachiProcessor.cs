using System;
using System.ComponentModel;
using System.Linq;
using Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace CoreLogic
{
    public class FibbonachiProcessor
    {
        private readonly IChannelProvider _input;
        private readonly IChannelProvider _output;

        public FibbonachiProcessor(IChannelProvider input,IChannelProvider output)
        {
            _input = input;
            _output = output;
        }

        public  void Start(bool isNeedStart=false,int number=0)
        {
            Action<Guid, long> send = (x, y) => {
                Console.Write(y + "  ");
                _output.Send(x, y);
            };
            _input.OnDataRecived += (_id, val) =>
            {
                long next = 0;
                try
                {
                    next = Calculator.GetNextDigit(_id, val);
                }
                catch (OverflowException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Overflow!! .This version have a limited calculation. Please, buy a professional version");
                    return;
                }               
                Console.Write(val + "  ");
                
                Thread.Sleep(500);
                send(_id, next);
            };
            _input.Start();//StartListening
            if (isNeedStart)
                Enumerable.Range(0, number)
                    .ToList()
                    .ForEach(x=>
                        Task.Run(()=> 
                            send(Guid.NewGuid(), 0))
                        );
               
            
        }
    }
}