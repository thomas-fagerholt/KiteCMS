using System;
using System.Xml;
using System.Web;
using System.Xml.XPath;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the user object
	/// </summary>
	public class UserData
	{
		Website website = new Website();

		public void User()
		{
		}

		public void Load(User user)
		{
			XmlNode oXmlNode;
			XmlDocument oXmlUser;
			XmlNodeList oXmlNodeRange;

			ValueCollection ModuleAccess = new ValueCollection();
			ValueCollection PageAccess = new ValueCollection();

			if (user.UserId == -1)
				throw new ArgumentNullException("userid");

			oXmlUser = website.LoadAccessXml();
			oXmlNode = oXmlUser.SelectSingleNode("//user[@id='"+ user.UserId +"']");
			if (oXmlNode == null)
				throw new Exception("Userid does not exist");
			
			user.UserId = int.Parse(oXmlNode.Attributes["id"].Value);
			user.Active = int.Parse(oXmlNode.Attributes["active"].Value);
			user.Failedlogins = int.Parse(oXmlNode.Attributes["failedlogins"].Value);
			user.Username = oXmlUser.SelectSingleNode("//user[@id='"+ user.UserId +"']/username").InnerText;
			if (oXmlUser.SelectSingleNode("//user[@id='"+ user.UserId +"']/fullname") != null)
				user.Fullname = oXmlUser.SelectSingleNode("//user[@id='"+ user.UserId +"']/fullname").InnerText;
			user.EncryptedPassword = oXmlUser.SelectSingleNode("//user[@id='"+ user.UserId +"']/password").InnerText;
			if (oXmlNode.Attributes["changepassword"] != null)
				user.Changepassword = int.Parse(oXmlNode.Attributes["changepassword"].Value);
			else
				user.Changepassword = 0;

			oXmlNodeRange = oXmlNode.SelectNodes("access");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				ModuleAccess.Add(oXmlNodeRange.Item(counter).InnerText);
			}
			user.ModuleAccess = ModuleAccess;

			oXmlNodeRange = oXmlNode.SelectNodes("menuid");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				PageAccess.Add(oXmlNodeRange.Item(counter).InnerText);
			}
			user.PageAccess = PageAccess;

            oXmlNodeRange = oXmlNode.SelectNodes("shortcut");
            string[] Shortcuts = new string[oXmlNodeRange.Count];
            for (int counter = 0; counter < oXmlNodeRange.Count; counter++)
            {
                Shortcuts[counter] = oXmlNodeRange.Item(counter).InnerText;
            }
            user.Shortcuts = Shortcuts;
        }

		public void Delete(User user)
		{
			XmlNode objNode;
			XmlNode objNodeParent;
			XmlNode objNodeNew;
			XmlDocument oXmlUser ;

			if (user == null)
				throw new ArgumentNullException("user");
			if (user.UserId <= 3 && Global.isDemo)
				return;
			if (user.UserId == -1)
				throw new ArgumentException("User object doesn't have a userid","user");

			oXmlUser = website.LoadAccessXml();

			objNode = oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]");
			objNodeParent = objNode.ParentNode;
			objNodeNew = objNodeParent.RemoveChild(objNode);

			user.UserId = -1;

			website.SaveAccessXml(oXmlUser);
		}

		public void Save(User user)
		{
			if (user == null)
				throw new ArgumentNullException("page");
			if (user.UserId <= 3 && Global.isDemo)
				return;
			if (user.Username == "")
				throw new ArgumentException("User object does not have a username","username");
			if (user.EncryptedPassword == "")
				throw new ArgumentException("User object does not have a password","password");

			if (user.UserId == -1)
				Insert(user);
			else
			{
				XmlNode objNode;
				XmlNodeList objNodeList;
				XmlNode objParentNode;
				XmlNode oXmlCdata;
				XmlElement oXmlElem;
				XmlDocument oXmlUser;
				XmlAttribute oXmlAtr;

				oXmlUser = website.LoadAccessXml();

				oXmlElem = (XmlElement)oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]");
				oXmlElem.Attributes["active"].Value = user.Active.ToString();
				oXmlElem.Attributes["failedlogins"].Value = user.Failedlogins.ToString();
				if (oXmlElem.Attributes["changepassword"] == null)
				{
					// this attribute does not exist and must be added
					oXmlAtr = oXmlElem.SetAttributeNode("changepassword","");
					oXmlAtr.Value = user.Changepassword.ToString();
				}
				else
					oXmlElem.Attributes["changepassword"].Value = user.Changepassword.ToString();

				objNode = oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]/username");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = user.Username;

				objParentNode = oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]");

				objNode = oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]/fullname");
				if (objNode != null)
				{
					oXmlCdata = objNode.FirstChild;
					oXmlCdata.InnerText = user.Fullname;
				}
				else
				{
					objNode = oXmlUser.CreateElement("fullname","");
					oXmlCdata = oXmlUser.CreateCDataSection(user.Fullname);
					objNode.AppendChild(oXmlCdata);
					objParentNode.AppendChild(objNode);
				}

				objNode = oXmlUser.SelectSingleNode("//user[@id="+ user.UserId +"]/password");
				oXmlCdata = objNode.FirstChild;
				oXmlCdata.InnerText = user.EncryptedPassword;

				// Remove old nodes
				objNodeList = objParentNode.SelectNodes("access");
				for (int counter = 0;counter <objNodeList.Count; counter++)
				{
					objParentNode.RemoveChild(objNodeList.Item(counter));
				}

                objNodeList = objParentNode.SelectNodes("menuid");
                for (int counter = 0; counter < objNodeList.Count; counter++)
                {
                    objParentNode.RemoveChild(objNodeList.Item(counter));
                }

                objNodeList = objParentNode.SelectNodes("shortcut");
                for (int counter = 0; counter < objNodeList.Count; counter++)
                {
                    objParentNode.RemoveChild(objNodeList.Item(counter));
                }

				// Add new nodes
				for(int counter = 0; counter < user.ModuleAccess.Count; counter++)
				{
					objNode = oXmlUser.CreateElement("access","");
					objNode.InnerText = user.ModuleAccess[counter].ToString();
					oXmlElem.AppendChild(objNode);
				}

                for (int counter = 0; counter < user.PageAccess.Count; counter++)
                {
                    objNode = oXmlUser.CreateElement("menuid", "");
                    objNode.InnerText = user.PageAccess[counter].ToString();
                    oXmlElem.AppendChild(objNode);
                }

                if (user.Shortcuts != null)
                    for (int counter = 0; counter < user.Shortcuts.Length; counter++)
                    {
                        objNode = oXmlUser.CreateElement("shortcut", "");
                        objNode.InnerText = user.Shortcuts[counter].ToString();
                        oXmlElem.AppendChild(objNode);
                    }

				website.SaveAccessXml(oXmlUser);
			}
		}
		
		public void Insert(User user)
		{
			if (user == null)
				throw new ArgumentNullException("user");
			if (user.UserId != -1)
				throw new ArgumentException("User object does have a userId","user");

			int maxUserid = -1;
			XmlNodeList oXmlNodeRange;
			XmlDocument oXmlUser;

			oXmlUser = website.LoadAccessXml();

			if (oXmlUser.SelectSingleNode("//user[username='"+ user.Username +"']") != null)
				throw new ArgumentException("User name exists","username");

			// get next userid to use
			oXmlNodeRange = oXmlUser.GetElementsByTagName("user");
			for (int counter = 0;counter <oXmlNodeRange.Count; counter++)
			{
				if (int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value) > maxUserid)
					maxUserid = int.Parse(oXmlNodeRange.Item(counter).Attributes["id"].Value);
			}

			XmlElement oXmlElem;
			XmlCDataSection oXmlCdata;
			XmlAttribute oXmlAtr;
			XmlNode oXmlNode;
			XmlNode oXmlNodeParent;

			// create new element with new data
			oXmlElem = oXmlUser.CreateElement("user","");
			oXmlAtr = oXmlElem.SetAttributeNode("id","");
			maxUserid ++;
			oXmlAtr.Value = maxUserid.ToString();
			oXmlAtr = oXmlElem.SetAttributeNode("active","");
			oXmlAtr.Value = user.Active.ToString();
			oXmlAtr = oXmlElem.SetAttributeNode("changepassword","");
			oXmlAtr.Value = user.Changepassword.ToString();
			oXmlAtr = oXmlElem.SetAttributeNode("failedlogins","");
			oXmlAtr.Value = "0";

			oXmlNode = oXmlUser.CreateElement("username","");
			oXmlCdata = oXmlUser.CreateCDataSection(user.Username);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = oXmlUser.CreateElement("fullname","");
			oXmlCdata = oXmlUser.CreateCDataSection(user.Fullname);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			oXmlNode = oXmlUser.CreateElement("password","");
			oXmlCdata = oXmlUser.CreateCDataSection(user.EncryptedPassword);
			oXmlNode.AppendChild(oXmlCdata);
			oXmlElem.AppendChild(oXmlNode);

			for(int counter = 0; counter < user.ModuleAccess.Count; counter++)
			{
				oXmlNode = oXmlUser.CreateElement("access","");
				oXmlNode.InnerText = user.ModuleAccess[counter].ToString();
				oXmlElem.AppendChild(oXmlNode);
			}

			for(int counter = 0; counter < user.PageAccess.Count; counter++)
			{
				oXmlNode = oXmlUser.CreateElement("menuid","");
				oXmlNode.InnerText = user.PageAccess[counter].ToString();
				oXmlElem.AppendChild(oXmlNode);
			}

			oXmlNodeParent = oXmlUser.SelectSingleNode("website");
			oXmlNodeParent.AppendChild(oXmlElem);

			website.SaveAccessXml(oXmlUser);
		}

		public bool UsernameExists(User user)
		{
			XmlDocument oXmlUser;
			bool usernameExists = false;

			oXmlUser = website.LoadAccessXml();

			if (oXmlUser.SelectSingleNode("//user[username='"+ user.Username +"']") != null)
				usernameExists = true;

			return usernameExists;
		}
	}
}
