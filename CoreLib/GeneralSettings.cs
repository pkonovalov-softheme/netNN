using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class GeneralSettings
    {
        public static bool ValuesTracingEnabled = false;
        public static bool GradientsTracingEnabled = false;

        static GeneralSettings()
        {
            System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(Console.Out));
        }
    }
}
