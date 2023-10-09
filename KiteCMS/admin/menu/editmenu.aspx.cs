using System;
using System.Collections;
using System.Collections.Specialized;
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
using System.Configuration;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for editmenu.
	/// </summary>
	public class editmenu : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action ="";

            // Check for permissions to this module
            admin.userHasAccess(1103,page.Pageid);
			header.Text = Functions.localText("editmenu");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

            if (action == "choosecontent")
                funcChooseContentHolders(page.Pageid);
            else if (action == "doedit")
                funcDoEditMenuItem(page.Pageid);
            else
				funcEditMenuItem();
		}

		private void funcDoEditMenuItem(int pageId)
		{
			string title = "";
			string isPublic = "";
			string friendlyUrl = "";
			int templateId = -1;
            bool saveContentHolders = false;
			ParameterCollection parameters = new ParameterCollection();

			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["public"] != null)
				isPublic = HttpContext.Current.Request["public"].ToString();
			if(HttpContext.Current.Request["template"] != null)
				templateId = int.Parse(HttpContext.Current.Request["template"]);
			if(HttpContext.Current.Request["friendlyurl"] != null && HttpContext.Current.Request["friendlyurl"].ToString() != "")
				friendlyUrl = HttpContext.Current.Request["friendlyurl"];

			// Save the new data
			if (title != "" && isPublic != "" && templateId != -1)
			{
				try
				{
					Page editpage = new Page(pageId,true);
					editpage.Title = title;
					editpage.IsPublic = int.Parse(isPublic);
					editpage.TemplateId = templateId;
					editpage.FriendlyUrl = friendlyUrl;
                    StringDictionary contentholders = new StringDictionary();
                    StringDictionary draftContentholders = new StringDictionary();

					// find extra parameters
					foreach(string key in HttpContext.Current.Request.Form)
					{
						if (key != "friendlyurl" && key != "template" && key != "public" && key != "title" && key != "pageid" && key != "action" && key != "child" && key != "parent")
						{
                            if (key.StartsWith("contentholder_"))
                            {
                                saveContentHolders = true;
                                string currentContentId = key.Replace("contentholder_", "");
                                string newContentId = HttpContext.Current.Request.Form[key];
                                if (HttpContext.Current.Request.Form[key] == "_delete")
                                { }
                                else if (contentholders.ContainsKey(newContentId))
                                {
                                    contentholders[newContentId] += Environment.NewLine + editpage.ContentHolders[currentContentId];
                                    if (editpage.HasDraft == 1)
                                        draftContentholders[newContentId] += Environment.NewLine + editpage.DraftContentHolders[currentContentId];
                                }
                                else
                                {
                                    contentholders.Add(newContentId, editpage.ContentHolders[currentContentId]);
                                    if (editpage.HasDraft == 1)
                                        draftContentholders.Add(newContentId, editpage.DraftContentHolders[currentContentId]);
                                }
                            }
							else if (HttpContext.Current.Request.Form[key] != null && HttpContext.Current.Request.Form[key].ToString() != "")
							{
								Parameter parameter = new Parameter(key, HttpContext.Current.Request.Form[key]);
								parameters.Add(parameter);
							}
						}
					}
					editpage.Parameters = parameters;
                    if (saveContentHolders)
                        editpage.ContentHolders = contentholders;
                    if (editpage.HasDraft == 1)
                        editpage.DraftContentHolders = draftContentholders;

                    editpage.Save();

                    Functions.publicUrlRedirect(pageId, Functions.localText("menusaved"));
				}
				catch
				{
					content.Text += Functions.localText("userfriendlyexists") + Functions.publicUrl();
				}
			}
			else
				throw new Exception ("Add Menuitem: Invalid parameters");

		}

        private void funcChooseContentHolders(int pageId)
        {
            string title = "";
            string isPublic = "";
            string friendlyUrl = "";
            int templateId = -1;
            ParameterCollection parameters = new ParameterCollection();

            if (HttpContext.Current.Request["title"] != null)
                title = HttpContext.Current.Request["title"].ToString();
            if (HttpContext.Current.Request["public"] != null)
                isPublic = HttpContext.Current.Request["public"].ToString();
            if (HttpContext.Current.Request["template"] != null)
                templateId = int.Parse(HttpContext.Current.Request["template"]);
            if (HttpContext.Current.Request["friendlyurl"] != null && HttpContext.Current.Request["friendlyurl"].ToString() != "")
                friendlyUrl = HttpContext.Current.Request["friendlyurl"];

			Page editpage = new Page(pageId);

            // If template is not changes, go directly to save
            if (templateId == editpage.TemplateId)
                funcDoEditMenuItem(pageId);

            else if (title != "" && isPublic != "" && templateId != -1)
            {
                string output;
                Functions functions = new Functions();
                XsltArgumentList xslArg = new XsltArgumentList();

                xslArg.AddExtensionObject("urn:localText", functions);

                // Create temp xml-document
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.CreateXmlDeclaration("1.0", "iso-8859-1", null);
                XmlNode root = xmldoc.CreateElement("website", "");
                xmldoc.AppendChild(root);

                // Insert form-parameters in xml for second step
                foreach (string param in HttpContext.Current.Request.Form)
                    if (param != "action" && param != "child" && param != "parent")
                    {
                        XmlElement elem = xmldoc.CreateElement("formparameter", "");
                        elem.SetAttribute("param", param);
                        XmlCDataSection oXmlCdataNode = xmldoc.CreateCDataSection(HttpContext.Current.Request.Form[param]);
                        elem.AppendChild(oXmlCdataNode);
                        xmldoc.SelectSingleNode("/website").AppendChild(elem);
                    }

                // Insert info on contentholders
                try
                {
                    // old template
                    Template oldTemplate = new Template(editpage.TemplateId);
                    XmlDocument templateXml = new XmlDocument();
                    templateXml.LoadXml(File.ReadAllText(Template.GetFullPath(oldTemplate.Xslurl)));

                    XmlElement elem = xmldoc.CreateElement("oldtemplate", "");
                    XmlNodeList nodes = templateXml.SelectNodes("//Editor");

                    for (int counter = 0; counter < nodes.Count; counter++)
                    {
                        elem.AppendChild(xmldoc.ImportNode(nodes[counter],true));
                    }
                    xmldoc.SelectSingleNode("/website").AppendChild(elem);

                    // new template
                    Template newTemplate = new Template(templateId);
                    templateXml.LoadXml(File.ReadAllText(Template.GetFullPath(newTemplate.Xslurl)));

                    elem = xmldoc.CreateElement("newtemplate", "");
                    nodes = templateXml.SelectNodes("//Editor");

                    for (int counter = 0; counter < nodes.Count; counter++)
                    {
                        elem.AppendChild(xmldoc.ImportNode(nodes[counter], true));
                    }
                    xmldoc.SelectSingleNode("/website").AppendChild(elem);
                }
                catch { }

                output = Functions.transformXml(xmldoc,Server.MapPath("/admin/menu/edit_menu_choose_content.xsl"), xslArg);

                content.Text += output;
            }
        }

        private void funcEditMenuItem()
        {
            string output;
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();

            xslArg.AddExtensionObject("urn:localText", functions);
            if (ConfigurationManager.AppSettings["useUserfriendlyUrl"] != null && bool.Parse(ConfigurationManager.AppSettings["useUserfriendlyUrl"]))
                xslArg.AddParam("useUserfriendlyUrl", "", "true");

            output = Functions.transformMenuXml(Server.MapPath("/admin/menu/edit_menu.xsl"), xslArg);

            content.Text += output;

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
