using System;

namespace KiteCMS.Admin
{

	public class Parameter
	{
		private string key = "";
		private string value = "";

		public Parameter()
		{
		}

		public Parameter(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
			}
		}
	}
}
