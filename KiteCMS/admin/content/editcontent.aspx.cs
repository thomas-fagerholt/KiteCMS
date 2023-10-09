using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Configuration;
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
using System.Text.RegularExpressions;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for editmenu.
	/// </summary>
	public class editcontent : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;
		protected System.Web.UI.WebControls.Label divHidden;
		protected Literal clientScript;
		protected Literal litStylesheet;
		protected System.Web.UI.UserControl IEeditor;
		protected System.Web.UI.UserControl NSeditor;

		public string PageHtml;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action ="";

			// Check for permissions to this module
			admin.userHasAccess(1201,page.Pageid);
			header.Text = Functions.localText("editcontent");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "save")
				funcDoEditMenuItem(page.Pageid);
			else
			{
				litStylesheet.Text = @"<LINK href="""+ Global.DefaultStylesheet +@""" type=""text/css"" rel=""stylesheet"">";
				funcEditContent(page);
			}
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
                    if (Global.ValidXhtml)
                    {
                        try
                        {
                            strHtml = Functions.CleanEditorCode(strHtml, log, true, true, true, ref blnHasEmptyAltTags);
                        }
                        catch
                        {
                            content.Text = Functions.localText("pagecontentnotvalid") + Functions.publicUrl() +"</p>";
                            blnContentOK = false;
                        }
                    }
                    strHtml = strHtml.Replace("http://" + HttpContext.Current.Request.ServerVariables["server_name"], "");
                    strHtml = Functions.EncodeSpecialChars(strHtml);
                    contentHolders.Add(key.Replace("tbContent_", "").Replace("_temp", ""), strHtml);

                }
            }

            Page editpage = new Page(pageId, true);
            editpage.ContentHolders = contentHolders;
            if (blnContentOK)
            {
                editpage.Save();
                if (blnHasEmptyAltTags && Global.WAI)
                    content.Text = Functions.localText("pagehasemptyalttags") + Functions.publicUrl() + "</p>";
                else
                    Functions.publicUrlRedirect(pageId, Functions.localText("pagecontentsaved"));
            }
        }

		private void funcEditContent(Page page)
		{
			Functions functions = new Functions();
			bool blnEdit = false;
			header.Visible = false;

			if (page != null)
			{
				PageHtml = page.ContentHolders["html"];
				PageHtml = PageHtml.Replace("<html>","");

				// Make the Html valid using SgmlReader
				StringWriter log = new StringWriter();
				try 
				{
                    bool na = false;
					PageHtml = Functions.CleanEditorCode(PageHtml, log, true, true, false, ref na);
				}
				catch {}

				// HTML-mode is not possible when the page contains script
				if (PageHtml.ToLower().IndexOf("<script")==-1 && PageHtml.ToLower().IndexOf("<form")==-1 && PageHtml.ToLower().IndexOf("<object")==-1)
					blnEdit = true;

				if (blnEdit)
					PageHtml ="<html><head><head><body>"+ PageHtml +"</body></html>";
	
				clientScript.Text = writeJavascriptBlock(blnEdit);

				divHidden.Text += "<input type='hidden' name='pageid' value='"+ page.Pageid +"'>";

				if (Session["IEBrowser"].ToString() == "True")
				{
					IEeditor.Visible = true;
					NSeditor.Visible = false;
				}
				else
				{
					IEeditor.Visible = false;
					NSeditor.Visible = true;				
				}
			}
			else
				throw new Exception ("Edit content: Page object missing");
		}

		private string writeJavascriptBlock(bool blnEdit)
		{
			StringBuilder javascript = new StringBuilder();

			javascript.Append("<script type=\"text/javascript\">"+ Environment.NewLine);
			javascript.Append("var blnBorder;"+ Environment.NewLine);
			javascript.Append("var strMode;"+ Environment.NewLine);
			javascript.Append("var arrAnchors = new Array;"+ Environment.NewLine);
			javascript.Append("function start(){"+ Environment.NewLine);
			javascript.Append("try {"+ Environment.NewLine);
			javascript.Append("if (!document.all){"+ Environment.NewLine);
			javascript.Append("document.getElementById('tbContent').contentWindow.document.designMode = \"on\";"+ Environment.NewLine);
			javascript.Append("document.getElementById('tbContent').contentWindow.document.execCommand(\"useCSS\", false, false);"+ Environment.NewLine);
			javascript.Append("}"+ Environment.NewLine);
			javascript.Append("blnBorder=0;"+ Environment.NewLine);

			if (blnEdit)
			{
				javascript.Append("strMode = 'html';"+ Environment.NewLine);
				javascript.Append("if (document.all)"+ Environment.NewLine);
				javascript.Append("document.getElementById('tbContent').innerHTML = unescape(stredit);"+ Environment.NewLine);
				javascript.Append("else"+ Environment.NewLine);
				javascript.Append("document.getElementById('tbContent').contentWindow.document.body.innerHTML = unescape(stredit);"+ Environment.NewLine);
			}
			else
			{
				javascript.Append("strMode = 'text';"+ Environment.NewLine);
				javascript.Append("if (document.all){document.getElementById('tbContent').innerText = unescape(stredit);} else {var html = document.createTextNode(unescape(stredit));	document.getElementById('tbContent').contentWindow.document.body.innerHTML = \"\";document.getElementById('tbContent').contentWindow.document.body.appendChild(html);	}"+ Environment.NewLine);
				javascript.Append("alert('"+ Functions.localText("hasjavascript") +"');"+ Environment.NewLine);
				javascript.Append("var objects = document.getElementsByTagName('DIV');"+ Environment.NewLine);
				javascript.Append("for( i = 0; i < objects.length; i++ ) {"+ Environment.NewLine);
				javascript.Append("if(objects[i].id!='divSave' && objects[i].id!='divCancel' && objects[i].id!='divHidden' && objects[i].id!='divText' && objects[i].className=='tbButton'){"+ Environment.NewLine);
				javascript.Append("objects[i].disabled=true;"+ Environment.NewLine);
				javascript.Append("objects[i].style.visibility = 'hidden';"+ Environment.NewLine);
				javascript.Append("}}"+ Environment.NewLine);
				javascript.Append("document.getElementById('selectClass').disabled=true;"+ Environment.NewLine);
			}
            javascript.Append("document.getElementById('KiteCMSToolbar').style.visibility='visible';" + Environment.NewLine);
			javascript.Append("} catch(e) {alert('"+ Functions.localText("editorerror") +"')}"+ Environment.NewLine);
			javascript.Append("}</script>"+ Environment.NewLine);

			return javascript.ToString();

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
