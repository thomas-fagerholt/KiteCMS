using System;
using System.Web;
using System.Web.SessionState;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Collections;
using System.Reflection;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
    public class page404 : System.Web.UI.Page
	{

        override protected void OnInit(EventArgs e)
        {
			string pageurl;
			string userfriendlyurl = "";

			if (HttpContext.Current.Request.ServerVariables["QUERY_STRING"] != null && HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString() != "")
			{
				userfriendlyurl = HttpContext.Current.Request.ServerVariables["QUERY_STRING"].ToString();
				userfriendlyurl = userfriendlyurl.Substring(userfriendlyurl.IndexOf(HttpContext.Current.Request.ServerVariables["SERVER_NAME"])+HttpContext.Current.Request.ServerVariables["SERVER_NAME"].Length);
				if (userfriendlyurl.IndexOf(":")>-1)
					userfriendlyurl =userfriendlyurl.Remove(0,userfriendlyurl.IndexOf(":"+ HttpContext.Current.Request.ServerVariables["SERVER_PORT"])+1+HttpContext.Current.Request.ServerVariables["SERVER_PORT"].Length);
				if (userfriendlyurl.LastIndexOf("/") == userfriendlyurl.Length - 1)
					userfriendlyurl = userfriendlyurl.Substring(0,userfriendlyurl.Length-1);

                if (userfriendlyurl.IndexOf(".") == -1 && ConfigurationManager.AppSettings["useUserfriendlyUrl"] != null && bool.Parse(ConfigurationManager.AppSettings["useUserfriendlyUrl"]))
				{
					pageurl = Functions.GetPageFromUserfriendlyUrl(userfriendlyurl);
					if (pageurl != "")
						HttpContext.Current.Server.Transfer(pageurl);
						//HttpContext.Current.Response.Redirect(pageurl);
					else
					{
                        HttpContext.Current.Response.StatusCode = 404;
                        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN""><html dir=ltr><head><META NAME=""ROBOTS"" CONTENT=""NOINDEX""><title>The page cannot be found</title></head><body bgcolor=FFFFFF><h1 style=""COLOR:000000; FONT: 13pt/15pt verdana;"">The page cannot be found</h1><font style=""COLOR:000000; FONT: 8pt/11pt verdana; width:360px"">The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.<br/><br/>HTTP 404 - File not found<br/>Internet Information Services</font></body></html>");
                        HttpContext.Current.Response.End();
                        return;
					}
				}
			}

            HttpContext.Current.Response.StatusCode = 404;
            HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN""><html dir=ltr><head><META NAME=""ROBOTS"" CONTENT=""NOINDEX""><title>The page cannot be found</title></head><body bgcolor=FFFFFF><h1 style=""COLOR:000000; FONT: 13pt/15pt verdana;"">The page cannot be found</h1><font style=""COLOR:000000; FONT: 8pt/11pt verdana; width:360px"">The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.<br/><br/>HTTP 404 - File not found<br/>Internet Information Services</font></body></html>");
            HttpContext.Current.Response.End();
            return;
		}
	}
}
