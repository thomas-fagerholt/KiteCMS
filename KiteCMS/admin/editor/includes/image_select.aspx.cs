using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
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
	public class image_select : System.Web.UI.Page
	{
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
            Admin admin = new Admin();
            int pageId = admin.loadLocalMenuXml();

            Page page = new Page(pageId);

            // Check for permissions to this module. Edit content or edit draft
            if (!admin.userHasAccess(1201, page.Pageid, false) && admin.userHasAccess(1302, page.Pageid, false))
                HttpContext.Current.Response.Redirect("/admin/default.aspx?pageId=" + pageId);

            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"].ToLower();
            string GraphicRootDirectory = ConfigurationManager.AppSettings["GraphicRootDirectory"].ToLower();

			string strFolder = "";
			string strFile = "";
			string CurrentFolder="";
			string CurrentFolderPath;
			string RootFolderPath;
			string ParentFolder = "";
            string popup = "false";
			string strCurrentFileExtension;
			bool FileFound = false;
            bool showThumb = true;
			StringBuilder output = new StringBuilder();

			GraphicRootDirectory = GraphicRootDirectory.Replace(".","");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"\",@"/");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"//",@"/");

            if (HttpContext.Current.Request["Folder"] != null)
                strFolder = HttpContext.Current.Request["Folder"].ToString().ToLower();
            if (HttpContext.Current.Request["popup"] != null)
                popup = HttpContext.Current.Request["popup"].ToString().ToLower();
            if (HttpContext.Current.Request["File"] != null)
                strFile = HttpContext.Current.Request["File"].ToString().ToLower();
            if (HttpContext.Current.Request["showThumb"] != null)
                showThumb = bool.Parse(HttpContext.Current.Request["showThumb"].ToString());
			
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


            output.Append(@"<div>" + Functions.localText("currentfolder") + @": \" + CurrentFolder + @"</div>" + Environment.NewLine);
            if (showThumb)
                output.Append(@"<div style='float:left'><a href='image_select.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=false'><img src='/admin/images/icon_list.gif' align='absmiddle' alt='" + Functions.localText("showtexts") + "' title='" + Functions.localText("showtexts") + "' border='0'/> " + Functions.localText("showtexts") + "</a></div>");
            else
                output.Append(@"<div style='float:left'><a href='image_select.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=true'><img src='/admin/images/icon_thumbs.gif' align='absmiddle' alt='" + Functions.localText("showthumbs") + "' title='" + Functions.localText("showthumbs") + "' border='0'/> " + Functions.localText("showthumbs") + "</a></div>");

            output.Append(@"<div style='float:right'><a href='/admin/upload/upload.aspx?simple=true' target='view'><img src='/admin/images/upload_file.gif' align='absmiddle' alt='" + Functions.localText("uploadfile") + "' title='" + Functions.localText("uploadfile") + "' border='0'/> " + Functions.localText("uploadfile") + "</a></div>" + Environment.NewLine);
            output.Append(@"<br clear='all'/><hr class='dottedHr'/>" + Environment.NewLine);
            output.Append("<div class=\"FolderContainer scrollbar\">" + Environment.NewLine);

            if (ParentFolder != "")
                output.Append(@"<div><a href='image_select.aspx?folder=:" + ParentFolder.Replace(@"\", ":") + "&amp;showThumb=" + showThumb + "'><img src='/admin/images/folder_up.gif' alt='" + Functions.localText("up") + "' title='" + Functions.localText("up") + "' border='0'/>&nbsp;.. (" + Functions.localText("up") + ")</a></div>" + Environment.NewLine);

            if (Directory.Exists(CurrentFolderPath))
            {
                // Find subfolders
                foreach (string dir in Directory.GetDirectories(CurrentFolderPath))
                    output.Append(@"<div><a href='image_select.aspx?folder=\" + CurrentFolder + dir.Replace(CurrentFolderPath, "") + "&amp;showThumb=" + showThumb + "&amp;popup=" + popup + "'><img src='/admin/images/folder.gif' alt='' border='0'/>&nbsp;" + dir.Replace(CurrentFolderPath, "") + "</a></div>" + Environment.NewLine);

                output.Append("</div><hr class=\"dottedHr\"/><div class=\"listfiles \">" + Environment.NewLine);
                // Find graphic files
                foreach (string file in Directory.GetFiles(CurrentFolderPath))
                {
                    bool isGraphic = false;
                    strCurrentFileExtension = Path.GetExtension(file);
                    foreach (string extension in strFiletypeGraphic.Split(','))
                        if (strCurrentFileExtension.ToLower() == "." + extension.ToLower())
                        {
                            isGraphic = true;
                            break;
                        }

                    if (isGraphic)
                    {
                        if (showThumb)
                        {
                            output.Append("<div class=\"imagebox\">");
                            output.Append(@"<a href='image_view.aspx?folder=\" + CurrentFolder + "&amp;file=" + HttpContext.Current.Server.UrlEncode(file.Replace(CurrentFolderPath, "")) + "&amp;popup=" + popup + "' target='view'><img src='" + (isGraphic ? "/modules/gallery/images/thumbs/getImage.aspx?imageurl=\\" + file.Replace(RootFolderPath, "") + "&amp;maxHeight=100&amp;maxWidth=110" : "/admin/images/file_empty_large.gif") + "' alt='" + file.Replace(CurrentFolderPath, "") + "' title='" + file.Replace(CurrentFolderPath, "") + "' border='0'/><br/>");
                            output.Append(file.Replace(CurrentFolderPath, "") + "</a>");
                            output.Append("</div>");
                        }
                        else
                        {
                            output.Append(@"<a href='image_view.aspx?folder=\" + CurrentFolder + "&amp;file=" + HttpContext.Current.Server.UrlEncode(file.Replace(CurrentFolderPath, "")) + "&amp;popup=" + popup + "' target='view'>");
                            output.Append("<img src='/admin/images/file_grafik.gif' alt='' border='0'/>&nbsp;" + file.Replace(CurrentFolderPath, ""));
                            output.Append("</a><br/>" + Environment.NewLine);
                        }
                        FileFound = true;
                    }
                }
                output.Append("</div>");
            }

            if (!FileFound)
                output.Append(@"<div><br/>[" + Functions.localText("empty") + "]</div>" + Environment.NewLine);

            output.Append("</div>");
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
