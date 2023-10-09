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
	public class edituser : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
        XmlDocument oXmlAccess = new XmlDocument();
        XmlNode oXmlNode;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1602,page.Pageid);
			header.Text = Functions.localText("edituser");

            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");
            oXmlNode = oXmlAccess.SelectSingleNode("//selectedpage");
            oXmlNode.InnerText = page.Pageid.ToString();
            
            if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "deleteuser")
				funcDeleteUser();
			else if (action == "edit")
				funcDoEditUser();
			else if (action == "formedit")
				funcFormEditUser();
			else
				funcListUser();

		}

		private void funcDeleteUser()
		{
			int userId = -1;

			if(HttpContext.Current.Request["userid"] != null)
				userId = int.Parse(HttpContext.Current.Request["userid"].ToString());

			if (userId != -1)
			{
				User user = new User(userId);
				user.Delete();

				content.Text += Functions.localText("userdeleted") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");
		}

		private void funcDoEditUser()
		{
			string password = "";
			string fullname = "";
			int userId = -1;
			int active = 0;
			int changePassword = 0;
            bool resetFailedLogins = false;

			if(HttpContext.Current.Request["userid"] != null)
				userId = int.Parse(HttpContext.Current.Request["userid"].ToString());
			if(HttpContext.Current.Request["fullname"] != null)
				fullname = HttpContext.Current.Request["fullname"].ToString();
			if(HttpContext.Current.Request["password"] != null && HttpContext.Current.Request["password"].ToString() != "")
				password = HttpContext.Current.Request["password"].ToString();
			if(HttpContext.Current.Request["active"] != null)
				active = 1;
            if (HttpContext.Current.Request["changepassword"] != null)
                changePassword = 1;
            if (HttpContext.Current.Request["resetFailedLogins"] != null)
                resetFailedLogins = true;

			if (userId != -1)
			{
				User user = new User(userId);
				user.Fullname = fullname;
				if (password != "")
					user.Password = password;
				user.Active = active;
				user.Changepassword = changePassword;
                if (resetFailedLogins)
                    user.Failedlogins = 0;

				ValueCollection menuids = new ValueCollection();
				ValueCollection moduleids = new ValueCollection();

				foreach(string obj in HttpContext.Current.Request.Form)
				{
					if(obj.StartsWith("menuid"))
					{
						menuids.Add(obj.Substring(6));
					}
					else if(obj.StartsWith("id"))
					{
						moduleids.Add(obj.Substring(2));
					}
				}

				user.ModuleAccess = moduleids;
				user.PageAccess = menuids;

				user.Save();

                content.Text += Functions.localText("usersaved") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");
		}

		private void funcFormEditUser()
		{
			string output;
			string output2;
			string userid= "-1";

			if(HttpContext.Current.Request["userid"]!= null)
				userid = HttpContext.Current.Request["userid"].ToString();

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("user","",userid);

			output = Functions.transformMenuXml(Server.MapPath("/admin/useradmin/form_user_pages.xsl"),xslArg);
            output2 = Functions.transformXml(oXmlAccess, Server.MapPath("/admin/useradmin/form_edit_user.xsl"), xslArg);

			content.Text += output2.Replace("<content />",output);
		}

		private void funcListUser()
		{
			string output;

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
            
            xslArg.AddExtensionObject("urn:localText", functions);

            output = Functions.transformXml(oXmlAccess, Server.MapPath("/admin/useradmin/list_edit_user.xsl"), xslArg);

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
