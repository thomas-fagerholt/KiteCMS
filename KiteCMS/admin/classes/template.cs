using System;
using System.Web;
using System.Xml;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Template
	{
		private int templateId = -1;
		private string title = "";
		private string publicurl = "";
		private string adminurl = "";
		private string xslurl = "";
		private string moduleClassPublic = "";
        private string moduleClassAdmin = "";
        private string userControl = "";
        private string templatecolor = "";

		public Template()
		{
		}

		public Template(bool withlock)
		{
			// Lock menuxml to prevent concurrency problems when saving
			if(withlock)
				core.Website.LockMenuXml();
		}

		public Template(int templateId)
		{
			TemplateData templateData = new TemplateData();
			this.templateId = templateId;
			templateData.Load(this);
		}

		public Template(int templateId, bool withlock)
		{
			TemplateData templateData = new TemplateData();
			this.templateId = templateId;
			templateData.Load(this, withlock);
		}

		public int Save()
		{
			TemplateData templateData = new TemplateData();
			templateData.Save(this);

			return this.TemplateId;
		}

		public int Delete(int newTemplateId)
		{
			TemplateData templateData = new TemplateData();
			templateData.Delete(this, newTemplateId);

			return this.TemplateId;
		}

		public int TemplateId
		{
			get
			{
				return templateId;
			}
			set
			{
				this.templateId = value;
			}
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

		public string Publicurl
		{
			get
			{
				return publicurl;
			}
			set
			{
				this.publicurl = value;
			}
		}
				
		public string Adminurl
		{
			get
			{
				return adminurl;
			}
			set
			{
				this.adminurl = value;
			}
		}
		
		public string Xslurl
		{
			get
			{
				return xslurl;
			}
			set
			{
				this.xslurl = value;
			}
		}
		
		public string ModuleClassPublic
		{
			get
			{
				return moduleClassPublic;
			}
			set
			{
				this.moduleClassPublic = value;
			}
		}

        public string ModuleClassAdmin
        {
            get
            {
                return moduleClassAdmin;
            }
            set
            {
                this.moduleClassAdmin = value;
            }
        }

        public string UserControl
        {
            get
            {
                return userControl;
            }
            set
            {
                this.userControl = value;
            }
        }

        public string Templatecolor
		{
			get
			{
				return templatecolor;
			}
			set
			{
				this.templatecolor = value;
			}
		}

        /// <summary>
        /// Maps a templates xslurl to the full filesystem path
        /// </summary>
        /// <param name="templateUrl"></param>
        /// <returns></returns>
        public static string GetFullPath(string templateUrl)
        {
            if (templateUrl.IndexOf("\\") == -1)
                templateUrl = Global.publicXmlPath.TrimEnd('\\') + "\\" + templateUrl;
            else
                templateUrl = HttpContext.Current.Server.MapPath(templateUrl.Replace(@"\\",@"\"));

            return templateUrl;
        }
	}
}
