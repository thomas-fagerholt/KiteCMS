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
	public class createfolder : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		private string uploadRootDirectory;
		private int pageId;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

            uploadRootDirectory = ConfigurationManager.AppSettings["uploadRootDirectory"];

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1503,page.Pageid);
			header.Text = Functions.localText("createfolder");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action =="docreatefolder")
				funcDoCreateFolder();
			else
				funcCreateFolderForm();
		}

		private void funcDoCreateFolder()
		{
			string uploadFolder = "";
			string newFolder = "";

			uploadFolder = HttpContext.Current.Request.Form["folder"].Replace(":",@"/");

			if(!uploadRootDirectory.EndsWith("/"))
				uploadRootDirectory += "/";

			if(!uploadFolder.EndsWith("/"))
				uploadFolder += "/";

			if(uploadFolder.StartsWith("/"))
				uploadFolder = uploadFolder.Substring(1);

			if(HttpContext.Current.Request.Form["newfolder"] != null)
				newFolder = HttpContext.Current.Request.Form["newfolder"].ToString();

			if(!Directory.Exists(Server.MapPath(uploadRootDirectory + uploadFolder + newFolder)))
			{
				Directory.CreateDirectory(Server.MapPath(uploadRootDirectory + uploadFolder) + newFolder);
                content.Text += (newFolder + " " + Functions.localText("foldercreated") + Functions.publicUrl()); 
			}
			else
                content.Text += (uploadRootDirectory + uploadFolder + Functions.localText("folderExists") + Functions.publicUrl());  
		}

		private void funcCreateFolderForm()
		{
			Functions functions = new Functions();

            XslCompiledTransform xslt = new XslCompiledTransform();

			xslt.Load(HttpContext.Current.Server.MapPath("/admin/upload/form_createfolder.xsl"));

			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			StringWriter writer = new StringWriter();

			xslt.Transform(Functions.FilesAndFolders(pageId),xslArg,writer);
			content.Text += writer.ToString();

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
