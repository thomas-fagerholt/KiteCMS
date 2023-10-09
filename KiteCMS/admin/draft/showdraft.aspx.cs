using System;
using System.Collections;
using System.ComponentModel;
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
	public class showdraft : System.Web.UI.Page
	{
		protected Literal lbContent;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			// Check for permissions to this module
			admin.userHasAccess(1302,page.Pageid);

			funcShowDraft(page);

		}

		private void funcShowDraft(Page page)
		{
			if (page.HasDraft == 1)
			{
				Menu menu = new Menu("false");

				lbContent.Text += this.draftHtml(page);
			}
			else
				throw new ArgumentException("page object doesn't have a draft");

		}

		private string draftHtml(Page page)
		{
			string output;
			XmlNode oXmlNode;
			Functions functions = new Functions();
			Template template = new Template(page.TemplateId);

			XmlDocument xslDoc = new XmlDocument();

            XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslDoc.Load(Template.GetFullPath(template.Xslurl));

			// Manupulate XSL so the draft is used instead for html
			XmlNamespaceManager nsmgr = new XmlNamespaceManager(xslDoc.NameTable);
			nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
			oXmlNode = xslDoc.SelectSingleNode("//xsl:value-of[@select='html']",nsmgr);
			if (oXmlNode != null)
				oXmlNode.Attributes["select"].Value = ("draft/html");

			xslt.Load(new XmlNodeReader(xslDoc));

			StringWriter writer = new StringWriter();

			xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,null,writer);

			output = writer.ToString();
			if (Global.hasBoxmodule)
			{
				// insert boxes if used on page
                KiteCMS.Page publicPage = new KiteCMS.Page(page.Pageid);
				output = functions.addBoxesToHtml(output, publicPage);

			}
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
