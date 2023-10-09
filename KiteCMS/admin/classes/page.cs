using System;
using System.Collections.Specialized;
using System.Web;
using System.Xml;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
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
		private int followinSiblingPageId = -1;
		private int hasDraft = -1;
		private string title = "";
        private StringDictionary contentHolders;
        private StringDictionary draftContentHolders;
		private string friendlyUrl = "";
        private string created = "";
        private string lastUpdated = "";
        private ParameterCollection parameters;
		private BoxCollection boxes;
        private bool withlock = false;

		public Page()
		{
		}

		public Page(bool withlock)
		{
            this.withlock = withlock;
			// Lock menuxml to prevent concurrency problems when saving
			if(withlock)
				core.Website.LockMenuXml();
		}

		public Page(int pageId)
		{
			PageData pageData = new PageData();
			this.pageId = pageId;
			pageData.Load(this, false);
		}

		public Page(int pageId, bool withlock)
		{
            this.withlock = withlock;
            PageData pageData = new PageData();
			this.pageId = pageId;
			pageData.Load(this, withlock);
		}

		public int Move(int newParentPageId, int newFollowinSiblingPageId)
		{
			PageData pageData = new PageData();
			pageData.Move(this,newParentPageId,newFollowinSiblingPageId);

			return this.Pageid;
		}

		public int Save()
		{
			Admin admin = new Admin();
			admin.userHasAccess(1103, Pageid);

			PageData pageData = new PageData();
            try
            {
                pageData.Save(this);

            }
            catch (ArgumentException)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(Functions.localText("menulocked") + Functions.publicUrl());
                HttpContext.Current.Response.End();
            }
            return this.Pageid;

		}

		public int Delete()
		{
			Admin admin = new Admin();
			admin.userHasAccess(1104, Pageid);

			PageData pageData = new PageData();
			pageData.Delete(this);

			return this.Pageid;
		}

		public int Copy(int newParentPageId, int newFollowinSiblingPageId)
		{
			return Copy(newParentPageId, newFollowinSiblingPageId, true, true);
		}

		public int Copy(int newParentPageId, int newFollowinSiblingPageId, bool includeSubpages, bool includeContent)
		{
			Admin admin = new Admin();
			admin.userHasAccess(1103, Pageid);

			PageData pageData = new PageData();
			pageData.Copy(this, newParentPageId, newFollowinSiblingPageId, includeSubpages, includeContent);

			return this.Pageid;
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

		public string FriendlyUrl
		{
			get
			{
				return friendlyUrl;
			}
			set
			{
                // check if friendlyurl exists only when the menu is locked for writting
                if (withlock)
                {
                    PageData pageData = new PageData();
                    if (!pageData.FriendlyUrlExists(this, value))
                        this.friendlyUrl = value.ToLower();
                    else
                        throw new ArgumentOutOfRangeException("Friendly url exists");
                }
                else
                    this.friendlyUrl = value.ToLower();
			}
		}

		public ParameterCollection Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				this.parameters = value;
			}
		}

		public BoxCollection Boxes
		{
			get
			{
				return boxes;
			}
			set
			{
				this.boxes = value;
			}
		}

        public StringDictionary ContentHolders
		{
			get
			{
                return contentHolders;
			}
			set
			{
                this.contentHolders = value;
			}
		}

		public int ZoneId
		{
			get
			{
				return zoneId;
			}
			set
			{
				this.zoneId = value;
			}
		}

		public int IsPublic
		{
			get
			{
				return isPublic;
			}
			set
			{
				this.isPublic = value;
			}
		}

		public int Pageid
		{
			get
			{
				return pageId;
			}
			set
			{
				this.pageId = value;
			}
		}

		public int ParentPageid
		{
			get
			{
				return parentPageId;
			}
			set
			{
				this.parentPageId = value;
			}
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

		public int FollowinSiblingPageId
		{
			get
			{
				return followinSiblingPageId;
			}
			set
			{
				this.followinSiblingPageId = value;
			}
		}

		public int HasDraft
		{
			get
			{
				return hasDraft;
			}
			set
			{
				this.hasDraft = value;
			}
		}

        public StringDictionary DraftContentHolders
		{
			get
			{
                return draftContentHolders;
			}
			set
			{
                this.draftContentHolders = value;
			}
		}

        public string Created
        {
            get
            {
                return created;
            }
            set
            {
                this.created = value;
            }
        }

        public string LastUpdated
        {
            get
            {
                return lastUpdated;
            }
            set
            {
                this.lastUpdated = value;
            }
        }


	}
}
