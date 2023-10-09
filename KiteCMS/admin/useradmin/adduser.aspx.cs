using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

namespace KiteCMS.Admin
{
    /// <summary>
    /// Summary description for edittemplate.
    /// </summary>
    public class adduser : System.Web.UI.Page
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
			admin.userHasAccess(1601,page.Pageid);
			header.Text = Functions.localText("adduser");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");
			oXmlNode = oXmlAccess.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = page.Pageid.ToString();

			if (action == "add")
				funcDoAddUser();
			else
				funcFormAddUser();

		}

		private void funcDoAddUser()
		{
			string username = "";
			string fullname = "";
			string password = "";
			int active = 0;
			int changePassword = 0;

			if(HttpContext.Current.Request["username"] != null)
				username = HttpContext.Current.Request["username"].ToString();
			if(HttpContext.Current.Request["fullname"] != null)
				fullname = HttpContext.Current.Request["fullname"].ToString();
			if(HttpContext.Current.Request["password"] != null)
				password = HttpContext.Current.Request["password"].ToString();
			if(HttpContext.Current.Request["active"] != null)
				active = 1;
			if(HttpContext.Current.Request["changepassword"] != null)
				changePassword = 1;

			if (username != "" && password != "")
			{
				User user = new User();
				user.Username = username;
				user.Fullname = fullname;
				user.Password = password;
				user.Active = active;
				user.Changepassword = changePassword;

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

				if (!user.UsernameExists())
				{
					user.Save();

					content.Text += Functions.localText("usersaved") + Functions.localText("goto") + Functions.publicUrl();
				}
				else
				{
					// There already exists one user with this username
					content.Text += Functions.localText("userexists") + Functions.publicUrl();
				}
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");
		}

		private void funcFormAddUser()
		{
			string output;
			string output2;
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			output = Functions.transformMenuXml(Server.MapPath("/admin/useradmin/form_user_pages.xsl"),xslArg);
            output2 = Functions.transformXml(oXmlAccess, Server.MapPath("/admin/useradmin/form_add_user.xsl"), xslArg);

			content.Text += output2.Replace("<content />",output);
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
