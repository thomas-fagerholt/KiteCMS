using System;
using System.Collections;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for pageCollection.
	/// </summary>
	public class BoxCollection : CollectionBase
	{
		public BoxCollection()
		{
		}

		public int Add(Box box)
		{
			if (this.IndexOf(box) == -1)
				return List.Add(box);
			else
				return -1;
		}

		public Box this[int index]
		{
			get { return (Box)List[index]; }
			set { List[index] = value; }
		}

		public BoxCollection Remove(Box box)
		{
			if (this.IndexOf(box) != -1)
					this.RemoveAt(this.IndexOf(box));

			return this;
		}

		public int IndexOf(Box box)
		{
			int output = -1;
			for(int counter = 0; counter < this.Count; counter++)
				if (this[counter].BoxId == box.BoxId)
					output = counter;
			return output;
		}
	}
}
