// Dandelion.Converting.ToObjectConverter
using Mathy.Utils.Dandelion.Converting;
using Mathy.Utils.Dandelion.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class ToObjectConverter : IObjectConverter
	{
		public IDictionary ConvertToDictionary(object obj, Func<object, object> valueConverter)
		{
			IDictionary dictionary2;
			if (obj is ExpandoObject)
			{
				IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
				dictionary2 = new Dictionary<string, object>();
				foreach (string key in dictionary.Keys)
				{
					dictionary2.Add(key, valueConverter(dictionary[key]));
				}
				return dictionary2;
			}
			dictionary2 = new Dictionary<string, object>();
			Field[] fields = EntityRepository.GetEntity(obj.GetType()).Fields;
			foreach (Field field in fields)
			{
				dictionary2.Add(field.Name, valueConverter(field.GetValue(obj)));
			}
			return dictionary2;
		}

		public object ConvertFromDictionary(IDictionary dict, Func<object, object> valueConverter)
		{
			ExpandoObject expandoObject = new ExpandoObject();
			foreach (string key in dict.Keys)
			{
				((IDictionary<string, object>)expandoObject).Add(key, valueConverter(dict[key]));
			}
			return expandoObject;
		}
	}
}
