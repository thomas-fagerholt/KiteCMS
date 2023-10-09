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
    public class editxslfile : System.Web.UI.Page
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
            admin.userHasAccess(1404, page.Pageid);
			header.Text = Functions.localText("editxslfile");

            if (HttpContext.Current.Request.Form["action"] != null)
                action = HttpContext.Current.Request.Form["action"].ToString().ToLower();
            if (action=="" && HttpContext.Current.Request.QueryString["action"] != null)
                action = HttpContext.Current.Request.QueryString["action"].ToString().ToLower();

            if (action == "doedit")
                funcDoEditXslFile();
            else if (action == "edit")
                funcEditXslFile();
            else
				funcListXslFile();

		}

        private void funcDoEditXslFile()
		{
			string xslurl = "";
			string filecontent = "";

			if(HttpContext.Current.Request["xslurl"] != null)
                xslurl = Template.GetFullPath(HttpContext.Current.Request["xslurl"].ToString());
            if (HttpContext.Current.Request["filecontent"] != null)
                filecontent = HttpContext.Current.Request["filecontent"].ToString();

            if (xslurl != "" && filecontent != "" && File.Exists(xslurl) && xslurl.IndexOf("..") < 0 && Path.GetExtension(xslurl).ToLower() == ".xsl")
			{
                XmlDocument xslt = new XmlDocument();
                try
                {
#pragma warning disable CS0162
                    if (!Global.isDemo)
                    {
                        File.WriteAllText(xslurl, filecontent, System.Text.Encoding.Default);
                        content.Text += Functions.localText("filesaved");
                    }
                    else
                    {
                        content.Text += "<span style='color:red;'>" + Functions.localText("notindemo") + "</span><br/>";
                    }
#pragma warning restore CS0162

                    content.Text += Functions.localText("goto") + Functions.publicUrl();

                }
                catch (XmlException exp)
                {
                    content.Text += Functions.localText("filenotvalid") + Functions.publicUrl();
                    content.Text += "<p>" + exp.Message + "</p>";
                }
            }
			else
				throw new ArgumentException("XSL-file object doesn't have all required data","xslfile");
		}

        private void funcEditXslFile()
		{
            string folder;
            string file = "";

            if (HttpContext.Current.Request.QueryString["datafolder"] != null && HttpContext.Current.Request.QueryString["datafolder"].ToLower() == "true")
                folder = Global.publicXmlPath + "/";
            else
                folder = Server.MapPath("//");

            if (HttpContext.Current.Request.QueryString["file"] != null)
                file = HttpContext.Current.Request.QueryString["file"];

            if (folder != "" && file != "" && File.Exists(folder + file) && folder.IndexOf("..") < 0 && file.IndexOf("..") < 0 && Path.GetExtension(file).ToLower() == ".xsl")
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
                XmlNode root = xmldoc.CreateElement("element", "website", "");
                xmldoc.AppendChild(root);

                XmlElement node = xmldoc.CreateElement("element", "xslurl", "");
//                node.InnerText = (folder + file).Replace(Server.MapPath("/"), "");
                node.InnerText = file;
                xmldoc.FirstChild.AppendChild(node);

                node = xmldoc.CreateElement("element", "filecontent", "");
                XmlCDataSection cdata = xmldoc.CreateCDataSection(Functions.string2hex(File.ReadAllText(folder + file, System.Text.Encoding.Default)).Replace("%00", ""));
                node.AppendChild(cdata);
                xmldoc.FirstChild.AppendChild(node);
                
                Functions functions = new Functions();
                XsltArgumentList xslArg = new XsltArgumentList();
                xslArg.AddExtensionObject("urn:localText", functions);

                content.Text += Functions.transformXml(xmldoc, Server.MapPath("/admin/template/form_edit_xslfile.xsl"), xslArg);
            }
            else
                throw new ArgumentException("XSL-file doesn't exists", "xslfile");
        }

        private void funcListXslFile()
        {
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
            XmlNode root = xmldoc.CreateElement("element", "website", "");
            xmldoc.AppendChild(root);

            // add xsl-files from /data-folder
            xmldoc = addFilesToXml(xmldoc, Global.publicXmlPath, Global.publicXmlPath.TrimEnd('\\') + "\\", true);
            
            // add xsl-files from /image-folder
            if (ConfigurationManager.AppSettings["AddXslFilesFromImageFolder"] != null)
            {
                String imageFolder = "/images";
                if (ConfigurationManager.AppSettings["GraphicRootDirectory"] != null)
                    imageFolder = ConfigurationManager.AppSettings["GraphicRootDirectory"];
                xmldoc = addFilesToXml(xmldoc, Server.MapPath(imageFolder), Server.MapPath("\\"), false);
            }
            content.Text += Functions.transformXml(xmldoc, Server.MapPath("/admin/template/list_edit_xslfile.xsl"), xslArg);
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
                    if (Path.GetExtension(file).ToLower() == ".xsl")
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
