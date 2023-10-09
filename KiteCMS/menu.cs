using System;
using System.Configuration;
using System.Diagnostics;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Xml.Xsl;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using KiteCMS.Admin;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for menu.
	/// </summary>
	public class Menu
	{
		private int pageId = 0;

		public int Pageid
		{
			get
			{
				return pageId;
			}
		}

        public string PageHtml
        {
            get
            {
                return pageHtml();
            }
        }

        public string BlankPageHtml
        {
            get
            {
                return blankPageHtml();
            }
        }

        public Menu(string blnTemplateCheck)
		{
//			int pageId = 0;
			XmlNode oXmlNode;
			XmlNode oXmlNodeParent;
			XmlNode objNodeNoAccess;
			XmlDocument oXmlPassword = new XmlDocument();
			XmlNodeList oXmlNodeRange;
			XmlNodeList oXmlNodeRangeRemove;
			XmlNodeList oXmlNodeRangeMove;
			int intAccesszone = 0;
			int blnAccess = 0;
			int intLogonPageId = 0;

            Trace.Write("start menu");

            // Take a local copy of the global menuXML
			((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml = (XmlDocument)Global.oMenuXml.Clone();

			// Remove not public pages from XML unless you are logged in
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
            {
                oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[public=-1]");
                for (int counter = 0; counter <= oXmlNodeRange.Count - 1; counter++)
                {
                    oXmlNodeParent = oXmlNodeRange.Item(counter).ParentNode;
                    oXmlNode = oXmlNodeParent.RemoveChild(oXmlNodeRange.Item(counter));
                }

            }
			// Insert selected pageId in xml
            // Get pageId frm request and see if it exists
            if (HttpContext.Current.Request["pageId"] != null && 
                int.TryParse(HttpContext.Current.Request["pageId"].ToString(), out pageId))
            {
                oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageId + "]");
                try
                {
                    pageId = int.Parse(oXmlNode.Attributes["id"].Value);
                }
                catch
                {
                    HttpContext.Current.Response.Redirect("/modules/errorpages/404.aspx");
                }
            }
            else
			{
				// No pageId in request, take first visible in menu
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[public=1]");
				pageId = int.Parse(oXmlNode.Attributes["id"].Value);
			}

			// Save pageId in xml
			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageId.ToString();

			
			// extranet check - start with logging in
			if(HttpContext.Current.Session["accesszone"] == null)
				HttpContext.Current.Session["accesszone"] = 0;

			// Find areas the user has access to
			if(HttpContext.Current.Request["extranetUsername"] != null)
			{
                oXmlPassword.Load(Global.publicXmlPath + "/extranet.xml");
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ pageId.ToString() +"]");

				// Find accesszone-id for password page
				oXmlNodeRange = oXmlNode.SelectNodes("ancestor-or-self::page");
				for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				{
					oXmlNode = oXmlNodeRange.Item(counter).SelectSingleNode("password[@aktiv=1]");

					// The page is password protected! Get its zone-id
					if (oXmlNode != null)
					{
						intAccesszone = int.Parse(oXmlNode.Attributes["accesszone"].Value);
						break;
					}
				}
                oXmlNodeRange = oXmlPassword.SelectNodes("//user[login='" + HttpContext.Current.Request["extranetUsername"].ToString().ToLower().Replace("'", "") + "' and password='" + HttpContext.Current.Request["extranetPassword"].ToString().ToLower().Replace("'", "") + "']/accesszone");
                HttpContext.Current.Session["accesszone"] = "0";
                if (oXmlNodeRange != null && oXmlNodeRange.Count > 0)
                {
                    try
                    {
                        HttpContext.Current.Session["extranetfullname"] = oXmlPassword.SelectSingleNode("//user[login='" + HttpContext.Current.Request["extranetUsername"].ToString().ToLower().Replace("'", "") + "' and password='" + HttpContext.Current.Request["extranetPassword"].ToString().ToLower().Replace("'", "") + "']/fullname").InnerText;
                    }
                    catch
                    {
                        HttpContext.Current.Session["extranetfullname"] = HttpContext.Current.Request["extranetUsername"];
                    }
                    for (int counter = 0; counter <= oXmlNodeRange.Count - 1; counter++)
                    {
                        if (oXmlNodeRange.Item(counter) != null)
                            HttpContext.Current.Session["accesszone"] = HttpContext.Current.Session["accesszone"] + "," + oXmlNodeRange.Item(counter).InnerText;
                    }
                }
			}

			// Log out of extranet if wnated
			if (HttpContext.Current.Request["extranetlogout"] != null)
            {
				HttpContext.Current.Session["accesszone"] = 0;
                HttpContext.Current.Session["extranetfullname"] = "";
            }

			// Is the page password protected?
			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ pageId.ToString() +"]");

			// Find accesszone-id for password page
            bool loginPageidFound = false;
			oXmlNodeRange = oXmlNode.SelectNodes("ancestor-or-self::page");
			for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
			{
				oXmlNode = oXmlNodeRange.Item(counter).SelectSingleNode("password[@aktiv=1]");

				// The page is password protected!
				if (oXmlNode != null)
				{
					// Do the user have access to the zone? Check if the user is logged in to the right zone
					string[] arrAccesszone = HttpContext.Current.Session["accesszone"].ToString().Split(',');
					for (int counter2 = 0; counter2 <= arrAccesszone.Length-1;counter2++)
					{
						intLogonPageId = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
						oXmlNode = oXmlNodeRange.Item(counter).SelectSingleNode("password[@accesszone="+ arrAccesszone[counter2] +" or @aktiv=0]");
						if(oXmlNode != null)
							blnAccess = 1;
					}
					// The user does not have acces to the page. Redirect to logon page if not in admin-mode
                    if (!loginPageidFound && blnAccess != 1 && ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
					{
                        loginPageidFound = true;
						pageId = intLogonPageId;

						// Save pageId in xml
						oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//selectedpage");
						oXmlNode.InnerText = pageId.ToString();

					}
				}
			}

			// Remove password protected pages from XML, that the user does not have acces to if the user is not logged in ad admin
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
            {
                oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page/password[@aktiv=1]");
                for (int counter = 0; counter <= oXmlNodeRange.Count - 1; counter++)
                {
                    blnAccess = 0;
                    string[] arrAccesszone = HttpContext.Current.Session["accesszone"].ToString().Split(',');
                    for (int counter2 = 0; counter2 <= arrAccesszone.Length - 1; counter2++)
                    {
                        if (arrAccesszone[counter2] != "")
                        {
                            objNodeNoAccess = oXmlNodeRange.Item(counter).ParentNode.SelectSingleNode("password[@accesszone=" + arrAccesszone[counter2] + "]");
                            if (objNodeNoAccess != null)
                                blnAccess = 1;
                        }
                    }

                    // If the user is not logged in to hte page, remove it
                    if (blnAccess != 1)
                    {
                        oXmlNodeParent = oXmlNodeRange.Item(counter).ParentNode.SelectSingleNode("password").ParentNode;

                        oXmlNodeRangeRemove = oXmlNodeParent.SelectNodes("page");
                        for (int counter2 = 0; counter2 <= oXmlNodeRangeRemove.Count - 1; counter2++)
                        {
                            oXmlNodeParent = oXmlNodeRangeRemove.Item(counter2).ParentNode;
                            oXmlNode = oXmlNodeParent.RemoveChild(oXmlNodeRangeRemove.Item(counter2));
                        }

                    }
                    else
                    {
                        // Is the user logged idn, then remove the login page from menu
                        oXmlNodeParent = oXmlNodeRange.Item(counter).ParentNode.SelectSingleNode("password").ParentNode;
                        if (int.Parse(oXmlNodeParent.Attributes["id"].InnerText) == pageId)
                        {
                            // The user is still on the login page og shall be redirected to the first page under this
                            oXmlNode = oXmlNodeParent.SelectSingleNode("page[public!=-1]");
                            if (oXmlNode != null)
                            {
                                XmlNode oXmlNodeTemp = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id=//page[@id=" + pageId + "]/usetemplate]/publicurl");
                                HttpContext.Current.Response.Redirect(oXmlNodeTemp.InnerText + "?pageid=" + oXmlNode.Attributes["id"].Value);
                            }
                        }

                        if (int.Parse(oXmlNodeParent.Attributes["id"].InnerText) != pageId)
                        {
                            //Login page is removed if it has sub pages
                            oXmlNodeRangeMove = oXmlNodeParent.SelectNodes("page");
                            for (int counter2 = 0; counter2 <= oXmlNodeRangeMove.Count - 1; counter2++)
                            {
                                oXmlNode = oXmlNodeParent.ParentNode.InsertBefore(oXmlNodeRangeMove.Item(counter2), oXmlNodeParent);
                            }
                            oXmlNode = oXmlNodeParent.ParentNode.RemoveChild(oXmlNodeParent);
                        }
                    }
                }
            }

			// Check if the right template is used if the user is not coming from an userfriendly-url
			if ((HttpContext.Current.Request.QueryString["userfriendly"] != null && HttpContext.Current.Request.QueryString["userfriendly"] != "1") || HttpContext.Current.Request.QueryString["userfriendly"] == null)
			{
                // Check if the page has an userfriendly url og and it is called without. Then redirect
                if (ConfigurationManager.AppSettings["useUserfriendlyUrl"] != null && bool.Parse(ConfigurationManager.AppSettings["useUserfriendlyUrl"]) && ConfigurationManager.AppSettings["disableRedirectUserfriendlyUrl"] == null)
                {
                    // Only in public mode
                    if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
                    {
                        // Only redirect if there is no other querystring parameters than pageid
                        if (!HttpContext.Current.Request.QueryString.HasKeys() || (HttpContext.Current.Request.QueryString.Count == 1 && HttpContext.Current.Request.QueryString["pageid"] != null))
                        {
                            oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id=" + pageId.ToString() + "]/friendlyurl");
                            if (oXmlNode != null && oXmlNode.InnerText != null && oXmlNode.InnerText != "")
                                HttpContext.Current.Response.Redirect(oXmlNode.InnerText);
                        }
                    }
                }

				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id=//page[@id="+ pageId +"]/usetemplate]/publicurl");
                if (oXmlNode.InnerText.Replace("\\", "/").ToLower() != HttpContext.Current.Request.ServerVariables["url"].Replace("\\", "/").ToLower() && (oXmlNode.InnerText.Replace("\\", "/") + "default.aspx").ToLower() != HttpContext.Current.Request.ServerVariables["url"].Replace("\\", "/").ToLower() && blnTemplateCheck.ToLower() == "true")
					HttpContext.Current.Response.Redirect (oXmlNode.InnerText +"?"+ HttpContext.Current.Request.QueryString);
			}
		}

		private string pageHtml()
		{
            Functions functions = new Functions();
            ContentFunctions contentFunctions = new ContentFunctions();
            XmlDocument xslDoc = new XmlDocument();
            XmlNode oXmlNode;
            XslCompiledTransform xslt = new XslCompiledTransform();
            XmlResolver resolver = new XmlUrlResolver();
            string output;
            string editorText;
            bool alternativeXSLFile = false;

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id=//page[@id="+ this.pageId +"]/usetemplate]/xslurl");
            string templateUrl=oXmlNode.InnerText;

            // use speciel xsl-file if format-querystring is given and the file exists
            // only in public-mode
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public && HttpContext.Current.Request.QueryString["format"] != null && HttpContext.Current.Request.QueryString["format"] != "")
            {
                if (File.Exists(Admin.Template.GetFullPath(templateUrl.Substring(0, templateUrl.LastIndexOf('.')) + "_" + HttpContext.Current.Request.QueryString["format"] + templateUrl.Substring(templateUrl.LastIndexOf('.')))))
                {
                    templateUrl = templateUrl.Substring(0, templateUrl.LastIndexOf('.')) + "_" + HttpContext.Current.Request.QueryString["format"] + templateUrl.Substring(templateUrl.LastIndexOf('.'));
                    alternativeXSLFile = true;
                }
            }

            xslDoc.Load(Admin.Template.GetFullPath(templateUrl));

            // insert editor into xsl-file
            editorText = Editor.InsertContentHolder(ref xslDoc, this.pageId);
            xslt.Load(new XmlNodeReader(xslDoc),new XsltSettings(false,true), resolver);

			XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

            StringWriterWithEncoding writer = new StringWriterWithEncoding(Encoding.GetEncoding("iso-8859-1"));

            // insert editmode in xml
            XmlDocument xmldoc = (XmlDocument)((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.Clone();
            XmlElement websiteNode = (XmlElement)xmldoc.SelectSingleNode("/website");
            websiteNode.SetAttribute("editmode", ((Global)HttpContext.Current.ApplicationInstance).EditMode.ToString());

            xslt.Transform(xmldoc, xslArg, writer);
            output = writer.ToString();

			if (Global.hasBoxmodule)
			{
				// insert boxes if used on page
				Page page = new Page(this.pageId);
				output = functions.addBoxesToHtml(output, page);
			}

            // add admin html if user is logged in
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode != Global.EditModeEnum.Public)
            {
                output = Editor.InsertStaticAdminHtml(output, editorText, this.pageId);
            }

            // Encode mailto-links and emails in public mode
            if (ConfigurationManager.AppSettings["encodeEmails"] != null && ConfigurationManager.AppSettings["encodeEmails"].ToString().ToLower() == "true" && ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
                output = Functions.EncodeEmails(output);

            // Add extra text to html
            if (!alternativeXSLFile)
            {
                output = Functions.ReplaceString(Functions.GetHtmlHeader() + Environment.NewLine + "</head>", output, "</head>", true);
                output = Functions.ReplaceString(Functions.GetBodyEnd() + Environment.NewLine + "</body>", output, "</body>", true);
            }

            // Send confirmationmail for mailform
            if (HttpContext.Current.Request.QueryString["mailstatus"] != null && HttpContext.Current.Request.QueryString["mailstatus"] == "ok")
                KiteCMS.mailform.SendConfirmMail(pageId);

            // clean output from xmlns=""
            output = output.Replace("xmlns=\"\"", "");
            return output;
		}

        private string blankPageHtml()
        {
            StringBuilder output = new StringBuilder();

            // add admin html if user is logged in
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode != Global.EditModeEnum.Public)
            {
                Admin.Admin admin = new Admin.Admin();
                Admin.Page page = new Admin.Page(this.pageId);
                output.Append(@"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/transitional.dtd"">" + Environment.NewLine);
                output.Append(@"<html><head><title></title><link href=""/admin/default.css"" type=""text/css"" rel=""stylesheet""/><link href=""/admin/adminpages.css"" type=""text/css"" rel=""stylesheet""/>" + Environment.NewLine);

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit || ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                    output.Append(@"<script type=""text/javascript"" src='/admin/editor/includes/functions.js'></script>" + Environment.NewLine);
                output.Append("</head><body>");

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Admin)
                    output.Append(admin.AdminPageHtml + admin.moduleMenu(page));

                output.Append("</body></html>");
                return output.ToString();
            }
            else
                return "";
        }
	}
}
