using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace KiteCMS.Admin
{
    /// <summary>
    /// This file is used to update the data files of the system to current version
    /// It shall allways be cumulative, upgrading from any version to current
    /// It shall also be defensive, so nothing is broken if run multiple times
    /// </summary>
    public class updatexml : System.Web.UI.Page
	{
		protected Literal lbContent;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
            if (Global.isDemo)
                throw new Exception("UpdateXML cannot run in demo-mode");

            XmlDocument xmldoc = new XmlDocument();
			XmlNode xmlnode;
            System.Xml.XmlNodeList list;
			XmlElement xmlelem;
			core.Website website = new core.Website();


            // rename files
            bool firstXmlFile = true;
            foreach (string file in Directory.GetFiles(Global.publicXmlPath + "/"))
            {
                if (Path.GetExtension(file).ToLower() == ".webinfo")
                {
                    if (firstXmlFile)
                    {
                        XmlDocument websiteXml = new XmlDocument();
                        websiteXml.Load(Global.publicXmlPath + "/website.webinfo");
                        Global.oMenuXml = websiteXml;
                        if (websiteXml.InnerXml.IndexOf(".webinfo]]></xmluri>") > -1)
                            lbContent.Text += "Please update local xmluri's for boxes to new extension .xml<br/>";
                    }
                    File.Move(file, file.Replace(".webinfo", ".xml"));
                    firstXmlFile = false;
                }

                if (Path.GetFileName(file).ToLower() == "email.xml" && !File.Exists(Global.publicXmlPath + "newsletter.xml"))
                {
                    File.Move(file, file.Replace("email.xml", "newsletter.xml"));
                }

                if (Path.GetExtension(file).ToLower() == ".xsl")
                {
                    bool changed = false;
                    XmlDocument xslDoc = new XmlDocument();
                    xslDoc.Load(file);
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xslDoc.NameTable);
                    nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                    foreach (XmlNode node in xslDoc.SelectNodes("//xsl:comment", nsmgr))
                        if (node.InnerText == "indhold")
                        {
                            node.InnerText = "modulecontent";
                            changed = true;
                        }
                    if (changed)
                        xslDoc.Save(file);
                }
            }

            //foreach (string file in Directory.GetFiles(Global.adminXmlPath + "\\"))
            //{
            //    if (file.ToLower().EndsWith("\\access.xml"))
            //    {
            //        File.Move(file, file.Replace(".xml", ".webinfo"));
            //    }
            //}
            
            // update email.xml
            if (File.Exists(Global.publicXmlPath + "/newsletter.xml"))
            {
                xmldoc.Load(Global.publicXmlPath + "/newsletter.xml");
                list = xmldoc.SelectNodes("//emailcategory/blnHTML");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    list.Item(counter).ParentNode.RemoveChild(list.Item(counter));
                }
                list = xmldoc.SelectNodes("//emailcategory/archive");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    list.Item(counter).ParentNode.RemoveChild(list.Item(counter));
                }

                xmldoc.Save(Global.publicXmlPath + "/newsletter.xml");
            }

            // update debate.xml
            if (File.Exists(Global.publicXmlPath + "/debate.xml"))
            {
                xmldoc.Load(Global.publicXmlPath + "/debate.xml");
                list = xmldoc.SelectNodes("//category[@name]");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    xmlelem = xmldoc.CreateElement("element", "title", "");
                    XmlCDataSection cdata = xmldoc.CreateCDataSection(list.Item(counter).Attributes["name"].Value);
                    xmlelem.AppendChild(cdata);
                    list.Item(counter).AppendChild(xmlelem);
                    list.Item(counter).Attributes.Remove(list.Item(counter).Attributes["name"]);
                }
                xmldoc.Save(Global.publicXmlPath + "/debate.xml");
            }

            if (File.Exists(Global.publicXmlPath + "/website.xml"))
            {
                xmldoc.Load(Global.publicXmlPath + "/website.xml");
                list = xmldoc.SelectNodes("//page/html");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    xmlelem = xmldoc.CreateElement("element", "contentholders", "");
                    XmlNode nodeTemp = xmlelem.AppendChild(list.Item(counter).Clone());
                    list.Item(counter).ParentNode.InsertBefore(xmlelem, list.Item(counter));
                    list.Item(counter).ParentNode.RemoveChild(list.Item(counter));
                }
                list = xmldoc.SelectNodes("//page/draft/html");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    xmlelem = xmldoc.CreateElement("element", "contentholders", "");
                    XmlNode nodeTemp = xmlelem.AppendChild(list.Item(counter).Clone());
                    list.Item(counter).ParentNode.InsertBefore(xmlelem, list.Item(counter));
                    list.Item(counter).ParentNode.RemoveChild(list.Item(counter));
                }

                list = xmldoc.SelectNodes("//page");
                for (int counter = 0; counter < list.Count; counter++)
                {
                    if (list.Item(counter).Attributes["created"] == null)
                        ((XmlElement)list.Item(counter)).SetAttributeNode("created", "");
                }

                xmldoc.Save(Global.publicXmlPath + "/website.xml");
                Global.oMenuXml = xmldoc;
            }

            // update access.webinfo
            if (File.Exists(Global.adminXmlPath + "/access.webinfo"))
            {
                xmldoc = new XmlDocument();
                xmldoc.Load(Global.adminXmlPath + "/access.webinfo");

                xmlnode = xmldoc.SelectSingleNode("//method[@id=1302]");
                xmlnode.Attributes["url"].Value = "?pageid={//selectedpage}&action=editdraftcontent";
                xmlnode = xmldoc.SelectSingleNode("//method[@id=1303]");
                xmlnode.Attributes["onclick"].Value = "window.open('?pageid={//selectedpage}&showdraft=1','newwindow');return false;";
                xmlnode = xmldoc.SelectSingleNode("//method[@id=1101]");
                xmlnode.Attributes["url"].Value = "/admin/menu/addmenu.aspx?pageid={//selectedpage}#selectedpage";

                xmlnode = xmldoc.SelectSingleNode("//module[@id=3100]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "module", "");
                    xmlelem.SetAttribute("id", "", "3100");
                    xmlelem.SetAttribute("name", "", "rss");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=0]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3101");
                    xmlelem.SetAttribute("name", "", "editrss");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3100]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//module[@id=3200]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "module", "");
                    xmlelem.SetAttribute("id", "", "3200");
                    xmlelem.SetAttribute("name", "", "comments");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=0]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3201");
                    xmlelem.SetAttribute("name", "", "addcomments");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3200]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3202");
                    xmlelem.SetAttribute("name", "", "editcomments");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3200]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3203");
                    xmlelem.SetAttribute("name", "", "activatecomment");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3200]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3204");
                    xmlelem.SetAttribute("name", "", "deletecomment");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3200]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//module[@id=2600]");
                if (xmlnode != null)
                {
                    xmlnode.Attributes["name"].Value = "calendar";

                    xmlnode = xmldoc.SelectSingleNode("//method[@id=2601]");
                    xmlnode.Attributes["name"].Value = "addcalendar";

                    xmlnode = xmldoc.SelectSingleNode("//method[@id=2602]");
                    xmlnode.Attributes["name"].Value = "editcalendar";

                    xmlnode = xmldoc.SelectSingleNode("//method[@id=2603]");
                    xmlnode.Attributes["name"].Value = "deletecalendar";
                }
                else
                {
                    xmlelem = xmldoc.CreateElement("element", "module", "");
                    xmlelem.SetAttribute("id", "", "2600");
                    xmlelem.SetAttribute("name", "", "calendar");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=0]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "2601");
                    xmlelem.SetAttribute("name", "", "addcalendar");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=2600]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "2602");
                    xmlelem.SetAttribute("name", "", "editcalendar");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=2600]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "2603");
                    xmlelem.SetAttribute("name", "", "deletecalendar");
                    xmlelem.SetAttribute("cache", "", "none");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=2600]");
                    xmlnode.AppendChild(xmlelem);

                }

                list = xmldoc.SelectNodes("//module[@id=1100 or @id=1300 or @id>=2100]");
                for (int counter = 0; counter < list.Count; counter++)
                    if (list.Item(counter).Attributes["submenu"] == null)
                        ((XmlElement)list.Item(counter)).SetAttribute("submenu", "page");

                list = xmldoc.SelectNodes("//module[@id=1400 or @id=1500 or @id=1600 or @id=1700]");
                for (int counter = 0; counter < list.Count; counter++)
                    if (list.Item(counter).Attributes["submenu"] == null)
                        ((XmlElement)list.Item(counter)).SetAttribute("submenu", "site");

                xmlnode = xmldoc.SelectSingleNode("//method[@id=1404]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1404");
                    xmlelem.SetAttribute("name", "", "editxslfile");
                    xmlelem.SetAttribute("url", "", "/admin/template/editxslfile.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1400]");
                    xmlnode.AppendChild(xmlelem);

                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1405");
                    xmlelem.SetAttribute("name", "", "editcssfile");
                    xmlelem.SetAttribute("url", "", "/admin/template/editcssfile.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1400]");
                    xmlnode.AppendChild(xmlelem);

                }
                
                xmlnode = xmldoc.SelectSingleNode("//method[@id=1406]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1406");
                    xmlelem.SetAttribute("name", "", "backup");
                    xmlelem.SetAttribute("url", "", "/admin/useradmin/backup.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1400]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=1504]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1504");
                    xmlelem.SetAttribute("name", "", "listfiles");
                    xmlelem.SetAttribute("url", "", "/admin/upload/listfiles.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1500]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=1505]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1505");
                    xmlelem.SetAttribute("name", "", "uploadandedit");
                    xmlelem.SetAttribute("url", "", "/admin/upload/uploadandedit.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1500]");
                    XmlNode xmlnode2 = xmldoc.SelectSingleNode("//method[@id=1501]");
                    xmlnode.InsertAfter(xmlelem, xmlnode2);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=2404]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "2404");
                    xmlelem.SetAttribute("name", "", "adddebatecategory");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=2400]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=3000]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "module", "");
                    xmlelem.SetAttribute("id", "", "3000");
                    xmlelem.SetAttribute("name", "", "gallery");
                    xmlelem.SetAttribute("submenu", "", "page");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=0]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=3001]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3001");
                    xmlelem.SetAttribute("name", "", "editgallery");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3000]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=3002]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "3002");
                    xmlelem.SetAttribute("name", "", "galleryeditimages");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=3000]");
                    xmlnode.AppendChild(xmlelem);
                }

                xmlnode = xmldoc.SelectSingleNode("//method[@id=1603]");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "method", "");
                    xmlelem.SetAttribute("id", "", "1603");
                    xmlelem.SetAttribute("name", "", "adminshortcuts");
                    xmlelem.SetAttribute("url", "", "/admin/useradmin/edit_adminshortcuts.aspx?pageid={//selectedpage}");

                    xmlnode = xmldoc.SelectSingleNode("//module[@id=1600]");
                    xmlnode.AppendChild(xmlelem);

                    list = xmldoc.SelectNodes("//user");
                    for (int counter = 0; counter < list.Count; counter++)
                    {
                        xmlelem = xmldoc.CreateElement("element", "shortcut", "");
                        xmlelem.InnerText = "ret indhold¤/modules/default.aspx?pageid=##pageid##&amp;action=editcontent";
                        list[counter].AppendChild(xmlelem);
                        xmlelem = xmldoc.CreateElement("element", "shortcut", "");
                        xmlelem.InnerText = "opret ny side¤/admin/menu/addmenu.aspx?pageid=##pageid##";
                        list[counter].AppendChild(xmlelem);
                        xmlelem = xmldoc.CreateElement("element", "shortcut", "");
                        xmlelem.InnerText = "upload fil¤/admin/upload/upload.aspx?pageid=##pageid##";
                        list[counter].AppendChild(xmlelem);
                    }
                }
                website.SaveAccessXml(xmldoc);
            }

            // Update web.config
            xmldoc.Load(HttpContext.Current.Server.MapPath("/web.config"));
            xmlnode = xmldoc.SelectSingleNode("/configuration/system.web/httpModules/add[@name='WebCoreModule']");
            if (xmlnode != null)
            {
                xmlnode.ParentNode.RemoveChild(xmlnode);
                xmlnode = xmldoc.SelectSingleNode("/configuration/system.web/httpModules/add[@type='Radactive.WebControls.ILoad.WebCoreModule, Radactive.WebControls.ILoad']");
                if (xmlnode != null)
                    xmlnode.ParentNode.RemoveChild(xmlnode);
            }

            if (System.Web.HttpRuntime.UsingIntegratedPipeline)
            {
                xmlnode = xmldoc.SelectSingleNode("/configuration/system.webServer/handlers/add[@name='PiczardWebResource']");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "add", "");
                    xmlelem.SetAttribute("verb", "*");
                    xmlelem.SetAttribute("name", "PiczardWebResource");
                    xmlelem.SetAttribute("path", "piczardWebResource.ashx");
                    xmlelem.SetAttribute("preCondition", "integratedMode");
                    xmlelem.SetAttribute("type", "CodeCarvings.Piczard.Web.WebResourceManager, CodeCarvings.Piczard");

                    xmlnode = xmldoc.SelectSingleNode("/configuration/system.webServer/handlers");
                    if (xmlnode != null)
                        xmlnode.AppendChild(xmlelem);
                    else
                    {
                        xmlnode = xmldoc.SelectSingleNode("/configuration/system.webServer");
                        XmlElement xmlelem2 = xmldoc.CreateElement("handlers");
                        xmlelem2.AppendChild(xmlelem);
                        xmlnode.AppendChild(xmlelem2);
                    }
                }
            } else {

                xmlnode = xmldoc.SelectSingleNode("/configuration/system.web/httpHandlers/add[@path='piczardWebResource.ashx']");
                if (xmlnode == null)
                {
                    xmlelem = xmldoc.CreateElement("element", "add", "");
                    xmlelem.SetAttribute("verb", "*");
                    xmlelem.SetAttribute("validate", "false");
                    xmlelem.SetAttribute("path", "piczardWebResource.ashx");
                    xmlelem.SetAttribute("type", "CodeCarvings.Piczard.Web.WebResourceManager, CodeCarvings.Piczard");

                    xmlnode = xmldoc.SelectSingleNode("/configuration/system.web/httpHandlers");
                    if (xmlnode != null)
                        xmlnode.AppendChild(xmlelem);
                    else
                    {
                        xmlnode = xmldoc.SelectSingleNode("/configuration/system.web");
                        XmlElement xmlelem2 = xmldoc.CreateElement("httpHandlers");
                        xmlelem2.AppendChild(xmlelem);
                        xmlnode.AppendChild(xmlelem2);
                    }
                }
            }


            try
            {
                xmldoc.Save(HttpContext.Current.Server.MapPath("/web.config"));
            }
            catch
            {
                lbContent.Text += "Error saving web.config. Please make it writeable and update again<br/>";
            }
            
			
 			lbContent.Text += "System has been updated";

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
