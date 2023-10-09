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

namespace profilEdit
{
	/// <summary>
	/// Summary description for pdf.
	/// </summary>
	public class Images : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
		}
		protected override void Render(HtmlTextWriter output) 
		{
			string url = null;
			int width = -1;
			int height = -1;

			if (HttpContext.Current.Request["imageurl"] != null)
				url = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["imageurl"].ToString());
			try 
			{
				if (HttpContext.Current.Request["width"] != null)
					width = int.Parse(HttpContext.Current.Request["width"]);
				if (HttpContext.Current.Request["height"] != null)
					height = int.Parse(HttpContext.Current.Request["height"]);
			}
			catch {}

			if (url != null &&  File.Exists(Server.MapPath(url)))
			{
				System.Drawing.Image oThumbNail;

				System.Drawing.Image oImg = System.Drawing.Image.FromFile(Server.MapPath(url));

				if (width != -1 && height != -1)
					oThumbNail = new Bitmap(oImg, width, height);
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
