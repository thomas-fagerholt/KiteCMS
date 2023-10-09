using System;
using System.Web;
using System.Xml;
using KiteCMS.Admin;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Template
	{
		private int templateId = -1;
		private string publicurl = "";
		private string xslurl = "";
		private string moduleClassPublic = "";
        private string userControl = "";

		public Template(int templateId)
		{
            KiteCMS.Admin.Template template = new KiteCMS.Admin.Template(templateId);

			this.publicurl = template.Publicurl;
			this.xslurl = template.Xslurl;
			this.moduleClassPublic = template.ModuleClassPublic;
            this.userControl = template.UserControl;
		}

		public int TemplateId
		{
			get
			{
				return templateId;
			}
		}
		
		public string Publicurl
		{
			get
			{
				return publicurl;
			}
		}
				
		public string Xslurl
		{
			get
			{
				return xslurl;
			}
		}

        public string ModuleClassPublic
        {
            get
            {
                return moduleClassPublic;
            }
        }

        public string UserControl
        {
            get
            {
                return userControl;
            }
        }
    }
}
