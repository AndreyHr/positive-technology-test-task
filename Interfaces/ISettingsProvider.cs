using System.Collections.Generic;

namespace Interfaces
{
    public interface ISettingsProvider
    {
        object GetSetting(string name);
    }
}