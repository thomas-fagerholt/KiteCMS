using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace KiteCMS.modules
{
    class Comments
    {

        public Comments()
        {
        }

        public string Menu(Admin.Admin admin, Admin.Page page)
        {
            // Show list of available methods
            return funcList(admin, page);
        }

        public string Public(int pageId)
		{
			// Entry point for the public part of the module
			// Input parameters is the page object for the current page
			// Return parameter is html-code to put in the content area of the page

			string header ="";
			string author ="";
			string comment ="";
            string action = "";
			StringBuilder mailBody = new StringBuilder();
			int maxId = 0;
			int blnLive = 1;
            bool useCaptcha = false;
            string ccMail = "";
            XmlNodeList oXmlNodeRange;
			XmlNode oXmlCategoryRoot;
			XmlDocument xmlComments = new XmlDocument();

            xmlComments = OXmlComments(pageId);

            if (HttpContext.Current.Request.QueryString["action"] != null)
                action = HttpContext.Current.Request.QueryString["action"].ToString();
            if (action == "" && HttpContext.Current.Request.Form["action"] != null)
                action = HttpContext.Current.Request.Form["action"].ToString();

            if (HttpContext.Current.Request.Form["fieldauthor"] != null)
                author = Functions.RemoveTags(HttpContext.Current.Request.Form["fieldauthor"].ToString());
            if (HttpContext.Current.Request.Form["fieldheader"] != null)
                header = Functions.RemoveTags(HttpContext.Current.Request.Form["fieldheader"].ToString());
            if (HttpContext.Current.Request.Form["fieldcomment"] != null)
                comment = Functions.RemoveTags(HttpContext.Current.Request.Form["fieldcomment"].ToString());

            if (ConfigurationManager.AppSettings["useCaptcha"] != null)
                useCaptcha = bool.Parse(ConfigurationManager.AppSettings["useCaptcha"]);

            if (ConfigurationManager.AppSettings["commentsEmailfieldToCC"] != null)
                ccMail = ConfigurationManager.AppSettings["commentsEmailfieldToCC"];

            // Find rootpageid
            int rootpageid = -1;
            bool rootfound = false;
            XmlNodeList nodes = xmlComments.SelectNodes("//comments");
            foreach (XmlNode node in nodes)
            {
                try
                {
                    int pageid = int.Parse(node.Attributes["rootpageid"].Value);
                    XmlNode rootnode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageid + "]//page[@id=" + pageId + "]");
                    if (rootnode != null)
                    {
                        rootfound = true;
                        rootpageid = pageid;
                    }
                }
                catch { }
            }
            
            // Insert new comment
            if (author != "" && comment != "")
            {
                // if CaptchaFixedValue is given, check if value is in request
                if (useCaptcha && ConfigurationManager.AppSettings["CaptchaFixedValue"] != null)
                {
                    if (HttpContext.Current.Request["captcha"] != null && HttpContext.Current.Request["captcha"] == ConfigurationManager.AppSettings["CaptchaFixedValue"])
                        useCaptcha = false;
                }

                if (useCaptcha && (HttpContext.Current.Request["captcha"] == null || HttpContext.Current.Session["CaptchaImageText"] == null || HttpContext.Current.Request["captcha"].CompareTo(HttpContext.Current.Session["CaptchaImageText"].ToString()) != 0))
                {
                    if (ConfigurationManager.AppSettings["useCaptchaErrortext"] != null)
                        HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["useCaptchaErrortext"]);
                    else
                        HttpContext.Current.Response.Write("Control characters (Captcha) not correct. Please use the \"back\"-button to return to the page<br/>");
                    HttpContext.Current.Response.End();
                }
                else
                {
                    if (rootfound)
                    {
                        // Is the category live?
                        oXmlCategoryRoot = xmlComments.SelectSingleNode("//comments[@rootpageid="+ rootpageid +"]");
                        if (oXmlCategoryRoot != null)
                            blnLive = int.Parse(oXmlCategoryRoot.Attributes["islive"].Value);

                        // Start to find max id
                        oXmlNodeRange = xmlComments.GetElementsByTagName("commentItem");
                        foreach (XmlNode xmlNode in oXmlNodeRange)
                            if (int.Parse(xmlNode.Attributes["id"].Value) > maxId)
                                maxId = int.Parse(xmlNode.Attributes["id"].Value);

                        // Insert comment in the xml
                        XmlElement oXmlElem;
                        XmlElement oXmlElemNew;
                        XmlCDataSection oXmlCdata;
                        XmlAttribute oXmlAtr;
                        string emailsender;
                        string emailsubscriber;

                        oXmlElem = xmlComments.CreateElement("commentItem", "");
                        oXmlAtr = oXmlElem.SetAttributeNode("pageid", "");
                        oXmlAtr.Value = pageId.ToString();
                        oXmlAtr = oXmlElem.SetAttributeNode("id", "");
                        maxId++;
                        oXmlAtr.Value = maxId.ToString();
                        oXmlAtr = oXmlElem.SetAttributeNode("created", "");
                        oXmlAtr.Value = DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                        oXmlAtr = oXmlElem.SetAttributeNode("active", "");
                        oXmlAtr.Value = blnLive.ToString();

                        oXmlElemNew = xmlComments.CreateElement("item", "");
                        oXmlAtr = oXmlElemNew.SetAttributeNode("type", "");
                        oXmlAtr.Value = "header";
                        oXmlCdata = xmlComments.CreateCDataSection(header);
                        oXmlElemNew.AppendChild(oXmlCdata);
                        oXmlElem.AppendChild(oXmlElemNew);

                        oXmlElemNew = xmlComments.CreateElement("item", "");
                        oXmlAtr = oXmlElemNew.SetAttributeNode("type", "");
                        oXmlAtr.Value = "author";
                        oXmlCdata = xmlComments.CreateCDataSection(author);
                        oXmlElemNew.AppendChild(oXmlCdata);
                        oXmlElem.AppendChild(oXmlElemNew);

                        oXmlElemNew = xmlComments.CreateElement("item", "");
                        oXmlAtr = oXmlElemNew.SetAttributeNode("type", "");
                        oXmlAtr.Value = "comment";
                        oXmlCdata = xmlComments.CreateCDataSection(comment.Replace(Environment.NewLine, "<br/>"));
                        oXmlElemNew.AppendChild(oXmlCdata);
                        oXmlElem.AppendChild(oXmlElemNew);

                        oXmlCategoryRoot.AppendChild(oXmlElem);

                        // Save the xml
                        xmlComments.Save(Global.publicXmlPath + "/Comments.xml");

                        // Send mail to the administrator
                        emailsubscriber = oXmlCategoryRoot.SelectSingleNode("adminemail").InnerText;

                        string domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
                        if (domain.Split('.').Length == 3)
                            domain = domain.Substring(domain.IndexOf(".") + 1);

                        if (ConfigurationManager.AppSettings["mailFrom"] != null)
                            emailsender = ConfigurationManager.AppSettings["mailFrom"].ToString();
                        else
                            emailsender = "info@" + domain;

                        if (emailsubscriber != null)
                        {
                            mailBody.Append("New comment with heading: " + header + "\n");
                            mailBody.Append("by : " + author+ "\n");
                            mailBody.Append("and content: " + comment + "\n");
                            mailBody.Append("http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString() + "/default.aspx?pageId=" + pageId + "&action=showcomment\n");
                            
                            KiteCMS.Functions.SendMail("New comment on " + domain, "ADMIN:\n" + mailBody.ToString(), emailsubscriber, emailsender);
                        }
                        if (!string.IsNullOrEmpty(ccMail))
                        {
                            XmlNode pagenode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageId + "]/contentholders/" + ccMail);
                            if (pagenode != null && pagenode.FirstChild != null)
                            {
                                KiteCMS.Functions.SendMail("New comment on " + domain, "ADMIN:\n" + mailBody.ToString(), pagenode.FirstChild.InnerText, emailsender);
                            }
                        }
                    }
                    else
                        throw new Exception("Commentmodule is missing data");
                }

                // Rediect to self so that the comment is not created again on reload
                HttpContext.Current.Response.Redirect("default.aspx?action=commentadded&commentadded=true&pageid="+ pageId.ToString());
            }
			

			// Load xsl to show the page
			XslTransform xslt = new XslTransform();
			XmlUrlResolver resolver = new XmlUrlResolver();
            ContentFunctions contentFunctions = new ContentFunctions();

			xslt.Load(HttpContext.Current.Server.MapPath("/modules/comments/comments.xsl"));

			StringWriter writer = new StringWriter();

			XsltArgumentList args = new XsltArgumentList();
            args.AddParam("action", "", action);
            args.AddParam("pageid", "", pageId);
            args.AddParam("rootpageid", "", rootpageid);
            args.AddExtensionObject("urn:contentFunctions", contentFunctions);

			xslt.Transform(xmlComments,args,writer,resolver);
			return writer.ToString();

		}

		public string Admin(Admin.Admin admin, Admin.Page page, out bool showMenu, out bool showModuleMenu)
		{
			// Entry point for the administration part of the module
			// Input parameters is the admin and page object for the current page
			// Output parameters is booleans for showing the left menu and the right module menu
			// Return parameter is html-code to put in the content area of the page

			string action="";
			string output = "";

			// Find out what to do passed on the action-field
			if(HttpContext.Current.Request.QueryString["action"] != null)
				action = HttpContext.Current.Request.QueryString["action"];
			if(HttpContext.Current.Request.Form["action"] != null && action == "")
				action = HttpContext.Current.Request.Form["action"];

			switch(action)
			{
                case ("editcomments"):
				{
                    output = editComments(admin, page);
					showMenu = false;
					showModuleMenu = false;
					break;
				}
                case ("doeditcommentdata"):
				{
                    output = doEditComments(admin, page);
					showMenu = false;
					showModuleMenu = false;
					break;
				}
                case ("formactivatecomment"):
                {
                    output = formActiveComment(admin, page);
                    showMenu = false;
                    showModuleMenu = false;
                    break;
                }
                case ("doactivatecomment"):
                {
					output = doActiveDebate(admin, page);
                    showMenu = false;
                    showModuleMenu = false;
                    break;
                }
                case ("formdeletecomment"):
				{
                    output = formDeleteComment(admin, page);
                    showMenu = false;
					showModuleMenu = false;
					break;
				}
                case ("dodeletecomment"):
                {
                    output = doDeleteDebate(admin, page);
                    showMenu = false;
                    showModuleMenu = false;
                    break;
                }
            default:
				{
					// Show list of available methods
					output = funcList(admin, page);
					showMenu= true;
					showModuleMenu = true;
					break;
				}
			}

			return output;
		}

        private string doActiveDebate(KiteCMS.Admin.Admin admin, KiteCMS.Admin.Page page)
        {
            int commentid = -1;

            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3203, page.Pageid);

            if (HttpContext.Current.Request.QueryString["commentid"] != null)
                commentid = int.Parse(HttpContext.Current.Request.QueryString["commentid"]);

            // append headers to output
            output.Append("<h2>" + Functions.localText("comment") + "</h2>");
            output.Append("<h3>" + Functions.localText("activatecomment") + "</h3>");

            if (commentid != -1)
            {
                XmlDocument oXml = new XmlDocument();
                XmlNode xmlnode;

                // Load the xml-file holding data on debate
                oXml = OXmlComments(page.Pageid);

                // Activate debateitem
                xmlnode = oXml.SelectSingleNode("//commentItem[@id='" + commentid + "']");

                if (xmlnode != null && xmlnode.Attributes["active"].Value == "0")
                {
                    // set item as active
                    xmlnode.Attributes["active"].Value = "1";

                    output.Append(Functions.localText("commentactivated") + Functions.publicUrl());

                    // Save the xml
                    oXml.Save(Global.publicXmlPath + "/comments.xml");
                }
            }

            return output.ToString();
        }

        private string doDeleteDebate(KiteCMS.Admin.Admin admin, KiteCMS.Admin.Page page)
        {
            int commentid = -1;

            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3204, page.Pageid);

            if (HttpContext.Current.Request.QueryString["commentid"] != null)
                commentid = int.Parse(HttpContext.Current.Request.QueryString["commentid"]);

            // append headers to output
            output.Append("<h2>" + Functions.localText("comment") + "</h2>");
            output.Append("<h3>" + Functions.localText("deletecomment") + "</h3>");

            if (commentid != -1)
            {
                XmlDocument oXml = new XmlDocument();
                XmlNode xmlnode;

                // Load the xml-file holding data on debate
                oXml = OXmlComments(page.Pageid);

                // Activate debateitem
                xmlnode = oXml.SelectSingleNode("//commentItem[@id='" + commentid + "']");

                if (xmlnode != null)
                {
                    XmlNode oXmlNodeParent;

                    oXmlNodeParent = xmlnode.ParentNode;
                    oXmlNodeParent.RemoveChild(xmlnode);

                    output.Append(Functions.localText("commentdeleted") + Functions.publicUrl());

                    // Save the xml
                    oXml.Save(Global.publicXmlPath + "/comments.xml");
                }
            }

            return output.ToString();
        }

        private string editComments(Admin.Admin admin, Admin.Page page)
        {
            int rootpageid = -1;
            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3201, page.Pageid);

            // append headers to output
            output.Append("<h2>" + Functions.localText("comments") + "</h2>");
            output.Append("<h3>" + Functions.localText("addcomments") + "</h3>");

            XmlDocument oXml = new XmlDocument();
            XslTransform xslt = new XslTransform();
            StringWriter writer = new StringWriter();

            // Load the xml-file holding data on debate
            oXml = OXmlComments(page.Pageid);

            //find rootpageid
            bool rootfound = false;
            XmlNodeList nodes = oXml.SelectNodes("//comments");
            foreach (XmlNode node in nodes)
            {
                try
                {
                    int pageid = int.Parse(node.Attributes["rootpageid"].Value);
                    XmlNode rootnode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageid + "]//page[@id=" + page.Pageid + "]");
                    if (rootnode != null)
                    {
                        rootfound = true;
                        rootpageid = pageid;
                    }
                }
                catch { }
            }
            if (!rootfound)
            {
                rootpageid = page.ParentPageid;
            }

            // Load the xsl-file to use for the current transformation
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/comments/form_edit_commentData.xsl"));
            XmlUrlResolver resolver = new XmlUrlResolver();

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("rootpageid", "", rootpageid);

            // Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXml, xslArg, writer, resolver);
            output.Append(writer.ToString());

            return output.ToString();
        }

        private string doEditComments(Admin.Admin admin, Admin.Page page)
        {
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            StringBuilder output = new StringBuilder();
            XmlDocument xmlComments = new XmlDocument();
            XmlNode xmlNode;
            XmlCDataSection xmlCdata;
            XmlNode xmlParentNode;

            string adminemail = "";
            int rootpageid = -1;
            string thankyoutext = "";

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3201, page.Pageid);

            // Append headers to output
            output.Append("<h2>" + Functions.localText("comments") + "</h2>");
            output.Append("<h3>" + Functions.localText("addcomments") + "</h3>");

            // Get the Xml
            xmlComments = OXmlComments(page.Pageid);

            if (HttpContext.Current.Request.Form["adminemail"] != null)
                adminemail = HttpContext.Current.Request.Form["adminemail"];
            if (HttpContext.Current.Request.Form["thankyoutext"] != null)
                thankyoutext = HttpContext.Current.Request.Form["thankyoutext"];

            //find rootpageid
            bool rootfound = false;
            XmlNodeList nodes = xmlComments.SelectNodes("//comments");
            foreach (XmlNode node in nodes)
            {
                try
                {
                    int pageid = int.Parse(node.Attributes["rootpageid"].Value);
                    XmlNode rootnode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageid + "]//page[@id=" + page.Pageid + "]");
                    if (rootnode != null)
                    {
                        rootfound = true;
                        rootpageid = pageid;
                    }
                }
                catch { }
            }
            if (!rootfound)
            {
                rootpageid = page.ParentPageid;   
            }


            if (adminemail != "" && thankyoutext != "")
            {
                if (rootfound)
                {
                    xmlParentNode = xmlComments.SelectSingleNode("//comments[@rootpageid=" + rootpageid + "]");
                    xmlParentNode.SelectSingleNode("adminemail").FirstChild.InnerText = adminemail;
                    xmlParentNode.SelectSingleNode("thankyoutext").FirstChild.InnerText = thankyoutext;
                }
                else
                {
                    xmlParentNode = xmlComments.SelectSingleNode("/website");

                    XmlElement xmlNodeRoot = xmlComments.CreateElement("comments", "");
                    xmlNodeRoot.SetAttribute("rootpageid",rootpageid.ToString());
                    xmlNodeRoot.SetAttribute("islive","1");
                    xmlParentNode.AppendChild(xmlNodeRoot);

                    xmlNode = xmlComments.CreateElement("adminemail", "");
                    xmlCdata = xmlComments.CreateCDataSection(adminemail);
                    xmlNode.AppendChild(xmlCdata);
                    xmlNodeRoot.AppendChild(xmlNode);

                    xmlNode = xmlComments.CreateElement("thankyoutext", "");
                    xmlCdata = xmlComments.CreateCDataSection(thankyoutext);
                    xmlNode.AppendChild(xmlCdata);
                    xmlNodeRoot.AppendChild(xmlNode);
                }

                xmlComments.Save(Global.publicXmlPath + @"/comments.xml");
                output.Append(Functions.localText("informationsaved") + Functions.publicUrl());

                return output.ToString();
            }
            else
                throw new Exception("Error reading comments data.");
        }

        private string formActiveComment(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3203, page.Pageid);


            // append headers to output
            output.Append("<h2>" + Functions.localText("comment") + "</h2>");
            output.Append("<h3>" + Functions.localText("activatecomment") + "</h3>");

            XmlDocument oXml = new XmlDocument();
            XslTransform xslt = new XslTransform();
            StringWriter writer = new StringWriter();

            // Load the xml-file holding data on debate
            oXml = OXmlComments(page.Pageid);

            // Load the xsl-file to use for the current transformation
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/comments/list_activate_comment.xsl"));
            XmlUrlResolver resolver = new XmlUrlResolver();

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("pageid", "", page.Pageid);
            xslArg.AddParam("dateShowNew", "", DateTime.Today.AddDays(0 - int.Parse(ConfigurationManager.AppSettings["DebateDaysAreNew"])).ToString("dd-MM-yy hh:mm"));

            // Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXml, xslArg, writer, resolver);
            output.Append(writer.ToString());

            return output.ToString();
        }

        private string formDeleteComment(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3204, page.Pageid);


            // append headers to output
            output.Append("<h2>" + Functions.localText("comment") + "</h2>");
            output.Append("<h3>" + Functions.localText("deletecomment") + "</h3>");

            XmlDocument oXml = new XmlDocument();
            XslTransform xslt = new XslTransform();
            StringWriter writer = new StringWriter();

            // Load the xml-file holding data on debate
            oXml = OXmlComments(page.Pageid);

            // Load the xsl-file to use for the current transformation
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/comments/list_delete_comment.xsl"));
            XmlUrlResolver resolver = new XmlUrlResolver();

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("pageid", "", page.Pageid);
            xslArg.AddParam("dateShowNew", "", DateTime.Today.AddDays(0 - int.Parse(ConfigurationManager.AppSettings["DebateDaysAreNew"])).ToString("dd-MM-yy hh:mm"));

            // Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXml, xslArg, writer, resolver);
            output.Append(writer.ToString());

            return output.ToString();
        }

        private string funcList(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();
            Admin.Template template = new Admin.Template(page.TemplateId);
            
            // append headers to output
            output.Append("<h2>" + Functions.localText("comments") + "</h2><ul>");

            // check for rights to methods in this module and list methods that the user have access to
//            if (admin.userHasAccess(2401, page.Pageid, false))
//                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=addcomments'>" + Functions.localText("addcomments") + "</a></li>");
            if (admin.userHasAccess(3202, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=editcomments'>" + Functions.localText("editcomments") + "</a></li>");
            if (admin.userHasAccess(3203, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=formactivatecomment'>" + Functions.localText("activatecomment") + "</a></li>");
            if (admin.userHasAccess(3204, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=formdeletecomment'>" + Functions.localText("deletecomment") + "</a></li>");
            output.Append("</ul>");

            return output.ToString();
        }

        private XmlDocument OXmlComments(int pageid)
        {
            XmlDocument output = new XmlDocument();
            XmlNode oXmlNode;

            // Load the xml-file holding data
            output.Load(Global.publicXmlPath + "/comments.xml");

            // Insert current pageid in the xml
            oXmlNode = output.SelectSingleNode("//selectedpage");
            oXmlNode.InnerText = pageid.ToString();

            return output;
        }
    }
}
