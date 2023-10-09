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
	public class deletefolder : System.Web.UI.Page
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
			admin.userHasAccess(1502,page.Pageid);
			header.Text = Functions.localText("deletefile");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action =="dodeletefile")
				funcDoDeleteFile();
			else
				funcDeleteFileForm();
		}

		private void funcDoDeleteFile()
		{
			string uploadFolder = "";
			string files = "";
			string[] file;

			uploadFolder = HttpContext.Current.Request.Form["folder"].Replace(":",@"/");

			if(!uploadRootDirectory.EndsWith("/"))
				uploadRootDirectory += "/";

			if(!uploadFolder.EndsWith("/"))
				uploadFolder += "/";

			if(uploadFolder.StartsWith("/"))
				uploadFolder = uploadFolder.Substring(1);

            //dont permit dots
            uploadFolder = uploadFolder.Replace(".", "");

			if(HttpContext.Current.Request.Form["file"] != null)
				files = HttpContext.Current.Request.Form["file"].ToString();
			file = files.Split(',');

#pragma warning disable CS0162
			if (!Global.isDemo)
			{
				for (int counter = 0; counter < file.Length; counter++)
				{
					if (File.Exists(Server.MapPath((uploadRootDirectory + uploadFolder + file[counter]))) && file[counter].IndexOf("..") == -1)
					{
						File.Delete(Server.MapPath((uploadRootDirectory + uploadFolder + file[counter])));
						content.Text += (file[counter] + " " + Functions.localText("filedeleted") + "<br/>");
					}
				}
			}
			else
			{
				content.Text += "<span style='color:red;'>" + Functions.localText("notindemo") + "</span><br/>";
			}
#pragma warning restore CS0162
            content.Text += Functions.localText("goto") + Functions.publicUrl();
		}

		private void funcDeleteFileForm()
		{
			Functions functions = new Functions();

            XslCompiledTransform xslt = new XslCompiledTransform();

			xslt.Load(HttpContext.Current.Server.MapPath("/admin/upload/form_deletefile.xsl"));

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
