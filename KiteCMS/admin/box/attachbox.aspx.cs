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
	public class attachbox : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1701,page.Pageid);
			header.Text = Functions.localText("attachbox");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "add")
				funcDoAddBoxAttached(pageId);
			else if (action == "moveup")
				funcDoMoveUpBox(pageId);
			else if (action == "movedown")
				funcDoMoveDownBox(pageId);
			else if (action == "delete")
				funcDoDeleteBoxAttached(pageId);

			funcListBoxAttached();

		}

		private void funcDoAddBoxAttached(int pageId)
		{
			int boxId = -1;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());

			if (pageId != -1 && boxId != -1)
			{
				Page page = new Page(pageId, true);
				Box box = new Box(boxId);

				BoxCollection boxes = page.Boxes;
			
				boxes.Add(box);

				page.Boxes = boxes;
				page.Save();
			}
		}

		private void funcDoMoveUpBox(int pageId)
		{
			int boxId = -1;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());

			if (pageId != -1 && boxId != -1)
			{
				try 
				{
					int thisindex = -1;
					Page page = new Page(pageId, true);
					Box box = new Box(boxId);
					Box otherBox;

					BoxCollection boxes = page.Boxes;
			
					thisindex = boxes.IndexOf(box);
					otherBox = boxes[thisindex-1];

					boxes[thisindex-1] = box;
					boxes[thisindex] = otherBox;

					page.Boxes = boxes;
					page.Save();
				}
				catch {}
			}
		}

		private void funcDoMoveDownBox(int pageId)
		{
			int boxId = -1;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());

			if (pageId != -1 && boxId != -1)
			{
				try 
				{
					int thisindex = -1;
					Page page = new Page(pageId, true);
					Box box = new Box(boxId);
					Box otherBox;

					BoxCollection boxes = page.Boxes;
			
					thisindex = boxes.IndexOf(box);
					otherBox = boxes[thisindex+1];

					boxes[thisindex+1] = box;
					boxes[thisindex] = otherBox;

					page.Boxes = boxes;
					page.Save();
				}
				catch {}

			}
		}

		private void funcDoDeleteBoxAttached(int pageId)
		{
			int boxId = -1;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());

			if (pageId != -1 && boxId != -1)
			{
				Page page = new Page(pageId, true);
				Box box = new Box(boxId);

				BoxCollection boxes = page.Boxes;
			
				boxes.Remove(box);

				page.Boxes = boxes;
				page.Save();
			}
		}


		private void funcListBoxAttached()
		{
			string output;

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/list_attach_box.xsl"),xslArg);

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
