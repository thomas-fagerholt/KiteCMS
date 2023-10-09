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
	public class upload : System.Web.UI.Page
	{
		protected Literal header;
        protected Literal content;
        protected Literal leftMenu;
        private int maxFileSizeKB;
		private int pageId;
		private string allowedFilesList;
		private string uploadRootDirectory;
        private bool Embedded = false;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			pageId = admin.loadLocalMenuXml();
            bool userCanOverwrite = admin.userHasAccess(1502, pageId, false);

            // Test if code-infront page is simple page
            if (HttpContext.Current.Request.QueryString["simple"] != null || HttpContext.Current.Request.Form["simple"] != null)
                Embedded = true;
            
            if (Embedded)
                leftMenu.Visible = false;
            else
                leftMenu.Visible = true;

			Page page = new Page(pageId);

            maxFileSizeKB = int.Parse(ConfigurationManager.AppSettings["MaxFileSizeKB"]);
            allowedFilesList = ConfigurationManager.AppSettings["AllowedFilesList"];
            uploadRootDirectory = ConfigurationManager.AppSettings["uploadRootDirectory"];

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1501,page.Pageid);
			header.Text = Functions.localText("uploadfile");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action =="doupload")
				funcDoUpload(userCanOverwrite);
			else
				funcUploadForm(userCanOverwrite);
		}

		private void funcDoUpload(bool userCanOverwrite)
		{
			string uploadFolder = "";
			bool overwrite = false;
			uploadFolder = HttpContext.Current.Request.Form["folder"].Replace(":",@"/");

			if (userCanOverwrite && HttpContext.Current.Request.Form["overwrite"] != null && HttpContext.Current.Request.Form["overwrite"].ToLower() == "true")
				overwrite = true;

			HttpFileCollection colFiles = HttpContext.Current.Request.Files;
			if (colFiles != null)
			{
	           try 
            {

                for (int intFileCntr = 0; intFileCntr < colFiles.Count; intFileCntr++)
                {

					HttpPostedFile objCurrentFile = colFiles.Get(intFileCntr); 
                    String strCurrentFileName;
                    String strCurrentFileExtension;
					bool ExtensionAllowed = false;

					if(!uploadRootDirectory.EndsWith("/"))
						uploadRootDirectory += "/";

					if(!uploadFolder.EndsWith("/"))
						uploadFolder += "/";

					if(uploadFolder.StartsWith("/"))
						uploadFolder = uploadFolder.Substring(1);

					strCurrentFileName = System.IO.Path.GetFileName(objCurrentFile.FileName); 
					if(strCurrentFileName != "" && strCurrentFileName.IndexOf("..") == -1)
					{ 
						// Check if the file type is permitted
						strCurrentFileExtension = Path.GetExtension(strCurrentFileName); 
						foreach(string extension in allowedFilesList.Split(','))
							if (strCurrentFileExtension.ToLower() == "."+ extension.ToLower())
								ExtensionAllowed = true;

						if (ExtensionAllowed && objCurrentFile.ContentLength <= maxFileSizeKB*1000 && !((uploadRootDirectory + uploadFolder).IndexOf("..")>-1))
						{
							if(Directory.Exists(Server.MapPath((uploadRootDirectory + uploadFolder))))
							{
                                if (File.Exists(Server.MapPath((uploadRootDirectory + uploadFolder) + strCurrentFileName)) && !overwrite)
									content.Text += ("<span style='color:red'>"+ strCurrentFileName + " "+ Functions.localText("fileexists") +"</span><br/>"); 
								else
								{
                                    objCurrentFile.SaveAs(Server.MapPath((uploadRootDirectory + uploadFolder)) + strCurrentFileName); 
									content.Text += (strCurrentFileName + " "+ Functions.localText("uploaded") +"<br/>"); 
								}
							}
							else
								content.Text += ((uploadRootDirectory + uploadFolder) + Functions.localText("folderNotExists") +"<br/>"); 
						}
						else
						{
                            content.Text += (strCurrentFileName + ": <span style='color:red'>" + Functions.localText("uploadfailed") + "</span><br/>"); 
						} 
					} 
				} 
			   }
			   catch (Exception)
			   {
                   content.Text += ("<span style='color:red'>" + Functions.localText("uploadfailed") + "</span>"); 
			   }
               if (Embedded)
                   content.Text += "<script type=\"text/javascript\">if (navigator.appVersion.indexOf(\"MSIE\") != -1) window.parent.frames(\"select\").location=window.parent.frames(\"select\").location; else window.frames[\"parent\"].frames[\"select\"].location=window.frames[\"parent\"].frames[\"select\"].location;</script>";
               else
                   content.Text += "<br/>" + String.Format(Functions.localText("uploadmore"), "upload.aspx?pageid=" + pageId.ToString()) + Functions.publicUrl();
			}

		}

		private void funcUploadForm(bool userCanOverwrite)
		{
			Functions functions = new Functions();

            XslCompiledTransform xslt = new XslCompiledTransform();

			xslt.Load(HttpContext.Current.Server.MapPath("/admin/upload/form_upload.xsl"));

			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("strAllowedFilesList","", allowedFilesList);
			xslArg.AddParam("intMaxFileSizeKB","", maxFileSizeKB.ToString());
            xslArg.AddParam("userCanOverwrite", "", userCanOverwrite);
            xslArg.AddParam("embedded", "", Embedded.ToString().ToLower());

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
