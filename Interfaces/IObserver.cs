using System;

namespace Interfaces
{
    
    public interface IObserver {
         void Invoke(Guid id, long val);
         void SetAction(Action<Guid, long> act);
    }
}