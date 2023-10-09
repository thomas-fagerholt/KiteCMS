using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Xml.Xsl;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Text;
using KiteCMS.Admin;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for instable.
	/// </summary>
	public class instable : System.Web.UI.Page
	{
		protected Literal userdefinedTables;

		private void Page_Load(object sender, System.EventArgs e)
		{
			XmlDocument oXmlTable = new XmlDocument(); 
			XmlNodeList oXmlNodeRange;
			StringBuilder output = new StringBuilder();

			if (File.Exists(Global.adminXmlPath + "/userDefinedTables.xml"))
			{
				oXmlTable.Load(Global.adminXmlPath + "/userDefinedTables.xml");
				oXmlNodeRange = oXmlTable.SelectNodes("//table");

				output.Append("<script type=\"text/javascript\"><!--"+ Environment.NewLine);
				output.Append("var arrTables = new Array("+ oXmlNodeRange.Count +");"+ Environment.NewLine);

				for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
					output.Append("arrTables["+ counter +"]='"+ oXmlNodeRange.Item(counter).SelectSingleNode("html").InnerText.Replace("'",@"\'").Replace(Environment.NewLine,"") +"';"+ Environment.NewLine);

				output.Append("// -->"+ Environment.NewLine);
				output.Append("</script>"+ Environment.NewLine);
 
				output.Append("<hr/><p>"+ Functions.localText("Specieltabel") +":</p><br/><p style='text-align:center'><select class='alminput' name='usertable' id='usertable'>"+ Environment.NewLine);
				for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
					output.Append("<option value=\""+ counter +"\">"+ oXmlNodeRange.Item(counter).Attributes["name"].Value +"</option>"+ Environment.NewLine);

				output.Append("</select></p>"+ Environment.NewLine);

				output.Append("<p><button onclick='funcUserOk();' class='almknap' type='submit'>"+ Functions.localText("ok") +"</button>"+ Environment.NewLine);
				output.Append("<button onclick='window.close();' class='almknap'>"+ Functions.localText("cancel") +"</button></p>"+ Environment.NewLine);

				userdefinedTables.Text = output.ToString();
			}
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
