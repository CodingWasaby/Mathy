// Dandelion.Reflection.Field
using Mathy.Utils.Dandelion;
using System;
using System.Reflection;
namespace Mathy.Utils.Dandelion.Reflection
{
	public class Field
	{
		public string Name
		{
			get;
			set;
		}

		public Type Type
		{
			get;
			set;
		}

		internal PropertyInfo PropertyInfo
		{
			get;
			set;
		}

		public object GetValue(object obj)
		{
			return PropertyInfo.GetValue(obj);
		}

		public void SetValue(object obj, object value)
		{
			if (PropertyInfo.SetMethod != null)
			{
				PropertyInfo.SetValue(obj, Types.ConvertValue(value, PropertyInfo.PropertyType));
			}
		}
	}
}
