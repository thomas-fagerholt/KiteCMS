using System;
using System.Net;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Box
	{
		private int boxId = -1;
		private BoxCategory boxCategory = null;
		private string title = "";
		private string content = "";
		private string xmlUri = "";
		private bool cascade;

		public Box()
		{
		}

		public Box(bool withlock)
		{
			// Lock menuxml to prevent concurrency problems when saving
			if(withlock)
				core.Website.LockMenuXml();
		}

		public Box(int boxId)
		{
			BoxData boxData = new BoxData();
			this.boxId = boxId;
			boxData.Load(this, false);
		}

		public Box(int boxId, bool withlock)
		{
			BoxData boxData = new BoxData();
			this.boxId = boxId;
			boxData.Load(this, withlock);
		}

		public int Save()
		{
			BoxData boxData = new BoxData();
			boxData.Save(this);

			return this.BoxId;
		}

        public int Delete()
        {
            BoxData boxData = new BoxData();
            boxData.Delete(this);

            return this.BoxId;
        }

        public string Html()
        {
            if (XmlUri != "")
            {
                // generate html from xml
                try
                {
                    XmlDocument xmldoc = new XmlDocument();
                    XslTransform xslt = new XslTransform();

                    if (XmlUri.StartsWith("http"))
                    {
                        //get xml on http
                        WebRequest request = WebRequest.Create(XmlUri);
                        WebResponse response = request.GetResponse();

                        XmlTextReader reader = new XmlTextReader(response.GetResponseStream());
                        xmldoc.Load(reader);
                    }
                    else
                    {
                        if (XmlUri.IndexOf("website.xml") > -1)
                            // get public menuxml
                            xmldoc = (XmlDocument)((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.Clone();
                        else
                            // get local xml file
                            xmldoc.Load(Global.publicXmlPath + "/" + XmlUri);
                    }

                    ContentFunctions contentFunctions = new ContentFunctions();
                    StringReader sreader = new StringReader(Content);
                    XPathDocument xsltdoc = new XPathDocument(sreader);
                    
                    xslt.Load(xsltdoc);

                    XmlUrlResolver resolver = new XmlUrlResolver();
                    XsltArgumentList xslArg = new XsltArgumentList();
                    xslArg.AddExtensionObject("urn:contentFunctions", contentFunctions);
                    xslArg.AddParam("pageid", "", contentFunctions.GetPageId());

                    StringWriter writer = new StringWriter();

                    xslt.Transform(xmldoc, xslArg, writer, resolver);

                    return writer.ToString();

                }
                catch (Exception e)
                {
                    return "Error generating html. Error: " + e.Message;
                }
            }
            else
                return Content;
        }

        public string Title
		{
			get
			{
				return title;
			}
			set
			{
				this.title = value;
			}
		}

		public string Content
		{
			get
			{
				return content;
			}
			set
			{
				this.content = value;
			}
		}

		public string XmlUri
		{
			get
			{
				return xmlUri;
			}
			set
			{
				if (value.StartsWith("http") || (value.IndexOf("/")==-1 && value.IndexOf("\\")==-1))
					this.xmlUri = value;
			}
		}

		public int BoxId
		{
			get
			{
				return boxId;
			}
			set
			{
				this.boxId = value;
			}
		}

		public BoxCategory BoxCategory
		{
			get
			{
				return boxCategory;
			}
			set
			{
				this.boxCategory = value;
			}
		}

		public bool Cascade
		{
			get 
			{
				return cascade;
			}
			set
			{
				this.cascade = value;
			}
		}


	}
}
