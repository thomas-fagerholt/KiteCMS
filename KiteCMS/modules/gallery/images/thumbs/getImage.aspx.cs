using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for pdf.
	/// </summary>
	public class ThumbImages : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
		}
		protected override void Render(HtmlTextWriter output) 
		{
			string url = null;
			int width = 45;
			int height = 45;
			int maxSize = -1;
            int maxHeight = -1;
            int maxWidth = -1;

			if (HttpContext.Current.Request["imageurl"] != null)
				url = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["imageurl"].ToString());

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["maxSize"]))
                maxSize = int.Parse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["maxSize"].ToString()));

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["maxHeight"]))
                maxHeight = int.Parse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["maxHeight"].ToString()));

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["maxWidth"]))
                maxWidth = int.Parse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["maxWidth"].ToString()));

            if (url != null && File.Exists(Server.MapPath(url)))
			{
                System.Drawing.Image oThumbNail;
                System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(url));

                width = oImg.Width;
                height = oImg.Height;

                if (maxSize != -1)
                {
                    width = oImg.Width;
                    height = oImg.Height;

                    if (height > maxSize)
                    {
                        width = width * maxSize / height;
                        height = maxSize;
                    }
                    if (width > maxSize)
                    {
                        height = height * maxSize / width;
                        width = maxSize;
                    }
                }

                if (maxHeight != -1)
                {
                    if (height > maxHeight)
                    {
                        width = width * maxHeight / height;
                        height = maxHeight;
                    }
                }

                if (maxWidth != -1)
                {
                    if (width > maxWidth)
                    {
                        height = height * maxWidth / width;
                        width = maxWidth;
                    }
                }
                if (width == 0)
                    width = 1;
                if (height == 0)
                    height = 1;

                if (width != -1 && height != -1)
                {
                    oThumbNail = new Bitmap(width, height);
                    System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(oThumbNail);
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                    System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, height);
                    gr.DrawImage(oImg, rectDestination, 0, 0, oImg.Width, oImg.Height, GraphicsUnit.Pixel);
                }
                else
                    oThumbNail = new Bitmap(oImg);

                oImg.Dispose();

                Response.Clear();
                Response.ContentType = "image/jpeg";
                MemoryStream ms = new MemoryStream();
                oThumbNail.Save(ms, ImageFormat.Jpeg);
                Response.BinaryWrite(ms.GetBuffer());

                oThumbNail.Dispose();
            }

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
