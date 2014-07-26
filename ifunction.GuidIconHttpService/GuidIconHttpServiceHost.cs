using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ifunction.GuidIconHttpService.Properties;

namespace ifunction.GuidIcon
{
    /// <summary>
    /// Class GuidIconHttpServiceHost.
    /// </summary>
    public partial class GuidIconHttpServiceHost : ServiceBase
    {
        /// <summary>
        /// The HTTP server
        /// </summary>
        static GuidIconHttpService httpServer;

        /// <summary>
        /// The locker
        /// </summary>
        static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidIconHttpServiceHost"/> class.
        /// </summary>
        public GuidIconHttpServiceHost()
            : base()
        {
            InitializeComponent();

            if (httpServer == null)
            {
                lock (locker)
                {
                    if (httpServer == null)
                    {
                        List<string> uriList = new List<string>();

                        foreach (var one in Settings.Default.UriPrefix)
                        {
                            uriList.Add(one);
                        }

                        httpServer = new GuidIconHttpService(uriList.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        /// <exception cref="InitializationFailureException">HttpServiceBase</exception>
        protected override void OnStart(string[] args)
        {
            try
            {
                httpServer.Start();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("GuidIconHttpServiceHost", ex);
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        /// <exception cref="OperationFailureException">OnStop</exception>
        protected override void OnStop()
        {
            try
            {
                httpServer.Stop();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("GuidIconHttpServiceHost", ex);
            }
        }
    }
}
