using System;
using System.Web;
using System.Xml;
using System.Threading;

namespace KiteCMS.Admin.core
{
	/// <summary>
	/// This is the data class for the website object
	/// </summary>

	public class Website
	{
		public static bool LockMenuXml()
		{
			bool isLocked = false;

			// Loop until menuxml is free
			for (int counter = 0 ; counter < 10; counter ++)
			{
				isLocked = TryToLockMenuXml();
				if (isLocked)
					break;
				else
					Thread.Sleep(250);
			}
			
			return isLocked;
		}

		public static bool UnLockMenuXml()
		{
			if(Global.menuXmlLockSession == HttpContext.Current.Session.SessionID)
			{
                Global.menuXmlLockSession = "";
				Global.menuXmlLockTime = new DateTime();
				return true;
			}
			else
				return false;
		}

		public void Save()
		{
			if (Global.menuXmlLockSession == HttpContext.Current.Session.SessionID && Global.menuXmlLockSession != "")
			{
                if (!Global.isDemo)
                {
                    try
                    {
                        ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.Save(Global.publicXmlPath + "/website.xml");
                    }
                    catch (System.IO.IOException ex)
                    {
                        throw new Exception(ex.InnerException + "<br/>" + "website.xml content:" + ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.OuterXml);
                    }
                }
				Global.oMenuXml = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;

				UnLockMenuXml();
			}
			else
                throw new ArgumentException("Menu locked by another user");
		}

		public XmlDocument LoadAccessXml()
		{
			XmlDocument output = new XmlDocument();
            output.Load(Global.adminXmlPath + "/access.webinfo");
			return output;
		}

		public void SaveAccessXml(XmlDocument input)
		{
            try
            {
                input.Save(Global.adminXmlPath + "/access.webinfo");
            }
            catch (System.IO.IOException ex)
            {
                throw new Exception(ex.InnerException + "<br/>" + "access.webinfo content:" + input.OuterXml);
            }

		}

		private static bool TryToLockMenuXml()
		{
			if( DateTime.Compare(DateTime.Now,Global.menuXmlLockTime.AddMinutes(1))>1 || Global.menuXmlLockSession == "")
			{
				Global.menuXmlLockSession = HttpContext.Current.Session.SessionID;
				Global.menuXmlLockTime = DateTime.Now;
				return true;
			}
			else
				return false;
		}

	}
}
