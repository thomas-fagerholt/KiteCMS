using System;
using System.Collections;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for pageparameters.
	/// </summary>
	public class ValueCollection : ArrayList
	{
		private ArrayList valueCollection = new ArrayList();

		public void Add(int value)
		{
			this.Add(value);
		}
		
		public int Get(int index)
		{
			return int.Parse(valueCollection[index].ToString());
		}
		

		public override void Clear()
		{
			this.Clear();
		}

	}
}
