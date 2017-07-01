using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces
{
    public interface IChannelProvider
    {
        event Action<Guid, long> OnDataRecived;
        void Send(Guid id, long value);
        void Start();
    }
}
