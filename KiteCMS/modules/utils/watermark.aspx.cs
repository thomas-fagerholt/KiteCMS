using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Mail;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for mailform.
	/// </summary>
	public class watermark : System.Web.UI.Page
	{
		protected Literal lbContent;

		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override void Render(HtmlTextWriter output) 
		{
			string image = "";
			string defaultpath = "/images";
			string watermarktext = "© www.roses.dk";

			if (ConfigurationManager.AppSettings["watermarkdefaultpath"] != null)
				defaultpath = ConfigurationManager.AppSettings["watermarkdefaultpath"].ToString().TrimEnd('/');
            if (ConfigurationManager.AppSettings["watermarktext"] != null)
                watermarktext = ConfigurationManager.AppSettings["watermarktext"].ToString().TrimEnd('/');
			if (HttpContext.Current.Request.QueryString["image"] != null)
				image = HttpContext.Current.Request.QueryString["image"].ToString().TrimStart('/');

			if (image != "" && image.IndexOf("..") < 0 && image.IndexOf(":") < 0 && File.Exists(Server.MapPath(defaultpath +"/" + image)))
			{
				// initialise our image, bitmap and graphic
				System.Drawing.Image bgimage = System.Drawing.Image.FromFile(Server.MapPath(defaultpath +"/" + image));
				Bitmap bmp = new Bitmap(1, 1);
				Graphics graphic = System.Drawing.Graphics.FromImage(bmp);

				// our font for the writing
				Font font = new Font("Arial", 10, FontStyle.Bold);

				// measure the image
				int height = bgimage.Height;
				int width = bgimage.Width;

				// measure the text height - we need this to position it at the base
				StringFormat stringformat = new StringFormat(StringFormat.GenericTypographic);
				int textheight = Convert.ToInt32(graphic.MeasureString(watermarktext, font, new PointF(0,0), stringformat).Height);

				// recreate our bmp and graphic objects with the new measurements
				bmp = new Bitmap(width, height);
				graphic = System.Drawing.Graphics.FromImage(bmp);

				// add our background
				graphic.DrawImage(bgimage, 0, 0, width, height);

				// add our text background's bar.  we create a rectangle at the base of the image
				//SolidBrush greyfill = new SolidBrush(Color.FromArgb(167, 167, 167));
				//Rectangle rect = new Rectangle(0, height-textheight, textwidth, height);
				//graphic.FillRectangle(greyfill, rect);

				// aliasing mode
				graphic.TextRenderingHint = TextRenderingHint.SystemDefault;

				// our brush which is the writing colour
				SolidBrush blackbrush = new SolidBrush(Color.White);

				// create our text
				graphic.DrawString(watermarktext, font, blackbrush, new Rectangle(0, height-textheight, width, height));

				// Set the content type and return the image
				Response.ContentType = "image/JPEG";
				bmp.Save(Response.OutputStream, ImageFormat.Jpeg);

				// dispose of our objects
				font.Dispose();
				stringformat.Dispose();
				bgimage.Dispose();
				graphic.Dispose();
				bmp.Dispose();
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
