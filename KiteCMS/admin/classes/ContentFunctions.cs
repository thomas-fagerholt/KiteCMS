using System;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Web;
using System.Web.Mail;
using System.IO;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using KiteCMS.Admin;
using System.Reflection;
using Sgml;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for functions.
	/// </summary>
    public class ContentFunctions
    {
        public ContentFunctions()
        {
        }

        public string HTMLHolder()
        {
            return "";
        }

        public string GetPageId()
        {
            return ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//selectedpage").InnerText;
        }


        public string GetRootPageId(string pageid)
        {
            XmlNode oXmlNode;

            // Find page with userfriendly url in XML document
            oXmlNode = Global.oMenuXml.SelectSingleNode("/website/page[descendant-or-self::page[@id='" + pageid + "']]");
            if (oXmlNode != null)
            {
                string parentid = oXmlNode.Attributes["id", ""].Value;
                return parentid;
            }
            else
                return "-1";
        }

        public string GetQuerystring(string key)
        {
            if (HttpContext.Current.Request.QueryString[key] != null)
                return HttpContext.Current.Request.QueryString[key];
            else
                return "";
        }

        public string GetForm(string key)
        {
            if (HttpContext.Current.Request.Form[key] != null)
                return HttpContext.Current.Request.Form[key];
            else
                return "";
        }

        public string GetSession(string key)
        {
            if (HttpContext.Current.Session[key] != null && key.ToLower() != "userid")
                return HttpContext.Current.Session[key].ToString();
            else
                return "";
        }

        public string GetServerVariable(string key)
        {
            if (HttpContext.Current.Request.ServerVariables[key] != null && key.ToLower() != "AUTH_PASSWORD")
                return HttpContext.Current.Request.ServerVariables[key].ToString();
            else
                return "";
        }

        public void SetSession(string key, string content)
        {
            if (key.ToLower() != "userid")
                HttpContext.Current.Session[key] = content;
        }

        public string CutString(string input, int length)
        {
            string output;
            if (input.Length <= length)
                output = input;
            else
            {
                output = input.Substring(0, length);
                output = output.Substring(0, output.LastIndexOf(" ")) + " ...";
            }
            return output;
        }

        public int GetRandom(int min, int max)
        {
            if (min < max)
            {
                Random RandomClass = new Random();
                return RandomClass.Next(min, max+1);
            }
            else
                return 0;
        }

        public string formatDate(string input, string format)
        {
            try
            {
                return DateTime.Parse(input).ToString(format);
            }
            catch
            {
                return input;
            }
        }

        public string GetImage(string input)
        {
            try
            {
                Regex regex = new Regex(".*(<img[^>]*).*");
                if (regex.IsMatch(input))
                    return regex.Replace(input, "$1>");
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public string GetImageSrc(string input)
        {
            try
            {
                input = GetImage(input);
                Regex regex = new Regex(@"^.*src=[""']?((?:.(?![""']?\s+(?:\S+)=|[>""']))+.)[""']?.*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (regex.IsMatch(input))
                    return regex.Replace(input, "$1");
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }

        public int GetCommentCount(int pageid)
        {
            XmlDocument xmlComment = new XmlDocument();
            try
            {
                xmlComment.Load(Global.publicXmlPath + "/comments.xml");

                XmlNodeList nodes = xmlComment.SelectNodes("//commentItem[@pageid=" + pageid + " and @active=1]");

                return nodes.Count;
            }
            catch
            {
                return 0;
            }
        }

        public static string ReturnSpaceIfEmpty(string input)
        {
            if (string.IsNullOrEmpty(input))
                return " ";
            else
                return "";
        }

        public static string localText(string strString)
        {
            // Used to get language specific texts in xsl
            if (HttpContext.Current.Session["languageCode"] == null && HttpContext.Current.Request.Cookies["languageCode"] != null)
                HttpContext.Current.Session["languageCode"] = HttpContext.Current.Request.Cookies["languageCode"].Value;

            string output = (string)Global.LanguageTexts.Get(strString + "_" + HttpContext.Current.Session["languageCode"]);
            if (output == null)
                return "!!TEXT " + strString + " UNKNOWN IN LANGUAGE!!";
            else
                return HttpContext.Current.Server.HtmlDecode(output);
        }



        public static string GetEditMode()
        {
            return ((Global)HttpContext.Current.ApplicationInstance).EditMode.ToString();
        }
}
	
}
