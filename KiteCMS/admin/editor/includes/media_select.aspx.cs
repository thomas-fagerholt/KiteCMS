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
using System.Text;
using System.IO;
using KiteCMS.Admin;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for image_select.
	/// </summary>
	public class media_select : System.Web.UI.Page
	{
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
            Admin admin = new Admin();
            int pageId = admin.loadLocalMenuXml();

            Page page = new Page(pageId);

			// Check for permissions to this module . Edit content or edit draft
			if (!admin.userHasAccess(1201, page.Pageid,false) && admin.userHasAccess(1302, page.Pageid,false))
                HttpContext.Current.Response.Redirect("/admin/default.aspx?pageId=" + pageId);

            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"].ToLower();
            string GraphicRootDirectory = ConfigurationManager.AppSettings["GraphicRootDirectory"].ToLower();

			string strFolder = "";
			string strFile = "";
			string CurrentFolder="";
			string CurrentFolderPath;
			string RootFolderPath;
			string ParentFolder = "";
			string strCurrentFileExtension;
			bool MediaFileFound = false;
			bool showgraphics = false;
			StringBuilder output = new StringBuilder();

			GraphicRootDirectory = GraphicRootDirectory.Replace(".","");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"\",@"/");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"//",@"/");

			if(HttpContext.Current.Request["Folder"] != null)
				strFolder = HttpContext.Current.Request["Folder"].ToString().ToLower();
			if(HttpContext.Current.Request["File"] != null)
				strFile = HttpContext.Current.Request["File"].ToString().ToLower();
			if(HttpContext.Current.Request["showgraphics"] != null && HttpContext.Current.Request["showgraphics"].ToString().ToLower() == "true")
				showgraphics = true;

			// Dont show under the root
			strFolder = strFolder.Replace(".","");
			strFolder = strFolder.Replace(@"\",@"/");
			strFolder = strFolder.Replace(@"//",@"/");

			strFile = strFile.Replace(@"\",@"/");

			// If folders is empty, then show the root folder
			if (strFolder.IndexOf(GraphicRootDirectory)!=0)
				strFolder = GraphicRootDirectory;

			// Format the strings
			if (!strFolder.EndsWith("/"))
				strFolder += "/";
			if (strFile.IndexOf("/")>-1)
				strFile = strFile.Substring(strFile.LastIndexOf("/"));

			RootFolderPath = HttpContext.Current.Server.MapPath("/").ToLower();
			CurrentFolderPath = HttpContext.Current.Server.MapPath(strFolder).ToLower();
			CurrentFolder = CurrentFolderPath.Replace(RootFolderPath,"");

			// Find parentfolder
			if(CurrentFolder.IndexOf(@"\")>-1 && CurrentFolder.IndexOf(@"\")!= CurrentFolder.Length-1)
				ParentFolder = CurrentFolder.Substring(0,CurrentFolder.Substring(0,CurrentFolder.Length-1).LastIndexOf(@"\"));
 
			output.Append("<script type='text/javascript'>"+ Environment.NewLine);
			output.Append("function funcImageChosen(){"+ Environment.NewLine);
			output.Append("window.returnValue = '"+ strFolder + strFile +"';"+ Environment.NewLine);
			output.Append("window.close();}"+ Environment.NewLine);
			output.Append("</script>"+ Environment.NewLine);

			output.Append("<form name='fileview' method='post' target='view' action='media_view.asp'>"+ Environment.NewLine);
			output.Append("<input type='hidden' name='folder' value='"+ CurrentFolder +"'/>"+ Environment.NewLine);
			output.Append("<table cellpadding='0' cellspacing='0' border='0'>"+ Environment.NewLine);

			output.Append(@"<tr><td>"+ Functions.localText("currentfolder") +@": \"+ CurrentFolder +@"</td></tr>"+ Environment.NewLine);
			if(showgraphics)
				output.Append(@"<tr><td><input type=""checkbox"" name=""showgraphics"" checked=""true"" onclick=""document.location=location.href.split('?')[0]+'?folder=/"+ CurrentFolder.Replace("\\","/") +@"'"" value=""true""/>");
			else
				output.Append(@"<tr><td><input type=""checkbox"" name=""showgraphics"" onclick=""document.location=location.href.split('?')[0].split('?')[0]+'?folder=/"+ CurrentFolder.Replace("\\","/") +@"&amp;showgraphics=true'"" value=""true""/>");
			output.Append(Functions.localText("showgraphics") +@"</td></tr>"+ Environment.NewLine);
			if (ParentFolder != "")
				output.Append(@"<tr><td><a href='media_select.aspx?folder=\"+ ParentFolder +"&amp;showgraphics="+ showgraphics +"'><img src='/admin/images/folder_up.gif' alt='' border='0'/>&nbsp;.. ("+ Functions.localText("up") +")</a></td></tr>"+ Environment.NewLine);
			
			if(Directory.Exists(CurrentFolderPath))
			{
				// Find subfolders
				foreach(string dir in Directory.GetDirectories(CurrentFolderPath))
					output.Append(@"<tr><td><a href='media_select.aspx?folder=\"+ CurrentFolder + dir.Replace(CurrentFolderPath,"") +"&amp;showgraphics="+ showgraphics +"'><img src='/admin/images/folder.gif' alt='' border='0'/>&nbsp;"+ dir.Replace(CurrentFolderPath,"") +"</a></td></tr>"+ Environment.NewLine);

				// Find graphic files
				foreach(string file in Directory.GetFiles(CurrentFolderPath))
				{
					bool FileIsMedia=true;

					// Check if the file type is permitted
					strCurrentFileExtension = Path.GetExtension(file); 
					if (!showgraphics)
						foreach(string extension in strFiletypeGraphic.Split(','))
							if (strCurrentFileExtension.ToLower() == "."+ extension.ToLower())
								FileIsMedia = false;
					if(FileIsMedia)
					{
						output.Append(@"<tr><td><a href='media_view.aspx?folder=\"+ CurrentFolder +"&amp;file="+ file.Replace(CurrentFolderPath,"") +"' target='view'><img src='/admin/images/file_tom.gif' alt='' border='0'/>&nbsp;"+ file.Replace(CurrentFolderPath,"") +"</a></td></tr>"+ Environment.NewLine);
						MediaFileFound = true;
					}
				}
			}
			
			if (!MediaFileFound)
				output.Append(@"<tr><td><br/>["+ Functions.localText("empty") +"]</td></tr>"+ Environment.NewLine);

			output.Append("</table>");
			output.Append("</form>");
			content.Text = output.ToString();
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
