using System;
using System.Collections;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for pageCollection.
	/// </summary>
	public class BoxCategoryCollection : CollectionBase
	{
		public BoxCategoryCollection()
		{
		}

		public int Add(BoxCategory boxCategory)
		{
			if (this.IndexOf(boxCategory) == -1)
				return List.Add(boxCategory);
			else
				return -1;
		}

		public BoxCategory this[int index]
		{
			get { return (BoxCategory)List[index]; }
			set { List[index] = value; }
		}

		public BoxCategoryCollection Remove(BoxCategory boxCategory)
		{
			if (this.IndexOf(boxCategory) != -1)
					this.RemoveAt(this.IndexOf(boxCategory));

			return this;
		}

		public int IndexOf(BoxCategory boxCategory)
		{
			int output = -1;
			for(int counter = 0; counter < this.Count; counter++)
				if (this[counter].BoxCategoryId == boxCategory.BoxCategoryId)
					output = counter;
			return output;
		}
	}
}
