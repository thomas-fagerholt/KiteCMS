using System;
using System.Collections;
using System.IO;
using System.Web.SessionState;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class TextField
	{
        private string id;
        private string width = "200px";
        private string label = "";
		private string value = "";
		private string defaultValue = "";
        private string cssStyle = "";
        private string cssClass = "";
        private bool noFormtag = false;
        private bool hidden = false;
        private bool hideInAdmin = true;
        private string editorText = "";

		public TextField()
		{
		}

        public TextField(XmlNode node, ref XmlDocument xmldoc, int pageid, string HtmlToEdit)
		{
            Page page = new Page(pageid);
            Template template = new Template(page.TemplateId);
            StringBuilder output = new StringBuilder();
            Hashtable attributes = new Hashtable();

            for (int counter = 0; counter < node.Attributes.Count; counter++)
            {
                if(attributes.ContainsKey(node.Attributes[counter].LocalName.ToLower()))
                    throw new Exception("Attribute '"+ node.Attributes[counter].LocalName +"' already exists");
                else
                    attributes.Add(node.Attributes[counter].LocalName.ToLower(),node.Attributes[counter].Value);
            }

            if (attributes["id"] == null)
                throw new Exception("Editor is missing required attribute Id");
            id = attributes["id"].ToString();

            if (attributes["width"] != null)
                width = attributes["width"].ToString();

            if (attributes["cssclass"] != null)
                cssClass = attributes["cssclass"].ToString();

            if (attributes["cssstyle"] != null)
                cssStyle = attributes["cssstyle"].ToString();

            if (attributes["label"] != null)
                label = attributes["label"].ToString();

            if (attributes["hideinadmin"] != null)
                hideInAdmin = bool.Parse(attributes["hideinadmin"].ToString());

            if (attributes["hidden"] != null)
                hidden = bool.Parse(attributes["hidden"].ToString());

            if (attributes["noformtag"] != null)
                noFormtag = bool.Parse(attributes["noformtag"].ToString());


            XmlElement editorOuterElement = xmldoc.CreateElement("div", "");

            if (label != "")
            {   
                XmlElement LabelElement = xmldoc.CreateElement("label", "");
                LabelElement.SetAttribute("for", id);
                LabelElement.InnerText = label;
                node.ParentNode.InsertBefore(LabelElement, node);
            }

            XmlElement editorElement = xmldoc.CreateElement("input", "");
            editorElement.SetAttribute("id", id);
            editorElement.SetAttribute("type", "text");
            editorElement.SetAttribute("style", "width:" + width + "; " + cssStyle);
            if (cssClass != "")
                editorElement.SetAttribute("class", cssClass);
            editorElement.SetAttribute("value", HtmlToEdit);

            node.ParentNode.InsertBefore(editorElement, node);
            node.ParentNode.RemoveChild(node);
		}


        public XmlNode XmlForSave()
		{
            return null;
		}

		public string Id
		{
			get {return id;}
			set {this.id = value;}
		}

		public string Label
		{
			get {return label;}
			set {this.label = value;}
		}

        public string Width
        {
            get { return width; }
            set { this.width = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { this.defaultValue = value; }
        }

        public bool NoFormtag
        {
            get { return noFormtag; }
            set { this.noFormtag = value; }
        }

        public bool Hidden
        {
            get { return hidden; }
            set { this.hidden = value; }
        }

        public bool HideInAdmin
        {
            get { return hideInAdmin; }
            set { this.hideInAdmin = value; }
        }

        public string EditorText
        {
            get { return editorText; }
        }

        public string CssClass
        {
            get { return cssClass; }
            set { this.cssClass = value; }
        }

        public string CssStyle
        {
            get { return cssStyle; }
            set { this.cssStyle = value; }
        }
	}
}
