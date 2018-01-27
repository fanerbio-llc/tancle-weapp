﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TancleClient.Utility
{
    public class LogHelper
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private LogHelper() { }

        public static log4net.ILog Instance => Log;
    }
}
