using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using KiteCMS.Admin;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for image_view.
	/// </summary>
	public class media_view : System.Web.UI.Page
	{
		public string strFolder = "";
		public string strFile = "";

		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"];
            string GraphicRootDirectory = ConfigurationManager.AppSettings["GraphicRootDirectory"];

			string CurrentFolder;
			string CurrentFolderPath;
			string RootFolderPath;
			FileInfo fileinfo;
			StringBuilder output = new StringBuilder();

			GraphicRootDirectory = GraphicRootDirectory.Replace(".","");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"\",@"/");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"//",@"/");

			if(HttpContext.Current.Request["Folder"] != null)
				strFolder = HttpContext.Current.Request["Folder"].ToString();
			if(HttpContext.Current.Request["File"] != null)
				strFile = HttpContext.Current.Request["File"].ToString();

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

			RootFolderPath = HttpContext.Current.Server.MapPath("/");
			CurrentFolderPath = HttpContext.Current.Server.MapPath(strFolder);
			CurrentFolder = CurrentFolderPath.Replace(RootFolderPath,"");

			if(File.Exists(CurrentFolderPath + strFile))
			{
				fileinfo = new FileInfo(CurrentFolderPath + strFile);
				output.Append("<table cellpadding='0' cellspacing='5' border='0'>"+ Environment.NewLine);
				output.Append("<tr><td valign='top'>"+ Environment.NewLine);
				output.Append("<table><tr>"+ Environment.NewLine);
				output.Append("<td align='right'>"+ Functions.localText("mediapath") +":</td>"+ Environment.NewLine);
				output.Append("<td>"+ strFolder + strFile +"</td>"+ Environment.NewLine);
				output.Append("</tr><tr>"+ Environment.NewLine);
				output.Append("<td align='right'>"+ Functions.localText("size") +":</td>"+ Environment.NewLine);
				output.Append("<td>"+ int.Parse((fileinfo.Length/1000).ToString()) +"kb</td>"+ Environment.NewLine);
				output.Append("</tr><tr>"+ Environment.NewLine);
				output.Append("<td align='right' valign='top' style='padding-top:6px'>"+ Functions.localText("openin") +":</td>"+ Environment.NewLine);
                output.Append("<td><select id='strTarget' class='alminput'><option value='_blank' selected='selected'>" + Functions.localText("newwindow") + "</option><option value=''>" + Functions.localText("samewindow") + "</option></select></td>" + Environment.NewLine);
				output.Append("</tr><tr>"+ Environment.NewLine);
				output.Append("<td align='right'>"+ Functions.localText("titletag") +":</td>"+ Environment.NewLine);
				output.Append("<td><input id='titletag' class='alminput'/></td>"+ Environment.NewLine);
				output.Append("</tr></table>"+ Environment.NewLine);

                output.Append("<input type=\"button\" onclick='funcMediaChosen();' value=\"" + Functions.localText("insertlink") + "\" class=\"almknap\"/>" + Environment.NewLine);
                output.Append("&nbsp;&nbsp;<input type=\"button\" onclick='funcMediaCancel();' value=\"" + Functions.localText("cancel") + "\" class=\"almknap\"/><br/>" + Environment.NewLine);

				output.Append("</td></tr></table>"+ Environment.NewLine);

				content.Text = output.ToString();
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
	}
}
