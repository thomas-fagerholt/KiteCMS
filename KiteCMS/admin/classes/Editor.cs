using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Editor
	{
        private string id;
        private EditorType type = EditorType.Editor;
        private string height = "100%";
        private string width = "100%";
        private string label = "";
		private string value = "";
		private string defaultValue = "";
        private string cssStyle = "";
        private string cssClass = "";
        private string configFile = "";
        private bool noFormtag = false;
        private bool hidden = false;
        private bool hideInAdmin = true;
        private bool showLabelAfter = false;
        private string editorText = "";

		public Editor()
		{
		}

        public Editor(XmlNode node, ref XmlDocument xmldoc, int pageid, string HtmlToEdit,ref bool editorcodeWritten)
        {
            Page page = new Page(pageid);
            bool blnEdit = false;
            StringBuilder output = new StringBuilder();
            Hashtable attributes = new Hashtable();

            for (int counter = 0; counter < node.Attributes.Count; counter++)
            {
                if (attributes.ContainsKey(node.Attributes[counter].LocalName.ToLower()))
                    throw new Exception("Attribute '" + node.Attributes[counter].LocalName + "' already exists");
                else
                    attributes.Add(node.Attributes[counter].LocalName.ToLower(), node.Attributes[counter].Value);
            }

            if (attributes["id"] == null)
                throw new Exception("Editor is missing required attribute Id");
            id = attributes["id"].ToString().ToLower();

            if (attributes["width"] != null)
                width = attributes["width"].ToString();

            if (attributes["height"] != null)
                height = attributes["height"].ToString();

            if (attributes["cssclass"] != null)
                cssClass = attributes["cssclass"].ToString();

            if (attributes["cssstyle"] != null)
                cssStyle = attributes["cssstyle"].ToString();

            if (attributes["configfile"] != null)
                configFile = attributes["configfile"].ToString();

            if (attributes["label"] != null)
                label = attributes["label"].ToString();

            if (attributes["hideinadmin"] != null)
                hideInAdmin = bool.Parse(attributes["hideinadmin"].ToString());

            if (attributes["hidden"] != null)
                hidden = bool.Parse(attributes["hidden"].ToString());

            if (attributes["showlabelafter"] != null && attributes["showlabelafter"].ToString().ToLower() == "true")
                showLabelAfter = true; ;

            if (attributes["noformtag"] != null)
                noFormtag = bool.Parse(attributes["noformtag"].ToString());

            if (attributes["type"] != null)
            {
                switch (attributes["type"].ToString().ToLower())
                {
                    case "text":
                        type = EditorType.Text;
                        break;
                    case "textarea":
                        type = EditorType.TextArea;
                        break;
                    case "checkbox":
                        type = EditorType.Checkbox;
                        break;
                    case "image":
                        type = EditorType.Image;
                        break;
                    case "editor":
                        type = EditorType.Editor;
                        break;
                    default:
                        throw new Exception("Editor type unknown");
                }
            }

            if (type == EditorType.Editor)
            {
                output.Append(@"<script type=""text/javascript"">" + Environment.NewLine);
                if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
                {
                    if (HtmlToEdit == null)
                    {
                        if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                            output.Append(@"var stredit_"+ id +@"  = """ + FormatHtmlForEditor(page.ContentHolders[id], id, ref blnEdit) + @"""" + Environment.NewLine);
                        else
                            output.Append(@"var stredit_"+ id +@"  = """ + FormatHtmlForEditor(page.DraftContentHolders[id], id, ref blnEdit) + @"""" + Environment.NewLine);
                    }
                    else
                        output.Append(@"var stredit_"+ id +@"  = """ + FormatHtmlForEditor(HtmlToEdit, id, ref blnEdit) + @"""" + Environment.NewLine);

                }
                else
                {
                    if (HtmlToEdit == null)
                    {
                        if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                            output.Append(@"var stredit_"+ id +@" = """ + FormatHtmlForEditor(page.ContentHolders[id], id, ref blnEdit, cssStyle, cssClass) + @"""" + Environment.NewLine);
                        else
                            output.Append(@"var stredit_"+ id +@"  = """ + FormatHtmlForEditor(page.DraftContentHolders[id], id, ref blnEdit, cssStyle, cssClass) + @"""" + Environment.NewLine);
                    }
                    else
                        output.Append(@"var stredit_"+ id +@"  = """ + FormatHtmlForEditor(HtmlToEdit, id, ref blnEdit, cssStyle, cssClass) + @"""" + Environment.NewLine);
                }
                output.Append("blnEdit['tbContent_"+ id +"'] = "+ blnEdit.ToString().ToLower() + ";" + Environment.NewLine);
                output.Append("</script>" + Environment.NewLine);
                if (!editorcodeWritten)
                {
                    output.Append(writeEditorBlock(configFile));
                    editorcodeWritten = true;
                }

                //create xmlNode with editor
                if (label != "" && !showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", "tbContent_" + id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }

                if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
                {
                    XmlElement editorHiddenElement = xmldoc.CreateElement("div", "");
                    editorHiddenElement.SetAttribute("contenteditable", "true");
                    editorHiddenElement.SetAttribute("id", "tbContent_"+ id + "_hidden");
                    editorHiddenElement.SetAttribute("unselectable", "off");
                    editorHiddenElement.SetAttribute("style", "width:1px; height:1px; left:-1500px; position:absolute; top:0px");
                    editorHiddenElement.InnerText = "";

                    XmlElement editorOuterElement = xmldoc.CreateElement("div", "");
                    editorHiddenElement.SetAttribute("class", "tbContent");

                    XmlElement editorElement = xmldoc.CreateElement("div", "");
                    editorElement.SetAttribute("contenteditable", "true");
                    editorElement.SetAttribute("id", "tbContent_"+ id);
                    editorElement.SetAttribute("onfocus", "currentcontentholder='tbContent_" + id + "';");
                    editorElement.SetAttribute("unselectable", "off");
                    editorElement.SetAttribute("style", "width:" + width + "; height:" + height + ";" + cssStyle);
                    string classNames = "tbContent";
                    if (cssClass != "")
                        classNames += " " + cssClass;
                        editorElement.SetAttribute("class", classNames);
                    editorElement.InnerText = "";
                    editorOuterElement.AppendChild(editorElement);

                    node.ParentNode.InsertBefore(editorHiddenElement, node);
                    node.ParentNode.InsertBefore(editorOuterElement, node);
                }
                else
                {
                    XmlElement iframeElement = xmldoc.CreateElement("iframe", "");
                    iframeElement.SetAttribute("width", width);
                    iframeElement.SetAttribute("height", height);
                    iframeElement.SetAttribute("id", "tbContent_" + id);
                    iframeElement.SetAttribute("name", "iframe_" + id);
                    iframeElement.SetAttribute("src", "/admin/content/editor_ns.aspx?contentid="+ id);
                    iframeElement.InnerText = "";
                    if (cssClass != "")
                        iframeElement.SetAttribute("class", cssClass);
                    if (cssStyle != "")
                        iframeElement.SetAttribute("style", cssStyle);

                    node.ParentNode.InsertBefore(iframeElement, node);
                }

                if (label != "" && showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", "tbContent_" + id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }
                node.ParentNode.RemoveChild(node);
            }
            editorText = output.ToString();

            if (type == EditorType.Text)
            {
                if (label != "" && !showLabelAfter)
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

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                    editorElement.SetAttribute("value", page.ContentHolders[id]);
                else
                    editorElement.SetAttribute("value", page.DraftContentHolders[id]);

                node.ParentNode.InsertBefore(editorElement, node);

                if (label != "" && showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }
                
                node.ParentNode.RemoveChild(node);
            }

            if (type == EditorType.Checkbox)
            {
                if (label != "" && !showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }

                XmlElement editorElement = xmldoc.CreateElement("input", "");
                editorElement.SetAttribute("id", id);
                editorElement.SetAttribute("name", id);
                editorElement.SetAttribute("type", "checkbox");
                editorElement.SetAttribute("style", cssStyle);
                if (cssClass != "")
                    editorElement.SetAttribute("class", cssClass);

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                {
                    if (page.ContentHolders[id] != null && page.ContentHolders[id].ToLower() == "true")
                        editorElement.SetAttribute("checked", "checked");
                }
                else
                {
                    if (page.DraftContentHolders[id] != null && page.DraftContentHolders[id].ToLower() == "true")
                        editorElement.SetAttribute("checked", "checked");
                }

                node.ParentNode.InsertBefore(editorElement, node);

                if (label != "" && showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }
                node.ParentNode.RemoveChild(node);
            }

            if (type == EditorType.TextArea)
            {
                XmlText ny;
                if (label != "" && !showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }

                XmlElement editorElement = xmldoc.CreateElement("textarea", "");
                editorElement.SetAttribute("id", id);
                editorElement.SetAttribute("type", "text");
                editorElement.SetAttribute("style", "width:" + width + "; " + "height:" + height + "; " + cssStyle);
                if (cssClass != "")
                    editorElement.SetAttribute("class", cssClass);

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                    ny = xmldoc.CreateTextNode(page.ContentHolders[id]);
                else
                    ny = xmldoc.CreateTextNode(page.DraftContentHolders[id]);

                editorElement.AppendChild(ny);

                node.ParentNode.InsertBefore(editorElement, node);

                if (label != "" && showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }
                node.ParentNode.RemoveChild(node);
            }

            if (type == EditorType.Image)
            {
                XmlElement editorOuterElement = xmldoc.CreateElement("div", "");
                editorOuterElement.SetAttribute("style", "border:dashed 1px green; width:" + width + "; height:" + height + "; " + cssStyle);
                if (cssClass != "")
                    editorOuterElement.SetAttribute("class", cssClass);

                if (label != "" && !showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }

                XmlElement editorInnerElement = xmldoc.CreateElement("div", "");
                editorInnerElement.SetAttribute("style", "position:absolute;");
                editorInnerElement.SetAttribute("class", "KiteCMSImageEditor");

                XmlElement editorElement = xmldoc.CreateElement("input", "");
                editorElement.SetAttribute("id", id);
                editorElement.SetAttribute("type", "text");
                editorElement.SetAttribute("onblur", "document.getElementById('img_"+ id +"').src = this.value");
                editorElement.SetAttribute("style", "width:100px; " + cssStyle);

                XmlElement popupElement = xmldoc.CreateElement("a", "");
                popupElement.SetAttribute("onclick", "funcImagePopup('" + id + "');");
                popupElement.InnerXml = "<img src=\"/admin/editor/images/image.gif\" align=\"middle\" border=\"0\"/>";

                XmlElement imageElement = xmldoc.CreateElement("img", "");
                imageElement.SetAttribute("id", "img_" + id);
                imageElement.SetAttribute("style", (width != "100%" ? "width:" + width : "") + (height != "100%" ? "; height:" + height + ";" : ""));
                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                {
                    imageElement.SetAttribute("src", page.ContentHolders[id]);
                    editorElement.SetAttribute("value", page.ContentHolders[id]);
                }
                else
                {
                    imageElement.SetAttribute("src", page.DraftContentHolders[id]);
                    editorElement.SetAttribute("value", page.DraftContentHolders[id]);
                }

                editorInnerElement.AppendChild(editorElement);
                editorInnerElement.AppendChild(popupElement);
                editorOuterElement.AppendChild(editorInnerElement);
                editorOuterElement.AppendChild(imageElement);

                node.ParentNode.InsertBefore(editorOuterElement, node);

                if (label != "" && showLabelAfter)
                {
                    XmlElement LabelElement = xmldoc.CreateElement("label", "");
                    LabelElement.SetAttribute("for", id);
                    LabelElement.InnerText = label;
                    node.ParentNode.InsertBefore(LabelElement, node);
                }
                node.ParentNode.RemoveChild(node);
            }

        }

        private static string GenerateGeneralEditorHtml(int pageid, StringDictionary allEditorIDs, Hashtable allEditorTypes)
        {
            return GenerateGeneralEditorHtml(pageid, allEditorIDs, allEditorTypes, false);
        }

        private static string GenerateGeneralEditorHtml(int pageid, StringDictionary allEditorIDs, Hashtable allEditorTypes, bool noFormtag)
        {
            Page page = new Page(pageid);
            Template template = new Template(page.TemplateId);
            StringBuilder output = new StringBuilder();

            // create all the javascript
            output.Append(@"<div style=""visibility:hidden"" id=""divHidden"">" + Environment.NewLine);
            if (!noFormtag)
            {
                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                    output.Append(@"<form method=""post"" action=""/admin/content/editcontent.aspx"" onsubmit=""funcUpdateFields();"" name=""tempForm"">" + Environment.NewLine);
                else
                    output.Append(@"<form method=""post"" action=""/admin/draft/editdraft.aspx"" onsubmit=""funcUpdateFields();"" name=""tempForm"">" + Environment.NewLine);
            }
            output.Append(@"<input type=""hidden"" name=""action"" value=""save""/>");
            output.Append(@"<input type=""hidden"" name=""pageid"" value=""" + pageid + @"""/>");
            foreach (string key in allEditorIDs.Keys)
            {
                if ((EditorType)allEditorTypes[key] != EditorType.TextArea  && (EditorType)allEditorTypes[key] != EditorType.Editor)
                    output.Append(@"<input type=""hidden"" name=""" + key + @""" id=""" + key + @""" value=""""/>");
                else if ((EditorType)allEditorTypes[key] == EditorType.TextArea )
                    output.Append(@"<textarea name=""" + key + @""" id=""" + key + @""" cols=""1"" rows=""1""></textarea>");
                else
                {
                    output.Append(@"<textarea id=""tbContent_" + key + @"_temp"" name=""tbContent_" + key + @"_temp"" cols=""1"" rows=""1""></textarea>");
                }
            }
            output.Append("</form>");
            output.Append("</div>");

            // Defining global variables
            output.Append("<script type='text/javascript'>" + Environment.NewLine);
            output.Append("var currentcontentholder = '';" + Environment.NewLine);
            output.Append(@"var chooseText = """ + Functions.localText("chooseText") + @"""" + Environment.NewLine);
            output.Append(@"var hasjavascriptText = """ + Functions.localText("hasjavascript") + @"""" + Environment.NewLine);
            output.Append(@"var notintextmodeText = """ + Functions.localText("notintextmode") + @"""" + Environment.NewLine);
            output.Append(@"var choosecontentholderText = """ + Functions.localText("choosecontentholder") + @"""" + Environment.NewLine);
            output.Append("var allEditors = [");
            foreach (string key in allEditorIDs.Keys)
                if ((EditorType)allEditorTypes[key] == EditorType.Editor)
                    output.Append("'" + key + "',");
            output.Append("''];" + Environment.NewLine);
            output.Append("var blnBorder;" + Environment.NewLine);
            output.Append("var strMode = new Array();" + Environment.NewLine);
            output.Append("var arrAnchors = new Array;" + Environment.NewLine);

            // defining startFunction
            output.Append("function startFunction(){" + Environment.NewLine);

            foreach (string key in allEditorIDs.Keys)
            {
                if ((EditorType)allEditorTypes[key] == EditorType.Editor)
                {
                    if (HttpContext.Current.Session["IEBrowser"].ToString() != "True")
                    {
                        output.Append("document.getElementById('tbContent_" + key + "').contentWindow.document.designMode = \"on\";" + Environment.NewLine);
                        output.Append("document.getElementById('tbContent_" + key + "').contentWindow.document.execCommand(\"useCSS\", false, false);" + Environment.NewLine);
                        //attach onfocus to iframe
                        output.Append("window.frames.iframe_" + key + ".document.addEventListener('click', FireFoxFocus, false);");
                    }
                    output.Append("blnBorder=0;" + Environment.NewLine);

                    if (isBlnEdit(page.ContentHolders[key]))
                    {
                        output.Append("strMode['tbContent_" + key + "'] = 'html';" + Environment.NewLine);
                        if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
                            output.Append("document.getElementById('tbContent_" + key + "').innerHTML = funcUnescapeUnicode(unescape(stredit_" + key + "));" + Environment.NewLine);
                        else
                            output.Append("document.getElementById('tbContent_" + key + "').contentWindow.document.body.innerHTML = funcUnescapeUnicode(unescape(stredit_" + key + "));" + Environment.NewLine);
                    }
                    else
                    {
                        output.Append("strMode['tbContent_" + key + "'] = 'text';" + Environment.NewLine);
                        if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
                            output.Append("document.getElementById('tbContent_" + key + "').innerText = unescape(stredit_" + key + ");" + Environment.NewLine);
                        else
                            output.Append("var html = document.createTextNode(unescape(stredit_" + key + "));	document.getElementById('tbContent_" + key + "').contentWindow.document.body.innerHTML = \"\";document.getElementById('tbContent_" + key + "').contentWindow.document.body.appendChild(html);" + Environment.NewLine);
                    }
                }
            }

            output.Append("document.getElementById('KiteCMSToolbar').style.visibility='visible';" + Environment.NewLine);
            output.Append("}" + Environment.NewLine);
            output.Append("var nowOnload = window.onload;"+ Environment.NewLine);
            output.Append(" window.onload = function() {startFunction(); if(nowOnload != null && typeof(nowOnload) == 'function') { nowOnload();}}" + Environment.NewLine);

            output.Append(@"function MENU_FILE_NOTSAVE_onclick() {" + Environment.NewLine);
            output.Append(@"if(confirm(""" + Functions.localText("leaveWithoutSaving") + @"""))" + Environment.NewLine);
            output.Append(@"document.location.href=('" + template.Publicurl + "?pageid=" + pageid + "');" + Environment.NewLine);
            output.Append("}" + Environment.NewLine);

            output.Append(@"function MENU_FILE_LEAVE_onclick() {" + Environment.NewLine);
            output.Append(@"document.location.href=('" + template.Publicurl + "?pageid=" + pageid + "');" + Environment.NewLine);
            output.Append("}" + Environment.NewLine);

            output.Append(@"function funcUpdateFields() {" + Environment.NewLine);
            foreach (string key in allEditorIDs.Keys)
            {
                if ((EditorType)allEditorTypes[key] == EditorType.Checkbox)
                    output.Append(@"document.tempForm." + key + ".value=document.getElementById('" + key + "').checked;" + Environment.NewLine);
                else if ((EditorType)allEditorTypes[key] != EditorType.Editor)
                    output.Append(@"document.tempForm." + key + ".value=document.getElementById('" + key + "').value;" + Environment.NewLine);
            }
            output.Append("document.tempForm.submit();" + Environment.NewLine);
            output.Append("}");

            output.Append(@"</script>" + Environment.NewLine);

            // onpaste script for each editor
            foreach (string key in allEditorIDs.Keys)
            {
                if ((EditorType)allEditorTypes[key] == EditorType.Editor)
                {
                    output.Append("<script for='tbContent_" + key + "' event='onpaste'>" + Environment.NewLine);
                    output.Append("<!-- //" + Environment.NewLine);
                    output.Append(" var curAe = document.selection.createRange();" + Environment.NewLine);
                    output.Append(" tbContent_" + key + "_hidden.innerHTML = '';" + Environment.NewLine);
                    output.Append(" tbContent_" + key + "_hidden.focus();" + Environment.NewLine);
                    output.Append(" document.execCommand('paste');" + Environment.NewLine);
                    output.Append(" var text = tbContent_" + key + "_hidden.innerHTML;" + Environment.NewLine);
                    output.Append(" if(text.search(/class=mso/gi) != -1 || text.search(/mso-/gi) != -1)" + Environment.NewLine);
                    if (Global.ValidXhtml)
                        output.Append("alert('" + Functions.localText("alertpasteword1") + "');" + Environment.NewLine);
                    else
                        output.Append("if(confirm('" + Functions.localText("alertpasteword1") + Functions.localText("alertpasteword2") + "')){" + Environment.NewLine);

                    output.Append(@"text = text.replace(/<(?!BR\b|\/?P\b|\/?ol\b|\/?h[1-6]\b|\/?ul\b|\/?li\b).[^<>]*>/gi, '');" + Environment.NewLine);
                    output.Append(@"text = text.replace(/<(\S+) .[^<>]*>/g, '<$1>');" + Environment.NewLine);

                    output.Append("tbContent_" + key + "_hidden.innerHTML = text;" + Environment.NewLine);
                    output.Append("tbContent_" + key + ".focus();" + Environment.NewLine);
                    output.Append("curAe.pasteHTML(tbContent_" + key + "_hidden.innerHTML);" + Environment.NewLine);
                    output.Append("tbContent_" + key + "_hidden.innerHTML = '';" + Environment.NewLine);
                    output.Append("return false;" + Environment.NewLine);

                    if (!Global.ValidXhtml)
                        output.Append("  } else {  tbContent_" + key + "_hidden.innerHTML = ''; return true;}" + Environment.NewLine);
                    output.Append("//-->" + Environment.NewLine);
                    output.Append("</script>" + Environment.NewLine);
                }
            }
            return output.ToString();
        }

        private string FormatHtmlForEditor(string input, string editorID, ref bool blnEdit)
        {
            return FormatHtmlForEditor(input, editorID, ref blnEdit, "", "");
        }

        private string FormatHtmlForEditor(string input, string editorID, ref bool blnEdit,string cssStyle, string cssClass)
        {
            string pageHtml = "";
            if (input != null)
                pageHtml = input.Replace("<html>", "");

            // Change Html to valid code using SgmlReader
            StringWriter log = new StringWriter();
            try
            {
                bool na = true;
                pageHtml = Functions.CleanEditorCode(pageHtml, log, true, false, false, ref na);
            }
            catch { }

            //HTML-mode is not possible if the page contains script
            blnEdit = isBlnEdit(pageHtml);

            if (blnEdit)
                pageHtml = @"<html><head><head><body class=""" + cssClass + @""" name=""" + editorID  + @""" style=""" + cssStyle + @""">" + pageHtml + "</body></html>";
            
            return Functions.string2hex(pageHtml).Replace("%00", "");

        }

        private static string writeEditorBlock()
        {
            return writeEditorBlock("/admin/data/fulleditor.xml");
        }

        private static string writeEditorBlock(string pathForXmlconfigFile)
        {
            XmlDocument configfile = new XmlDocument();
            if (pathForXmlconfigFile == "")
                pathForXmlconfigFile = "/admin/data/fulleditor.xml";

            string filename = HttpContext.Current.Server.MapPath(pathForXmlconfigFile);
            try
            {
                configfile.Load(filename);
            }
            catch
            {
                throw new Exception("Invalid path for editor configfile");
            }

            StringBuilder output = new StringBuilder();
            output.Append(@"<div id='KiteCMSToolbar'>" + Environment.NewLine);
            output.Append(@"<table class=""tbToolbar"" id=""tbToolbar"" cellspacing=""0"" cellpadding=""0"" border=""0"">"+ Environment.NewLine);
            output.Append(@"<tr>");
            if (EnabledButton(configfile, "leave"))
            {
                output.Append("<td>" + Environment.NewLine);
                output.Append(@"<div id=""divLeave"" class=""KiteCMSButton"" onclick=""return MENU_FILE_LEAVE_onclick();"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/leave.gif"" alt=""" + KiteCMS.Functions.localText("leave") + @""" title=""" + KiteCMS.Functions.localText("leave") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "save"))
            {
                output.Append("<td>" + Environment.NewLine);
                output.Append(@"<div id=""divSave"" class=""KiteCMSButton"" onclick=""return MENU_FILE_SAVE_onclick();"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/save.gif"" alt=""" + KiteCMS.Functions.localText("save") + @""" title=""" + KiteCMS.Functions.localText("save") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "cancel"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div id=""divCancel"" class=""KiteCMSButton"" onclick=""return MENU_FILE_NOTSAVE_onclick();"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/nosave.gif"" alt=""" + KiteCMS.Functions.localText("cancel") + @""" title=""" + KiteCMS.Functions.localText("cancel") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "bold"))
            {
                output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcBold()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/bold.gif"" alt=""" + KiteCMS.Functions.localText("bold") + @""" title=""" + KiteCMS.Functions.localText("bold") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "italic"))
            {

                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcItalic()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/Italic.gif"" alt=""" + KiteCMS.Functions.localText("italic") + @""" title=""" + KiteCMS.Functions.localText("italic") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "underline"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcUnderline()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/under.gif"" alt=""" + KiteCMS.Functions.localText("underline") + @""" title=""" + KiteCMS.Functions.localText("underline") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
            {
                if (EnabledButton(configfile, "copy"))
                {
                    output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
                    output.Append(@"<td>" + Environment.NewLine);
                    output.Append(@"<div class=""KiteCMSButton"" onclick=""document.execCommand('copy',false,null);"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                    output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/copy.gif"" alt=""" + KiteCMS.Functions.localText("copy") + @""" title=""" + KiteCMS.Functions.localText("copy") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                    output.Append(@"</div>" + Environment.NewLine);
                    output.Append(@"</td>" + Environment.NewLine);
                }
                if (EnabledButton(configfile, "cut"))
                {

                    output.Append(@"<td>" + Environment.NewLine);
                    output.Append(@"<div class=""KiteCMSButton"" onclick=""document.execCommand('cut',false,null);"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                    output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/cut.gif"" alt=""" + KiteCMS.Functions.localText("cut") + @""" title=""" + KiteCMS.Functions.localText("cut") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                    output.Append(@"</div>" + Environment.NewLine);
                    output.Append(@"</td>" + Environment.NewLine);
                }
                if (EnabledButton(configfile, "paste"))
                {
                    output.Append(@"<td>" + Environment.NewLine);
                    output.Append(@"<div class=""KiteCMSButton"" onclick=""focusCurrentContentholder();document.execCommand('paste',false,null);"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                    output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/paste.gif"" alt=""" + KiteCMS.Functions.localText("paste") + @""" title=""" + KiteCMS.Functions.localText("paste") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                    output.Append(@"</div>" + Environment.NewLine);
                    output.Append(@"</td>" + Environment.NewLine);
                }
            }
            if (EnabledButton(configfile, "pasteword"))
                {

                    output.Append(@"<td>" + Environment.NewLine);
                    output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {focusCurrentContentholder();funcPasteWord()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                    output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/pasteWord.gif"" alt=""" + KiteCMS.Functions.localText("paste_word") + @""" title=""" + KiteCMS.Functions.localText("paste_word") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                    output.Append(@"</div>" + Environment.NewLine);
                    output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "left"))
            {

                output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcJustifyLeft()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/left.gif"" alt=""" + KiteCMS.Functions.localText("align_left") + @""" title=""" + KiteCMS.Functions.localText("align_left") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "center"))
            {

                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcJustifyCenter()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/center.gif"" alt=""" + KiteCMS.Functions.localText("align_center") + @""" title=""" + KiteCMS.Functions.localText("align_center") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "right"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcJustifyRight()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/right.gif"" alt=""" + KiteCMS.Functions.localText("align_right") + @""" title=""" + KiteCMS.Functions.localText("align_right") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
                output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "link"))
            {

                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcLink()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/link.GIF"" alt=""" + KiteCMS.Functions.localText("insert_link") + @""" title=""" + KiteCMS.Functions.localText("insert_link") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "anchor"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcAnchor()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/anchor.GIF"" alt=""" + KiteCMS.Functions.localText("insert_anchor") + @""" title=""" + KiteCMS.Functions.localText("insert_anchor") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "media"))
            {

                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcMedia()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/media.GIF"" alt=""" + KiteCMS.Functions.localText("insert_media") + @""" title=""" + KiteCMS.Functions.localText("insert_media") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "unlink"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcUnlink()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/unlink.GIF"" alt=""" + KiteCMS.Functions.localText("remove_link") + @""" title=""" + KiteCMS.Functions.localText("remove_link") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
            if (EnabledButton(configfile, "abbr"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcAbbr()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/abbr.GIF"" alt=""" + KiteCMS.Functions.localText("insert_abbr") + @""" title=""" + KiteCMS.Functions.localText("insert_abbr") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "ul"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcUnorderedlist()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/bullist.GIF"" alt=""" + KiteCMS.Functions.localText("bullist") + @""" title=""" + KiteCMS.Functions.localText("bullist") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "color"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcColor()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/fgcolor.GIF"" alt=""" + KiteCMS.Functions.localText("font_color") + @""" title=""" + KiteCMS.Functions.localText("font_color") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "image"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcImage()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/image.GIF"" alt=""" + KiteCMS.Functions.localText("insert_picture") + @""" title=""" + KiteCMS.Functions.localText("insert_picture") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "style"))
            {
                output.Append(@"<td class=""KiteCMSButton"">" + Environment.NewLine);
                output.Append(@"<select id=""selectClass"" onchange=""if(CheckForEditMode()) {funcSetStyle(this[this.selectedIndex].value);this.selectedIndex=0};"" class=""alminput"">" + Environment.NewLine);
                output.Append(@"<option selected=""selected"">" + KiteCMS.Functions.localText("text_class") + @"</option>" + Environment.NewLine);
                output.Append(@"<option value=""P"">" + KiteCMS.Functions.localText("normal") + @"</option>" + Environment.NewLine);
                if (configfile.SelectSingleNode("/website/editor/button[@id='style' and @enabled='true']/removeclass[@name='h1' or @name='H1']") == null)
                    output.Append(@"<option value=""H1"">" + KiteCMS.Functions.localText("heading") + @" 1</option>" + Environment.NewLine);
                if (configfile.SelectSingleNode("/website/editor/button[@id='style' and @enabled='true']/removeclass[@name='h2' or @name='H2']") == null)
                    output.Append(@"<option value=""H2"">" + KiteCMS.Functions.localText("heading") + @" 2</option>" + Environment.NewLine);
                if (configfile.SelectSingleNode("/website/editor/button[@id='style' and @enabled='true']/removeclass[@name='h3' or @name='H3']") == null)
                    output.Append(@"<option value=""H3"">" + KiteCMS.Functions.localText("heading") + @" 3</option>" + Environment.NewLine);
                output.Append(ExtraStyles(configfile));
                output.Append(@"</select>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "table"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {InsertTable()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/instable.GIF"" alt=""" + KiteCMS.Functions.localText("insert_table") + @""" title=""" + KiteCMS.Functions.localText("insert_table") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (HttpContext.Current.Session["IEBrowser"].ToString() == "True")
            {
                if (EnabledButton(configfile, "tablerow"))
                {
                    output.Append(@"<td>" + Environment.NewLine);
                    output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {InsertTableRow()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                    output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/insrow.GIF"" alt=""" + KiteCMS.Functions.localText("insert_tablerow") + @""" title=""" + KiteCMS.Functions.localText("insert_tablerow") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                    output.Append(@"</div>" + Environment.NewLine);
                    output.Append(@"</td>" + Environment.NewLine);
                }
            }
            if (EnabledButton(configfile, "toggleborder"))
            {
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div class=""KiteCMSButton"" onclick=""if(CheckForEditMode()) {funcToggleBorder()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/borders.GIF"" alt=""" + KiteCMS.Functions.localText("visible_border") + @""" title=""" + KiteCMS.Functions.localText("visible_border") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }
            if (EnabledButton(configfile, "togglemode"))
            {
                output.Append(@"<td width=""1"" class=""KiteCMSSeparator""><img src=""/admin/images/prop_divider.gif"" width=""1"" height=""28"" alt="""" border=""0""/></td>" + Environment.NewLine);
                output.Append(@"<td>" + Environment.NewLine);
                output.Append(@"<div id=""divText"" class=""KiteCMSButton"" onclick=""if(CheckForEditCurrentcontentholder()) {funcToggleMode()};"" onmouseover=""button_over(this);"" onmouseout=""button_out(this);"">" + Environment.NewLine);
                output.Append(@"<img class=""KiteCMSIcon"" src=""/admin/editor/images/details.GIF"" alt=""" + KiteCMS.Functions.localText("text_html") + @""" title=""" + KiteCMS.Functions.localText("text_html") + @""" width=""23"" height=""22""/>" + Environment.NewLine);
                output.Append(@"</div>" + Environment.NewLine);
                output.Append(@"</td>" + Environment.NewLine);
            }

            output.Append(@"</tr>" + Environment.NewLine);
            output.Append(@"</table>" + Environment.NewLine);
            output.Append(@"</div>" + Environment.NewLine);
            return output.ToString();
        }


        private static bool EnabledButton(XmlDocument xmldoc, string button)
        {
            XmlNode node;
            node = xmldoc.SelectSingleNode("/website/editor/button[@id='" + button + "' and @enabled='true']");
            if (node != null)
                return true;
            else
                return false;
        }

        private static bool isBlnEdit(string pageHtml)
        {
            bool blnEdit = false;
            if (pageHtml == null || (pageHtml.ToLower().IndexOf("<script") == -1 && pageHtml.ToLower().IndexOf("<form") == -1 && pageHtml.ToLower().IndexOf("<object") == -1))
                blnEdit = true;

            return blnEdit;

        }

        private static string ExtraStyles(XmlDocument xmldoc)
        {
            XmlNodeList nodes;
            string output = "";
            nodes = xmldoc.SelectNodes("/website/editor/button[@id='style' and @enabled='true']/extraclass");
            for (int counter = 0; counter < nodes.Count; counter++)
            {
                try
                {
                    output += "<option value='" + nodes.Item(counter).Attributes["value"].Value + "'>" + nodes.Item(counter).Attributes["name"].Value + "</option>";
                }
                catch { }
            }
            return output;
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

        public string Height
        {
            get { return height; }
            set { this.height = value; }
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

        public string ConfigFile
        {
            get { return configFile; }
            set { this.configFile = value; }
        }

        public string CssStyle
        {
            get { return cssStyle; }
            set { this.cssStyle = value; }
        }

        public EditorType Type
        {
            get { return type; }
            set { this.type = value; }
        }

        public enum EditorType : int
        {
            Editor = 0,
            Text = 1,
            TextArea = 2,
            Image = 3,
            Checkbox = 4
        }
        public static string InsertContentHolder(ref XmlDocument xmldoc, int pageid)
        {
            return InsertContentHolder(ref xmldoc, pageid, null);
        }

        public static string InsertContentHolder(ref XmlDocument xmldoc, int pageid, string HtmlToEdit)
        {
            return InsertContentHolder(ref xmldoc, pageid, null, false);
        }

        public static string InsertContentHolder(ref XmlDocument xmldoc, int pageid, string HtmlToEdit, bool noFormtag)
        {
            StringBuilder output = new StringBuilder();
            StringDictionary allEditorIDs = new StringDictionary();
            Hashtable allEditorTypes = new Hashtable();
            Page page = new Page(pageid);
            bool canSeeDraft = false;
            XmlNodeList list;
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode != Global.EditModeEnum.Public)
            {
                Admin admin = new Admin();

                // Check for permissions to this module in adminmode
                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit)
                {
                    admin.userHasAccess(1201, pageid);
                }
                else if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                {
                    admin.userHasAccess(1302, pageid);
                }
                canSeeDraft = admin.userHasAccess(1303, pageid, false);
            }

            System.Xml.XmlNode docElem = xmldoc.DocumentElement;
            string namespaceUri = docElem.Attributes["xmlns:xsl"].Value;

            // Insert editors
            output.Append("<script type=\"text/javascript\">" + Environment.NewLine + "var blnEdit = new Array();" + Environment.NewLine + "</script>" + Environment.NewLine);
            list = xmldoc.SelectNodes("//*['editor'=translate(local-name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')]");

            // Default namespace is defined. Do some trick to fix : http://blogs.msdn.com/john_pollard/archive/2005/11/12/using-selectsinglenode-or-selectnodes-on-xml-where-the-default-namespace-has-been-set.aspx
            if (list.Count == 0)
            {
                XmlNamespaceManager namespaceManager = new XmlNamespaceManager(xmldoc.NameTable);
                namespaceManager.AddNamespace("KiteCMS", "http://www.w3.org/1999/xhtml");
                list = xmldoc.SelectNodes("//*['editor'=translate(local-name(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')]", namespaceManager);
            }
            if (list.Count == 0)
            {
                output.Append(writeEditorBlock("/admin/data/leave.xml"));
            }

            bool editorcodeWritten = false;
            for (int counter = 0; counter < list.Count; counter++)
            {
                XmlNode ny;
                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit || ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                {
                    allEditorIDs.Add(list.Item(counter).Attributes["id"].Value, "");
                    Editor editor = new Editor(list.Item(counter), ref xmldoc, pageid, HtmlToEdit, ref editorcodeWritten);
                    allEditorTypes.Add(list.Item(counter).Attributes["id"].Value.ToLower(), editor.Type);
                    output.Append(editor.EditorText);
                }
                else
                {
                    if ((list.Item(counter).Attributes.GetNamedItem("hidden", "") == null || list.Item(counter).Attributes.GetNamedItem("hidden", "").Value.ToLower() != "true") || (list.Item(counter).Attributes.GetNamedItem("hideinadmin", "") != null && list.Item(counter).Attributes.GetNamedItem("hideinadmin", "").Value.ToLower() == "false" && ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Admin))
                    {
                        if (HtmlToEdit == null)
                        {
                            if (list.Item(counter).Attributes.GetNamedItem("type", "") != null && list.Item(counter).Attributes.GetNamedItem("type", "").InnerText == "image")
                            {
                                ny = xmldoc.CreateNode("element", "xsl:if", namespaceUri);
                                XmlAttribute ifAttr = ((XmlElement)ny).SetAttributeNode("test", "");

                                XmlNode image = xmldoc.CreateNode("element", "xsl:element", namespaceUri);

                                XmlAttribute Attr1 = ((XmlElement)image).SetAttributeNode("name", "");
                                Attr1.Value = "img";

                                XmlNode attrNode1 = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                XmlAttribute Attr2 = ((XmlElement)attrNode1).SetAttributeNode("name", "");
                                Attr2.Value = "src";
                                XmlNode attrNode2 = xmldoc.CreateNode("element", "xsl:value-of", namespaceUri);
                                XmlAttribute Attr3 = ((XmlElement)attrNode2).SetAttributeNode("select", "");

                                if (HttpContext.Current.Request.QueryString["showdraft"] != null && canSeeDraft)
                                {
                                    Attr3.Value = "draft/contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower();
                                    ifAttr.Value = "draft/contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower() + "!=''";
                                }
                                else
                                {
                                    Attr3.Value = "contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower();
                                    ifAttr.Value = "contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower() + "!=''";
                                }
                                attrNode1.AppendChild(attrNode2);
                                image.AppendChild(attrNode1);

                                if (list.Item(counter).Attributes.GetNamedItem("cssclass", "") != null)
                                {
                                    XmlNode attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    Attr1 = ((XmlElement)attrNode).SetAttributeNode("name", "");
                                    Attr1.Value = "class";
                                    attrNode.InnerText = list.Item(counter).Attributes.GetNamedItem("cssclass", "").Value;
                                    image.AppendChild(attrNode);
                                }

                                if (list.Item(counter).Attributes.GetNamedItem("cssstyle", "") != null)
                                {
                                    XmlNode attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    Attr1 = ((XmlElement)attrNode).SetAttributeNode("name", "");
                                    Attr1.Value = "style";
                                    attrNode.InnerText = list.Item(counter).Attributes.GetNamedItem("cssstyle", "").Value;
                                    image.AppendChild(attrNode);
                                }

                                if (list.Item(counter).Attributes.GetNamedItem("width", "") != null)
                                {
                                    XmlNode attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    XmlAttribute attr = ((XmlElement)attrNode).SetAttributeNode("name", "");
                                    attr.Value = "width";
                                    attrNode.InnerText = list.Item(counter).Attributes.GetNamedItem("width", "").InnerText;
                                    image.AppendChild(attrNode);
                                }

                                if (list.Item(counter).Attributes.GetNamedItem("height", "") != null)
                                {
                                    XmlNode attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    XmlAttribute attr = ((XmlElement)attrNode).SetAttributeNode("name", "");
                                    attr.Value = "height";
                                    attrNode.InnerText = list.Item(counter).Attributes.GetNamedItem("height", "").InnerText;
                                    image.AppendChild(attrNode);
                                }

                                if (list.Item(counter).Attributes.GetNamedItem("alt", "") != null)
                                {
                                    XmlNode attrNode = xmldoc.CreateNode("element", "xsl:attribute", namespaceUri);
                                    XmlAttribute attr = ((XmlElement)attrNode).SetAttributeNode("name", "");
                                    attr.Value = "alt";
                                    attrNode.InnerText = list.Item(counter).Attributes.GetNamedItem("alt", "").InnerText;
                                    image.AppendChild(attrNode);
                                }
                                ny.AppendChild(image);
                            }
                            else
                            {
                                ny = xmldoc.CreateNode("element", "xsl:value-of", namespaceUri);
                                XmlAttribute Attr1 = ((XmlElement)ny).SetAttributeNode("select", "");
                                if (HttpContext.Current.Request.QueryString["showdraft"] != null && canSeeDraft)
                                    Attr1.Value = "draft/contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower();
                                else
                                    Attr1.Value = "contentholders/" + list.Item(counter).Attributes.GetNamedItem("id", "").Value.ToLower();
                                XmlAttribute Attr2 = ((XmlElement)ny).SetAttributeNode("disable-output-escaping", "");
                                Attr2.Value = "yes";
                            }
                        }
                        else
                            ny = xmldoc.CreateTextNode(HtmlToEdit);

                        list.Item(counter).ParentNode.InsertBefore(ny, list.Item(counter));
                    }
                    list.Item(counter).ParentNode.RemoveChild(list.Item(counter));
                }
            }
            // include editorcode if not generated allready
            if (!editorcodeWritten)
                output.Append(writeEditorBlock());

            output.Append(GenerateGeneralEditorHtml(pageid, allEditorIDs, allEditorTypes, noFormtag));
            return output.ToString();
        }

        public static string InsertStaticAdminHtml(string htmlstring, string adminHtml, int pageid)
        {
            if (htmlstring.ToLower().Contains("<body") && htmlstring.ToLower().Contains("</body>") && htmlstring.ToLower().Contains("</head>"))
            {
                Admin admin = new Admin();
                Page page = new Page(pageid);
                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Admin)
                    htmlstring = Functions.ReplaceString(admin.AdminPageHtml + admin.moduleMenu(page) + "</body>", htmlstring, "</body>", true);
                else
                    htmlstring = Functions.ReplaceString(adminHtml + "</body>", htmlstring, "</body>", true);

                htmlstring = Functions.ReplaceString(@"<link rel=""stylesheet"" href=""/admin/default.css"" type=""text/css""/>" + Environment.NewLine + "</head>", htmlstring, "</head>", true);

                // add editmode-specific class to body
                if (Regex.IsMatch(htmlstring, @"(<body[^>]*class=['""][^'""]*)(['""][^>]*>)", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    htmlstring = Regex.Replace(htmlstring, @"(<body[^>]*class=['""][^'""]*)(['""][^>]*>)", "$1 EditMode" + ((Global)HttpContext.Current.ApplicationInstance).EditMode.ToString().Replace("Draft", "") + "$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                else
                    htmlstring = Regex.Replace(htmlstring, @"(<body[^>]*)(>)", "$1 class='EditMode" + ((Global)HttpContext.Current.ApplicationInstance).EditMode.ToString() + "'$2", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit || ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                    htmlstring = Functions.ReplaceString(@"<script type=""text/javascript"" src='/admin/editor/includes/functions.js'></script>" + Environment.NewLine + "</head>", htmlstring, "</head>", true);

                return htmlstring;
            }
            else
                throw new Exception("html must contain <body, </body> and </head>");
        }
	}
}
