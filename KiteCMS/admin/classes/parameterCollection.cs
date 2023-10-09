using System;
using System.Collections;
using System.Collections.Specialized;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for pageparameters.
	/// </summary>
	public class ParameterCollection : StringDictionary 
	{
		private StringDictionary parameterCollection = new StringDictionary();

		public void Add(Parameter parameter)
		{
			parameterCollection.Add(parameter.Key, parameter.Value );
		}
		
		public string Get(string key)
		{
			return (string)parameterCollection[key];
		}
		
		public void Delete(string key)
		{
			parameterCollection.Remove( key );
		}

		public override void Clear()
		{
			parameterCollection.Clear();
		}

		public override int Count
		{
			get
			{
				return parameterCollection.Count;
			}
		}

		public override ICollection Keys
		{
			get
			{
				return parameterCollection.Keys;
			}
		}

	}
}
