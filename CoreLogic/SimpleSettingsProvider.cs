using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Interfaces;

namespace CoreLogic
{
    public class SimpleSettingsProvider:ISettingsProvider
    {
        public object GetSetting(string name)
        {
            var setting = ConfigurationManager.AppSettings[name];
            if (setting == null)
            {
                Logger.Log.Fatal($"setting {name} doesn't exist");
                throw new NullReferenceException();
            }
            return setting;
        }
    }
}