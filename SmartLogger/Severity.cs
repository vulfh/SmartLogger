using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogger
{
    public enum Severity : byte
    {
        DEBUG = 0,
        INFORMATION = 1,
        WARNING = 2,
        ERROR = 3,
        FATAL = 4
    }
}
