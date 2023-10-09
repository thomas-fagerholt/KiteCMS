using System;
using System.Collections;
using System.Collections.Specialized;
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
using System.Text;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for editmenu.
	/// </summary>
	public class editdraft : System.Web.UI.Page
	{
		protected Literal content;

		public string PageHtml;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action ="";

            // Check for permissions to this module
            admin.userHasAccess(1302,page.Pageid);

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "save")
				funcDoEditMenuItem(page.Pageid);
		}

		private void funcDoEditMenuItem(int pageId)
        {
            //Make the Html valid using SgmlReader
            StringWriter log = new StringWriter();
            StringDictionary contentHolders = new StringDictionary();
            bool blnContentOK = true;
            bool blnHasEmptyAltTags = false;

            // Save the new data
            // Get all other parameters than the defaults
            foreach (string key in HttpContext.Current.Request.Form)
            {
                if (key != "action" && key != "pageid" && key.IndexOf("tbContent") < 0)
                {
                    contentHolders.Add(key, HttpContext.Current.Request.Form[key]);
                }
                else if (key.IndexOf("tbContent") == 0)
                {
                    string strHtml = HttpContext.Current.Request.Form[key];
                    if (ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
                    {
                        try
                        {
                            strHtml = Functions.CleanEditorCode(strHtml, log, true, true, true, ref blnHasEmptyAltTags);
                        }
                        catch
                        {
                            content.Text = Functions.localText("pagecontentnotvalid") + Functions.publicUrl() + "</p>";
                            blnContentOK = false;
                        }
                    }
                    strHtml = strHtml.Replace("http://" + HttpContext.Current.Request.ServerVariables["server_name"], "");
                    strHtml = Functions.EncodeSpecialChars(strHtml);
                    contentHolders.Add(key.ToLower().Replace("tbcontent_", "").Replace("_temp", ""), strHtml);

                }
            }

            Page editpage = new Page(pageId, true);
            editpage.DraftContentHolders = contentHolders;
            if (blnContentOK)
            {
                editpage.Save();
                if (blnHasEmptyAltTags)
                    content.Text = Functions.localText("pagehasemptyalttags") + Functions.publicUrl() + "</p>";
                else
                    Functions.publicUrlRedirect(pageId, Functions.localText("pagecontentsaved"));
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
