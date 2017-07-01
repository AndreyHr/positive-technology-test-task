using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core
{
    public class SimpleProcessProvider : ICalculateProvider
    {
        private Guid _id;
        public SimpleProcessProvider() {
            _id = Guid.NewGuid();
            Task.Run(() => {
                while (true)
                {
                    var newValues = container.Where(x => x.Key != _id);
                    newValues.ToList().ForEach(x=> {
                        OnDataRecived?.Invoke(x.Value.Item1, x.Value.Item2);
                        container.Remove(x.Key);
                    });
                    Thread.Sleep(1000);
                }

            });


        }
        static Dictionary<Guid, Tuple<Guid, int>> container = new Dictionary<Guid, Tuple<Guid, int>>();
        public event Action<Guid, int> OnDataRecived;

        public void Send(Guid id, int value)
        {
            container.Add(_id,new Tuple<Guid, int>(id,value));
        }
    }
}
