using System;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml;

namespace KiteCMS.Admin
{
    /// <summary>
    /// Summary description for image_view.
    /// </summary>
    public class chooseinternallink : System.Web.UI.Page
	{

		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			StringBuilder output = new StringBuilder();
			XmlNodeList nodes = Global.oMenuXml.SelectNodes("//page");

			foreach(XmlNode node in nodes)
			{
				XmlNodeList indentNodes = node.SelectNodes("ancestor::page");
				for(int counter = 0 ; counter < indentNodes.Count; counter ++)
					output.Append("&nbsp;&nbsp;");
				switch (node.SelectSingleNode("public").InnerText)
				{
					case "-1":
					{
						output.Append("<a onclick='funcLinkChosen("+ node.Attributes["id",""].Value +");'>("+ node.SelectSingleNode("menutitle").InnerText + ")</a><br/>");
						break;
					}
					case "0":
					{
						output.Append("<a onclick='funcLinkChosen("+ node.Attributes["id",""].Value +");'>{"+ node.SelectSingleNode("menutitle").InnerText + "}</a><br/>");
						break;
					}
					default:
					{
						output.Append("<a onclick='funcLinkChosen("+ node.Attributes["id",""].Value +");'>"+ node.SelectSingleNode("menutitle").InnerText + "</a><br/>");
						break;
					}
				}
			}

			content.Text = output.ToString();
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
