using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interface
{
    interface ICalculateProvider
    {
        event Action<Guid, int> OnDataRecived;
        void Send(Guid id, int value);
    }
}
