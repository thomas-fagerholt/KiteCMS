using System;
using System.Xml;
using System.Web;
using System.Xml.XPath;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the page object
	/// </summary>
	public class BoxCategoryData
	{
		Website website = new Website();

		public void Load(BoxCategory boxCategory)
		{
			Load(boxCategory,false);
		}

		public void Load(BoxCategory boxCategory, bool withlock)
		{
			XmlNode oXmlNode;
			XmlNode oXmlNodeProperty;

			if (boxCategory == null)
				throw new ArgumentNullException("boxCategory");
			if (boxCategory.BoxCategoryId == -1)
				throw new ArgumentException("BoxCategory object doesn't have a boxCategoryId","boxCategory");

			try
			{
                // Lock menuxml to prevent concurrency problems when saving
				if(withlock)
					core.Website.LockMenuXml();

				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule//boxcategory[@id="+ boxCategory.BoxCategoryId +"]");
				boxCategory.BoxCategoryId = int.Parse(oXmlNode.Attributes["id"].InnerText);
				boxCategory.BoxCategoryType = (BoxCategory.BoxCategoryTypeEnum)int.Parse(oXmlNode.Attributes["type"].InnerText);

				oXmlNodeProperty = oXmlNode.SelectSingleNode("title");
				boxCategory.Title = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("htmlstring");
				boxCategory.Htmlstring = oXmlNodeProperty.InnerText;

			}
			catch
			{
				throw new Exception ("Error loading boxcategory");
			}
		}

		public void Delete(BoxCategory boxCategory, int newBoxCategoryId)
		{
			if (newBoxCategoryId != -1)
			{
				//change boxes
				XmlNodeList oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("/website/boxmodule/box[@boxcategoryid="+ boxCategory.BoxCategoryId +"]");
				for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				{
                    oXmlNodeRange.Item(counter).Attributes["boxcategoryid",""].Value = newBoxCategoryId.ToString();
				}
			}

			Delete(boxCategory);
		}

		public void Delete(BoxCategory boxCategory)
		{
			XmlNode objNode;
			XmlNode objNodeParent;
			XmlNode objNodeNew;

			if (boxCategory == null)
				throw new ArgumentNullException("box");
			if (boxCategory.BoxCategoryId == -1)
				throw new ArgumentException("boxCategory object doesn't have a boxCategoryId","boxCategory");

			// test if there is no boxes associated with the page
			objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule/box[@boxcategoryid="+ boxCategory.BoxCategoryId +"]");
			if (objNode == null)
			{
				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule/boxcategory[@id="+ boxCategory.BoxCategoryId +"]");
				objNodeParent = objNode.ParentNode;
				objNodeNew = objNodeParent.RemoveChild(objNode);
			}

			website.Save();

		}

		public void Save(BoxCategory boxCategory)
		{
			if (boxCategory == null)
				throw new ArgumentNullException("boxCategory");

			if (boxCategory.BoxCategoryId == -1)
				Insert(boxCategory);
			else
			{
				XmlNode objPageNode;
				XmlNode objNode;
				XmlNode oXmlCdata;

				objPageNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule/boxcategory[@id="+ boxCategory.BoxCategoryId +"]");

				objPageNode.Attributes["type"].Value = ((int)boxCategory.BoxCategoryType).ToString();

				objNode = objPageNode.SelectSingleNode("title");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = boxCategory.Title;

				objNode = objPageNode.SelectSingleNode("htmlstring");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = boxCategory.Htmlstring;

				website.Save();
			}
		}

		public void Insert(BoxCategory boxCategory)
		{
			if (boxCategory == null)
				throw new ArgumentNullException("boxcategory");
			if (boxCategory.BoxCategoryId != -1)
				throw new ArgumentException("BoxCategory object does have a boxCategoryId","boxCategory");

			int maxBoxCategoryId = -1;
			XmlNodeList oXmlNodeRange;
			XmlNode oXmlNodeNew;
			XmlNode oXmlParentNode;

			// get next pageid to use
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("boxcategory");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				if (int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value) > maxBoxCategoryId)
					maxBoxCategoryId = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
			}

			XmlElement oXmlElem;
			XmlCDataSection oXmlCdata;
			XmlAttribute oXmlAtr;
			XmlNode oXmlNode;

			// create new element with new data
			oXmlElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("boxcategory","");
			oXmlAtr = oXmlElem.SetAttributeNode("id","");
			maxBoxCategoryId ++;
			oXmlAtr.Value = maxBoxCategoryId.ToString();

			oXmlAtr = oXmlElem.SetAttributeNode("type","");
            oXmlAtr.Value = ((int)boxCategory.BoxCategoryType).ToString();

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("title","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(boxCategory.Title);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("htmlstring","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(boxCategory.Htmlstring);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/boxmodule");
            if (oXmlParentNode == null)
            {
                oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("element", "boxmodule", "");
                oXmlParentNode.AppendChild(oXmlElem);
                ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("website").AppendChild(oXmlParentNode);
            }
            else
                oXmlNodeNew = oXmlParentNode.AppendChild(oXmlElem);

			website.Save();

		}
	}
}
