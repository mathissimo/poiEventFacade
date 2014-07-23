using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace PoiEventNetwork.Data.Tools
{
    public static class ExceptionHelper
    {
        public static void ThrowArgumentException(Logger logger, string msg, System.Exception ex)
        {
            if (string.IsNullOrWhiteSpace(msg))
                msg = "Exception occured.";

            if(logger != null)
                logger.WarnException(msg, ex);

            throw ex;
        }
    }
}
