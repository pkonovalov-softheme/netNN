using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using CoreLib;
using CoreLib.Layers;

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


            const int passCount = 10000;
            double targetY = 5;

            InputLayer inputLayer = new InputLayer(1);
            AffineLayer affineLayer = new AffineLayer(1, new ReLU());
            inputLayer.Values[0, 0] = 0.01; //Current value
            affineLayer.Weights[0, 0] = 0.02; 

            // OutputLayer outputLayer = new OutputLayer(1);

            for (int i = 0; i < passCount; i++)
            {
                affineLayer.ForwardPass(inputLayer);

                double dif = affineLayer.Values[0, 0] - targetY;
                affineLayer.Gradients[0, 0] = dif;

                if (i%1000 == 0)
                {
                    Console.WriteLine(dif);
                }

                affineLayer.BackwardPass();

            }
        }
    }
}
