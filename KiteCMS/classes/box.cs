using System;
using System.Web;
using System.Xml;
using KiteCMS.Admin;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for page.
	/// </summary>
	public class Box
	{
		private int boxId = -1;
		private string title = "";
		private string replaceString = "";
		private BoxCategoryTypeEnum boxCategoryType;

		public Box(int boxId)
		{
			this.boxId = boxId;
            KiteCMS.Admin.Box box = new KiteCMS.Admin.Box(boxId);

			this.title = box.Title;
			this.replaceString = box.BoxCategory.Htmlstring;
			this.boxCategoryType = (BoxCategoryTypeEnum)box.BoxCategory.BoxCategoryType;
		}

		public string Title
		{
			get
			{
				return title;
			}
		}

		public string ReplaceString
		{
			get
			{
				return replaceString;
			}
		}

		public BoxCategoryTypeEnum BoxCategoryType
		{
			get
			{
				return boxCategoryType;
			}
		}
	}

	public enum BoxCategoryTypeEnum : int
	{
		replace = 0, insertAbove = 1, insertBelow = 2 
	}

}
