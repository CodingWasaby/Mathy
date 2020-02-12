// Dandelion.Converting.ToDynamicObjConverter
using Mathy.Utils.Dandelion.Collections;
using Mathy.Utils.Dandelion.Converting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class ToDynamicObjConverter : IObjectConverter
	{
		public IDictionary ConvertToDictionary(object obj, Func<object, object> valueConverter)
		{
			DynamicObj dynamicObj = obj as DynamicObj;
			IDictionary dictionary = new Dictionary<string, object>();
			string[] keys = dynamicObj.Keys;
			foreach (string text in keys)
			{
				dictionary.Add(text, valueConverter(dynamicObj.GetValue(text)));
			}
			return dictionary;
		}

		public object ConvertFromDictionary(IDictionary dict, Func<object, object> valueConverter)
		{
			DynamicObj dynamicObj = new DynamicObj();
			foreach (string key in dict.Keys)
			{
				dynamicObj.Put(key, valueConverter(dict[key]));
			}
			return dynamicObj;
		}
	}
}
