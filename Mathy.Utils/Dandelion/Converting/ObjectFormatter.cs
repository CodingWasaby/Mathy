// Dandelion.Converting.ObjectFormatter
using Mathy.Utils.Dandelion.Collections;
using Mathy.Utils.Dandelion.Converting;
using System;
using System.Collections;

namespace Mathy.Utils.Dandelion.Converting
{
	internal class ObjectFormatter
	{
		public enum Options
		{
			Dictionary,
			DynamicObj,
			Object
		}

		public static readonly ObjectFormatter Instance = new ObjectFormatter();

		private ToDictionaryConverter toDictionary = new ToDictionaryConverter();

		private ToDynamicObjConverter toDynamicObj = new ToDynamicObjConverter();

		private ToObjectConverter toObject = new ToObjectConverter();

		private IObjectConverter[] objectConverters;

		private Func<object, object>[] valueConverters;

		private ObjectFormatter()
		{
			objectConverters = new IObjectConverter[3]
			{
			toDictionary,
			toDynamicObj,
			toObject
			};
			Func<object, object>[] array = new Func<object, object>[3];
			Func<object, object> func = array[0] = ((object i) => Convert(i, 0));
			array[1] = ((object i) => Convert(i, 1));
			array[2] = ((object i) => Convert(i, 2));
			valueConverters = array;
		}

		public object Convert(object obj, Options options)
		{
			return Convert(obj, (int)options);
		}

		private object Convert(object obj, int destIndex)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj.GetType().IsPrimitive || obj is string || obj is DateTime)
			{
				return obj;
			}
			if (obj is IList)
			{
				IList list = null;
				try
				{
					list = (Activator.CreateInstance(obj.GetType()) as IList);
				}
				catch
				{
					throw new ArgumentException(string.Concat("Cannot create a duplicate of ", obj.GetType(), "."));
				}
				foreach (object item in obj as IList)
				{
					list.Add(Convert(item, destIndex));
				}
				return list;
			}
			if (obj is IEnumerable && !(obj is IDictionary))
			{
				throw new ArgumentException("Cannot convert an IEnumerable that is not an IList.");
			}
			int num = (!(obj is IDictionary)) ? ((obj is DynamicObj) ? 1 : 2) : 0;
			return objectConverters[destIndex].ConvertFromDictionary(objectConverters[num].ConvertToDictionary(obj, valueConverters[num]), valueConverters[destIndex]);
		}
	}
}
