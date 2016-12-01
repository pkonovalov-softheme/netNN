using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
    public static class Utils
    {
        public static void DebbuggerBreak()
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        public static bool NearlyEqual(double a, double b, double epsilon)
        {
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || diff < Double.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

        public static double GetRandomNumber(Random random, double minimum, double maximum)
        {
            return random.NextDouble() * (maximum - minimum) + minimum;
        }


        public static List<double> CsvToDoubles(string path)
        {
            //IEnumerable<string[]> lines = File.ReadAllLines(path).Select(a => a.Split(';'));
            //IEnumerable<string[]> csv = from line in lines
            //          select (line[0].Split(',')).ToArray();


            string str = File.ReadAllLines(path).First();
            str = str.Replace("[", "").Replace("]", "").Replace("\"", "");
            List<string> res = str.Split(',').ToList();
            return res.Select(Convert.ToDouble).ToList();
        }

        public static void InvokePythonScript(string path, string[] args)
        {
            // full path of python interpreter 
            string python = @"I:\Anaconda3\python.exe";

            // python app to call 
            string myPythonApp = path;


            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;
            myProcessStartInfo.RedirectStandardError = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + String.Join(" ", args);

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;

            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            string stderrx = myProcess.StandardError.ReadToEnd();
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */

            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
           
            // write the output we got from python app 
            Debug.WriteLine("Value received from script: " + myString);
            Debug.WriteLine("Exit code : {0}", myProcess.ExitCode);
            Debug.WriteLine("Stderr : {0}", stderrx);

            if (myProcess.ExitCode != 0)
            {
                throw new InvalidOperationException(stderrx);
            }

            myProcess.Close();
        }
    }
}
