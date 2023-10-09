namespace KiteCMS.Admin
{
	using System;
	using System.Text;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using System.Configuration;
	using KiteCMS.Admin.core;

	/// <summary>
	///		Summary description for IEeditor.
	/// </summary>
	public class IEeditor : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label editor;
		protected Literal options;
		protected Literal litJavascript;
		public string pageId;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			StringBuilder javascript = new StringBuilder();
			pageId = admin.loadLocalMenuXml().ToString();

			editor.Text = "<SPAN CONTENTEDITABLE WIDTH=1 height=1 id=tbContentTemp unselectable=off style='LEFT:-1500px;POSITION:absolute;TOP:0px'></SPAN>";
            editor.Text += "<span class=tbContent><SPAN CONTENTEDITABLE style='width:" + ConfigurationManager.AppSettings["EditorWidth"] + ";height:200px' id=tbContent unselectable=off></SPAN></SPAN>";

			// find styles to add to dropdown from stylesheet
            if (ConfigurationManager.AppSettings["EditorStyles"] != null)
			{
                string[] styles = ConfigurationManager.AppSettings["EditorStyles"].Split(',');
				for (int i = 0; i < styles.Length; i++)
					options.Text += "<option value='"+ styles[i].Trim() +"'>"+ styles[i].Trim() +"</option>";
			}

			javascript.Append("<script for='tbContent' event='onpaste'>");
			javascript.Append(" var curAe = document.selection.createRange();");
			javascript.Append(" tbContentTemp.innerHTML = '';");
			javascript.Append(" tbContentTemp.focus();");
			javascript.Append(" document.execCommand('paste');");
			javascript.Append(" var text = tbContentTemp.innerHTML;");
			javascript.Append(" if(text.search(/class=mso/gi) != -1 || text.search(/mso-/gi) != -1)");
            if (Global.ValidXhtml)
				javascript.Append("alert('"+ Functions.localText("alertpasteword1") +"');");
			else
				javascript.Append("if(confirm('"+ Functions.localText("alertpasteword1") + Functions.localText("alertpasteword2") +"')){");

			javascript.Append(@"text = text.replace(/<(?!BR\b|\/?P\b|\/?ol\b|\/?h[1-6]\b|\/?ul\b|\/?li\b).[^<>]*>/gi, '');");
   			javascript.Append(@"text = text.replace(/<(\S+) .[^<>]*>/g, '<$1>');");

			javascript.Append("tbContentTemp.innerHTML = text;");
			javascript.Append("focusCurrentContentholder();");
			javascript.Append("curAe.pasteHTML(tbContentTemp.innerHTML);");
			javascript.Append("tbContentTemp.innerHTML = '';");
			javascript.Append("return false;");

            if (!Global.ValidXhtml)
				javascript.Append("  } else {  tbContentTemp.innerHTML = ''; return true;}");
			javascript.Append("</script>");

			litJavascript.Text = javascript.ToString();
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
