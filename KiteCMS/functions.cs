using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using KiteCMS.Admin;
using System.Reflection;
using Sgml;
using System.Security.Cryptography;
using System.Net.Mail;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for functions.
	/// </summary>
	public class Functions
	{
		public Functions()
		{
		}
	
		public static string funcGetParameter(string input,int pageId)
		{

			//Qerystring for a menu item is written as: name1==value1&&name2==value2&& etc.

			XmlDocument oXmlMenu = new XmlDocument();
			XmlNode oXmlNode;
			Regex regex1 = new Regex("&&");
			Regex regex2 = new Regex("==");
			string innerText = "";
			string strReturn = "";

			oXmlMenu = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;

			// Find parameter in XML document
			oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ pageId +"]/parameters/querystring");
			if (oXmlNode != null)
			{
				innerText = oXmlNode.InnerText;
				foreach(string parameter in regex1.Split(innerText))
					if (regex2.Split(parameter)[0].ToLower()==input.ToLower())
						strReturn = regex2.Split(parameter)[1];
			}
			return strReturn;
		}

        /// <summary>
        /// Should a specific language be shown on logon screen?
        /// If app-setting is not present, all languages will be shown
        /// </summary>
        public static bool IsLanguageActive(string language)
        {
            if (ConfigurationManager.AppSettings["languageCodes"] != null)
            {
                string[] languageCodes = ConfigurationManager.AppSettings["languageCodes"].Split(',');
                foreach (string languageCode in languageCodes)
                {
                    if (languageCode.ToLower() == language.ToLower())
                        return true;
                }
                return false;
            }
            return true;
        }

		public static string localText(string strString)
		{
			// Used to get language specific teksts in xsl
			if (HttpContext.Current.Session["languageCode"] == null && HttpContext.Current.Request.Cookies["languageCode"] != null)
				HttpContext.Current.Session["languageCode"] = HttpContext.Current.Request.Cookies["languageCode"].Value;

			string output = (string)Global.LanguageTexts.Get(strString +"_"+ HttpContext.Current.Session["languageCode"]);
            if (output == null)
                return "!!TEXT " + strString + " UNKNOWN IN LANGUAGE!!";
            else
                return HttpContext.Current.Server.HtmlDecode(output);
        }

		public static void loadAdminTexts()
		{
			// load language texts into custom collection
			XmlDocument oLanguageTextXml = new XmlDocument(); 
			Parameter parameter = new Parameter();
			XmlNodeList oXmlNodeRange;
			XmlNodeList oXmlNodeRange2;

			oLanguageTextXml.Load(Global.adminXmlPath + "/languageTexts.xml");

			// start with clearing the collection
			Global.LanguageTexts.Clear();

			oXmlNodeRange = oLanguageTextXml.SelectNodes("/website/language");
			for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
			{
				oXmlNodeRange2 = oXmlNodeRange.Item(counter).SelectNodes("item");
				for (int counter2 = 0; counter2 <= oXmlNodeRange2.Count-1;counter2++)
				{
					parameter.Key = oXmlNodeRange2.Item(counter2).Attributes["code"].Value + "_" + oXmlNodeRange.Item(counter).Attributes["code"].Value;
					parameter.Value = oXmlNodeRange2.Item(counter2).InnerText;
					Global.LanguageTexts.Add(parameter);
				}
			}
		}

		public static string GetPageFromUserfriendlyUrl(string input)
		{
            string url = "";
            string querystring = "";
			XmlNode oXmlNode;

            if (input.IndexOf('?') > -1)
            {
                url = input.Substring(0, input.IndexOf('?'));
                querystring = "&" + input.Substring(input.IndexOf('?')+1);
            }
            else
                url = input;

			// Find page with userfriendly url in XML document
			oXmlNode = Global.oMenuXml.SelectSingleNode("//page[friendlyurl='"+ url.ToLower().Replace("'","") +"' and public!='-1']");
			if (oXmlNode != null)
			{
				string templateid;
				string templateurl;
				XmlNode templateNode;
				
				templateid = oXmlNode.SelectSingleNode("usetemplate").InnerText;
				templateNode = Global.oMenuXml.SelectSingleNode("//template[@id='"+ templateid +"']");;
				templateurl = templateNode.SelectSingleNode("publicurl").InnerText;
				
				return templateurl +"?userfriendly=1&pageid="+ oXmlNode.Attributes["id"].Value + querystring;
			}
			else
				return "";
		}

		public static string Version()
		{
            // Used to get version number for KiteCMS from glocal.asax
			return Global.version;
		}

		public static string transformMenuXml(string xslUrl)
		{
			return transformMenuXml(xslUrl, null);
		}

		public static string transformMenuXml(string xslPath,XsltArgumentList xslArg)
		{
            XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(xslPath);

			StringWriter writer = new StringWriter();

			xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,xslArg,writer);

			return writer.ToString();
		}

		public static string transformXml(string xmlPath, string xslPath)
		{
			return transformXml(xmlPath, xslPath, null);
		}

		public static string transformXml(string xmlPath, string xslPath, XsltArgumentList xslArg)
		{
			XmlDocument oXml = new XmlDocument();
			oXml.Load(xmlPath);
			return transformXml(oXml, xslPath, xslArg);
		}

		public static string transformXml(XmlDocument xmlDoc, string xslPath, XsltArgumentList xslArg)
		{

            XslCompiledTransform xslt = new XslCompiledTransform();
			xslt.Load(xslPath);

			StringWriter writer = new StringWriter();

			xslt.Transform(xmlDoc,xslArg,writer);

			return writer.ToString();
		}

        public static string adminUrl()
        {
            string output;
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();

            xslArg.AddExtensionObject("urn:localText", functions);

            output = transformMenuXml(HttpContext.Current.Server.MapPath("/admin/menu/admin_url.xsl"), xslArg);

            return output;
        }

        public static string publicUrl()
        {
            string output;
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();

            xslArg.AddExtensionObject("urn:localText", functions);

            output = transformMenuXml(HttpContext.Current.Server.MapPath("/admin/menu/public_url.xsl"), xslArg);

            return output;
        }

        public static void publicUrlRedirect(int pageId)
        {
            Page page = new Page(pageId);
            Template template = new Template(page.TemplateId);
            HttpContext.Current.Response.Redirect(template.Publicurl + "?pageid=" + pageId);
        }

        public static void publicUrlRedirect(int pageId, string message)
        {
            Page page = new Page(pageId);
            Template template = new Template(page.TemplateId);
            HttpContext.Current.Response.Redirect(template.Publicurl + "?pageid=" + pageId + "&message=" + HttpContext.Current.Server.UrlEncode(message));
        }

        public static string ReplaceString(string string2add, string basestring, string regexp, bool WriteErrorMessage)
        {
            if (Regex.Match(basestring, regexp, RegexOptions.IgnoreCase | RegexOptions.Multiline).Length > 0)
            {
                return (Regex.Replace(basestring, regexp, string2add, RegexOptions.IgnoreCase | RegexOptions.Multiline));
            }
            else
            {
                if (WriteErrorMessage)
                    return "<span style='color:red'>String: " + HttpUtility.HtmlEncode(regexp) + " not found</span>" + basestring;
                else
                    return basestring;
            }
        }

        public static string RemoveTags(string input)
        {
            if (input != null)
                return Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
            else
                return "";
        }

        public bool userHasAccess(int intMethodId, int pageid)
        {
            Admin.Admin admin = new Admin.Admin();
            return admin.userHasAccess(intMethodId, pageid,false);
        }

        public string GetAdminShortcuts(int pageid)
        {
            XmlDocument oXmlAccess = new XmlDocument();
            XmlNode oXmlNodeUser;
            StringBuilder output = new StringBuilder();

            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");

			oXmlNodeUser = oXmlAccess.SelectSingleNode("//user[@id="+ HttpContext.Current.Session["userid"] +" and @active=1]");
            if (oXmlNodeUser != null)
            {
                XmlNodeList nodes = oXmlNodeUser.SelectNodes("shortcut");
                foreach (XmlNode node in nodes)
                    if (node.InnerText.IndexOf('¤')>0)
                        output.Append("<li><a href='" + node.InnerText.Split('¤')[1] + "'>" + node.InnerText.Split('¤')[0] + "</a></li>");
            }
            return output.ToString().Replace("##pageid##", pageid.ToString());
        }

        public string GetModulPageMenu(int pageid)
        {
   			Admin.Admin admin = new Admin.Admin();
			Admin.Page page = new Admin.Page(pageid);
			
			Admin.Template template = new Admin.Template(page.TemplateId);

            if (template.ModuleClassAdmin != "")
            {
                string[] moduleclass = template.ModuleClassAdmin.Split('@');

                Assembly module = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("/bin/" + moduleclass[1]));

                Type type = module.GetType(moduleclass[0], true, true);

                MethodInfo method = type.GetMethod("Menu");

                Object[] parameters = new object[2];
                parameters[0] = admin;
                parameters[1] = page;

                Object Agent = Activator.CreateInstance(type);

                string output = (string)method.Invoke(Agent, parameters).ToString();

                return output;
            }
            else
                return "";

        }

		public static string Encrypt(string password)
		{
			using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
				UTF8Encoding utf8 = new UTF8Encoding();
				var data = md5.ComputeHash(utf8.GetBytes(password));
				return Convert.ToBase64String(data);
            }
		}

		public static XmlDocument FilesAndFolders(int pageid)
		{
			XmlDocument xmlOutput = new XmlDocument();
			string uploadRootDirectory;
				
			uploadRootDirectory = System.Configuration.ConfigurationManager.AppSettings["uploadRootDirectory"];

			XmlElement xmlElem = xmlOutput.CreateElement("element","website","");
			if(Directory.Exists(HttpContext.Current.Server.MapPath(uploadRootDirectory)))
			{
				xmlElem.AppendChild(findFolders(HttpContext.Current.Server.MapPath(uploadRootDirectory), xmlOutput));
			}
			XmlElement xmlElem2 = xmlOutput.CreateElement("element","selectedpage","");
			xmlElem2.InnerText = pageid.ToString();
			xmlElem.AppendChild(xmlElem2);

			xmlOutput.AppendChild(xmlElem);

			return xmlOutput;
		}

		private static XmlElement findFolders(string currentFolderPath, XmlDocument xmlOutput)
		{
			string uploadRootDirectory;
            uploadRootDirectory = ConfigurationManager.AppSettings["uploadRootDirectory"];

			XmlElement xmlElem = xmlOutput.CreateElement("element","folder","");
			xmlElem.SetAttribute("rootpath", "", currentFolderPath.Replace(HttpContext.Current.Server.MapPath("/"),""));
            xmlElem.SetAttribute("uploadrootpath", "", currentFolderPath.Replace(HttpContext.Current.Server.MapPath(uploadRootDirectory), ""));
            xmlElem.SetAttribute("name", "", currentFolderPath.Replace(Directory.GetParent(currentFolderPath).ToString() + @"\", ""));

			// find subfolders
			foreach(string dir in Directory.GetDirectories(currentFolderPath))
				xmlElem.AppendChild(findFolders(dir, xmlOutput));

			//find files
			findFiles(currentFolderPath, xmlOutput, xmlElem);

			return xmlElem;
		}

		private static void findFiles(string currentFolderPath, XmlDocument xmlOutput, XmlElement xmlElemFolder)
		{
            string uploadRootDirectory = ConfigurationManager.AppSettings["uploadRootDirectory"];
            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"];

			// find graphic files
			foreach(string file in Directory.GetFiles(currentFolderPath))
			{
				bool fileIsGraphic = false;
				XmlElement xmlElem = xmlOutput.CreateElement("element","file","");

				// check if file type is allowed
				string strCurrentFileExtension = Path.GetExtension(file); 
				foreach(string extension in strFiletypeGraphic.Split(','))
					if (strCurrentFileExtension == "."+ extension)
					{
						fileIsGraphic = true;
					}

				xmlElem.SetAttribute("rootpath", "", file.Replace(HttpContext.Current.Server.MapPath("/"),""));
                xmlElem.SetAttribute("uploadrootpath", "", file.Replace(HttpContext.Current.Server.MapPath(uploadRootDirectory), ""));
				xmlElem.SetAttribute("fileisgraphic", "", fileIsGraphic.ToString().ToLower());
				xmlElem.SetAttribute("name","", file.Replace(currentFolderPath,""));
				xmlElemFolder.AppendChild(xmlElem);
			}
		}

		public static string EncodeSpecialChars(string text)
		{
			text = text.Replace(((char)0x91).ToString(),@"'");
			text = text.Replace(((char)0x92).ToString(),@"'");
			text = text.Replace(((char)0x93).ToString(),@"&quot;");
			text = text.Replace(((char)0x94).ToString(),@"&quot;");
			text = text.Replace(((char)0x96).ToString(),@"-");
			text = text.Replace(((char)0x97).ToString(),@"-");
			text = text.Replace(((char)0x85).ToString(),@"...");

			return text;
		}

		public string addBoxesToHtml(string output, Page page)
		{
			if (page.Boxes !=null)
			{
				for(int counter = 0; counter < page.Boxes.Count; counter++)
				{
					if (page.Boxes[counter].BoxCategory.Htmlstring != "")
					{
                        switch ((BoxCategoryTypeEnum)page.Boxes[counter].BoxCategory.BoxCategoryType)
						{
							case (BoxCategoryTypeEnum.replace): //replace
							{
								output = Regex.Replace(output, page.Boxes[counter].BoxCategory.Htmlstring, page.Boxes[counter].Html(),RegexOptions.IgnoreCase|RegexOptions.Multiline);
								break;
							}
							case (BoxCategoryTypeEnum.insertAbove): //insertAbove
							{
								output = Regex.Replace(output, "("+ page.Boxes[counter].BoxCategory.Htmlstring +")", page.Boxes[counter].Html() +"$1",RegexOptions.IgnoreCase|RegexOptions.Multiline);
								break;
							}
							case (BoxCategoryTypeEnum.insertBelow): //insertBelow
							{
								output = Regex.Replace(output, "("+ page.Boxes[counter].BoxCategory.Htmlstring +")",  "$1"+ page.Boxes[counter].Html(),RegexOptions.IgnoreCase|RegexOptions.Multiline);
								break;
							}

						}
					}
				}
			}

			output = output.Replace("##pageid##", page.Pageid.ToString());
			if (output.IndexOf("##pagetitle##")>-1)
			{
				string pagetitle = page.Title;
				output = output.Replace("##pagetitle##", pagetitle);
			}

			return output;
		}

		public static bool SendMail(string subject, string body, string mailTo, string mailFrom, string mailBcc = null)
		{
			bool error = false;

			MailMessage mailMessage = new MailMessage();
			mailMessage.IsBodyHtml = false;
			mailMessage.From = new MailAddress(mailFrom);
			mailMessage.To.Add(mailTo);
			if (mailBcc != null)
            {
				mailMessage.Bcc.Add(mailBcc);
            }
			mailMessage.Subject = subject;
			mailMessage.Body = body;

			try 
			{
				SmtpClient emailClient = new SmtpClient(ConfigurationManager.AppSettings["smtpMailServer"]);
				if (ConfigurationManager.AppSettings["smtpMailServerUsername"] != null && ConfigurationManager.AppSettings["smtpMailServerPassword"] != null)
				{
					System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpMailServerUsername"], ConfigurationManager.AppSettings["smtpMailServerPassword"]);
					emailClient.UseDefaultCredentials = false;
					emailClient.Credentials = SMTPUserInfo;
				}
				emailClient.Send(mailMessage);
			}
			catch (Exception e)
			{
				string er = e.Message;
				error = true;
			}

			return error;
		}

        public static string BreakLines(string input)
        {
            // insert linebreaks as the body of an mail can only be 1024 characters
            input = Regex.Replace(input, "(<br[ ]?/>)", "$1" + Environment.NewLine, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return input;
        }

        public static string string2hex(string input)
        {
            input = EncodeNonAsciiCharacters(input);
            Byte[] byteArray;
            StringBuilder hexNumbers = new StringBuilder();
            byteArray = System.Text.ASCIIEncoding.Unicode.GetBytes(input);

            for (int i = 0; i < byteArray.Length; i++)
                if (byteArray[i].ToString("x").Length == 1)
                    hexNumbers.Append("%0" + byteArray[i].ToString("x"));
                else
                    hexNumbers.Append("%" + byteArray[i].ToString("x"));

            return hexNumbers.ToString();
        }

        public static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

		public static string CleanEditorCode(string html, TextWriter log, bool lower, bool formatted, bool formatFromEditor, ref bool hasEmptyAltTags)
		{
			string output = "";
			string strXml = "";
			if (html != "")
			{
				SgmlReader r = new SgmlReader();
				r.DocType = "HTML";
				r.InputStream = new StringReader(html);
				if (lower)
					r.CaseFolding = CaseFolding.ToLower;
				else
					r.CaseFolding = CaseFolding.ToUpper;
				StringWriter sw = new StringWriter();
				XmlTextWriter w = new XmlTextWriter(sw);
				if (formatted) 
				{
					w.Formatting = Formatting.Indented;
					r.WhitespaceHandling = WhitespaceHandling.None;
				}
				while (!r.EOF) 
				{
					w.WriteNode(r, true);
				}
				w.Close();
				XmlDocument xmldoc = new XmlDocument();
				strXml = sw.ToString();
				if (!strXml.StartsWith("<html>"))
					strXml = "<html>"+ strXml +"</html>";
				
				xmldoc.LoadXml(strXml);

				// clean up html code
				if (formatFromEditor)
				{
                    XmlNodeList nodes = xmldoc.SelectNodes("//*[local-name() = 'p' or local-name() = 'h1' or local-name() = 'h2' or local-name() = 'h3' or local-name() = 'h4' or local-name() = 'h5'][@align and @align!='']");
					if (nodes != null)
						convertAlign(ref nodes);
					nodes = xmldoc.SelectNodes("//*[local-name() = 'img'][not(@alt!='')]");
					if (nodes != null)
						addAltTag(ref nodes);
					convertFont(ref xmldoc);
                    convert2Strong(ref xmldoc);
                    convert2EM(ref xmldoc);

                    // does the html contain images with empty alt-tags?
  					nodes = xmldoc.SelectNodes("//*[local-name() = 'img'][@alt='']");
                    if ((nodes != null && nodes.Count >0) || hasEmptyAltTags)
                        hasEmptyAltTags = true;
                    else
                        hasEmptyAltTags = false;

                }
				else
				{
                    XmlNodeList nodes = xmldoc.SelectNodes("//*[local-name() = 'p' or local-name() = 'h1' or local-name() = 'h2' or local-name() = 'h3' or local-name() = 'h4' or local-name() = 'h5'][@style and @style!='']");
					convertStyle(ref nodes);
				}
				output = xmldoc.SelectSingleNode("//html").InnerXml;
				output = output.Replace("<html>","");
				output = output.Replace("</html>","");
				output = output.Replace("<![CDATA[","");
				output = output.Replace("]]>","");
			}
			return output;              
		}

		private static void convertAlign(ref XmlNodeList nodes)
		{
			for (int counter = 0; counter <= nodes.Count-1;counter++)
			{
				XmlElement elem = (XmlElement)(nodes.Item(counter));
				if (elem.Attributes["style"] == null)
					elem.SetAttribute("style", "", "text-align: "+ elem.Attributes["align",""].InnerText.Trim() +";");
				else
					if (elem.Attributes["style"].InnerText.ToLower().IndexOf("text-align:")==-1)
					elem.Attributes["style"].InnerText = "text-align:"+ elem.Attributes["align",""].InnerText.Trim() +";"+ elem.Attributes["style"].InnerText;
				else
					elem.Attributes["style"].InnerText = Regex.Replace(elem.Attributes["style"].InnerText,"(.*)text-align:[ ]*(left|center|right|justify)[ ]*[;]?(.*)","text-align: "+ elem.Attributes["align",""].InnerText.Trim() +";$1$3",RegexOptions.IgnoreCase|RegexOptions.Multiline).Trim();
				elem.RemoveAttribute("align");
			}
		}

		private static void convertStyle(ref XmlNodeList nodes)
		{
			for (int counter = 0; counter <= nodes.Count-1;counter++)
			{
				XmlElement elem = (XmlElement)(nodes.Item(counter));
                if (Regex.Match(elem.Attributes["style"].InnerText,"(.*)text-align:[ ]*(left|center|right|justify)[ ]*[;]?(.*)",RegexOptions.IgnoreCase|RegexOptions.Multiline).Length > 0)
	    			elem.SetAttribute("align", "", Regex.Replace(elem.Attributes["style"].InnerText,"(.*)text-align:[ ]*(left|center|right|justify)[ ]*[;]?(.*)","$2",RegexOptions.IgnoreCase|RegexOptions.Multiline));
				string styleValue =Regex.Replace(elem.Attributes["style"].InnerText,"(.*)text-align:[ ]*(left|center|right|justify)[ ]*[;]?(.*)","$1$3",RegexOptions.IgnoreCase|RegexOptions.Multiline);
				if (styleValue.Replace(";","").Trim() == "")
					elem.RemoveAttribute("style");
				else
					elem.Attributes["style"].InnerText = styleValue.Trim();
			}
		}

		private static void convertFont(ref XmlDocument xmldoc)
		{
			XmlNode node;
			
			node = xmldoc.SelectSingleNode("//font[@color != '']");
			while (node != null)
			{
				XmlElement newElem = xmldoc.CreateElement("element","span","");
				newElem.SetAttribute("style", "", "color:"+ node.Attributes["color"].InnerText +";");
				node.ParentNode.InsertAfter(newElem,node);
				newElem.InnerXml = node.InnerXml;
				node.ParentNode.InsertAfter(newElem,node);
				node.ParentNode.RemoveChild(node);

				node = xmldoc.SelectSingleNode("//font[@color != '']");
			}
		}

        private static void convert2Strong(ref XmlDocument xmldoc)
        {
            XmlNode node;

            node = xmldoc.SelectSingleNode("//span[@style = 'font-weight: bold;']");
            while (node != null)
            {
                XmlElement newElem = xmldoc.CreateElement("element", "strong", "");
                node.ParentNode.InsertAfter(newElem, node);
                newElem.InnerXml = node.InnerXml;
                node.ParentNode.InsertAfter(newElem, node);
                node.ParentNode.RemoveChild(node);

                node = xmldoc.SelectSingleNode("//span[@style = 'font-weight: bold;']");
            }
        }

        private static void convert2EM(ref XmlDocument xmldoc)
        {
            XmlNode node;

            node = xmldoc.SelectSingleNode("//span[@style = 'font-style: italic;']");
            while (node != null)
            {
                XmlElement newElem = xmldoc.CreateElement("element", "em", "");
                node.ParentNode.InsertAfter(newElem, node);
                newElem.InnerXml = node.InnerXml;
                node.ParentNode.InsertAfter(newElem, node);
                node.ParentNode.RemoveChild(node);

                node = xmldoc.SelectSingleNode("//span[@style = 'font-style: italic;']");
            }
        }
        
        private static void convertSpan(ref XmlDocument xmldoc)
		{
			XmlNode node;
			
			node = xmldoc.SelectSingleNode("//*[local-name() = 'span']");
			while (node != null)
			{
				if (Regex.Match(node.Attributes["style"].InnerText,@"color:[ ]*['""]?(([a-z]+)|(#[\da-f]{3,6}))['""]?[ ]*;?",RegexOptions.IgnoreCase|RegexOptions.Multiline).Length == 1)
				{
					XmlElement newElem = xmldoc.CreateElement("element","font","");
					node.ParentNode.InsertAfter(newElem,node);
					newElem.SetAttribute("color", "", Regex.Replace(node.Attributes["style"].InnerText,@"color:[ ]*['""]?(([a-z]+)|(#[\da-f]{3,6}))['""]?[ ]*;?","$2",RegexOptions.IgnoreCase|RegexOptions.Multiline).Trim());
					newElem.InnerXml = node.InnerXml;
					node.ParentNode.InsertAfter(newElem,node);
					node.ParentNode.RemoveChild(node);
				}
				else
				{
					XmlElement elem = (XmlElement)node;
					elem.SetAttribute("hascolor","","false");
				}
				node = xmldoc.SelectSingleNode("//*[local-name() = 'span'][@hascolor='']");
			}
			node = xmldoc.SelectSingleNode("//*[local-name() = 'span'][@hascolor!='']");
			while (node != null)
			{
				XmlElement elem = (XmlElement)node;
				elem.RemoveAttribute("hascolor");
				node = xmldoc.SelectSingleNode("//*[local-name() = 'span'][@hascolor!='']");
			}
		}

		private static void addAltTag(ref XmlNodeList nodes)
		{
			for (int counter = 0; counter <= nodes.Count-1;counter++)
			{
				XmlElement elem = (XmlElement)(nodes.Item(counter));
				elem.SetAttribute("alt", "", "");
			}
		}

        public static string EncodeEmails(string htmlString)
		{
				// if the </html> closing tag is found, this is the end of the file, so let's do the replace work
            if (Regex.IsMatch(htmlString, "</html>", RegexOptions.IgnoreCase))
            {
                StringBuilder result = new StringBuilder();

                // search for e-mail links
                int index = 0;
                Regex regExpEmail = new Regex(@"\w+([-+.]\w+)*?@\w+([-.]\w+)*\.\w+([-.]\w+)*?"); 
                Regex re = new Regex(@"(?<startOfTag><a .*?)(?<email>mailto:\w+([-+.]\w+)*?@\w+([-.]\w+)*\.\w+([-.]\w+)*?(\?.*?)?)(?<restOfTag>"".*?>)(?<text>(.|\s)*?)</a>", RegexOptions.IgnoreCase);
                foreach (Match ma in re.Matches(htmlString))
                {
                    string newText = "";
                    // add to the output text the substring before this match
                    result.Append(htmlString.Substring(index, ma.Index - index));
                    // add to the output string the text resulting from the encoding of the mail address
                    string email = BitConverter.ToString(ASCIIEncoding.ASCII.GetBytes(ma.Groups["email"].Value)).Replace("-", "");
                    string text = ma.Groups["text"].Value;
                    if (regExpEmail.IsMatch(text))
                    {
                        for (int i = text.Length-1; i >= 0; i--)
                            newText += text.Substring(i, 1);
                        text = "<script type=\"text/javascript\">document.write(\""+ newText +"\".split(\"\").reverse().join(\"\"));</script>";
                    }
                    result.AppendFormat("{0}javascript:sendEmailDecode('{1}');{2}{3}</a>", ma.Groups["startOfTag"],email,ma.Groups["restOfTag"].Value,text);
                    // increment the index so that it starts after this processed match
                    index = ma.Index + ma.Length;
                }

                // append the rest of the input string until </body>
                Match ma2 = Regex.Match(htmlString, "</body>", RegexOptions.IgnoreCase);
                result.Append(htmlString.Substring(index, ma2.Index - index));

                // appendthe client-side javascript that decodes the e-mail address and launches a new instance of the default e-mail client
                // NOTE: this JS routine is taken from the "Fotovision Web Application" by Vertigo Software, described here:
                // http://msdn.microsoft.com/smartclient/codesamples/fotovision/default.aspx?pull=/library/en-us/dnnetcomp/html/fotovisionweb.asp
                result.Append(@"
                        <script type=""text/javascript"">
                        // <![CDATA[ 
                        function sendEmailDecode(encodedEmail)
                        {
	                        var email = """";
	                        for (i=0; i < encodedEmail.length;)
	                        {
		                        var letter = """";
		                        letter = encodedEmail.charAt(i) + encodedEmail.charAt(i+1)
		                        email += String.fromCharCode(parseInt(letter,16));
		                        i += 2;
	                        }
	                        location.href = email;
                        }
                        // ]]> 
                        </script>");

                // append the rest of the original html source
                result.Append(htmlString.Substring(ma2.Index));

                return result.ToString();
            }
            else
                return htmlString;
		}

        public static void AddHtmlHeader(string value)
        {
            SetRequestVariable("htmlheader", value);
        }

        internal static string GetHtmlHeader()
        {
            return GetRequestVariable("htmlheader");
        }

        public static void AddBodyEnd(string value)
        {
            SetRequestVariable("bodyend", value);
        }

        internal static string GetBodyEnd()
        {
            return GetRequestVariable("bodyend");
        }

        internal static void SetRequestVariable(string key, string value)
        {
            if (HttpContext.Current.Items[key] == null)
                HttpContext.Current.Items[key] = value;
            else
                HttpContext.Current.Items[key] += value;
        }

        internal static string GetRequestVariable(string key)
        {
            if (HttpContext.Current.Items[key] == null)
                return "";
            else
                return HttpContext.Current.Items[key].ToString();
        }
        internal static void UpdateAppSettingKey(string key,string value)
        {

            Configuration configuration = WebConfigurationManager.OpenWebConfiguration("~");
            AppSettingsSection appSettingsSection = (AppSettingsSection)configuration.GetSection("appSettings");

            if (appSettingsSection != null)
            {
                if (appSettingsSection.Settings[key] != null)
                {
                    appSettingsSection.Settings[key].Value = value;
                }
                else
                {
                    appSettingsSection.Settings.Add(key, value);
                }
                configuration.Save();
            }
        }
    }
}
