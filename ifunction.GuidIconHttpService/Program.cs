using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ifunction.GuidIconHttpService.Properties;

namespace ifunction.GuidIcon
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

#if DEBUG
            List<string> uriList = new List<string>();

            foreach (var one in Settings.Default.UriPrefix)
            {
                uriList.Add(one);
            }

            var httpServer = new GuidIconHttpService(uriList.ToArray());
            httpServer.Start();

            Console.WriteLine("CoreCloudService has been launched.");

            while (true)
            {
                Console.WriteLine(string.Format("{0} Working...", DateTime.Now.ToString()));
                Thread.Sleep(1000);
            }
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new GuidIconHttpServiceHost() 
            };
            ServiceBase.Run(ServicesToRun);


#endif
        }
    }
}
