using System;
using System.Collections.Specialized;
using System.Xml;
using System.Web;
using System.Xml.XPath;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the page object
	/// </summary>
	public class PageData
	{
		Website website = new Website();

		public void Load(Page page)
		{
			Load(page,false);
		}

		public void Load(Page page, bool withlock)
		{
			XmlNode oXmlNode;
			XmlNode oXmlNodeProperty;
			XmlNodeList oXmlNodeParameters;
			ParameterCollection parameters = new ParameterCollection();
            StringDictionary contentHolders = new StringDictionary();
            StringDictionary draftContentHolders = new StringDictionary();

			if (page == null)
				throw new ArgumentNullException("page");
			if (page.Pageid == -1)
				throw new ArgumentException("Page object doesn't have a pageId","page");

			try
			{
				// Lock menuxml to prevent concurrency problems when saving
				if(withlock)
					core.Website.LockMenuXml();

				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");
				page.Pageid = int.Parse(oXmlNode.Attributes["id"].InnerText);
				if (oXmlNode.Attributes["zone"] != null)
					page.ZoneId = int.Parse(oXmlNode.Attributes["zone"].InnerText);
                page.Created = oXmlNode.Attributes["created"].InnerText;
                page.LastUpdated = oXmlNode.Attributes["updated"].InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("menutitle");
				page.Title = oXmlNodeProperty.InnerText;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("public");
				page.IsPublic = int.Parse(oXmlNodeProperty.InnerText);

                oXmlNodeParameters = oXmlNode.SelectNodes("contentholders/child::node()");
                for (int counter = 0; counter < oXmlNodeParameters.Count; counter++)
                {
                    contentHolders.Add(oXmlNodeParameters.Item(counter).LocalName, oXmlNodeParameters.Item(counter).FirstChild.InnerText);
                }
                page.ContentHolders = contentHolders;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("friendlyurl");
				if(oXmlNodeProperty != null)
					page.FriendlyUrl = oXmlNodeProperty.InnerText;

                oXmlNodeParameters = oXmlNode.SelectNodes("parameters/child::node()");
				for (int counter = 0; counter < oXmlNodeParameters.Count; counter ++)
				{
					Parameter parameter = new Parameter();
					parameter.Key = oXmlNodeParameters.Item(counter).LocalName;
					parameter.Value = oXmlNodeParameters.Item(counter).FirstChild.InnerText;
					parameters.Add(parameter);
				}
				page.Parameters = parameters;

				oXmlNodeProperty = oXmlNode.SelectSingleNode("usetemplate");
				page.TemplateId = int.Parse(oXmlNodeProperty.InnerText);

				oXmlNodeProperty = oXmlNode.SelectSingleNode("draft");
				
				if(oXmlNodeProperty != null)
				{
					page.HasDraft = 1;

                    oXmlNodeParameters = oXmlNode.SelectNodes("draft/contentholders/child::node()");
                    for (int counter = 0; counter < oXmlNodeParameters.Count; counter++)
                    {
                        draftContentHolders.Add(oXmlNodeParameters.Item(counter).LocalName, oXmlNodeParameters.Item(counter).FirstChild.InnerText);
                    }
                    page.DraftContentHolders = draftContentHolders;
                }
				else
				{
					page.HasDraft = 0;
				}

				//boxmodule
				BoxCollection boxColl = new BoxCollection();

				// Load boxes asigned to page
				oXmlNodeParameters = oXmlNode.SelectNodes("boxmodule[@boxid]");
				if(oXmlNodeParameters != null)
				{
					for(int counter = 0; counter < oXmlNodeParameters.Count; counter ++)
						boxColl.Add(new Box(int.Parse(oXmlNodeParameters.Item(counter).Attributes["boxid"].Value)));

				}

				// Load boxes from parents
				oXmlNodeParameters = oXmlNode.SelectNodes("ancestor::page/boxmodule[@boxid]");
				if(oXmlNodeParameters != null)
				{
					for(int counter = 0; counter < oXmlNodeParameters.Count; counter ++)
					{
						Box box = new Box(int.Parse(oXmlNodeParameters.Item(counter).Attributes["boxid"].Value));
						if(box.Cascade)
							boxColl.Add(box);
					}
				}
				page.Boxes = boxColl;

				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[page/@id="+ page.Pageid +"]");
				if (oXmlNode != null && oXmlNode.Attributes["id"] != null)
				{
					page.ParentPageid = int.Parse(oXmlNode.Attributes["id"].InnerText);
				}
				else
				{
					page.ParentPageid = -1;
				}

				try
				{
					XPathNavigator nav = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateNavigator();
					XPathNodeIterator ni = nav.Select("//page[@id="+ page.Pageid +"]/following-sibling::page");

					ni.MoveNext();
					XPathNavigator nav2 = ni.Current.Clone();
					page.FollowinSiblingPageId = int.Parse(nav2.GetAttribute("id", ""));
				}
				catch
				{
					page.FollowinSiblingPageId = -1;
				}

			}
			catch
			{
				throw new Exception ("Error loading pageproperties");
			}
		}

		public void Delete(Page page)
		{
			XmlNode objNode;
			XmlNode objNodeParent;
			XmlNode objNodeNew;

			if (page == null)
				throw new ArgumentNullException("page");
			if (page.Pageid == -1)
				throw new ArgumentException("Page object doesn't have a pageId","page");

            if (((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website/page[@id="+ page.Pageid +"]")!=null && ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("/website/page").Count==1)
                throw new ArgumentException("Trying to delete all pages","page");

			objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");
			objNodeParent = objNode.ParentNode;
			objNodeNew = objNodeParent.RemoveChild(objNode);

			website.Save();
		}

		public void Copy(Page page,int newParentPageId, int newFollowinSiblingPageId, bool includeSubpages, bool includeContent)
		{
			XmlNode objNode;
			XmlNode objNodeSource;
			XmlNode objNodeParent;
			XmlNode objNodeChild;
			XmlNode objNewMenu;
			XmlNodeList nodes;
			int maxPageid = -1;
            int maxAccesszoneId = -1;

			if (page == null)
				throw new ArgumentNullException("page");
			if (page.Pageid == -1)
				throw new ArgumentException("Page object doesn't have a pageId","page");

			objNodeSource = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");
			objNode = objNodeSource.Clone();

			if (!includeSubpages)
			{
				//remove subpages
				nodes = objNode.SelectNodes("page");
				for (int counter = 0; counter <= nodes.Count-1;counter++)
					objNode.RemoveChild(nodes.Item(counter));
			}

			if (!includeContent)
			{
				//remove content
				nodes = objNode.SelectNodes("//contentholders");
				for (int counter = 0; counter <= nodes.Count-1;counter++)
                    objNode.RemoveChild(nodes[counter]);
            }

            // get next AccesszoneId to use
            nodes = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("password");
            for (int counter = 0; counter < nodes.Count; counter++)
            {
                if (int.Parse(nodes.Item(counter).Attributes["accesszone"].Value) > maxAccesszoneId)
                    maxAccesszoneId = int.Parse(nodes.Item(counter).Attributes["accesszone"].Value);
            }

            //asign new accesszoneid
            nodes = objNode.SelectNodes("//password");
            for (int counter = 0; counter <= nodes.Count - 1; counter++)
            {
                maxAccesszoneId++;
                nodes[counter].Attributes["accesszone", ""].Value = maxAccesszoneId.ToString();
            }

			// get next pageid to use
			nodes = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("page");
			for (int counter = 0;counter <nodes.Count; counter++)
			{
				if (int.Parse(nodes.Item(counter).Attributes["id"].Value) > maxPageid)
					maxPageid = int.Parse(nodes.Item(counter).Attributes["id"].Value);
			}

            //asign new pageids
			maxPageid ++;
			objNode.Attributes["id",""].Value = maxPageid.ToString();
            objNode.Attributes["created"].Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");
            objNode.Attributes["updated"].Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");

			if(objNode.SelectSingleNode("friendlyurl") != null)
				objNode.SelectSingleNode("friendlyurl").FirstChild.InnerText = "";

			nodes = objNode.SelectNodes("//page");
			for (int counter = 0; counter <= nodes.Count-1;counter++)
			{
                nodes.Item(counter).Attributes["created"].Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");
                nodes.Item(counter).Attributes["updated"].Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");

                maxPageid++;
				nodes.Item(counter).Attributes["id",""].Value = maxPageid.ToString();

				if(nodes.Item(counter).SelectSingleNode("friendlyurl") != null)
					nodes.Item(counter).SelectSingleNode("friendlyurl").FirstChild.InnerText = "";
			}

			//place new nodes in menutree
			if(newFollowinSiblingPageId != -1)
			{
				objNodeChild = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ newFollowinSiblingPageId +"]");
				if (objNodeChild.ParentNode != null)
					objNodeParent = objNodeChild.ParentNode;
				else
					objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("/website");
				objNewMenu = objNodeParent.InsertBefore(objNode,objNodeChild);
			}
			else
			{
				objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ newParentPageId +"]");
				if (objNodeParent == null)
					objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page").ParentNode;

				objNewMenu = objNodeParent.AppendChild(objNode);
			}

			website.Save();
		}

		public void Save(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");

			if (page.Pageid == -1)
				Insert(page);
			else
			{
				XmlNode objPageNode;
				XmlNode objNode;
				XmlNode objNode2;
				XmlNode oXmlCdata;
				XmlNodeList objNodes;
				XmlCDataSection oXmlCdataNode;
				XmlElement objNodeParent;
				XmlElement objElem;
				XmlAttribute oXmlAtr;

				objPageNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");

				objPageNode.Attributes["updated"].Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");

				objNode = objPageNode.SelectSingleNode("menutitle");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = page.Title;

				//Remove possible old node
				objNode = objPageNode.SelectSingleNode("friendlyurl");
				if (objNode != null)
					objPageNode.RemoveChild(objNode);
				if (page.FriendlyUrl != null && page.FriendlyUrl != "")
				{
					objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("friendlyurl","");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.FriendlyUrl.ToLower());
					objNode.AppendChild(oXmlCdata);
					objPageNode.AppendChild(objNode);
				}

				// Start with deleting possible parameters
				objNode = objPageNode.SelectSingleNode("parameters");
				if (objNode != null)
					objPageNode.RemoveChild(objNode);

				if (page.Parameters != null && page.Parameters.Count != 0)
				{
					objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("parameters","");
					foreach (string key in page.Parameters.Keys)
					{
						objNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key,"");
						oXmlCdataNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.Parameters.Get(key));
						objNode2.AppendChild(oXmlCdataNode);
						objNode.AppendChild(objNode2);
					}
					objPageNode.AppendChild(objNode);
				}

				// Start with deleting possible contentholders
				objNode = objPageNode.SelectSingleNode("contentholders");
                if (objNode != null)
                    objPageNode.RemoveChild(objNode);

                if (page.ContentHolders != null && page.ContentHolders.Count != 0)
                {
                    objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("contentholders", "");
                    foreach (string key in page.ContentHolders.Keys)
                    {
                        objNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key, "");
                        oXmlCdataNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.ContentHolders[key]);
                        objNode2.AppendChild(oXmlCdataNode);
                        objNode.AppendChild(objNode2);
                    }
                    objPageNode.AppendChild(objNode);
                }

				objNode = objPageNode.SelectSingleNode("public");
				objNode.InnerText = page.IsPublic.ToString();

				objNode = objPageNode.SelectSingleNode("usetemplate");
				objNode.InnerText = page.TemplateId.ToString();

				//Remove possible old node
				objNode = objPageNode.SelectSingleNode("draft");
				if (objNode != null)
					objPageNode.RemoveChild(objNode);

				//Remove possible old node
				objNodes = objPageNode.SelectNodes("boxmodule");
				if (objNodes != null)
					for (int counter = 0; counter < objNodes.Count; counter ++)
                        objPageNode.RemoveChild(objNodes.Item(counter));
				foreach(Box box in page.Boxes)
				{
					objElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("boxmodule","");
					oXmlAtr = objElem.SetAttributeNode("boxid","");
					oXmlAtr.Value = box.BoxId.ToString();
					objPageNode.AppendChild(objElem);
				}

				if(page.HasDraft==1)
				{
                    objNodeParent = (XmlElement)objPageNode.SelectSingleNode("draft");
                    if (objNodeParent == null)
    					objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("draft","");

					// Start with deleting possible contentholders
					objNode = objPageNode.SelectSingleNode("draft/contentholders");
                    if (objNode != null)
                        objPageNode.RemoveChild(objNode);

                    if (page.DraftContentHolders != null && page.DraftContentHolders.Count != 0)
                    {
                        objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("contentholders", "");
                        foreach (string key in page.DraftContentHolders.Keys)
                        {
                            objNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key, "");
                            oXmlCdataNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.DraftContentHolders[key]);
                            objNode2.AppendChild(oXmlCdataNode);
                            objNode.AppendChild(objNode2);
                        }
                        objNodeParent.AppendChild(objNode);
                    }
					objPageNode.AppendChild(objNodeParent);
				}

				website.Save();
			}

		}

		public void Move(Page page, int newParentPageId, int newFollowingSiblingPageId)
		{
			XmlNode objNode;
			XmlNode objNodeChild;
			XmlNode objNodeParent;
			XmlNode objNewMenu;

			if (page == null)
				throw new ArgumentNullException("page");
			if (page.Pageid == -1)
				throw new ArgumentException("Page object doesn't have a pageId","page");

			if(newFollowingSiblingPageId != -1)
			{
				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");
				objNodeChild = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ newFollowingSiblingPageId +"]");
				objNodeParent = objNodeChild.ParentNode;
				objNewMenu = objNodeParent.InsertBefore(objNode,objNodeChild);
			}
			else
			{
				objNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.Pageid +"]");
				objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ newParentPageId +"]");
				if (objNodeParent == null)
					objNodeParent = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page").ParentNode;

				objNewMenu = objNodeParent.AppendChild(objNode);
			}

			//TODO: Should pageproperties be reloaded to get right parent and followingsibling?
			website.Save();
		}
		
		public void Insert(Page page)
		{
			if (page == null)
				throw new ArgumentNullException("page");
			if (page.Pageid != -1)
				throw new ArgumentException("Page object does have a pageId","page");

			int maxPageid = -1;
			XmlNodeList oXmlNodeRange;
			XmlNode oXmlNodeTemp;
			XmlNode oXmlNodeNew;
			XmlNode oXmlParentNode;

			// get next pageid to use
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.GetElementsByTagName("page");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				if (int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value) > maxPageid)
					maxPageid = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
			}

			XmlElement oXmlElem;
			XmlElement oXmlElemNode;
			XmlCDataSection oXmlCdata;
			XmlAttribute oXmlAtr;
			XmlNode oXmlNode;
			XmlNode oXmlNode2;

			// create new element with new data
			oXmlElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("page","");
			oXmlAtr = oXmlElem.SetAttributeNode("id","");
			maxPageid ++;
			oXmlAtr.Value = maxPageid.ToString();
            page.Pageid = maxPageid;
			oXmlAtr = oXmlElem.SetAttributeNode("zone","");
			oXmlAtr.Value = "0";
			oXmlAtr = oXmlElem.SetAttributeNode("language","");
			oXmlAtr.Value = "dk";
            oXmlAtr = oXmlElem.SetAttributeNode("created", "");
            oXmlAtr.Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");
            oXmlAtr = oXmlElem.SetAttributeNode("updated", "");
            oXmlAtr.Value = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm");

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("usetemplate","");
			oXmlNode.InnerText = page.TemplateId.ToString();
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("public","");
			oXmlNode.InnerText = page.IsPublic.ToString();
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("menutitle","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.Title);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

            if (page.ContentHolders != null && page.ContentHolders.Count != 0)
            {
                oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("contentholders", "");
                foreach (string key in page.ContentHolders.Keys)
                {
                    oXmlNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key, "");
                    oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.ContentHolders[key]);
                    oXmlNode2.AppendChild(oXmlCdata);
                    oXmlNode.AppendChild(oXmlNode2);
                }
                oXmlElem.AppendChild(oXmlNode);
            }

			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("friendlyurl","");
			oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.FriendlyUrl.ToLower());
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			if (page.Parameters != null && page.Parameters.Count != 0)
			{
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("parameters","");
				foreach (string key in page.Parameters.Keys)
				{
					oXmlNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key,"");
					oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.Parameters.Get(key));
					oXmlNode2.AppendChild(oXmlCdata);
					oXmlNode.AppendChild(oXmlNode2);
				}
				oXmlElem.AppendChild(oXmlNode);
			}

			if (page.Boxes != null)
				foreach(Box box in page.Boxes)
				{
					oXmlElemNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("boxmodule","");
					oXmlAtr = oXmlElemNode.SetAttributeNode("boxid","");
					oXmlAtr.Value = box.BoxId.ToString();
					oXmlElem.AppendChild(oXmlElemNode);
				}

			if(page.HasDraft==1)
			{
                oXmlElem = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("draft", "");

                if (page.DraftContentHolders != null && page.DraftContentHolders.Count != 0)
                {
                    oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("contentholders", "");
                    foreach (string key in page.DraftContentHolders.Keys)
                    {
                        oXmlNode2 = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement(key, "");
                        oXmlCdata = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateCDataSection(page.DraftContentHolders[key]);
                        oXmlNode2.AppendChild(oXmlCdata);
                        oXmlNode.AppendChild(oXmlNode2);
                    }
                    oXmlElem.AppendChild(oXmlNode);
                }
			}

			// get parentNode
			if (page.ParentPageid == -1)
				oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page").ParentNode;
			else
				oXmlParentNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.ParentPageid +"]");

			if (page.FollowinSiblingPageId != -1 && page.FollowinSiblingPageId != -2)
			{
				oXmlNodeTemp = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ page.FollowinSiblingPageId +"]");
				oXmlParentNode = oXmlNodeTemp.ParentNode;
				oXmlNodeNew = oXmlParentNode.InsertBefore(oXmlElem,oXmlNodeTemp);
			}
			else
				oXmlNodeNew = oXmlParentNode.AppendChild(oXmlElem);

			website.Save();
		}

		public bool FriendlyUrlExists(Page page, string friendlyUrl)
		{
			bool friendlyUrlExists = false;

			if (friendlyUrl!="")
                if (((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[friendlyurl='" + friendlyUrl.ToLower().Replace("'", "") + "' and @id !='" + page.Pageid + "']") != null) 
					friendlyUrlExists = true;

			if(!friendlyUrlExists)
			{
				// test for reserved urls
                if (friendlyUrl.ToLower() == "/admin" || friendlyUrl.ToLower().StartsWith("/admin/") || friendlyUrl.ToLower() == "/data" || friendlyUrl.ToLower().StartsWith("/data/") || friendlyUrl.ToLower() == "/bin" || friendlyUrl.ToLower().StartsWith("/bin/") || friendlyUrl.ToLower() == "/classes" || friendlyUrl.ToLower().StartsWith("/classes/") || friendlyUrl.ToLower() == "/images" || friendlyUrl.ToLower().StartsWith("/images/") || friendlyUrl.ToLower() == "/modules" || friendlyUrl.ToLower().StartsWith("/modules/"))
					friendlyUrlExists = true;
			}
			return friendlyUrlExists;
		}
	}
}
