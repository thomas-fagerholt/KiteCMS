using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for edittemplate.
	/// </summary>
    public class editcssfile : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action ="";

            // Check for permissions to this module
            admin.userHasAccess(1405, page.Pageid);
			header.Text = Functions.localText("editcssfile");

            if (HttpContext.Current.Request.Form["action"] != null)
                action = HttpContext.Current.Request.Form["action"].ToString().ToLower();
            if (action=="" && HttpContext.Current.Request.QueryString["action"] != null)
                action = HttpContext.Current.Request.QueryString["action"].ToString().ToLower();

            if (action == "doedit")
                funcDoEditCssFile();
            else if (action == "edit")
                funcEditCssFile();
            else
				funcListCssFile();

		}

        private void funcDoEditCssFile()
		{
			string cssurl = "";
			string filecontent = "";

			if(HttpContext.Current.Request["cssurl"] != null)
				cssurl = Server.MapPath("/"+ HttpContext.Current.Request["cssurl"].ToString());
            if (HttpContext.Current.Request["filecontent"] != null)
                filecontent = HttpContext.Current.Request["filecontent"].ToString();

            if (cssurl != "" && filecontent != "" && File.Exists(cssurl) && cssurl.IndexOf("..") < 0 && Path.GetExtension(cssurl).ToLower() == ".css")
			{
#pragma warning disable CS0162
                if (!Global.isDemo)
                {
                    File.WriteAllText(cssurl, filecontent, System.Text.Encoding.UTF8);
                    content.Text += Functions.localText("filesaved");
                }
                else
                {
                    content.Text += "<span style='color:red;'>" + Functions.localText("notindemo") + "</span><br/>";
                }
#pragma warning restore CS0162
                content.Text += Functions.localText("goto") + Functions.publicUrl();
            }
			else
				throw new ArgumentException("CSS-file object doesn't have all required data","cssfile");
		}

        private void funcEditCssFile()
		{
            string folder;
            string file = "";

            if (HttpContext.Current.Request.QueryString["datafolder"] != null && HttpContext.Current.Request.QueryString["datafolder"].ToLower() == "true")
                folder = Global.publicXmlPath + "/";
            else
                folder = Server.MapPath("//");

            if (HttpContext.Current.Request.QueryString["file"] != null)
                file = HttpContext.Current.Request.QueryString["file"];

            if (folder != "" && file != "" && File.Exists(folder + file) && folder.IndexOf("..") < 0 && file.IndexOf("..") < 0 && Path.GetExtension(file).ToLower() == ".css")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
                XmlNode root = xmldoc.CreateElement("element", "website", "");
                xmldoc.AppendChild(root);

                XmlElement node = xmldoc.CreateElement("element", "cssurl", "");
                node.InnerText = (folder + file).Replace(Server.MapPath("/"),"");
                xmldoc.FirstChild.AppendChild(node);

                node = xmldoc.CreateElement("element", "filecontent", "");
                XmlCDataSection cdata = xmldoc.CreateCDataSection(Functions.string2hex(File.ReadAllText(folder + file)).Replace("%00", ""));
                node.AppendChild(cdata);
                xmldoc.FirstChild.AppendChild(node);
                
                Functions functions = new Functions();
                XsltArgumentList xslArg = new XsltArgumentList();
                xslArg.AddExtensionObject("urn:localText", functions);

                content.Text += Functions.transformXml(xmldoc, Server.MapPath("/admin/template/form_edit_cssfile.xsl"), xslArg);
            }
            else
                throw new ArgumentException("CSS-file doesn't exists", "cssfile");
        }

        private void funcListCssFile()
        {
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            XmlNode root = xmldoc.CreateElement("element", "website", "");
            xmldoc.AppendChild(root);

            // add css-files from /image-folder
            String imageFolder = "/images";
            if (ConfigurationManager.AppSettings["GraphicRootDirectory"] != null)
                imageFolder = ConfigurationManager.AppSettings["GraphicRootDirectory"];
            xmldoc = addFilesToXml(xmldoc, Server.MapPath(imageFolder), Server.MapPath("\\"), false);

            content.Text += Functions.transformXml(xmldoc, Server.MapPath("/admin/template/list_edit_cssfile.xsl"), xslArg);
        }

        private XmlDocument addFilesToXml(XmlDocument xmldoc, String folder, String baseFolder, bool datafolder)
        {
            if (Directory.Exists(folder))
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    // find subfolders
                    foreach (string dir in Directory.GetDirectories(folder))
                        xmldoc = addFilesToXml(xmldoc, dir, baseFolder, datafolder);

                    // find files
                    if (Path.GetExtension(file).ToLower() == ".css")
                    {
                        XmlElement node = xmldoc.CreateElement("element", "file", "");
                        node.SetAttribute("datafolder", datafolder.ToString());
                        node.InnerText = file.Replace(baseFolder, "");
                        xmldoc.FirstChild.AppendChild(node);
                    }
                }
            }

            return xmldoc;
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
