using System;
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
    public class Extranet
	{

		public Extranet()
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

			return "";
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
			if(HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"];

			// Set password node on page on first load of admin page
			funcSetPasswordNode(page.Pageid);

			switch(action)
			{
				case("add"):
				{
					// Show form to add user to the extranet
					output = funcAddUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("doadd"):
				{
					// add user to the extranet
					output = funcDoAddUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("asign"):
				{
					// Show form to add user to the extranet
					output = funcAsignUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("doasign"):
				{
					// Show form to add user to the extranet
					output = funcDoAsignUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("delete"):
				{
					// Show form to add user to the extranet
					output = funcDeleteUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("dodelete"):
				{
					// add user to the extranet
					output = funcDoDeleteUser(admin, page);
					showMenu= false;
					showModuleMenu = false;
					break;
				}
				case("activate"):
				{
					// activate protection
					funcActivate(admin, page);
					output = funcList(admin, page);
					showMenu= true;
					showModuleMenu = true;
					break;
				}
				case("deactivate"):
				{
					// deactivate protection
					funcDeActivate(admin, page);
					output = funcList(admin, page);
					showMenu= true;
					showModuleMenu = true;
					break;
				}
				case("formedit"):
				{
					// edit extranet user
					output = formEditUser(admin, page);
					showMenu = false;
					showModuleMenu = false;
					break;
				}
				case("doedituser"):
				{
					// edit extranet user
					output = funcDoEditUser(admin, page);
					showMenu = false;
					showModuleMenu = false;
					break;
				}
				default:
				{
					// Show list of available methods
					output = funcList(admin, page);
					showMenu= true;
					showModuleMenu = true;
					break;
				}
			}

			return output;
		}

		private string formEditUser(Admin.Admin admin, Admin.Page page)
		{
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			StringBuilder output = new StringBuilder();
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode xmlNode;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2203,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("deleteuser") +"</h3>");

			// Get Xml and insert parameters
            oXmlExtranet.Load(Global.publicXmlPath + @"/extranet.xml");

			xmlNode = oXmlExtranet.SelectSingleNode("//selectedpage");
			xmlNode.InnerText = page.Pageid.ToString();

			xmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ HttpContext.Current.Request["userid"].ToString() +"']");

			if (xmlNode != null)
			{
				xslArg.AddExtensionObject("urn:localText", functions);
				xslArg.AddParam("user","",HttpContext.Current.Request["userid"]);

				output.Append (Functions.transformXml(oXmlExtranet,HttpContext.Current.Server.MapPath("/admin/modules/extranet/form_edit_user.xsl"),xslArg));
				return output.ToString();
			}
			else
				throw new ArgumentException("user not found","user");
		}

		private string funcDoEditUser(Admin.Admin admin, Admin.Page page)
		{
			string login = "";
			string fullname = "";
			string password = "";
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode oXmlNode;
			XmlCDataSection oXmlCdata;

			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2203,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("deleteuser") +"</h3>");

			if(HttpContext.Current.Request["login"] != null)
				login = HttpContext.Current.Request["login"].ToString().ToLower();
			if(HttpContext.Current.Request["fullname"] != null)
				fullname = HttpContext.Current.Request["fullname"].ToString();
			if(HttpContext.Current.Request["password"] != null)
                password = HttpContext.Current.Request["password"].ToString().ToLower();

			// Load the xml-file holding data on extranet users
            oXmlExtranet.Load(Global.publicXmlPath + "/extranet.xml");

			if (login != "")
			{

				// check if login exists
				oXmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']");
				if (oXmlNode != null)
				{
					oXmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']/fullname");
					if (oXmlNode != null)
						if (fullname != "")
							oXmlNode.FirstChild.InnerText = fullname;
						else
							oXmlNode.ParentNode.RemoveChild(oXmlNode);
					else
					{
						oXmlNode = oXmlExtranet.CreateElement("fullname","");
						oXmlCdata = oXmlExtranet.CreateCDataSection(fullname);
						oXmlNode.AppendChild(oXmlCdata);
						oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']").AppendChild(oXmlNode);
					}

					if (password != "")
					{
						oXmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']/password");
						oXmlNode.FirstChild.InnerText = password;
					}

                    output.Append(Functions.localText("usersaved") + Functions.localText("goto") + Functions.publicUrl());

					// save the xml-file holding data on extranet users
                    oXmlExtranet.Save(Global.publicXmlPath + "/extranet.xml");
				}
				else
				{
					throw new ArgumentException("user not found","user");
				}
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");

			return output.ToString();
		}

		private string funcAddUser(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2201,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("adduser") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			//get extranet xml
			oXml = OXmlExtranet(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/extranet/form_add_user.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string funcDoAddUser(Admin.Admin admin, Admin.Page page)
		{
			string login = "";
			string fullname = "";
			string password = "";
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode oXmlNode;

			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2201,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("adduser") +"</h3>");

			if(HttpContext.Current.Request["login"] != null)
				login = HttpContext.Current.Request["login"].ToString().ToLower();
			if(HttpContext.Current.Request["fullname"] != null)
				fullname = HttpContext.Current.Request["fullname"].ToString();
			if(HttpContext.Current.Request["password"] != null)
                password = HttpContext.Current.Request["password"].ToString().ToLower();

			// Load the xml-file holding data on extranet users
            oXmlExtranet.Load(Global.publicXmlPath + "/extranet.xml");

			if (login != "" && password != "")
			{
				// check if login exists
				oXmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']");
				if (oXmlNode == null)
				{
					XmlElement oXmlElem;
					XmlCDataSection oXmlCdata;
					XmlNode oXmlNodeParent;

					// create new element with new data
					oXmlElem = oXmlExtranet.CreateElement("user","");

					oXmlNode = oXmlExtranet.CreateElement("login","");
					oXmlCdata = oXmlExtranet.CreateCDataSection(login);
					oXmlNode.AppendChild(oXmlCdata);
					oXmlElem.AppendChild(oXmlNode);

					oXmlNode = oXmlExtranet.CreateElement("fullname","");
					oXmlCdata = oXmlExtranet.CreateCDataSection(fullname);
					oXmlNode.AppendChild(oXmlCdata);
					oXmlElem.AppendChild(oXmlNode);

					oXmlNode = oXmlExtranet.CreateElement("password","");
					oXmlCdata = oXmlExtranet.CreateCDataSection(password);
					oXmlNode.AppendChild(oXmlCdata);
					oXmlElem.AppendChild(oXmlNode);

					oXmlNodeParent = oXmlExtranet.SelectSingleNode("website");
					oXmlNodeParent.AppendChild(oXmlElem);

                    output.Append(Functions.localText("usersaved") + Functions.localText("goto") + Functions.publicUrl());

					// save the xml-file holding data on extranet users
                    oXmlExtranet.Save(Global.publicXmlPath + "/extranet.xml");
				}
				else
				{
					// There already exists one user with this login
					output.Append(Functions.localText("userexists") + Functions.publicUrl());
				}
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");

			return output.ToString();
		}

		private string funcAsignUser(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2202,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("asignuser") +"</h3>");

            XmlDocument oXmlExtranet = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			// Load the xml-file holding data on extranet users
            oXmlExtranet = OXmlExtranet(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/extranet/list_user.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);

			// Transform and return output to the profilEdit core for rendering
            xslt.Transform(oXmlExtranet, xslArg, writer, resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string funcDoAsignUser(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode oXmlNode;
			XmlNode oXmlNodeTemp;
			XmlNode oXmlNodeParent;
			XmlElement oXmlElem;
			string userid;

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2202,page.Pageid);

			if(HttpContext.Current.Request["add"] != null)
			{
				// add user to extranet
				userid = HttpContext.Current.Request["add"].ToString();
				oXmlExtranet = OXmlExtranet(page.Pageid);
                oXmlElem = (XmlElement)oXmlExtranet.SelectSingleNode("//user[not(.//accesszone=//selectedaccesszone) and login='"+ userid +"']");
				if(oXmlElem != null)
				{
					// only add user if accesszone does not already exist
					oXmlNode = oXmlExtranet.CreateElement("accesszone","");
					oXmlNode.InnerText = oXmlExtranet.SelectSingleNode("//selectedaccesszone").InnerText;
					oXmlElem.AppendChild(oXmlNode);

					// Save xml file with changes
                    oXmlExtranet.Save(Global.publicXmlPath + "/extranet.xml");
				}
			}
			else
				if(HttpContext.Current.Request["remove"] != null)
			{
				// add user to extranet
				userid = HttpContext.Current.Request["remove"].ToString();
				oXmlExtranet = OXmlExtranet(page.Pageid);
				oXmlNode = oXmlExtranet.SelectSingleNode("//user/accesszone[.=//selectedaccesszone and ../login='"+ userid +"']");
				if(oXmlNode != null)
				{
					oXmlNodeParent = oXmlNode.ParentNode;
					oXmlNodeTemp = oXmlNodeParent.RemoveChild(oXmlNode);

					// Save xml file with changes
                    oXmlExtranet.Save(Global.publicXmlPath + "/extranet.xml");
				}
			}

			return funcAsignUser(admin, page);
		}

		private string funcDeleteUser(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2203,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("deleteuser") +"</h3>");

			XmlDocument oXml = new XmlDocument();
			XslTransform xslt = new XslTransform();
			StringWriter writer = new StringWriter();

			//get extranet xml
			oXml = OXmlExtranet(page.Pageid);

			// Load the xsl-file to use for the current transformation
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/modules/extranet/list_edit_user.xsl"));
			XmlUrlResolver resolver = new XmlUrlResolver();

			// Add xslargument with Functions-class to use for lanugage specific texts in the xsl
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);

			// Transform and return output to the profilEdit core for rendering
			xslt.Transform(oXml,xslArg,writer,resolver);
			output.Append(writer.ToString());

			return output.ToString();
		}

		private string funcDoDeleteUser(Admin.Admin admin, Admin.Page page)
		{
			string login = "";
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode oXmlNode;

			StringBuilder output = new StringBuilder();

			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2203,page.Pageid);

			// append headers to output
			output.Append("<h2>"+ Functions.localText("extranet") + "</h2>");
			output.Append("<h3>"+ Functions.localText("deleteuser") +"</h3>");

			if(HttpContext.Current.Request["login"] != null)
				login = HttpContext.Current.Request["login"].ToString().ToLower();

			// Load the xml-file holding data on extranet users
            oXmlExtranet.Load(Global.publicXmlPath + "/extranet.xml");

			if (login != "")
			{

				// check if login exists
				oXmlNode = oXmlExtranet.SelectSingleNode("//user[login='"+ login +"']");
				if (oXmlNode != null)
				{
					XmlNode oXmlNodeParent;

					oXmlNodeParent = oXmlNode.ParentNode;
					oXmlNodeParent.RemoveChild(oXmlNode);

                    output.Append(Functions.localText("userdeleted") + Functions.localText("goto") + Functions.publicUrl());

					// save the xml-file holding data on extranet users
                    oXmlExtranet.Save(Global.publicXmlPath + "/extranet.xml");
				}
			}
			else
				throw new ArgumentException("user object doesn't have all required data","user");

			return output.ToString();
		}

		private void funcActivate(Admin.Admin admin, Admin.Page page)
		{
			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2204,page.Pageid);

			XmlDocument oXmlMenu = new XmlDocument();
			XmlNode oXmlNode;

			oXmlMenu = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;
			oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ page.Pageid +"]/password");
			if(oXmlNode != null)
			{
				oXmlNode.Attributes["aktiv"].Value = "1";

				// save menuXML. (off the record as lock status is not checked!)
				if (!Global.isDemo)
                    oXmlMenu.Save(Global.publicXmlPath + "/website.xml");
				Global.oMenuXml = oXmlMenu;

                Functions.publicUrlRedirect(page.Pageid, Functions.localText("extranetactivated"));

            }
			else
				throw new NullReferenceException("password node not found");
		}

		private void funcDeActivate(Admin.Admin admin, Admin.Page page)
		{
			// check for rights to this method. If user is not allowed he will be redirected to the default page
			admin.userHasAccess(2204,page.Pageid);

			XmlDocument oXmlMenu = new XmlDocument();
			XmlNode oXmlNode;

			oXmlMenu = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;
			oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ page.Pageid +"]/password");
			if(oXmlNode != null)
			{
				oXmlNode.Attributes["aktiv"].Value = "0";

				// save menuXML. (off the record as lock status is not checked!)
				if (!Global.isDemo)
                    oXmlMenu.Save(Global.publicXmlPath + "/website.xml");
				Global.oMenuXml = oXmlMenu;

                Functions.publicUrlRedirect(page.Pageid, Functions.localText("extranetdeactivated"));
            }
			else
				throw new NullReferenceException("password node not found");
		}

		private string funcList(Admin.Admin admin, Admin.Page page)
		{
			StringBuilder output = new StringBuilder();
            Admin.Template template = new Admin.Template(page.TemplateId);

			// append headers to output
            output.Append("<h2>" + Functions.localText("extranet") + "</h2><ul>");

			// check for rights to methods in this module and list methods that the user have access to
			if (admin.userHasAccess(2201, page.Pageid, false))
				output.Append("<li><a href='"+ template.Adminurl +"?pageid="+ page.Pageid+"&action=add'>"+ Functions.localText("adduser") +"</a></li>");
			if (admin.userHasAccess(2202, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=asign'>" + Functions.localText("asignuser") + "</a></li>");
			if (admin.userHasAccess(2203, page.Pageid, false))
                output.Append("<li><a href='" + template.Adminurl + "?pageid=" + page.Pageid + "&action=delete'>" + Functions.localText("deleteuser") + "</a></li>");
			if (admin.userHasAccess(2204, page.Pageid, false))
				if (funcIsProtected(page.Pageid))
					output.Append("<li><a href=\"javascript:void(0);\" onclick=\""+ Functions.localText("deactivateextranetlink").Replace(@"{//selectedpage}",page.Pageid.ToString()) +"\" style='cursor:hand'>"+ Functions.localText("deactivateextranet") +"</a></li>");
				else
                    output.Append("<li><a href=\"javascript:void(0);\" onclick=\"" + Functions.localText("activateextranetlink").Replace(@"{//selectedpage}", page.Pageid.ToString()) + "\" style='cursor:hand'>" + Functions.localText("activateextranet") + "</a></li>");
            output.Append("</ul>");

            return output.ToString();
		}
		private bool funcIsProtected(int pageid)
		{
			XmlDocument oXmlMenu = new XmlDocument();
			XmlNode oXmlNode;
			bool isProtected=false;

			oXmlMenu = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;
			oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ pageid +"]/password");
			if(oXmlNode != null)
			{
				isProtected = (oXmlNode.Attributes["aktiv"].Value=="1");
			}

			return isProtected;
		}

		private void funcSetPasswordNode(int pageid)
		{
			int maxAccessid = 0;
			XmlNodeList oXmlNodeRange;
			XmlDocument oXmlMenu = new XmlDocument();
			XmlDocument oXmlExtranet = new XmlDocument();
			XmlNode oXmlNode;
			XmlAttribute oXmlAtr;
			XmlElement oXmlElem;

			oXmlMenu = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml;
			oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ pageid +"]/password");
			if(oXmlNode == null)
			{
				// this is first load of extranet and the password node must be added
				// get next accessid to use
				oXmlNodeRange = oXmlMenu.SelectNodes("//page/password");
				for (int counter = 0;counter < oXmlNodeRange.Count; counter++)
				{
					if (int.Parse(oXmlNodeRange.Item(counter).Attributes["accesszone"].Value) > maxAccessid)
						maxAccessid = int.Parse(oXmlNodeRange.Item(counter).Attributes["accesszone"].Value);
				}

				// check if accessids in extranetxml
                oXmlExtranet.Load(Global.publicXmlPath + "/extranet.xml");
				oXmlNodeRange = oXmlExtranet.SelectNodes("//accesszone");
				for (int counter = 0;counter < oXmlNodeRange.Count; counter++)
				{
					if (int.Parse(oXmlNodeRange.Item(counter).InnerText) > maxAccessid)
						maxAccessid = int.Parse(oXmlNodeRange.Item(counter).InnerText);
				}
				
				//add extranet node to menuxml
				oXmlElem = oXmlMenu.CreateElement("password","");
				oXmlAtr = oXmlElem.SetAttributeNode("accesszone","");
				oXmlAtr.Value = (maxAccessid+1).ToString();
				oXmlAtr = oXmlElem.SetAttributeNode("aktiv","");
				oXmlAtr.Value = "0";
				oXmlNode = oXmlMenu.SelectSingleNode("//page[@id="+ pageid +"]");
				oXmlNode.AppendChild(oXmlElem);

				// save menuXML. (off the record as lock status is not checked!)
				if (!Global.isDemo)
                    oXmlMenu.Save(Global.publicXmlPath + "/website.xml");
				Global.oMenuXml = oXmlMenu;
			}
		}

		private XmlDocument OXmlExtranet(int pageid)
		{
			XmlDocument output = new XmlDocument();
			XmlNode oXmlNode;
			XmlNode oXmlNodeTemp;
			string Zoneid;

			// Load the xml-file holding data on extranet users
            output.Load(Global.publicXmlPath + "/extranet.xml");

			// Insert current pageid in the extranet xml
			oXmlNode = output.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageid.ToString();

			// Find accesszone-id for passwordpage in menuxml
			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ pageid +"]/password");
			if(oXmlNode != null)
			{
				Zoneid = oXmlNode.Attributes["accesszone"].Value;
				oXmlNodeTemp = output.SelectSingleNode("//selectedaccesszone");
				oXmlNodeTemp.InnerText = Zoneid;
			}
			return output;
		}


	}
}
