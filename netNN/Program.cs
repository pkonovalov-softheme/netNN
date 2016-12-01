using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.CostsFunctions;
using CoreLib.Layers;
using Python.Runtime;

namespace netNN
{
    class Program
    {
        static void Main(string[] args)
        {
            // ConnectionOptions options = new ConnectionOptions();
            // options.Username = @"newdom\administrator";
            // options.Password = "123asdQ!";
            //// options.Authority = "ntlmdomain:newdom";

            // ManagementScope scope = new ManagementScope("\\\\cnode2\\root\\cimv2", options);
            // scope.Options.EnablePrivileges = true;
            // scope.Connect();

            // //Query system for Operating System information
            // ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            // ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            // ManagementObjectCollection queryCollection = searcher.Get();
            // foreach (ManagementObject m in queryCollection)
            // {
            //     // Display the remote computer information
            //     Console.WriteLine("Computer Name     : {0}", m["csname"]);
            //     Console.WriteLine("Windows Directory : {0}", m["WindowsDirectory"]);
            //     Console.WriteLine("Operating System  : {0}", m["Caption"]);
            //     Console.WriteLine("Version           : {0}", m["Version"]);
            //     Console.WriteLine("Manufacturer      : {0}", m["Manufacturer"]);
            // }

            //using (Py.GIL())
            //{
            //    dynamic np = Py.Import("numpy");
            //    dynamic sin = np.sin;
            //    Console.WriteLine(np.cos(np.pi * 2));
            //    Console.WriteLine(sin(5));
            //    double c = np.cos(5) + sin(5);
            //    Console.WriteLine(c);
            //    /* this block is temporarily disabled due to regression
            //    dynamic a = np.array(new List<float> { 1, 2, 3 });
            //    dynamic b = np.array(new List<float> { 6, 5, 4 }, Py.kw("dtype", np.int32));
            //    Console.WriteLine(a.dtype);
            //    Console.WriteLine(b.dtype);
            //    Console.WriteLine(a * b);
            //    */
            //    Console.ReadKey();
            //}

            //const int passCount = 10000;
            //double targetY = 5;


           // Func<double, double> y = x => 3.3*x + 1.4;

            Model model = new Model(1, ActivationType.ReLU, 1, ActivationType.ReLU, CostType.Square);
            //model.FirstInputValue = initValue;
            Random rnd = new Random();

            model.InitWithRandomWeights(rnd);


            for (int i = 0; i < 200; i++)
            {
                double x = rnd.NextDouble();
                model.FirstInputValue = x;

                double y = 3.3*x + 1.4;
                Matrix target = new Matrix(y);

                model.ForwardPass(target);
                model.BackwardPass(target);

                double w1 = model[0].Weights.Primal[0, 0]; 
                double b1 = model[0].Biases.Primal[0, 0];
                
                double w2 = model[1].Weights.Primal[0, 0];
                double b2 = model[1].Biases.Primal[0, 0];

                if (i%50 == 0)
                {
                    Console.WriteLine("w1: {0} b1: {1} w2:{2} b2:{3}", w1, b1, w2, b2);
                }
            }

            double x_test = 1.3;
            model.FirstInputValue = 1.3;
            double y_test = 3.3 * x_test + 1.4;

            model.ForwardPass(new Matrix(y_test));
            double output = model.FirstOutputValue;
        }
    }
}
