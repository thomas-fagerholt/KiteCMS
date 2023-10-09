using System;
using System.Xml;
using System.Web;
using System.Xml.XPath;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the template object
	/// </summary>
	public class TemplateData
	{
		Website website = new Website();

		public void Template()
		{
		}
		public void Load(Template template)
		{
			Load(template,false);
		}

		public void Load(Template template, bool withlock)
		{
			XmlNode oXmlNode;
			XmlNode oXmlNodeProperty;

			if (template == null)
				throw new ArgumentNullException("page");
			if (template.TemplateId == -1)
				throw new ArgumentException("Template object doesn't have a templateId","template");

			try
			{
				// Lock menuxml to prevent concurrency problems when saving
				if(withlock)
					core.Website.LockMenuXml();

				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]");
				template.TemplateId = int.Parse(oXmlNode.Attributes["id"].InnerText);
 
				oXmlNodeProperty = oXmlNode.SelectSingleNode("title");
				template.Title = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("moduleclasspublic");
				if (oXmlNodeProperty != null)
					template.ModuleClassPublic = oXmlNodeProperty.InnerText;

                oXmlNodeProperty = oXmlNode.SelectSingleNode("moduleclassadmin");
                if (oXmlNodeProperty != null)
                    template.ModuleClassAdmin = oXmlNodeProperty.InnerText;

                oXmlNodeProperty = oXmlNode.SelectSingleNode("usercontrol");
                if (oXmlNodeProperty != null)
                    template.UserControl = oXmlNodeProperty.InnerText;

                oXmlNodeProperty = oXmlNode.SelectSingleNode("templatecolor");
				if (oXmlNodeProperty != null)
					template.Templatecolor = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("publicurl");
				template.Publicurl = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("adminurl");
				template.Adminurl = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("xslurl");
				template.Xslurl = oXmlNodeProperty.InnerText;
			}
			catch
			{
				// Unlock menuxml
				if(withlock)
					core.Website.UnLockMenuXml();

				throw new Exception ("Error loading template properties");
			}
		}

		public void Delete(Template template, int newTemplateId)
		{
			XmlNode objNode;
			XmlNode objNodeParent;
			XmlNode objNodeNew;
			XmlNodeList oXmlNodeRange;

			if (template == null)
				throw new ArgumentNullException("template");
			if (template.TemplateId == -1)
				throw new ArgumentException("template object doesn't have a templateId","template");

			if (newTemplateId != -1)
			{
				//change template on effected pages
				oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[usetemplate="+ template.TemplateId +"]/usetemplate");
				for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				{
					oXmlNodeRange.Item(counter).InnerText = newTemplateId.ToString();
				}
			}

			objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]");
			objNodeParent = objNode.ParentNode;
			objNodeNew = objNodeParent.RemoveChild(objNode);

			website.Save();
		}

		public void Save(Template template)
		{
			if (template == null)
				throw new ArgumentNullException("template");

			if (template.TemplateId == -1)
				Insert(template);
			else
			{
				XmlNode objNode;
				XmlNode oXmlCdata;
	
				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/title");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = template.Title;

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/moduleclasspublic");
				if (objNode == null)
				{
					XmlNode oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("moduleclasspublic","");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.ModuleClassPublic);
					oXmlNode.AppendChild(oXmlCdata);
					((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]").AppendChild(oXmlNode);
				}
				else
				{
					oXmlCdata = objNode.FirstChild;
					oXmlCdata.InnerText = template.ModuleClassPublic;
				}

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/moduleclassadmin");
				if (objNode == null)
				{
					XmlNode oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("moduleclassadmin","");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.ModuleClassAdmin);
					oXmlNode.AppendChild(oXmlCdata);
					((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]").AppendChild(oXmlNode);
				}
				else
				{
					oXmlCdata = objNode.FirstChild;
					oXmlCdata.InnerText = template.ModuleClassAdmin;
				}

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/moduleclassadmin");
				if (objNode == null)
				{
					XmlNode oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("moduleclassadmin","");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.ModuleClassAdmin);
					oXmlNode.AppendChild(oXmlCdata);
					((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]").AppendChild(oXmlNode);
				}
				else
				{
					oXmlCdata = objNode.FirstChild;
					oXmlCdata.InnerText = template.ModuleClassAdmin;
				}

                objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id=" + template.TemplateId + "]/usercontrol");
                if (objNode == null)
                {
                    XmlNode oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("usercontrol", "");
                    oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.UserControl);
                    oXmlNode.AppendChild(oXmlCdata);
                    ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id=" + template.TemplateId + "]").AppendChild(oXmlNode);
                }
                else
                {
                    oXmlCdata = objNode.FirstChild;
                    oXmlCdata.InnerText = template.UserControl;
                }

                objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/templatecolor");
				if (objNode == null)
				{
					XmlNode oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("templatecolor","");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Templatecolor);
					oXmlNode.AppendChild(oXmlCdata);
					((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]").AppendChild(oXmlNode);
				}
				else
				{
					oXmlCdata = objNode.FirstChild;
					oXmlCdata.InnerText = template.Templatecolor;
				}

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/publicurl");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = template.Publicurl;

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/adminurl");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = template.Adminurl;

				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//template[@id="+ template.TemplateId +"]/xslurl");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = template.Xslurl;

                website.Save();
			}
		}

		public void Move(Template template)
		{
			// to be implemented
		}
		
		public void Insert(Template template)
		{
			if (template == null)
				throw new ArgumentNullException("template");
			if (template.TemplateId != -1)
				throw new ArgumentException("Template object does have a templateId","template");

			int maxTemplateid = -1;
			XmlNodeList oXmlNodeRange;

			// get next templateid to use
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("template");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				if (int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value) > maxTemplateid)
					maxTemplateid = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
			}

			XmlElement oXmlElem;
			XmlCDataSection oXmlCdata;
			XmlAttribute oXmlAtr;
			XmlNode oXmlNode;
			XmlNode oXmlParentNode;
			XmlNode oXmlNodeNew;

			// create new element with new data
			oXmlElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("template","");
			oXmlAtr = oXmlElem.SetAttributeNode("id","");
			maxTemplateid ++;
			oXmlAtr.Value = maxTemplateid.ToString();
			template.TemplateId = maxTemplateid;

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("title","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Title);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("templatecolor","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Templatecolor);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("publicurl","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Publicurl);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("adminurl","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Adminurl);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("xslurl","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Xslurl);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("moduleclasspublic","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.ModuleClassPublic);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

            oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("moduleclassadmin", "");
            oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.ModuleClassAdmin);
            oXmlNode.AppendChild(oXmlCdata);
            oXmlElem.AppendChild(oXmlNode);

            oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("usercontrol", "");
            oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.UserControl);
            oXmlNode.AppendChild(oXmlCdata);
            oXmlElem.AppendChild(oXmlNode);

            oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("xslurl", "");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(template.Xslurl);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

            // insert new template in xml-document
			oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website");
			oXmlNodeNew = oXmlParentNode.AppendChild(oXmlElem);

			website.Save();

		}

	}
}
