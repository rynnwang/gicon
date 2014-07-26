using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
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
            Guid? guid = null;
            int size;
            context.Response.ContentType = "image/png";

            var hash = context.Request.QueryString.Get("hash");
            int.TryParse(context.Request.QueryString.Get("size"), out size);

            try
            {
                guid = new Guid(hash);
            }
            catch { }

            if (guid == null)
            {
                guid = Guid.NewGuid();
            }

            if (size < 64)
            {
                size = 64;
            }

            using (var bmp = GuidIconGenerator.GenerateBitmap(guid.Value, size))
            {
                bmp.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Png);
            }
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