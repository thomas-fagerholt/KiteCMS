using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace profilEdit.gallery
{
	/// <summary>
	/// Summary description for redirect.
	/// </summary>
	public class generateXml : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			menu menu = new menu("false");

			XmlDocument oXmlGallery = OXmlGallery(menu.Pageid);
			StringBuilder output = new StringBuilder();

			XmlNode xmlnode;
			xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]");
			if (xmlnode != null)
			{
				string fileName;
				int maximagesize;
				string width = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/width").InnerText;
				string height = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/height").InnerText;
				string thumbrows = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/thumbrows").InnerText;
				string thumbcols = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/thumbcols").InnerText;
				string textcolor = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/textcolor").InnerText;
				string framecolor = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/framecolor").InnerText;
				string framewidth = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/framewidth").InnerText;
				string imagefolder = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ menu.Pageid +"]/imagefolder").InnerText;

				try 
				{
					maximagesize = Math.Max(int.Parse(width), int.Parse(height));
				}
				catch
				{
					maximagesize = 1000;
				}

				output.Append(@"<SIMPLEVIEWER_DATA maxImageDimension="""+ maximagesize +@""" textColor=""0x"+ textcolor +@""" bgColor=""0x"+ framecolor +@""" frameColor=""0x"+ framecolor +@""" frameWidth="""+ framewidth +@""" stagePadding=""40"" thumbnailColumns="""+ thumbcols +@""" thumbnailRows="""+ thumbrows +@""" navPosition=""right"" navDirection=""LTR"" title="""" imagePath=""/modules/gallery/images/"" thumbPath=""/modules/gallery/images/thumbs/"">"  + Environment.NewLine);

				if(Directory.Exists(HttpContext.Current.Server.MapPath(imagefolder)))
					foreach(string image in Directory.GetFiles(HttpContext.Current.Server.MapPath(imagefolder)))
					{
						fileName = image.Replace(HttpContext.Current.Server.MapPath(imagefolder),"");
						fileName = fileName.Replace(@"\","");
						if(Path.GetExtension(image).ToLower() == ".jpg")
						{
							// add node to xml
							output.Append("<IMAGE><NAME><![CDATA[getImage.aspx?imageurl="+ imagefolder + "/"+  fileName +"&maximagesize="+ maximagesize +"]]></NAME></IMAGE>" + Environment.NewLine); 
						}
					}
				output.Append("</SIMPLEVIEWER_DATA>");

				HttpContext.Current.Response.Clear();
				HttpContext.Current.Response.ContentType = "text/xml";
				HttpContext.Current.Response.BinaryWrite(Encoding.ASCII.GetBytes(output.ToString())); 

				HttpContext.Current.Response.End();

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

		private XmlDocument OXmlGallery(int pageid)
		{
			XmlDocument output = new XmlDocument();
			XmlNode oXmlNode;

			// Load the xml-file holding data
			output.Load(Global.publicXmlPath +"/gallery.xml");

			// Insert current pageid in the xml
			oXmlNode = output.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageid.ToString();

			return output;
		}
	}
}
