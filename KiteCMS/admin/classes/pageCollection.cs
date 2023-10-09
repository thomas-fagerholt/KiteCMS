using System;
using System.Collections;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for pageCollection.
	/// </summary>
	public class PageCollection : CollectionBase
	{
		public PageCollection()
		{
		}

		public int Add(Page page)
		{
			return List.Add(page);
		}

		public Page this[int index]
		{
			get { return (Page)List[index]; }
			set { List[index] = value; }
		}	
	}
}
