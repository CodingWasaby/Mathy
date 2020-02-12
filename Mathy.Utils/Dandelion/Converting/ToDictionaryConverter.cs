// Dandelion.Converting.ToDictionaryConverter
using Mathy.Utils.Dandelion.Converting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class ToDictionaryConverter : IObjectConverter
	{
		public IDictionary ConvertToDictionary(object obj, Func<object, object> valueConverter)
		{
			IDictionary dictionary = obj as IDictionary;
			IDictionary dictionary2 = new Dictionary<string, object>();
			foreach (string key in dictionary.Keys)
			{
				dictionary2.Add(key, valueConverter(dictionary[key]));
			}
			return dictionary2;
		}

		public object ConvertFromDictionary(IDictionary dict, Func<object, object> valueConverter)
		{
			IDictionary dictionary = new Dictionary<string, object>();
			foreach (string key in dict.Keys)
			{
				dictionary.Add(key, valueConverter(dict[key]));
			}
			return dictionary;
		}
	}
}
