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
using System.IO.Compression;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for addmenu.
	/// </summary>
	public class Backup : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;
        int pageId;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
            Admin admin = new Admin();

            pageId = admin.loadLocalMenuXml();
            Page page = new Page(pageId);

            string action = "";

            // Check for permissions to this module
            admin.userHasAccess(1602, page.Pageid);
            header.Text = Functions.localText("backup");

            if (HttpContext.Current.Request["action"] != null)
                action = HttpContext.Current.Request["action"].ToString().ToLower();

            if (action == "dobackup")
                funcDoBackup();
            else
                funcBackup();
        }

		private void funcBackup()
		{
			Functions functions = new Functions();
			XmlDocument oXmlAccess = new XmlDocument();

			XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/useradmin/form_backup.xsl"));

			StringWriter writer = new StringWriter();

			xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,xslArg,writer);
			content.Text += writer.ToString();
		}

        private void funcDoBackup()
        {
            string backupType = "";
            if (HttpContext.Current.Request.Form["backup"] != null)
                backupType = HttpContext.Current.Request.Form["backup"];
            if (backupType != "")
            {
                MemoryStream ms = new MemoryStream();
                ZipStorer zip = ZipStorer.Create(ms,"");
                string rootFolderPath = HttpContext.Current.Server.MapPath("/");
                if (backupType == "datafiles" || backupType == "userfiles")
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(Server.MapPath("/web.config"));
                    FileStream fileStream = File.OpenRead(Server.MapPath("/web.config"));
                    zip.AddStream(ZipStorer.Compression.Store, "/web.config", fileStream, fileInfo.LastWriteTime, "");
                    fileStream.Dispose();

                    if (Directory.Exists(Global.publicXmlPath))
                    {
                        addFilesToZip(Global.publicXmlPath, ref zip, rootFolderPath);
                    }
                    if (Directory.Exists(Global.adminXmlPath) && Global.adminXmlPath.CompareTo(Global.publicXmlPath)!=0)
                    {
                        addFilesToZip(Global.adminXmlPath, ref zip, rootFolderPath);
                    }
                    if (backupType == "userfiles" && Directory.Exists(Server.MapPath(ConfigurationManager.AppSettings["GraphicRootDirectory"])))
                    {
                        addFilesToZip(Server.MapPath(ConfigurationManager.AppSettings["GraphicRootDirectory"]), ref zip, rootFolderPath);
                    }
                    zip.WriteEndRecord();
                    streamZip(ms);
                    zip.Close();
                }
                else if (backupType == "allfiles")
                {
                    addFilesToZip(Server.MapPath("/"), ref zip, rootFolderPath);
                    if (Directory.Exists(Global.publicXmlPath) && !Global.publicXmlPath.ToLower().StartsWith(Server.MapPath("/").ToLower()))
                    {
                        addFilesToZip(Global.publicXmlPath, ref zip, rootFolderPath);
                    }
                    if (Directory.Exists(Global.adminXmlPath) && Global.adminXmlPath.CompareTo(Global.publicXmlPath) != 0 && !Global.adminXmlPath.ToLower().StartsWith(Server.MapPath("/").ToLower()))
                    {
                        addFilesToZip(Global.adminXmlPath, ref zip, rootFolderPath);
                    }
                    zip.WriteEndRecord();
                    streamZip(ms);
                    zip.Close();
                }
            }
        }

        private void addFilesToZip(string folder, ref ZipStorer zip, string rootFolderPath)
        {
            if (Directory.Exists(folder))
            {
                foreach (string subfolder in Directory.GetDirectories(folder))
                {
                    addFilesToZip(subfolder, ref zip, rootFolderPath);
                }

                foreach (string file in Directory.GetFiles(folder))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
                    FileStream fileStream = File.OpenRead(file);
                    zip.AddStream(ZipStorer.Compression.Store, file.Replace(rootFolderPath, ""), fileStream, fileInfo.LastWriteTime, "");
                    fileStream.Dispose();
                }
            }
        }

        private void streamZip(MemoryStream ms)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/zip";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=backup_" + DateTime.Now.ToString("yyyy-MM-dd") +".zip");
            HttpContext.Current.Response.AppendHeader("Content-Length", ms.Length.ToString());
            HttpContext.Current.Response.BinaryWrite(ms.GetBuffer());
            ms.Dispose();

            HttpContext.Current.Response.End();
            //Server.Transfer("/default.aspx?pageid=" + pageId);
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
