using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class GeneralSettings
    {
        public static bool ValuesTracingEnabled = false;
        public static bool GradientsTracingEnabled = false;
        const string LogPath = "C:\\temp\\TextWriterOutput.log";

        static GeneralSettings()
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            if (File.Exists(LogPath))
            {
                File.Delete(LogPath);
            }

            //TextWriter tw = new StreamWriter("C:\\temp\\out.txt");
            //System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.TextWriterTraceListener(tw));
            Trace.Listeners.Add(new TextWriterTraceListener(LogPath, "myListener"));
            Trace.AutoFlush = true;
        }
    }
}
