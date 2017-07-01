using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic
{
    public class Calculator
    {
        static ConcurrentDictionary<Guid, List<long>> storage = new ConcurrentDictionary<Guid, List<long>>();
        public static long GetNextDigit(Guid _id,long current) {
            long newVal = 1;
            if (storage.ContainsKey(_id))
            {
                var list = storage[_id];
                if (current > 0)
                { 
                     newVal = checked(list[list.Count - 1] + current);
                }
            }
            else
            {
                storage[_id] = new List<long>();
            }

            storage[_id].Add(current);
            storage[_id].Add(newVal);
            return newVal;
        }
    }
}
