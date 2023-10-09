using KiteCMS.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace KiteCMS.modules
{
    /// <summary>
    /// Summary description for extranet.
    /// </summary>
    public class Newsletter
	{

		public Newsletter()
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

			string action="";
			string output;
			string email = "";
			string name = "";

			// Find out what to do passed on the action-field
			if(HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"];
			if(HttpContext.Current.Request["email"] != null)
				email = HttpContext.Current.Request["email"];
			if(HttpContext.Current.Request["name"] != null)
				name = HttpContext.Current.Request["name"];

            if (HttpContext.Current.Request.QueryString["format"] != null && HttpContext.Current.Request.QueryString["format"].ToLower() == "rss")
                action = "rss";
            
            if (email != "" && action == "")
				action = "unsubscribe";

			switch(action)
			{
                case ("rss"):
                {
                    output = getRss(pageId);
                    break;
                }
				case("subscribe"):
				{
					output = doSubscribe(pageId, email, name);
					output += listNewsletter(pageId);
					break;
				}
				case("unsubscribe"):
				{
					output = doUnsubscibe(pageId, email);
					output += listNewsletter(pageId);
					break;
				}
				default:
				{
					output = listNewsletter(pageId);
					break;
				}
			}

			return output;
		}

		public string Admin(Admin.Admin admin, Admin.Page page, out bool showMenu, out bool showModuleMenu)
		{
			// Entry point for the administration part of the module
			// Input parameters is the admin and page object for the current page
			// Output parameters is booleans for showing the left menu and the right module menu
			// Return parameter is html-code to put in the content area of the page

			string action="";
			string output;

			// Find out what to do passed on the action-field
			if(HttpContext.Current.Request.QueryString["action"] != null)
				action = HttpContext.Current.Request.QueryString["action"];
			if(HttpContext.Current.Request.Form["action"] != null && action == "")
				action = HttpContext.Current.Request.Form["action"];

			switch (action)
			{
				case ("dosend"):
					{
						output = doSendNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("testsend"):
					{
						output = doSendTestNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("sendnewsletter"):
					{
						output = sendNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("formsendnewsletter"):
					{
						output = formSendNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("doeditnewsletter"):
					{
						output = doEditNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("editnewsletter"):
					{
						output = editNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("editsubscriber"):
					{
						output = editSubscriber(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("formeditsubscriber"):
					{
						output = formEditSubscriber(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("removesubscriber"):
					{
						output = removeSubscriber(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("formeditnewsletter"):
					{
						output = formEditNewsletter(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("addcategory"):
					{
						output = formAddCategory(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("doaddcategory"):
					{
						output = doAddCategory(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("editarchive"):
					{
						output = editArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("formaddarchive"):
					{
						output = formAddArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("doaddarchive,save"):
					{
						output = doAddArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("formeditarchive"):
					{
						output = formEditArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("doeditarchive,save"):
					{
						output = doEditArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				case ("dodeletearchive"):
					{
						output = doDeleteArchive(admin, page);
						showMenu = false;
						showModuleMenu = false;
						break;
					}
				default:
					{
						// Show list of available methods
						output = funcList(admin, page);
						showMenu = true;
						showModuleMenu = true;
						break;
					}
			}

			return output;
		}

        private string getRss(int pageId)
        {
            string title = "";
            string description = "";
            XmlNode xmlNode;
            XmlCDataSection cdataNode;

            string domainname = "http://" + HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            Page page = new Page(pageId);
            Template template = new Template(page.TemplateId);

            XmlDocument xmlNewsletter = OXmlNewsletter(pageId);
            xmlNode = xmlNewsletter.SelectSingleNode("/website/emailcategory[@pageid=//selectedpage]/rsstitle");
            if (xmlNode != null)
                title = xmlNode.InnerText;
            xmlNode = xmlNewsletter.SelectSingleNode("/website/emailcategory[@pageid=//selectedpage]/rssdescription");
            if (xmlNode != null)
                description = xmlNode.InnerText;

            XmlDocument xmlOutput = new XmlDocument();
            xmlOutput.CreateXmlDeclaration("1.0", "iso-8859-1", null);

            XmlElement root = xmlOutput.CreateElement("element", "rss", "");
            root.SetAttributeNode("version", "").Value = "2.0";
            xmlOutput.AppendChild(root);

            XmlElement channel = xmlOutput.CreateElement("element", "channel", "");

            xmlNode = xmlOutput.CreateNode("element", "title", "");
            xmlNode.InnerText = title;
            channel.AppendChild(xmlNode);

            xmlNode = xmlOutput.CreateNode("element", "link", "");
            xmlNode.InnerText = domainname + template.Publicurl + "?pageid=" + pageId;
            channel.AppendChild(xmlNode);

            xmlNode = xmlOutput.CreateNode("element", "language", "");
            xmlNode.InnerText = "da-dk";
            channel.AppendChild(xmlNode);

            xmlNode = xmlOutput.CreateNode("element", "description", "");
            cdataNode = xmlOutput.CreateCDataSection(description);
            xmlNode.AppendChild(cdataNode);
            channel.AppendChild(xmlNode);

            xmlNode = xmlOutput.CreateNode("element", "pubDate", "");
            xmlNode.InnerText = DateTime.Now.ToString("r");
            channel.AppendChild(xmlNode);

            XmlNodeList nodes = xmlNewsletter.SelectNodes("/website/emailcategory[@pageid=//selectedpage]/newsitem");
            List<XmlNode> list = new List<XmlNode>();
            foreach (XmlNode node in nodes)
            {
                list.Add(node);
            }
            list.Sort(Compare);

            foreach (XmlNode node in list)
            {
                XmlElement xmlItem = xmlOutput.CreateElement("element", "item", "");

                xmlNode = xmlOutput.CreateNode("element", "title", "");
                xmlNode.InnerText = node.SelectSingleNode("title").InnerText;
                xmlItem.AppendChild(xmlNode);

                xmlNode = xmlOutput.CreateNode("element", "description", "");
                xmlNode.InnerXml = node.SelectSingleNode("content").InnerXml;
                xmlItem.AppendChild(xmlNode);

                xmlNode = xmlOutput.CreateNode("element", "pubDate", "");
                xmlNode.InnerText = DateTime.Parse(node.Attributes["created", ""].Value).ToString("r");
                xmlItem.AppendChild(xmlNode);

                xmlNode = xmlOutput.CreateNode("element", "link", "");
                xmlNode.InnerText = domainname + template.Publicurl + "?pageid=" + pageId + "&newsid=" + node.Attributes["id"].Value;
                xmlItem.AppendChild(xmlNode);

                xmlNode = xmlOutput.CreateNode("element", "guid", "");
                xmlNode.InnerText = domainname + template.Publicurl + "?pageid=" + pageId + "&newsid=" + node.Attributes["id"].Value;
                xmlItem.AppendChild(xmlNode);

                channel.AppendChild(xmlItem);
            }

            root.AppendChild(channel);

            return xmlOutput.OuterXml;
        }

        private int Compare(XmlNode nodeOne, XmlNode nodeTwo)
        {
            return -1 * DateTime.Parse(nodeOne.Attributes["created"].Value).CompareTo(DateTime.Parse(nodeTwo.Attributes["created"].Value));
        }

		private string listNewsletter(int pageId)
		{
			string newsid = "-1";
			XmlDocument xmlNewsletter = new XmlDocument();

			if (HttpContext.Current.Request.QueryString["newsid"] != null)
				newsid = HttpContext.Current.Request.QueryString["newsid"].ToString();

			// Get the Xml
			xmlNewsletter = OXmlNewsletter(pageId);

			StringWriter writer = new StringWriter();
			XsltArgumentList args = new XsltArgumentList();
            ContentFunctions contentFunctions = new ContentFunctions();
            args.AddExtensionObject("urn:contentFunctions", contentFunctions);
            args.AddParam("newsid", "", newsid);
            args.AddParam("today", "", DateTime.Now.ToString("yyyy-MM-dd"));

            // add news age
            XmlNodeList nodes = xmlNewsletter.SelectNodes("/website/emailcategory[@pageid=" + pageId + "]/newsitem");
            foreach (XmlNode node in nodes)
            {
                DateTime created = DateTime.MinValue;
                DateTime.TryParse(node.Attributes["created"].Value, out created);
                TimeSpan age = DateTime.Now.Subtract(created);
                XmlAttribute att = xmlNewsletter.CreateAttribute("daysold", "");
                att.Value = age.Days.ToString();
                node.Attributes.Append(att);
            }
 

			// load xsl to whow the page
			XslTransform xslt = new XslTransform();
			XmlUrlResolver resolver = new XmlUrlResolver();

			xslt.Load(HttpContext.Current.Server.MapPath("/modules/newsletter/list_newsletter.xsl"));
			
			xslt.Transform(xmlNewsletter,args,writer,resolver);

			return writer.ToString();
		}

		private string doSubscribe(int pageId, string email, string name)
		{
			string CategoryId = "";
			XmlNode xmlNode;
			XmlNode xmlCdata;
			XmlNode xmlParentNode;
			XmlElement xmlElem;
			XmlAttribute xmlAtr;
			XmlDocument xmlNewsletter = new XmlDocument();

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(pageId);

			if (HttpContext.Current.Request["category"] != null)
				CategoryId = HttpContext.Current.Request["category"];
			
			xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id='"+ CategoryId +"']");
			if (xmlNode != null && email != "")
			{
				xmlNode = xmlNewsletter.SelectSingleNode("//subscribe[@category='"+ CategoryId +"' and email='"+ email +"']");
				if (xmlNode == null)
				{
					xmlElem = xmlNewsletter.CreateElement("subscribe","");

					xmlAtr = xmlElem.SetAttributeNode("category","");
					xmlAtr.Value = CategoryId;

					xmlAtr = xmlElem.SetAttributeNode("active","");
					xmlAtr.Value = "1";

					xmlNode = xmlNewsletter.CreateElement("email","");
					xmlCdata = xmlNewsletter.CreateCDataSection(email);
					xmlNode.AppendChild(xmlCdata);
					xmlElem.AppendChild(xmlNode);

					xmlNode = xmlNewsletter.CreateElement("name","");
					xmlCdata = xmlNewsletter.CreateCDataSection(name);
					xmlNode.AppendChild(xmlCdata);
					xmlElem.AppendChild(xmlNode);

                    // check for extra fields
                    XmlNodeList nodes = xmlNewsletter.SelectNodes("/website/field");
                    foreach (XmlNode node in nodes)
                    {
                        string nodeId = node.Attributes["id"].Value;
                        if (!string.IsNullOrEmpty(nodeId) && HttpContext.Current.Request[nodeId] != null)
                        {
                            string value = HttpContext.Current.Request[nodeId];
                            if (node.Attributes["type"] != null && node.Attributes["type"].Value != null && node.Attributes["type"].Value.ToLower() == "checkbox")
                                value = ((!(string.IsNullOrEmpty(value))).ToString().ToLower());
                            xmlNode = xmlNewsletter.CreateElement(nodeId, "");
                            xmlCdata = xmlNewsletter.CreateCDataSection(value);
                            xmlNode.AppendChild(xmlCdata);
                            xmlElem.AppendChild(xmlNode);
                        }
                    }

					xmlParentNode = xmlNewsletter.SelectSingleNode("/website");
					xmlParentNode.AppendChild(xmlElem);

                    SaveNewsletterXml(xmlNewsletter);

				}
				return xmlNewsletter.SelectSingleNode("//emailcategory[@id="+ CategoryId +"]/onSubscribe").InnerText;
			}
			return "";
		}

		private string doUnsubscibe(int pageId,string email)
		{
			string CategoryId = "";
			XmlNode xmlNode;
			XmlNode xmlParentNode;
			XmlDocument xmlNewsletter = new XmlDocument();

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(pageId);

			if (HttpContext.Current.Request["category"] != null)
				CategoryId = HttpContext.Current.Request["category"];
			
			xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id='"+ CategoryId +"']");
			if (xmlNode != null)
			{
				xmlNode = xmlNewsletter.SelectSingleNode("//subscribe[@category='"+ CategoryId +"' and email='"+ email +"']");
				if (xmlNode != null)
				{
					xmlParentNode = xmlNode.ParentNode;
					xmlParentNode.RemoveChild(xmlNode);

                    SaveNewsletterXml(xmlNewsletter);

				}
				return xmlNewsletter.SelectSingleNode("//emailcategory[@id="+ CategoryId +"]/onUnSubscribe").InnerText;
			}
			return "";
		}

		private string doSendNewsletter(Admin.Admin admin, Admin.Page page)
		{
			int intId = -1;
            int categoryId = -1;
            XmlNode oXmlNode;
			XmlNodeList oXmlNodeRange;
			string subject = "";
			string sender = "";
			string header = "<html>";
			string body = "";
			string footer = "";
			string indhold = "";
			int startPos = -1;
			int endPos = -1;
			int namePos = -1;
			string linieskift = "";
			StringBuilder output = new StringBuilder();
			StringWriter log = new StringWriter();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2101,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("sendnewsletter") +"</h3>");
			output.Append("<font color=red>"+ Functions.localText("dontrefresh") + "</font><br/>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

            if (HttpContext.Current.Request.Form["itemid"] != null)
                intId = int.Parse(HttpContext.Current.Request.Form["itemid"]);
            if (HttpContext.Current.Request.Form["categoryId"] != null)
                categoryId = int.Parse(HttpContext.Current.Request.Form["categoryId"]);
            if (HttpContext.Current.Request.Form["subject"] != null)
				subject = Functions.EncodeSpecialChars(HttpContext.Current.Request.Form["subject"]);

			// Load the xml-file holding data on newsletters
            oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

            if (intId != -1)
                categoryId = int.Parse(oXml.SelectSingleNode("//emailcategory[newsitem/@id='" + intId + "']").Attributes["id"].Value);

            sender = oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/sender").InnerText;
            if (ConfigurationManager.AppSettings["DefaultStylesheet"] != null)
                header += @"<head><link href=""http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + ConfigurationManager.AppSettings["DefaultStylesheet"] + @""" type=""text/css"" rel=""stylesheet""/></head>";
            header += "<body class='htmlnewsletterbody'><base href='http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "'/>";
            header += oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/header").InnerText;
            footer = oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/footer").InnerText + "</body></html>";

            if (((intId != -1 && oXml.SelectSingleNode("//emailcategory[newsitem/@id='" + intId + "' and @pageid=//selectedpage]") != null) || (categoryId != -1) )&& subject != "" )
			{
                linieskift = "<br/>";
                if (intId != -1)
                    indhold = oXml.SelectSingleNode("//newsitem[@id='" + intId + "']/content").InnerText;
                else
                {
                    indhold = HttpContext.Current.Request.Form["tbContent_indhold_temp"];
                    if (ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
                    {
                        try
                        {
                            bool emptyTags = false;
                            indhold = Functions.CleanEditorCode(indhold, log, true, false, true, ref emptyTags);
                        }
                        catch { }
                    }
                    indhold = Functions.EncodeSpecialChars(indhold);
                }
                // insert linebreaks as the body can only be 250 characters
                indhold = ReplaceSpaceWithNewline(indhold, 250);

                body = header + linieskift + indhold + linieskift + footer;

                //replacement codes
                body = body.Replace("##id##", intId.ToString());
                body = body.Replace("##categoryid##", categoryId.ToString());
                body = body.Replace("##pageid##", page.Pageid.ToString());

                //attach linked images
                Regex re = new Regex("(?<=<img[^<]+?src=\")[^\"]+", RegexOptions.IgnoreCase);
                Hashtable attachments = new Hashtable();
                foreach (Match ma in re.Matches(body))
                {
                    if (!attachments.ContainsKey(ma.Value))
                    {
                        FileInfo imageFileInfo = new FileInfo(HttpContext.Current.Server.MapPath(ma.Value.Replace("http://" + HttpContext.Current.Request.ServerVariables["server_name"], "")));
                        if (imageFileInfo != null && imageFileInfo.Exists)
                        {
                            try
                            {
                                string contentId = ma.Value.Replace(".", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty);

                                Attachment attachment = new Attachment(imageFileInfo.FullName);
                                attachment.ContentId = contentId;
                                // Add Image to the Email
                                attachments.Add(contentId, attachment);

                                //Change the src to "cid:<contentId>"
                                body = body.Replace(ma.Value, "cid:" + contentId);
                            }
                            catch { }
                        }
                    }
                }

                // Trick to make links visible in text-email-readers and insert domain (but not for mailto and external)
                Regex regExLink = new Regex("(<a.*?href\\s*=\\s*[\"'])(.*?)(['\"].*?>.*?)</a>", RegexOptions.IgnoreCase);
                if (regExLink.IsMatch(body))
                    foreach (Match ma in regExLink.Matches(body))
                    {
                        string link = ma.Groups[2].Value;
                        if (!link.StartsWith("mailto:") && !link.StartsWith("http://") && !link.StartsWith("https://"))
                            link = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + link;
                        body = body.Replace(ma.Value, ma.Groups[1].Value + link + ma.Groups[3].Value + "<span style='display:none'>: " + link.Replace("mailto:", "") + "</span></a>");
                    }

                // insert name in mail
				startPos = body.IndexOf("<#");
				endPos = body.IndexOf("#>");

				oXmlNodeRange = oXml.SelectNodes("/website/subscribe[@category='"+ categoryId +"' and @active='1']");


				for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				{
                    string thisBody = body;
					string email = oXmlNodeRange.Item(counter).SelectSingleNode("email").InnerText;
					try
					{
						string name = oXmlNodeRange.Item(counter).SelectSingleNode("name").InnerText;
					
						if (startPos > -1 && endPos > startPos)
						{
							if (name != "")
							{
                                namePos = thisBody.ToLower().IndexOf("navn", startPos);
								if (namePos >= startPos && namePos < endPos)
								{
                                    thisBody = thisBody.Substring(0, namePos) + name + thisBody.Substring(namePos + 4);
                                    thisBody = thisBody.Replace("<#", "").Replace("#>", "");
								}
							}
							else
                                thisBody = thisBody.Substring(0, startPos) + thisBody.Substring(endPos + 2);
						}

                        //replacement codes
                        thisBody = thisBody.Replace("##email##", email);
                        
                        // Sending mail
						MailMessage mailMessage = new MailMessage();
                        mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1"); 
                        mailMessage.IsBodyHtml = true;
						mailMessage.From = new MailAddress(sender);
						mailMessage.To.Add(new MailAddress(email));
						mailMessage.Subject = subject;
                        mailMessage.Body = thisBody;
                        foreach (string key in attachments.Keys)
                            mailMessage.Attachments.Add((Attachment)attachments[key]);
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = ConfigurationManager.AppSettings["smtpMailServer"];

#pragma warning disable CS0162
						if (!Global.isDemo)
						{
							smtp.Send(mailMessage);
							output.Append(Functions.localText("sentto") + ": " + email + "<br/>");
						}
						else
						{
							output.Append("<font color=red>DEMOSITE: NO SENDING</font>" + Functions.localText("sentto") + ": " + email + "<br/>");
						}
#pragma warning restore CS0162

                    }
                    catch
					{
						output.Append("<b>"+ Functions.localText("nosentto")+ ":</b> " + email + "<br/>");
					}
				}
				output.Append("<br/>"+ Functions.localText("emailsent") + "<br/>" + Functions.localText("goto") + Functions.publicUrl());

                if(intId != -1)
                    ((XmlElement)(oXml.SelectSingleNode("//newsitem[@id='" + intId + "']"))).SetAttribute("sentdate", DateTime.Now.ToString("yyyy'-'MM'-'dd"));
                SaveNewsletterXml(oXml);

			}
			else
				throw new Exception ("Error reading newsletter data.");

			return output.ToString();
		}

		private string doSendTestNewsletter(Admin.Admin admin, Admin.Page page)
		{
			int intId = -1;
            int categoryId = -1;
            XmlNode oXmlNode;
			string email = "";
			string subject = "";
			string sender = "";
            string indhold;
            string header = "<html>";
            string body = "";
			string footer = "";
			int startPos = -1;
			int endPos = -1;
			int namePos = -1;
			string linieskift = "<br/>";
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2101,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("sendnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();
			StringWriter log = new StringWriter();

            if (HttpContext.Current.Request.Form["itemid"] != null)
                intId = int.Parse(HttpContext.Current.Request.Form["itemid"]);
            if (HttpContext.Current.Request.Form["categoryId"] != null)
                categoryId = int.Parse(HttpContext.Current.Request.Form["categoryId"]);
            if (HttpContext.Current.Request.Form["email"] != null)
				email = HttpContext.Current.Request.Form["email"];
			if (HttpContext.Current.Request.Form["subject"] != null)
				subject = HttpContext.Current.Request.Form["subject"];

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

            if (intId != -1)
                categoryId = int.Parse(oXml.SelectSingleNode("//emailcategory[newsitem/@id='" + intId + "']").Attributes["id"].Value);

            sender = oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/sender").InnerText;
            if (ConfigurationManager.AppSettings["DefaultStylesheet"] != null)
                header += @"<head><link href=""http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + ConfigurationManager.AppSettings["DefaultStylesheet"] + @""" type=""text/css"" rel=""stylesheet""/></head>";
            header += "<body class='htmlnewsletterbody'><base href='http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "'/>";
            header += oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/header").InnerText;
            footer = oXml.SelectSingleNode("//emailcategory[@id='" + categoryId + "']/footer").InnerText + "</body></html>";
            
            if (email != "" && subject != "")
			{
                linieskift = "<br/>";
                if (intId != -1)
                    indhold = oXml.SelectSingleNode("//newsitem[@id='" + intId + "']/content").InnerText;
                else
                {
                    indhold = HttpContext.Current.Request.Form["tbContent_indhold_temp"];
                    if (ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
                    {
                        try
                        {
                            bool emptyTags = false;
                            indhold = Functions.CleanEditorCode(indhold, log, true, false, true, ref emptyTags);
                        }
                        catch { }
                    }
                    indhold = Functions.EncodeSpecialChars(indhold);
                }
                // insert linebreaks as the body can only be 250 characters
                indhold = ReplaceSpaceWithNewline(indhold, 250);

                body = header + linieskift + indhold + linieskift + footer;

                //replacement codes
                body = body.Replace("##id##", intId.ToString());
                body = body.Replace("##categoryid##", categoryId.ToString());
                body = body.Replace("##pageid##", page.Pageid.ToString());
                body = body.Replace("##email##", email);

                //attach linked images
                Regex re = new Regex("(?<=<img[^<]+?src=\")[^\"]+", RegexOptions.IgnoreCase);
                Hashtable attachments = new Hashtable();
                foreach (Match ma in re.Matches(body))
                {
                    if (!attachments.ContainsKey(ma.Value))
                    {
                        FileInfo imageFileInfo = new FileInfo(HttpContext.Current.Server.MapPath(ma.Value.Replace("http://" + HttpContext.Current.Request.ServerVariables["server_name"], "")));
                        if (imageFileInfo != null && imageFileInfo.Exists)
                        {
                            try
                            {
                                string contentId = ma.Value.Replace(".", string.Empty).Replace("\\", string.Empty).Replace("/", string.Empty);

                                Attachment attachment = new Attachment(imageFileInfo.FullName);
                                attachment.ContentId = contentId;
                                // Add Image to the Email
                                attachments.Add(contentId, attachment);

                                //Change the src to "cid:<contentId>"
                                body = body.Replace(ma.Value, "cid:" + contentId);
                            }
                            catch { }
                        }
                    }
                }

				// insert name in mail
				startPos = body.IndexOf("<#");
				endPos = body.IndexOf("#>");
				if (startPos > -1 && endPos > startPos)
				{
					namePos = body.ToLower().IndexOf("navn",startPos);
					if (namePos >= startPos && namePos < endPos)
					{
						body = body.Substring(0,namePos) + "TEST" + body.Substring(namePos+4);
						body = body.Replace("<#","").Replace("#>","");
					}
				}

                // Trick to make links visible in text-email-readers and insert domain (but not for mailto and external)
                Regex regExLink = new Regex("(<a.*?href\\s*=\\s*[\"'])(.*?)(['\"].*?>.*?)</a>", RegexOptions.IgnoreCase);
                if (regExLink.IsMatch(body))
                    foreach (Match ma in regExLink.Matches(body))
                    {
                        string link = ma.Groups[2].Value;
                        if (!link.StartsWith("mailto:") && !link.StartsWith("http://") && !link.StartsWith("https://"))
                            link = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + link;
                        body = body.Replace(ma.Value, ma.Groups[1].Value + link + ma.Groups[3].Value + "<span style='display:none'>: " + link.Replace("mailto:","") + "</span></a>");
                    }

                // Sending mail
				MailMessage mailMessage = new MailMessage();
                mailMessage.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                mailMessage.IsBodyHtml = true;
				mailMessage.From = new MailAddress(sender);
				mailMessage.To.Add(new MailAddress(email));
				mailMessage.Subject = subject;
                mailMessage.Body = body;
                foreach (string key in attachments.Keys)
                    mailMessage.Attachments.Add((Attachment)attachments[key]);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["smtpMailServer"];
				smtp.Send(mailMessage);

				output.Append(Functions.localText("testemailsent"));
			}

			return output.ToString();
		}

		private string formSendNewsletter(Admin.Admin admin, Admin.Page page)
		{
            int intId = -1;
            int categoryId = -1;
            XmlNode oXmlNode;
            string editorText = "";
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2101,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("sendnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

            if (HttpContext.Current.Request.QueryString["newsitemid"] != null)
                intId = int.Parse(HttpContext.Current.Request.QueryString["newsitemid"]);
            if (HttpContext.Current.Request.QueryString["categoryid"] != null)
                categoryId = int.Parse(HttpContext.Current.Request.QueryString["categoryid"]);

            // Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_send_newsletter.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

            if (intId == -1 && categoryId != -1)
            {
                XmlDocument xslDoc = new XmlDocument();
                xslDoc.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_send_newsletter.xsl"));

                // set editmode to the right mode
                ((Global)HttpContext.Current.ApplicationInstance).EditMode = Global.EditModeEnum.AdminEdit;

                // insert editor into xsl-file
                editorText = Editor.InsertContentHolder(ref xslDoc, page.Pageid, "");
                xslt.Load(new XmlNodeReader(xslDoc));
            }
            
            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddParam("newsitemid","", intId);
            xslArg.AddParam("categoryid","", categoryId);
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString() + editorText;
		}

		private string doEditNewsletter(Admin.Admin admin, Admin.Page page)
		{
			int intItem = -1;
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			XmlDocument xmlNewsletter = new XmlDocument();
			XmlNode xmlNode;
			XmlNode xmlCdata;
			XmlNode oXmlNode;
			IFormatProvider culture = new CultureInfo("da-DK", true);

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2102,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editnewsletter") +"</h3>");

			if (HttpContext.Current.Request.Form["emailcategory"] != null)
				intItem = int.Parse(HttpContext.Current.Request.Form["emailcategory"]);

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(page.Pageid);

			oXmlNode = xmlNewsletter.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intItem.ToString();


			if (intItem != -1 && xmlNewsletter.SelectSingleNode("//emailcategory[@id="+ intItem+"]") != null)
			{
				// gemt categoryid i xml
				xmlNode = xmlNewsletter.SelectSingleNode("//selectedemailtype");
				xmlNode.InnerText = intItem.ToString();

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/title");
				xmlNode.InnerText = HttpContext.Current.Request.Form["title"];

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/onSubscribe");
				xmlCdata = xmlNode.FirstChild;
				xmlCdata.InnerText = HttpContext.Current.Request.Form["onSubscribe"];

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/onUnSubscribe");
				xmlCdata = xmlNode.FirstChild;
				xmlCdata.InnerText = HttpContext.Current.Request.Form["onUnSubscribe"];

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/header");
				xmlCdata = xmlNode.FirstChild;
				xmlCdata.InnerText = HttpContext.Current.Request.Form["header"].ToString();

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/footer");
				xmlCdata = xmlNode.FirstChild;
				xmlCdata.InnerText = HttpContext.Current.Request.Form["footer"].ToString();

				xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/sender");
				xmlNode.InnerText = HttpContext.Current.Request.Form["sender"].ToString();

                xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/rsstitle");
                if (xmlNode != null)
                    xmlNode.FirstChild.InnerText = HttpContext.Current.Request.Form["rsstitle"].ToString();
                else
                {
                    xmlNode = xmlNewsletter.CreateElement("rsstitle", "");
                    xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["rsstitle"].ToString());
                    xmlNode.AppendChild(xmlCdata);
                    xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]").AppendChild(xmlNode);
                }

                xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]/rssdescription");
                if (xmlNode != null)
                    xmlNode.FirstChild.InnerText = HttpContext.Current.Request.Form["rssdescription"].ToString();
                else
                {
                    xmlNode = xmlNewsletter.CreateElement("rssdescription", "");
                    xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["rssdescription"].ToString());
                    xmlNode.AppendChild(xmlCdata);
                    xmlNewsletter.SelectSingleNode("//emailcategory[@id=//selectedemailtype]").AppendChild(xmlNode);
                }

                SaveNewsletterXml(xmlNewsletter);

                output.Append(Functions.localText("emaildatasaved") + Functions.localText("goto") + Functions.publicUrl());

				return output.ToString();
			}
			else
				throw new Exception ("Error reading newsletter data. Id is invalid");
		}

		private string removeSubscriber(Admin.Admin admin, Admin.Page page)
		{
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			XmlDocument xmlNewsletter = new XmlDocument();
			XmlNode xmlNode;
			XmlNode xmlNodeTemp;
			string email = "";
			int CatId = -1;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2103,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editsubscriber") +"</h3>");

			// Get the Xml
			xmlNewsletter = OXmlNewsletter(page.Pageid);

			if (HttpContext.Current.Request.QueryString["email"] != null)
				email = HttpContext.Current.Request.QueryString["email"];
			if (HttpContext.Current.Request.QueryString["categoryid"] != null)
				CatId = int.Parse(HttpContext.Current.Request.QueryString["categoryid"]);

			if (email != "" && CatId != -1)
			{
				// Remove subscriber in xml
				xmlNode = xmlNewsletter.SelectSingleNode("/website/subscribe[@category='"+ CatId +"' and email='"+ email +"']");
				if (xmlNode != null)
					xmlNodeTemp = xmlNode.ParentNode.RemoveChild(xmlNode);

                SaveNewsletterXml(xmlNewsletter);
			}

			return formEditSubscriber(admin, page);
		}

		private string formEditNewsletter(Admin.Admin admin, Admin.Page page)
		{
			int intId = -1;
			XmlNode oXmlNode;
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2102,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			intId = int.Parse(HttpContext.Current.Request.QueryString["categoryid"]);

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_edit_category.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
			XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string editArchive(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/list_edit_archive.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string formAddArchive(Admin.Admin admin, Admin.Page page)
		{
			XmlNode oXmlNode;
			StringBuilder output = new StringBuilder();
			int intId = -1;
            string editorText;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

            // set editmode to the right mode
            ((Global)HttpContext.Current.ApplicationInstance).EditMode = Global.EditModeEnum.AdminEdit;

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
            XmlDocument xslDoc = new XmlDocument();
            StringWriter writer = new StringWriter();

			intId = int.Parse(HttpContext.Current.Request.QueryString["categoryid"]);

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

			// Load the xsl-file to use for the current transformation
			xslDoc.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_add_archive.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

            // insert editor into xsl-file
            editorText = Editor.InsertContentHolder(ref xslDoc, page.Pageid,"");
            xslt.Load(new XmlNodeReader(xslDoc));

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

            return output.ToString() + editorText;
		}

		private string doAddArchive(Admin.Admin admin, Admin.Page page)
		{
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
            StringWriter log = new StringWriter();
            XmlDocument oXml = new XmlDocument();
			string subject = "";
            string teaser = "";
			string content = "";

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

			// save newsletter in xml-file
			XmlNode xmlNode;
			XmlNode xmlCdata;
			XmlNode xmlParentNode;
			XmlElement xmlElem;
			XmlAttribute xmlAtr;
			XmlNodeList xmlNodeRange;
			int maxId = -1;
			int categoryId = -1;

            if (HttpContext.Current.Request.Form["tbContent_indhold_temp"] != null)
                content = HttpContext.Current.Request.Form["tbContent_indhold_temp"];
            if (HttpContext.Current.Request.Form["teaser"] != null)
                teaser = Functions.EncodeSpecialChars(HttpContext.Current.Request.Form["teaser"]);
            if (HttpContext.Current.Request.Form["subject"] != null)
				subject = Functions.EncodeSpecialChars(HttpContext.Current.Request.Form["subject"]);

            categoryId = int.Parse(HttpContext.Current.Request.Form["categoryid"]);

			if (content != "" && subject != "" && categoryId != -1)
			{
                if (ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
                {
                    try
                    {
                        bool emptyTags = false;
                        content = Functions.CleanEditorCode(content, log, true, false, true, ref emptyTags);
                    }
                    catch { }
                }
                content = Functions.EncodeSpecialChars(content);
                
                // Load the xml-file holding data on newsletters
                oXml = OXmlNewsletter(page.Pageid);

				// Find next id
				xmlNodeRange = oXml.GetElementsByTagName("newsitem");
				for (int counter = 0;counter <xmlNodeRange.Count; counter++)
				{
					if (int.Parse(xmlNodeRange.Item(counter).Attributes["id"].Value) > maxId)
						maxId = int.Parse(xmlNodeRange.Item(counter).Attributes["id"].Value);
				}

				xmlElem = oXml.CreateElement("newsitem","");

				xmlAtr = xmlElem.SetAttributeNode("id","");
				maxId ++;
				xmlAtr.Value = maxId.ToString();

				xmlAtr = xmlElem.SetAttributeNode("created","");
				xmlAtr.Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");

				xmlNode = oXml.CreateElement("title","");
				xmlCdata = oXml.CreateCDataSection(subject);
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

                xmlNode = oXml.CreateElement("content", "");
                xmlCdata = oXml.CreateCDataSection(content);
                xmlNode.AppendChild(xmlCdata);
                xmlElem.AppendChild(xmlNode);

                xmlNode = oXml.CreateElement("teaser", "");
                xmlCdata = oXml.CreateCDataSection(teaser);
                xmlNode.AppendChild(xmlCdata);
                xmlElem.AppendChild(xmlNode);

                xmlParentNode = oXml.SelectSingleNode("//emailcategory[@id=" + categoryId + "]");
				xmlParentNode.AppendChild(xmlElem);

                SaveNewsletterXml(oXml);

                output.Append(Functions.localText("newsitemsaved") + Functions.localText("goto") + Functions.publicUrl());

				return output.ToString();
			}
			else
				throw new Exception ("Error reading newsitem data. Not all values given");
		}

		private string formEditArchive(Admin.Admin admin, Admin.Page page)
		{
			int intId = -1;
			XmlNode oXmlNode;
			StringBuilder output = new StringBuilder();
            string editorText;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

            // set editmode to the right mode
            ((Global)HttpContext.Current.ApplicationInstance).EditMode = Global.EditModeEnum.AdminEdit;
            
            // append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

            XmlDocument oXml = new XmlDocument();
            XslTransform xslt = new XslTransform();
            XmlDocument xslDoc = new XmlDocument();
            StringWriter writer = new StringWriter();

			intId = int.Parse(HttpContext.Current.Request.QueryString["newsitemid"]);

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

            // Load the xsl-file to use for the current transformation
            xslDoc.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_edit_archive.xsl"));
            XmlUrlResolver resolver = new XmlUrlResolver();

            // insert editor into xsl-file
            editorText = Editor.InsertContentHolder(ref xslDoc, page.Pageid, oXml.SelectSingleNode("//newsitem[@id=//selectedemailtype]/content").InnerXml, true);
            xslt.Load(new XmlNodeReader(xslDoc));

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

            // Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXml, xslArg, writer, resolver);
            output.Append(writer.ToString());

            return output.ToString() + editorText;
        }

		private string doEditArchive(Admin.Admin admin, Admin.Page page)
		{
			int intItem = -1;
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			StringWriter log = new StringWriter();
			XmlDocument xmlNewsletter = new XmlDocument();
            string teaser = "";
			string indhold = "";
            string created = "";
			XmlNode xmlNode;
			XmlNode xmlCdata;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

			if (HttpContext.Current.Request.Form["newsitemid"] != null)
				intItem = int.Parse(HttpContext.Current.Request.Form["newsitemid"]);
            if (HttpContext.Current.Request.Form["tbContent_indhold_temp"] != null)
                indhold = HttpContext.Current.Request.Form["tbContent_indhold_temp"];
            if (HttpContext.Current.Request.Form["teaser"] != null)
                teaser = HttpContext.Current.Request.Form["teaser"];
            if (HttpContext.Current.Request.Form["created"] != null)
                created = HttpContext.Current.Request.Form["created"];

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(page.Pageid);

            if (indhold != "" && intItem != -1 && xmlNewsletter.SelectSingleNode("//emailcategory[@pageid = " + page.Pageid + "]/newsitem[@id=" + intItem + "]") != null && created != "")
			{
				if(ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
				{
					try 
					{
                        bool emptyTags = false;
                        indhold = Functions.CleanEditorCode(indhold, log, true, false, true, ref emptyTags);
					}
					catch {}
				}
				indhold = Functions.EncodeSpecialChars(indhold);
			
				// Save categoryid in xml
				xmlNode = xmlNewsletter.SelectSingleNode("//selectedemailtype");
				xmlNode.InnerText = intItem.ToString();

                xmlNewsletter.SelectSingleNode("//newsitem[@id=//selectedemailtype]").Attributes["created"].Value = created;

				xmlNode = xmlNewsletter.SelectSingleNode("//newsitem[@id=//selectedemailtype]/title");
				xmlCdata = xmlNode.FirstChild;
				xmlCdata.InnerText = Functions.EncodeSpecialChars(HttpContext.Current.Request.Form["subject"]);

                xmlNode = xmlNewsletter.SelectSingleNode("//newsitem[@id=//selectedemailtype]/teaser");
                if (xmlNode != null)
                {
                    xmlCdata = xmlNode.FirstChild;
                    xmlCdata.InnerText = teaser;
                }
                else
                {
                    xmlNode = xmlNewsletter.CreateElement("teaser", "");
                    xmlCdata = xmlNewsletter.CreateCDataSection(teaser);
                    xmlNode.AppendChild(xmlCdata);
                    xmlNewsletter.SelectSingleNode("//newsitem[@id=//selectedemailtype]").AppendChild(xmlNode);
                }

                xmlNode = xmlNewsletter.SelectSingleNode("//newsitem[@id=//selectedemailtype]/content");
                xmlCdata = xmlNode.FirstChild;
                xmlCdata.InnerText = indhold;

                SaveNewsletterXml(xmlNewsletter);
                output.Append(Functions.localText("newsitemsaved") + Functions.localText("goto") + Functions.publicUrl());

				return output.ToString();
			}
			else
				throw new Exception ("Error reading newsitem data. Id is invalid");
		}

		private string doDeleteArchive(Admin.Admin admin, Admin.Page page)
		{
			int intItem = -1;
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			XmlDocument xmlNewsletter = new XmlDocument();
			XmlNode xmlNode;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2105,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editarchive") +"</h3>");

			if (HttpContext.Current.Request.QueryString["newsitemid"] != null)
				intItem = int.Parse(HttpContext.Current.Request.QueryString["newsitemid"]);

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(page.Pageid);

			xmlNode = xmlNewsletter.SelectSingleNode("//emailcategory[@pageid = "+ page.Pageid +"]/newsitem[@id="+ intItem +"]");
			if (intItem != -1 && xmlNode != null)
			{
				XmlNode oXmlNodeParent;

				oXmlNodeParent = xmlNode.ParentNode;
				oXmlNodeParent.RemoveChild(xmlNode);

				// save the xml-file holding data on newsletters
                SaveNewsletterXml(xmlNewsletter);

                output.Append(Functions.localText("newsitemdeleted") + Functions.localText("goto") + Functions.publicUrl());

				return output.ToString();
			}
			else
				throw new Exception ("Error reading newsitem data. Id is invalid");
		}


		private string formAddCategory(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2104,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("addcategory") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_add_category.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string doAddCategory(Admin.Admin admin, Admin.Page page)
		{
			int maxId = -1;
			string title = "";
			string sender = "";
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			XmlDocument xmlNewsletter = new XmlDocument();
			XmlNode xmlNode;
			XmlNode xmlCdata;
			XmlNode xmlParentNode;
			XmlElement xmlElem;
			XmlAttribute xmlAtr;
			XmlNodeList xmlNodeRange;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2104,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("addcategory") +"</h3>");

			// Get the Xml
            xmlNewsletter = OXmlNewsletter(page.Pageid);

			if (HttpContext.Current.Request.Form["title"] != null)
				title = HttpContext.Current.Request.Form["title"];
			if (HttpContext.Current.Request.Form["sender"] != null)
				sender = HttpContext.Current.Request.Form["sender"];

			if (title != "" && sender != "")
			{
				// Find next id
				xmlNodeRange = xmlNewsletter.GetElementsByTagName("emailcategory");
				for (int counter = 0;counter <xmlNodeRange.Count; counter++)
				{
					if (int.Parse(xmlNodeRange.Item(counter).Attributes["id"].Value) > maxId)
						maxId = int.Parse(xmlNodeRange.Item(counter).Attributes["id"].Value);
				}

				xmlElem = xmlNewsletter.CreateElement("emailcategory","");

				xmlAtr = xmlElem.SetAttributeNode("id","");
				maxId ++;
				xmlAtr.Value = maxId.ToString();

				xmlAtr = xmlElem.SetAttributeNode("pageid","");
				xmlAtr.Value = page.Pageid.ToString();

				xmlNode = xmlNewsletter.CreateElement("title","");
				xmlNode.InnerText = title;
				xmlElem.AppendChild(xmlNode);

				xmlNode = xmlNewsletter.CreateElement("onSubscribe","");
				xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["onSubscribe"].ToString());
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

				xmlNode = xmlNewsletter.CreateElement("onUnSubscribe","");
				xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["onUnSubscribe"].ToString());
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

				xmlNode = xmlNewsletter.CreateElement("header","");
				xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["header"].ToString());
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

				xmlNode = xmlNewsletter.CreateElement("footer","");
				xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["footer"].ToString());
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

				xmlNode = xmlNewsletter.CreateElement("sender","");
				xmlCdata = xmlNewsletter.CreateCDataSection(sender);
				xmlNode.AppendChild(xmlCdata);
				xmlElem.AppendChild(xmlNode);

                xmlNode = xmlNewsletter.CreateElement("rsstitle", "");
                xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["rsstitle"].ToString());
                xmlNode.AppendChild(xmlCdata);
                xmlElem.AppendChild(xmlNode);

                xmlNode = xmlNewsletter.CreateElement("rssdescription", "");
                xmlCdata = xmlNewsletter.CreateCDataSection(HttpContext.Current.Request.Form["rssdescription"].ToString());
                xmlNode.AppendChild(xmlCdata);
                xmlElem.AppendChild(xmlNode);
                
                xmlParentNode = xmlNewsletter.SelectSingleNode("/website");
				xmlParentNode.AppendChild(xmlElem);

                SaveNewsletterXml(xmlNewsletter);
                output.Append(Functions.localText("categoryadded") + Functions.localText("goto") + Functions.publicUrl());

				return output.ToString();
			}
			else
				throw new Exception ("Error reading newsletter data. Id is invalid");
		}


		private string editNewsletter(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2102,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/list_edit_newsletter.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string sendNewsletter(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2101,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("sendnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/list_send_newsletter.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string formEditSubscriber(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();
			int intId = -1;
			XmlNode oXmlNode;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2102,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editnewsletter") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			intId = int.Parse(HttpContext.Current.Request.QueryString["categoryid"]);

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			oXmlNode = oXml.SelectSingleNode("//selectedemailtype");
			oXmlNode.InnerText = intId.ToString();

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/form_edit_subscriber.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string editSubscriber(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2103,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") + "</h2>");
			output.Append("<h3>"+ Functions.localText("editsubscriber") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on newsletters
			oXml = OXmlNewsletter(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/newsletter/list_edit_subscriber.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            ContentFunctions contentFunctions = new ContentFunctions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string funcList(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// append headers to output
			output.Append("<h2>"+ Functions.localText("newsletter") +"</h2><ul>");

			// check for rights to methods in this module and list methods that the user have access to
            if (admin.userHasAccess(2105, page.Pageid, false))
                output.Append("<li><a href='/admin/modules/default.aspx?pageid=" + page.Pageid + "&action=editarchive'>" + Functions.localText("editarchive") + "</a></li>");
            if (admin.userHasAccess(2101, page.Pageid, false))
                output.Append("<li><a href='/admin/modules/default.aspx?pageid=" + page.Pageid + "&action=sendnewsletter'>" + Functions.localText("sendnewsletter") + "</a></li>");
			if (admin.userHasAccess(2102, page.Pageid, false))
                output.Append("<li><a href='/admin/modules/default.aspx?pageid=" + page.Pageid + "&action=editnewsletter'>" + Functions.localText("editnewsletter") + "</a></li>");
			if (admin.userHasAccess(2103, page.Pageid, false))
                output.Append("<li><a href='/admin/modules/default.aspx?pageid=" + page.Pageid + "&action=editsubscriber'>" + Functions.localText("editsubscriber") + "</a></li>");
			if (admin.userHasAccess(2104, page.Pageid, false))
                output.Append("<li><a href='/admin/modules/default.aspx?pageid=" + page.Pageid + "&action=addcategory'>" + Functions.localText("addcategory") + "</a></li>");
            output.Append("</ul>");

			return output.ToString();
		}

		private XmlDocument OXmlNewsletter(int pageid)
		{
			XmlDocument output = new XmlDocument();
			XmlNode oXmlNode;

			// Load the xml-file holding data
            output.Load(Global.publicXmlPath + "/newsletter.xml");

			// Insert current pageid in the xml
			oXmlNode = output.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageid.ToString();

			return output;
		}

        public void SaveNewsletterXml(XmlDocument xml)
        {
            // try saving 3 times - this should be enought :-)
            try
            {
                xml.Save(Global.publicXmlPath + @"/newsletter.xml");
            }
            catch (System.IO.IOException)
            {
                try
                {
                    xml.Save(Global.publicXmlPath + @"/newsletter.xml");
                }
                catch (System.IO.IOException)
                {
                    try
                    {
                        xml.Save(Global.publicXmlPath + @"/newsletter.xml");
                    }
                    catch (System.IO.IOException ex)
                    {
                        throw new Exception(ex.InnerException + "<br/>" + "newsletter.xml content:" + xml.OuterXml);
                    }
                }
            }
        }

        static string ReplaceSpaceWithNewline(string s, int maxCharsInLine)
        {
            if (s.Length <= maxCharsInLine)
                return s;

            // search for last ' ' before maxCharsInLine
            int breakPos = maxCharsInLine;
            while ((breakPos > 0) && (s[breakPos] != ' '))
                breakPos--;

            // if none before, search for first ' ' after maxCharsInLine
            int charOffset = 1;
            if (breakPos == 0)
            {
                breakPos = maxCharsInLine + 1;
                while ((breakPos < s.Length) && (s[breakPos] != ' '))
                    breakPos++;

                // if none before or after, use entire string
                if (breakPos == s.Length)
                    charOffset = 0;
            }

            return s.Substring(0, breakPos) + '\n' +
                ReplaceSpaceWithNewline(s.Substring(breakPos + charOffset), maxCharsInLine);
        }
    

	}
}
