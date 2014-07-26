using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ifunction.GuidIconHttpService.Resources;

namespace ifunction.GuidIcon
{
    /// <summary>
    /// Class GuidIconHttpService.
    /// </summary>
    public class GuidIconHttpService : IDisposable
    {
        /// <summary>
        /// The locker
        /// </summary>
        protected object locker = new object();

        /// <summary>
        /// The listener
        /// </summary>
        protected HttpListener listener = new HttpListener();

        /// <summary>
        /// The is active
        /// </summary>
        protected bool isActive = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerBase" /> class.
        /// </summary>
        /// <param name="urlPrefixes">The URL prefixes.</param>
        /// <param name="authenticationSchema">The authentication schema.</param>
        /// <exception cref="InvalidObjectException">urlPrefixes</exception>
        public GuidIconHttpService(string[] urlPrefixes, AuthenticationSchemes authenticationSchema = AuthenticationSchemes.Anonymous)
        {
            // URI prefixes are required,
            // for example "http://ifunction.org:8080/index/".
            if (urlPrefixes == null || urlPrefixes.Length == 0)
            {
                throw new ArgumentNullException("urlPrefixes");
            }

            // Add the prefixes.
            foreach (string s in urlPrefixes)
            {
                listener.Prefixes.Add(s);
            }

            listener.AuthenticationSchemes = authenticationSchema;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            if (!isActive)
            {
                lock (locker)
                {
                    if (!isActive)
                    {
                        isActive = true;
                        listener.Start();
                        new Thread(new ThreadStart(delegate
                        {
                            while (isActive)
                            {
                                HttpListenerContext httpContext = listener.GetContext();

                                Thread thread = new Thread(new ParameterizedThreadStart(InvokeProcessContext));
                                thread.IsBackground = true;
                                thread.Start(httpContext);
                            }
                        })).Start();
                    }
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (isActive)
            {
                lock (locker)
                {
                    if (isActive)
                    {
                        isActive = false;
                        listener.Stop();
                    }
                }
            }
        }

        /// <summary>
        /// Processes the HTTP request.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public void ProcessHttpRequest(HttpListenerContext httpContext)
        {
            Guid? guid = null;
            int size;

            try
            {
                guid = new Guid(httpContext.Request.QueryString.Get("hash"));
            }
            catch { }

            int.TryParse(httpContext.Request.QueryString.Get("size"), out size);

            if (guid == null && size < 1)
            {
                ResponsePage(httpContext.Response);
            }
            else
            {
                if (guid == null)
                {
                    guid = Guid.NewGuid();
                }

                if (size < 32)
                {
                    size = 256;
                }

                ResponseImage(httpContext.Response, guid.Value, size);
            }
        }

        /// <summary>
        /// Responses the page.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        protected void ResponsePage(HttpListenerResponse httpResponse)
        {
            httpResponse.ContentType = "html/text";

            WriteResponse(httpResponse, Html.Index);
        }

        /// <summary>
        /// Responses the image.
        /// </summary>
        /// <param name="httpResponse">The HTTP response.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="size">The size.</param>
        protected void ResponseImage(HttpListenerResponse httpResponse, Guid guid, int size)
        {
            httpResponse.ContentType = "image/png";

            using (var bmp = GuidIconGenerator.GenerateBitmap(guid, size))
            {
                bmp.Save(httpResponse.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
                httpResponse.OutputStream.Flush();
            }
        }

        /// <summary>
        /// Processes the exception.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <param name="exception">The exception.</param>
        protected void ProcessException(HttpListenerContext httpContext, int code, string content)
        {
            httpContext.Response.StatusCode = code;
            WriteResponse(httpContext.Response, content);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (isActive)
            {
                Stop();
                listener.Close();
            }
        }

        /// <summary>
        /// Invokes the process context.
        /// </summary>
        /// <param name="obj">The object.</param>
        protected void InvokeProcessContext(object obj)
        {
            HttpListenerContext httpContext = obj as HttpListenerContext;
            int responseCode = 200;
            string errorMessage = string.Empty;

            if (httpContext != null)
            {
                try
                {
                    this.ProcessHttpRequest(httpContext);
                }
                catch (Exception ex)
                {
                    this.ProcessException(httpContext, responseCode, errorMessage);
                }
            }
        }

        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="content">The content.</param>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="encoding">The encoding.</param>
        protected static void WriteResponse(HttpListenerResponse response, string content, string contentType = "text/html", Encoding encoding = null)
        {
            if (response != null)
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                response.ContentType = contentType;

                using (StreamWriter writer = new StreamWriter(response.OutputStream))
                {
                    writer.WriteLine(content ?? string.Empty);
                }
            }
        }
    }
}
