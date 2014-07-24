using System;
using System.Collections.Generic;
using System.Web;

namespace ifunction.GuidIcon.Web
{
    /// <summary>
    /// Summary description for gicon
    /// </summary>
    public class gicon : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}