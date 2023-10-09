using System;
using System.Web;
using System.Xml;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class BoxCategory
	{
		private int boxCategoryId = -1;
		private string title = "";
		private string htmlstring = "";
		private BoxCategoryTypeEnum boxCategoryType;

		public BoxCategory()
		{
		}

		public BoxCategory(bool withlock)
		{
			// Lock menuxml to prevent concurrency problems when saving
			if(withlock)
				core.Website.LockMenuXml();
		}

		public BoxCategory(int boxCategoryId)
		{
			BoxCategoryData boxCategoryData = new BoxCategoryData();
			this.BoxCategoryId = boxCategoryId;
            boxCategoryData.Load(this, false);
		}

		public BoxCategory(int boxCategoryId, bool withlock)
		{
			BoxCategoryData boxCategoryData = new BoxCategoryData();
			this.BoxCategoryId = boxCategoryId;
            boxCategoryData.Load(this, withlock);
		}

		public int Save()
		{
			BoxCategoryData boxCategoryData = new BoxCategoryData();
			boxCategoryData.Save(this);

			return this.BoxCategoryId;
		}

		public int Delete()
		{
			BoxCategoryData boxCategoryData = new BoxCategoryData();
			boxCategoryData.Delete(this);

			return this.BoxCategoryId;
		}

		public int Delete(int nexBoxCategoryId)
		{
			BoxCategoryData boxCategoryData = new BoxCategoryData();
			boxCategoryData.Delete(this, nexBoxCategoryId);

			return this.BoxCategoryId;
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

		public string Htmlstring
		{
			get
			{
				return htmlstring;
			}
			set
			{
				this.htmlstring = value;
			}
		}

		public int BoxCategoryId
		{
			get
			{
				return boxCategoryId;
			}
			set
			{
				this.boxCategoryId = value;
			}
		}

		public BoxCategoryTypeEnum BoxCategoryType
		{
			get
			{
				return boxCategoryType;
			}
			set
			{
				this.boxCategoryType = value;
			}
		}

		public enum BoxCategoryTypeEnum : int
		{
			replace = 0, insertAbove = 1, insertBelow = 2 
		}

		public static BoxCategoryCollection getAllBoxCategories()
		{
			XmlNodeList oXmlNodes; 
			oXmlNodes = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("/website/boxmodule/boxcategory");

			BoxCategoryCollection boxCategoryColl = new BoxCategoryCollection();
			for(int counter = 0; counter < oXmlNodes.Count; counter ++)
				boxCategoryColl.Add(new BoxCategory(int.Parse(oXmlNodes.Item(counter).Attributes["id"].Value)));

			return boxCategoryColl;
		}

	}
}
