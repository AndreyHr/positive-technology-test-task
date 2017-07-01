using log4net;
using log4net.Config;

namespace CoreLogic
{
    public static class Logger
    {
        public static ILog Log = LogManager.GetLogger("LOGGER");
    }
}