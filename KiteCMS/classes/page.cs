using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using KiteCMS.Admin;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Page
	{
		private int pageId = -1;
		private int templateId = -1;
		private int parentPageId = -1;
		private int zoneId = -1;
		private int isPublic = -1;
		private string title = "";
		private StringDictionary contentHolders;
		private string lastUpdated = "";
		private ParameterCollection parameters = new ParameterCollection();
		private BoxCollection boxes;

		public Page(int pageId)
		{
			this.pageId = pageId;
            KiteCMS.Admin.Page page = new KiteCMS.Admin.Page(pageId);

			this.templateId = page.TemplateId;
			this.parentPageId = page.ParentPageid;
			this.zoneId = page.ZoneId;
			this.isPublic = page.IsPublic;
			this.title = page.Title;
			this.contentHolders = page.ContentHolders;
			this.parameters = page.Parameters;
			this.boxes = page.Boxes;
			this.lastUpdated = page.LastUpdated;
		}

		public string Title
		{
			get
			{
				return title;
			}
		}

		public ParameterCollection Parameters
		{
			get
			{
				return parameters;
			}
		}

		public BoxCollection Boxes
		{
			get
			{
				return boxes;
			}
		}

		public StringDictionary ContentHolders
		{
			get
			{
				return contentHolders;
			}
		}

		public int ZoneId
		{
			get
			{
				return zoneId;
			}
		}

		public int IsPublic
		{
			get
			{
				return isPublic;
			}
		}

		public int Pageid
		{
			get
			{
				return pageId;
			}
		}

		public int ParentPageid
		{
			get
			{
				return parentPageId;
			}
		}

		public int TemplateId
		{
			get
			{
				return templateId;
			}
		}

		public string LastUpdated
		{
			get
			{
				return lastUpdated;
			}
		}


	}
}
