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
	public class listfiles : System.Web.UI.Page
	{
		protected Literal content;
        protected Literal header;
        protected Literal menu;

		private void Page_Load(object sender, System.EventArgs e)
		{
            Admin admin = new Admin();
            int pageId = admin.loadLocalMenuXml();

            Page page = new Page(pageId);
            Template template = new Template(page.TemplateId);

            // Check for permissions to this module
            admin.userHasAccess(1504, page.Pageid);
            header.Text = Functions.localText("listfiles");

            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"].ToLower();
            string GraphicRootDirectory = ConfigurationManager.AppSettings["GraphicRootDirectory"].ToLower();

			string strFolder = "";
			string strFile = "";
			string CurrentFolder="";
			string CurrentFolderPath;
			string RootFolderPath;
			string ParentFolder = "";
			string strCurrentFileExtension;
			bool FileFound = false;
            bool showThumb = true;
            bool candelete = false;
			StringBuilder output = new StringBuilder();

            menu.Text = "<a class=\"adminBacklink\" href=\"" + template.Publicurl + "?pageid=" + page.Pageid + "\">" + Functions.localText("back") + "</a>";

			GraphicRootDirectory = GraphicRootDirectory.Replace(".","");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"\",@"/");
            GraphicRootDirectory = GraphicRootDirectory.Replace(@"//", @"/");

		    if(HttpContext.Current.Request["Folder"] != null)
			    strFolder = HttpContext.Current.Request["Folder"].ToString().Replace(":", @"/").ToLower();
            if (HttpContext.Current.Request["File"] != null)
                strFile = HttpContext.Current.Request["File"].ToString().Replace(":", @"/").ToLower();
            if (HttpContext.Current.Request["showThumb"] != null)
                showThumb = bool.Parse(HttpContext.Current.Request["showThumb"].ToString());

            candelete = (!Global.isDemo && admin.userHasAccess(1502, pageId, false));

            //delete file if action=delete
            if (candelete && HttpContext.Current.Request.QueryString["action"] != null && HttpContext.Current.Request.QueryString["action"] == "delete")
                funcDoDeleteFile();

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

			output.Append("<div class=\"FolderContainer scrollbar\">"+ Environment.NewLine);

            output.Append(@"<div>" + Functions.localText("currentfolder") + @": \" + CurrentFolder + @"</div>" + Environment.NewLine);
            if (showThumb)
                output.Append(@"<div><a href='listfiles.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=false'><img src='/admin/images/icon_list.gif' align='absmiddle' alt='" + Functions.localText("showtexts") + "' title='" + Functions.localText("showtexts") + "' border='0'/> " + Functions.localText("showtexts") + "</a></div><hr class=\"dottedHr\"/>");
            else
                output.Append(@"<div><a href='listfiles.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=true'><img src='/admin/images/icon_thumbs.gif' align='absmiddle' alt='" + Functions.localText("showthumbs") + "' title='" + Functions.localText("showthumbs") + "' border='0'/> " + Functions.localText("showthumbs") + "</a></div><hr class=\"dottedHr\"/>");

            if (ParentFolder != "")
                output.Append(@"<div><a href='listfiles.aspx?folder=:" + ParentFolder.Replace(@"\", ":") + "&amp;showThumb=" + showThumb + "'><img src='/admin/images/folder_up.gif' alt='" + Functions.localText("up") + "' title='" + Functions.localText("up") + "' border='0'/>&nbsp;.. (" + Functions.localText("up") + ")</a></div>" + Environment.NewLine);

            if (Directory.Exists(CurrentFolderPath))
            {
                // Find subfolders
                foreach (string dir in Directory.GetDirectories(CurrentFolderPath))
                    output.Append(@"<div><a href='listfiles.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + dir.Replace(CurrentFolderPath, "").Replace(@"\", ":") + "&amp;showThumb=" + showThumb + "'><img src='/admin/images/folder.gif' alt='' border='0'/>&nbsp;" + dir.Replace(CurrentFolderPath, "") + "</a></div>" + Environment.NewLine);

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

                    if (showThumb)
                    {
                        output.Append("<div class=\"listfilesImage\">");
                        output.Append("<div class=\"image\"><a href='/" + CurrentFolder + "/" + HttpContext.Current.Server.UrlEncode(file.Replace(CurrentFolderPath, "")) + "' target='_blank'><img src='" + (isGraphic ? "/modules/gallery/images/thumbs/getImage.aspx?imageurl=\\" + file.Replace(RootFolderPath, "") + "&amp;maxSize=170" : "/admin/images/file_empty_large.gif") + "' alt='" + file.Replace(CurrentFolderPath, "") + "' title='" + file.Replace(CurrentFolderPath, "") + "' border='0'/></a></div>");
                        output.Append("<div class=\"listfilesText\">");
                        output.Append("<div class=\"listfilesTitle\" title=\"" + file.Replace(CurrentFolderPath, "") + "\">" + file.Replace(CurrentFolderPath, "") + "</div><div class=\"left\"><a href='/" + CurrentFolder + "/" + HttpContext.Current.Server.UrlEncode(file.Replace(CurrentFolderPath, "")) + "' target='_blank'><img src=\"/admin/images/link_openimage.gif\" width=\"16\" height=\"16\" alt=\"" + Functions.localText("open") + "\" title=\"" + Functions.localText("open") + "\"/></a></div>");
                        output.Append("<div class=\"left\"><a href=\"javascript:if(confirm('" + Functions.localText("oktodelete") + @"')) location.href='listfiles.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=" + showThumb + "&amp;action=delete&amp;file=" + file.Replace(CurrentFolderPath, "") + "'\"><img src=\"/admin/images/link_deleteimage.gif\" width=\"14\" height=\"16\" alt=\"" + Functions.localText("delete") + "\" title=\"" + Functions.localText("delete") + "\"/></a></div>");
                        output.Append("<br clear=\"all\"/></div>");
                        output.Append("<div class=\"attributes\"><div class=\"textleft\">" + (int)((new FileInfo(file).Length + 500) / 1000) + "kb</div><div class=\"textright\">" + GetImageDimensions(file) + "</div><br clear=\"all\"/>" + Environment.NewLine);
                        output.Append("</div></div>");
                    }
                    else
                    {
                        output.Append(@"<a href='/" + CurrentFolder + "/" + HttpContext.Current.Server.UrlEncode(file.Replace(CurrentFolderPath, "")) + "' target='_blank'>");
                        output.Append("<img src='/admin/images/file_" + (isGraphic ? "grafik" : "tom") + ".gif' alt='" + GetImageDimensions(file) + "' title='" + GetImageDimensions(file) + "' border='0'/>&nbsp;" + file.Replace(CurrentFolderPath, ""));
                        output.Append("</a>&nbsp;" + (int)((new FileInfo(file).Length + 500) / 1000) + "kb ");
                        if (candelete)
                            output.Append("<a href=\"javascript:if(confirm('" + Functions.localText("oktodelete") + "')) location.href='listfiles.aspx?folder=:" + CurrentFolder.Replace(@"\", ":") + "&amp;showThumb=" + showThumb + "&amp;action=delete&amp;file=" + file.Replace(CurrentFolderPath, "") + "'\"><img src=\"/admin/images/link_deleteimage.gif\" alt=\"" + Functions.localText("delete") + "\" title=\"" + Functions.localText("delete") + "\" border=\"0\" width=\"7\" height=\"8\"/></a>");
                        output.Append("<br/>" + Environment.NewLine);
                    }
                    FileFound = true;
                }
                output.Append("</div>");
            }

            if (!FileFound)
                output.Append(@"<div><br/>[" + Functions.localText("empty") + "]</div>" + Environment.NewLine);

            output.Append("</div>");
			content.Text = output.ToString();
        }

        private void funcDoDeleteFile()
        {
            string uploadFolder = "";
            string files = "";
            string[] file;
            string uploadRootDirectory;

            uploadRootDirectory = ConfigurationManager.AppSettings["uploadRootDirectory"].ToLower();
            uploadFolder = HttpContext.Current.Request.QueryString["folder"].Replace(":", @"/").ToLower();

            if (!uploadRootDirectory.EndsWith("/"))
                uploadRootDirectory += "/";

            if (!uploadFolder.EndsWith("/"))
                uploadFolder += "/";

            if (!uploadFolder.StartsWith("/"))
                uploadFolder = @"/" + uploadFolder;

            //dont permit dots
            uploadFolder = uploadFolder.Replace(".", "");

            if (HttpContext.Current.Request.QueryString["file"] != null)
                files = HttpContext.Current.Request.QueryString["file"].ToString();
            file = files.Split(',');

            if (!Global.isDemo && uploadFolder.IndexOf(uploadRootDirectory)==0)
            {
                for (int counter = 0; counter < file.Length; counter++)
                {
                    if (File.Exists(Server.MapPath((uploadFolder + file[counter]))) && file[counter].IndexOf("..") == -1)
                    {
                        File.Delete(Server.MapPath((uploadFolder + file[counter])));
                    }
                }
            }
        }

        private string GetImageDimensions(string path)
        {
            string output = "";
            Bitmap bmpRepresentation;
            try
            {
                bmpRepresentation = new Bitmap(path, false);
                output = bmpRepresentation.Height + " x " + bmpRepresentation.Width;
                bmpRepresentation.Dispose();
            }
            catch {}

            return output;
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
