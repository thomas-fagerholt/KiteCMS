using System;
using System.Web;
using System.Net;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the page object
	/// </summary>
	public class BoxData
	{
		Website website = new Website();

		public void Load(Box box)
		{
			Load(box,false);
		}

		public void Load(Box box, bool withlock)
		{
			XmlNode oXmlNode;
			XmlNode oXmlNodeProperty;

			if (box == null)
				throw new ArgumentNullException("box");
			if (box.BoxId == -1)
				throw new ArgumentException("box object doesn't have a boxId","box");

			try
			{
				// Lock menuxml to prevent concurrency problems when saving
				if (withlock)
					core.Website.LockMenuXml();
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule//box[@id="+ box.BoxId +"]");
                box.BoxCategory = new BoxCategory(int.Parse(oXmlNode.Attributes["boxcategoryid"].InnerText), false);

				if (oXmlNode.Attributes["cascade"] != null)
					box.Cascade = bool.Parse(oXmlNode.Attributes["cascade"].InnerText);
				else
					box.Cascade = false;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("title");
				if (oXmlNodeProperty != null)
					box.Title = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("content");
				if (oXmlNodeProperty != null)
					box.Content = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("xmluri");
				if (oXmlNodeProperty != null)
					box.XmlUri = oXmlNodeProperty.InnerText;
			}
			catch
			{
				throw new Exception ("Error loading box");
			}
		}

		public void Delete(Box box)
		{
			XmlNode objNode;
			XmlNodeList objNodes;
			XmlNode objNodeParent;
			XmlNode objNodeNew;

			if (box == null)
				throw new ArgumentNullException("box");
			if (box.BoxId == -1)
				throw new ArgumentException("Box object doesn't have a boxId","box");

			objNodes = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page/boxmodule[@boxid='"+ box.BoxId +"']");
			for (int counter = 0; counter <= objNodes.Count-1;counter++)
			{
				objNodeParent = objNodes.Item(counter).ParentNode;
				objNode = objNodeParent.RemoveChild(objNodes.Item(counter));
			}

			objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule/box[@id="+ box.BoxId +"]");
			objNodeParent = objNode.ParentNode;
			objNodeNew = objNodeParent.RemoveChild(objNode);

			website.Save();
		}

		public void Save(Box box)
		{
			if (box == null)
				throw new ArgumentNullException("box");

			if (box.BoxId == -1)
				Insert(box);
			else
			{
				XmlNode objPageNode;
				XmlNode objNode;
				XmlNode oXmlCdata;

				objPageNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule/box[@id="+ box.BoxId +"]");

				objPageNode.Attributes["boxcategoryid"].Value = box.BoxCategory.BoxCategoryId.ToString();

				if (objPageNode.Attributes["cascade"] != null)
					objPageNode.Attributes["cascade"].Value = box.Cascade.ToString();
				else
				{
					XmlAttribute oXmlAtr = ((XmlElement)objPageNode).SetAttributeNode("cascade","");
					oXmlAtr.Value = box.Cascade.ToString();
				}

				objNode = objPageNode.SelectSingleNode("title");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = box.Title;

				objNode = objPageNode.SelectSingleNode("content");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = box.Content;

				objNode = objPageNode.SelectSingleNode("xmluri");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = box.XmlUri;

				website.Save();
			}

		}
		
		public void Insert(Box box)
		{
			if (box == null)
				throw new ArgumentNullException("box");
			if (box.BoxId != -1)
				throw new ArgumentException("Box object does have a boxId","box");

			int maxBoxId = -1;
			XmlNodeList oXmlNodeRange;
			XmlNode oXmlNodeNew;
			XmlNode oXmlParentNode;

			// get next pageid to use
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("box");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				if (int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value) > maxBoxId)
					maxBoxId = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
			}

			XmlElement oXmlElem;
			XmlCDataSection oXmlCdata;
			XmlAttribute oXmlAtr;
			XmlNode oXmlNode;

			// create new element with new data
			oXmlElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("box","");
			oXmlAtr = oXmlElem.SetAttributeNode("id","");
			maxBoxId ++;
			oXmlAtr.Value = maxBoxId.ToString();
			oXmlAtr = oXmlElem.SetAttributeNode("boxcategoryid","");
			oXmlAtr.Value = box.BoxCategory.BoxCategoryId.ToString();

			oXmlAtr = oXmlElem.SetAttributeNode("cascade","");
			oXmlAtr.Value = box.Cascade.ToString();

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("title","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(box.Title);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("content","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(box.Content);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("xmluri","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(box.XmlUri);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule");
			oXmlNodeNew = oXmlParentNode.AppendChild(oXmlElem);

			website.Save();
		}

	}
}
