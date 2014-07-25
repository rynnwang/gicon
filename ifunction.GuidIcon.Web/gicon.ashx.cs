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
            int width;
            context.Response.ContentType = "image/png";

            var input = context.Request.QueryString.Get("guid");
            int.TryParse(context.Request.QueryString.Get("width"), out width);

            try
            {
                guid = new Guid(input);
            }
            catch { }

            if (guid == null)
            {
                guid = Guid.NewGuid();
            }

            if (width < 64)
            {
                width = 64;
            }

            using (var bmp = GuidIconGenerator.GenerateBitmap(guid.Value, width))
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