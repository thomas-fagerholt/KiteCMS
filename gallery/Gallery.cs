using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;

namespace KiteCMS.modules
{
    /// <summary>
    /// Summary description for extranet.
    /// </summary>
    public class Gallery
	{

		public Gallery()
		{
		}

        public string Menu(Admin.Admin admin, Admin.Page page)
        {
            // Show list of available methods
            return funcList(admin, page);
        }

        public string Public(int pageId)
		{
			// Entry point for the public part of the module
			// Input parameters is the page object for the current page
			// Return parameter is html-code to put in the content area of the page

			XmlDocument oXmlGallery = OXmlGallery(pageId);
            StringBuilder output = new StringBuilder();

            string width = "500";
            string height = "500";
            string thumbwidth = "100";
            string thumbheight = "100";
            bool textinlist = false;
            string imagefolders = "";
            string fileName = "";
            string gallerydownloadtext = "";
			XmlNode xmlnode;

			xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid="+ pageId +"]");
            if (xmlnode != null)
            {
                imagefolders = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/imagefolder").InnerText;
                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/width");
                if (xmlnode != null && xmlnode.InnerText != "")
                    width = xmlnode.InnerText;

                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/height");
                if (xmlnode != null && xmlnode.InnerText != "")
                    height = xmlnode.InnerText;

                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/thumbwidth");
                if (xmlnode != null && xmlnode.InnerText != "")
                    thumbwidth = xmlnode.InnerText;

                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/textinlist");
                if (xmlnode != null && xmlnode.InnerText != "")
                    textinlist = bool.Parse(xmlnode.InnerText);

                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/thumbheight");
                if (xmlnode != null && xmlnode.InnerText != "")
                    thumbheight = xmlnode.InnerText;

                xmlnode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/gallerydownloadtext");
                if (xmlnode != null && xmlnode.InnerText != "")
                    gallerydownloadtext = xmlnode.InnerText;

                if (ConfigurationManager.AppSettings["excludeslimbox"] == null || ConfigurationManager.AppSettings["excludeslimbox"].ToLower() != "true")
                    Functions.AddHtmlHeader("<link rel=\"stylesheet\" href=\"/modules/gallery/css/slimbox2.css\" type=\"text/css\" />");
                if (ConfigurationManager.AppSettings["excludejquery"] == null || ConfigurationManager.AppSettings["excludejquery"].ToLower() != "true")
                    output.Append(@"<script type=""text/javascript"" src=""http://ajax.googleapis.com/ajax/libs/jquery/1.3/jquery.min.js""></script>" + Environment.NewLine);
                if (gallerydownloadtext != "")
                    output.Append(@"<script type=""text/javascript"">var gallerydownloadtext='" + gallerydownloadtext.Replace("'","\"") + "';</script>" + Environment.NewLine);
                if (ConfigurationManager.AppSettings["excludeslimbox"] == null || ConfigurationManager.AppSettings["excludeslimbox"].ToLower() != "true")
                    output.Append(@"<script type=""text/javascript"" src=""/modules/gallery/js/slimbox2.js""></script>" + Environment.NewLine);

                for (int counter = 0; counter < imagefolders.Split(';').Length; counter++)
                {
                    string imagefolder = imagefolders.Split(';')[counter];
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(imagefolder)))
                    {
                        string fileNodeHeaderText = "&nbsp;";
                        XmlNode fileNodeHeader = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/folder[folderid='" + imagefolder.Replace("/", "").Replace("_", "") + "']/header");
                        if (fileNodeHeader != null)
                            fileNodeHeaderText = fileNodeHeader.InnerText;
                        if (fileNodeHeaderText != "")
                            output.Append("<h2 class='galleryheader'>" + fileNodeHeaderText + "</h2>" + Environment.NewLine);
                        foreach (string image in Directory.GetFiles(HttpContext.Current.Server.MapPath(imagefolder)))
                        {
                            string alt = "";
                            string title = "";
                            fileName = image.Replace(HttpContext.Current.Server.MapPath(imagefolder), "");
                            fileName = fileName.Replace(@"\", "");
                            string fileId = imagefolder.ToLower().Replace("/", "").Replace("_", "") + ":" + fileName;
                            XmlNode fileNode = oXmlGallery.SelectSingleNode("//gallery[@pageid=" + pageId + "]/image[fileid='" + fileId.ToLower() + "']");
                            if (fileNode != null)
                            {
                                if (fileNode.SelectSingleNode("alt") != null)
                                    alt = fileNode.SelectSingleNode("alt").FirstChild.InnerText;
                                if (fileNode.SelectSingleNode("title") != null)
                                    title = fileNode.SelectSingleNode("title").FirstChild.InnerText;
                            }
                            if (Path.GetExtension(image).ToLower() == ".jpg")
                            {

                                // add node to xml
                                output.Append("<div style='float:left;width:" + thumbwidth + "px;height:" + thumbheight + "px' class='galleryImageContainer'>" + Environment.NewLine);
                                output.Append("<a href='/modules/gallery/images/getImage.aspx?imageurl=" + imagefolder + "/" + fileName + "&amp;maxwidth=" + width + "&amp;maxheight=" + height + "' title=\"" + title + "\" rel=\"lightbox[gallery" + counter + "]\"><img src='/modules/gallery/images/thumbs/getImage.aspx?imageurl=" + imagefolder + "/" + fileName + "&amp;maxwidth=" + thumbwidth + "&amp;maxheight=" + thumbheight + "' alt='" + alt + "' class='galleryImage'/></a>" + Environment.NewLine);
                                if (textinlist)
                                    output.Append("<div class=\"galleryImageText\">"+ title + "</div>" + Environment.NewLine);
                                output.Append("</div>" + Environment.NewLine);
                            }
                        }
                    }
                    output.Append("<br clear=\"all\"/>");
                    //output.Append(@"<script type=""text/javascript"">window.onload = function() {initLightbox();}</script>" + Environment.NewLine);
                }
            }
			return output.ToString();
		}


		public string Admin(Admin.Admin admin, Admin.Page page, out bool showMenu, out bool showModuleMenu)
		{
			// Entry point for the administration part of the module
			// Input parameters is the admin and page object for the current page
			// Output parameters is booleans for showing the left menu and the right module menu
			// Return parameter is html-code to put in the content area of the page

			string action="";
			string output;

			// Find out what to do passed on the action-field
			if(HttpContext.Current.Request.QueryString["action"] != null)
				action = HttpContext.Current.Request.QueryString["action"];
			if(HttpContext.Current.Request.Form["action"] != null && action == "")
				action = HttpContext.Current.Request.Form["action"];

			switch(action)
			{
				case ("doeditgallery"):
				{
					// Show list of available methods
					output = funcDoEditGallery(admin, page);
					showMenu= true;
					showModuleMenu = true;
					break;
				}

            case ("editgallery"):
                {
                    // Show list of available methods
                    output = funcEditGallery(admin, page);
                    showMenu = true;
                    showModuleMenu = true;
                    break;
                }

            case ("doeditgalleryimages"):
                {
                    // Show list of available methods
                    output = doEditGalleryImages(admin, page);
                    showMenu = true;
                    showModuleMenu = true;
                    break;
                }

            case ("editgalleryimages"):
                {
                    // Show list of available methods
                    output = editGalleryImages(admin, page);
                    showMenu = true;
                    showModuleMenu = true;
                    break;
                }

            default:
                {
                    // Show list of available methods
                    output = funcList(admin, page);
                    showMenu = true;
                    showModuleMenu = true;
                    break;
                }
        }

			return output;
		}

        private string editGalleryImages(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();
            XmlNode xmlnode;
            XmlCDataSection cdata;
            XslTransform xslt = new XslTransform();
            XmlDocument oXml = new XmlDocument();
            StringWriter writer = new StringWriter();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3002, page.Pageid);

            // append headers to output
            output.Append("<h2>" + Functions.localText("gallery") + "</h2>");
            output.Append("<h3>" + Functions.localText("galleryeditimages") + "</h3>");

            // Load the xml-file holding data on gallery
            oXml = OXmlGallery(page.Pageid);

            xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/imagefolder");
            if (xmlnode != null)
            {
                string imagefolders = xmlnode.InnerText;
                for (int counter = 0; counter < imagefolders.Split(';').Length; counter++)
                {
                    string imagefolder = imagefolders.Split(';')[counter];
                    if (Directory.Exists(HttpContext.Current.Server.MapPath(imagefolder)))
                    {
                        // add header node
                        bool folderalreadyincluded = false;
                        XmlNodeList nodelistFolder = oXml.SelectNodes("//gallery[@pageid=" + page.Pageid + "]/folder");
                        foreach (XmlNode node in nodelistFolder)
                            if (node.SelectSingleNode("folderid") != null && node.SelectSingleNode("folderid").InnerText.ToLower() == imagefolder.Replace("/", "").Replace("_", ""))
                            {
                                folderalreadyincluded = true;
                                XmlNode xmlnodeFolderHeader = oXml.CreateElement("element", "folderpath", "");
                                cdata = oXml.CreateCDataSection(imagefolder);
                                xmlnodeFolderHeader.AppendChild(cdata);
                                node.AppendChild(xmlnodeFolderHeader);
                            }
                        if (!folderalreadyincluded)
                        {
                            xmlnode = oXml.CreateElement("element", "folder", "");
                            XmlNode xmlnodeFolderHeader = oXml.CreateElement("element", "folderid", "");
                            cdata = oXml.CreateCDataSection(imagefolder.Replace("/", "").Replace("_", ""));
                            xmlnodeFolderHeader.AppendChild(cdata);
                            xmlnode.AppendChild(xmlnodeFolderHeader);

                            xmlnodeFolderHeader = oXml.CreateElement("element", "folderpath", "");
                            cdata = oXml.CreateCDataSection(imagefolder);
                            xmlnodeFolderHeader.AppendChild(cdata);
                            xmlnode.AppendChild(xmlnodeFolderHeader);

                            XmlNode temp = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlnode);
                        }

                        foreach (string image in Directory.GetFiles(HttpContext.Current.Server.MapPath(imagefolder)))
                        {
                            string fileName = image.Replace(HttpContext.Current.Server.MapPath(imagefolder), "");
                            fileName = fileName.Replace(@"\", "").ToLower();
                            string fileId = imagefolder.ToLower().Replace("/", "").Replace("_", "") +":"+ fileName;
                            if (Path.GetExtension(image).ToLower() == ".jpg")
                            {
                                bool alreadyincluded = false;
                                // temp add folder
                                XmlNode xmlnodeFolder = oXml.CreateElement("element", "folder", "");
                                cdata = oXml.CreateCDataSection(imagefolder);
                                xmlnodeFolder.AppendChild(cdata);

                                XmlNodeList nodelist = oXml.SelectNodes("//gallery[@pageid=" + page.Pageid + "]/image");
                                foreach (XmlNode node in nodelist)
                                {
                                    if (node.SelectSingleNode("fileid") != null && node.SelectSingleNode("fileid").FirstChild.InnerText.ToLower() == fileId)
                                    {
                                        node.AppendChild(xmlnodeFolder);
                                        alreadyincluded = true;
                                    }
                                    
                                    if (!alreadyincluded && node.SelectSingleNode("fileid") == null)
                                    {
                                        if (node.SelectSingleNode("filename").InnerText.ToLower() == fileName && node.SelectSingleNode("../imagefolder").InnerText.ToLower() == imagefolder.ToLower())
                                        {
                                            // add fileid
                                            XmlNode fileIdNode = oXml.CreateElement("element", "fileid", "");
                                            cdata = oXml.CreateCDataSection(fileId);
                                            fileIdNode.AppendChild(cdata);
                                            node.AppendChild(fileIdNode);

                                            node.AppendChild(xmlnodeFolder);
                                            alreadyincluded = true;
                                        }
                                    }
                                }

                                if (!alreadyincluded)
                                {
                                    // add node to xml
                                    xmlnode = oXml.CreateElement("element", "image", "");
                                    XmlNode xmlnode2 = oXml.CreateElement("element", "fileid", "");
                                    cdata = oXml.CreateCDataSection(fileId);
                                    xmlnode2.AppendChild(cdata);
                                    xmlnode.AppendChild(xmlnode2);
                                    xmlnode2 = oXml.CreateElement("element", "filename", "");
                                    cdata = oXml.CreateCDataSection(fileName);
                                    xmlnode2.AppendChild(cdata);
                                    xmlnode.AppendChild(xmlnode2);

                                    xmlnode.AppendChild(xmlnodeFolder);
                                }

                                XmlNode temp = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlnode);
                            }
                        }
                    }
                }
            }
            // Load the xsl-file to use for the current transformation
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/gallery/form_edit_gallery_images.xsl"));
            XmlUrlResolver resolver = new XmlUrlResolver();

            // Add xslargument with Functions-class to use for lanugage specific texts in the xsl
            Functions functions = new Functions();
            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("pageid", "", page.Pageid);

            // Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXml, xslArg, writer, resolver);
            output.Append(writer.ToString());

            return output.ToString();
        }

        private string doEditGalleryImages(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();
            XmlDocument oXml;
            XmlNode parentNode;
            XmlNode xmlnode;
            XmlCDataSection cdata;
            string gallerydownloadtext = "";

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3002, page.Pageid);

            // append headers to output
            output.Append("<h2>" + Functions.localText("gallery") + "</h2>");
            output.Append("<h3>" + Functions.localText("galleryeditimages") + "</h3>");

            // Load the xml-file holding data on gallery
            oXml = OXmlGallery(page.Pageid);

            //start with deleting all old imageinfo
            XmlNodeList nodes = oXml.SelectNodes("//gallery[@pageid=" + page.Pageid + "]/node()[local-name()='image' or local-name()='folder']");
            foreach(XmlNode node in nodes)
                parentNode = node.ParentNode.RemoveChild(node);

            if (HttpContext.Current.Request["gallerydownloadtext"] != null)
                gallerydownloadtext = HttpContext.Current.Request["gallerydownloadtext"].ToString();

            xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]");
            if (xmlnode != null)
            {
                if (oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/gallerydownloadtext") != null)
                {
                    xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/gallerydownloadtext");
                    xmlnode.FirstChild.InnerText = gallerydownloadtext;
                }
                else
                {
                    XmlElement xmlelem = oXml.CreateElement("gallerydownloadtext", "");
                    cdata = oXml.CreateCDataSection(gallerydownloadtext);
                    xmlelem.AppendChild(cdata);
                    oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlelem);
                }
            }
            foreach (string key in HttpContext.Current.Request.Form)
            {
                if (key.IndexOf("filealt") == 0 && key.Replace("filealt_", "").IndexOf(':')>-1)
                {
                    string fileid = key.Replace("filealt_", "").ToLower();
                    string filename = fileid.Substring(fileid.IndexOf(':')+1);
                    fileid = fileid.Substring(0, fileid.IndexOf(':')).Replace("/", "").Replace("_", "") + ":" + filename;
                    string alt = HttpContext.Current.Request.Form[key];
                    string title = HttpContext.Current.Request.Form[key.Replace("filealt","filetitle")];
                    XmlElement elem = oXml.CreateElement("element", "image", "");
                    XmlElement subelem = oXml.CreateElement("element", "fileid", "");
                    cdata = oXml.CreateCDataSection(fileid);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);

                    subelem = oXml.CreateElement("element", "filename", "");
                    cdata = oXml.CreateCDataSection(filename);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);

                    subelem = oXml.CreateElement("element", "title", "");
                    cdata = oXml.CreateCDataSection(title);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);

                    subelem = oXml.CreateElement("element", "alt", "");
                    cdata = oXml.CreateCDataSection(alt);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);

                    if (alt != "" || title != "")
                        oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(elem);
                }
                if (key.IndexOf("folder") == 0)
                {
                    string folderid = key.Replace("folder_", "").ToLower();
                    string header = HttpContext.Current.Request.Form[key];
                    XmlElement elem = oXml.CreateElement("element", "folder", "");
                    XmlElement subelem = oXml.CreateElement("element", "folderid", "");
                    cdata = oXml.CreateCDataSection(folderid);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);

                    subelem = oXml.CreateElement("element", "header", "");
                    cdata = oXml.CreateCDataSection(header);
                    subelem.AppendChild(cdata);
                    elem.AppendChild(subelem);
                    
                    oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(elem);
                }

            }
            oXml.Save(Global.publicXmlPath + @"/gallery.xml");

            output.Append(Functions.localText("gallerysaved") + Functions.publicUrl());

            return output.ToString();
        }

        private string funcDoEditGallery(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();

            // check for rights to this method. If user is not allowed he will be redirected to the default page
            admin.userHasAccess(3001, page.Pageid);

            // append headers to output
            output.Append("<h2>" + Functions.localText("gallery") + "</h2>");
            output.Append("<h3>" + Functions.localText("editgallery") + "</h3>");

            XmlDocument oXml = new XmlDocument();
            string imageFolder = "";
            string width = "";
            string height = "";
            string thumbwidth = "";
            string thumbheight = "";
            bool textinlist = false;

            if (HttpContext.Current.Request["imagefolder"] != null)
                imageFolder = HttpContext.Current.Request["imagefolder"].ToString();
            if (HttpContext.Current.Request["width"] != null)
                width = HttpContext.Current.Request["width"].ToString();
            if (HttpContext.Current.Request["height"] != null)
                height = HttpContext.Current.Request["height"].ToString();
            if (HttpContext.Current.Request["thumbwidth"] != null)
                thumbwidth = HttpContext.Current.Request["thumbwidth"].ToString();
            if (HttpContext.Current.Request["thumbheight"] != null)
                thumbheight = HttpContext.Current.Request["thumbheight"].ToString();
            if (HttpContext.Current.Request["textinlist"] != null)
                textinlist = (HttpContext.Current.Request["textinlist"].ToString()!="");


            if (imageFolder != "" && imageFolder.IndexOf(".") < 0 && width != "" && height != "" && thumbwidth != "" && thumbheight != "")
            {
                XmlNode xmlnode;

                // Load the xml-file holding data on gallery
                oXml = OXmlGallery(page.Pageid);

                xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]");
                if (xmlnode != null)
                {
                    xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/imagefolder");
                    xmlnode.FirstChild.InnerText = imageFolder;

                    xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/width");
                    xmlnode.InnerText = width;

                    xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/height");
                    xmlnode.InnerText = height;

                    if (oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/thumbheight") != null)
                    {
                        xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/thumbheight");
                        xmlnode.InnerText = thumbheight;

                        xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/thumbwidth");
                        xmlnode.InnerText = thumbwidth;
                    }
                    else
                    {
                        XmlElement xmlelem = oXml.CreateElement("thumbheight", "");
                        xmlelem.InnerText = thumbheight;
                        oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlelem);

                        xmlelem = oXml.CreateElement("thumbwidth", "");
                        xmlelem.InnerText = thumbwidth;
                        oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlelem);
                    }
                    if (oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/textinlist") != null)
                    {
                        xmlnode = oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]/textinlist");
                        xmlnode.InnerText = textinlist.ToString().ToLower();
                    }
                    else
                    {
                        XmlElement xmlelem = oXml.CreateElement("textinlist", "");
                        xmlelem.InnerText = textinlist.ToString().ToLower();
                        oXml.SelectSingleNode("//gallery[@pageid=" + page.Pageid + "]").AppendChild(xmlelem);
                    }

                }
                else
                {
                    XmlAttribute xmlAtr;
                    XmlElement xmlelem;
                    XmlElement xmlelem2;
                    XmlCDataSection xmlCdata;

                    xmlelem = oXml.CreateElement("gallery", "");
                    xmlAtr = xmlelem.SetAttributeNode("pageid", "");
                    xmlAtr.Value = page.Pageid.ToString();

                    xmlelem2 = oXml.CreateElement("width", "");
                    xmlelem2.InnerText = width;
                    xmlelem.AppendChild(xmlelem2);

                    xmlelem2 = oXml.CreateElement("height", "");
                    xmlelem2.InnerText = height;
                    xmlelem.AppendChild(xmlelem2);

                    xmlelem2 = oXml.CreateElement("thumbwidth", "");
                    xmlelem2.InnerText = thumbwidth;
                    xmlelem.AppendChild(xmlelem2);

                    xmlelem2 = oXml.CreateElement("thumbheight", "");
                    xmlelem2.InnerText = thumbheight;
                    xmlelem.AppendChild(xmlelem2);

                    xmlelem2 = oXml.CreateElement("textinlist", "");
                    xmlelem2.InnerText = textinlist.ToString().ToLower();
                    xmlelem.AppendChild(xmlelem2);

                    xmlCdata = oXml.CreateCDataSection(imageFolder);
                    xmlelem2 = oXml.CreateElement("imagefolder", "");
                    xmlelem2.AppendChild(xmlCdata);
                    xmlelem.AppendChild(xmlelem2);

                    xmlnode = oXml.SelectSingleNode("/website");
                    xmlnode.AppendChild(xmlelem);
                }

                oXml.Save(Global.publicXmlPath + @"/gallery.xml");
            }

            output.Append(Functions.localText("gallerysaved") + Functions.publicUrl());

			return output.ToString();
		}

		private string funcEditGallery(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(3001,page.Pageid);

			// append headers to output
			output.Append("<h3>"+ Functions.localText("editgallery") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on debate
			oXml = OXmlGallery(page.Pageid);


			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/gallery/form_edit_gallery.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("pageid","", page.Pageid);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

        private string funcList(Admin.Admin admin, Admin.Page page)
        {
            StringBuilder output = new StringBuilder();
            Admin.Template template = new Admin.Template(page.TemplateId);

            // append headers to output
            output.Append("<h2>" + Functions.localText("gallery") + "</h2><ul>");

            // check for rights to methods in this module and list methods that the user have access to
            if (admin.userHasAccess(3001, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=editgallery'>" + Functions.localText("editgallery") + "</a></li>");
            if (admin.userHasAccess(3002, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=editgalleryimages'>" + Functions.localText("galleryeditimages") + "</a></li>");
            output.Append("</ul>");

            return output.ToString();
        }

		private XmlDocument OXmlGallery(int pageid)
		{
			XmlDocument output = new XmlDocument();
			XmlNode oXmlNode;

			// Load the xml-file holding data
            output.Load(Global.publicXmlPath + "/gallery.xml");

			// Insert current pageid in the xml
			oXmlNode = output.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageid.ToString();

			return output;
		}

	}
}
