// Dandelion.Reflection.EntityRepository
using Mathy.Utils.Dandelion;
using Mathy.Utils.Dandelion.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
namespace Mathy.Utils.Dandelion.Reflection
{
	public class EntityRepository
	{
		private class PropertyComparer : IComparer<PropertyInfo>
		{
			public int Compare(PropertyInfo x, PropertyInfo y)
			{
				if (x.DeclaringType.Equals(y.DeclaringType))
				{
					return 0;
				}
				if (x.DeclaringType.IsAssignableFrom(y.DeclaringType))
				{
					return -1;
				}
				return 1;
			}
		}

		private static Dictionary<Type, Entity> entities = new Dictionary<Type, Entity>();

		private static object locker = new object();

		public static Entity GetEntity(Type type)
		{
			if (!entities.ContainsKey(type))
			{
				lock (locker)
				{
					if (!entities.ContainsKey(type))
					{
						entities.Add(type, RegisterType(type));
					}
				}
			}
			return entities[type];
		}

		private static Entity RegisterType(Type type)
		{
			List<Field> list = new List<Field>();
			bool flag = false;
			List<PropertyInfo> list2 = type.GetProperties().ToList();
			list2.Sort(new PropertyComparer());
			foreach (PropertyInfo item in list2)
			{
				if (item.GetCustomAttributes(typeof(FieldAttribute), inherit: false).Any() || item.GetCustomAttributes(typeof(DataMemberAttribute), inherit: false).Any())
				{
					flag = true;
					break;
				}
			}
			foreach (PropertyInfo item2 in list2)
			{
				string name = item2.Name;
				bool flag2 = !flag;
				FieldAttribute fieldAttribute = item2.GetCustomAttributes(typeof(FieldAttribute)).FirstOrDefault() as FieldAttribute;
				if (fieldAttribute != null)
				{
					flag2 = true;
					if (!string.IsNullOrEmpty(fieldAttribute.FieldName))
					{
						name = fieldAttribute.FieldName;
					}
				}
				if (!flag2)
				{
					DataMemberAttribute dataMemberAttribute = item2.GetCustomAttributes(typeof(DataMemberAttribute), inherit: false).FirstOrDefault() as DataMemberAttribute;
					if (dataMemberAttribute != null)
					{
						flag2 = true;
						if (!string.IsNullOrEmpty(dataMemberAttribute.Name))
						{
							name = dataMemberAttribute.Name;
						}
					}
				}
				if (flag2)
				{
					list.Add(new Field
					{
						Name = name,
						Type = item2.PropertyType,
						PropertyInfo = item2
					});
				}
			}
			EntityAttribute entityAttribute = type.GetCustomAttributes(typeof(EntityAttribute), inherit: true).FirstOrDefault() as EntityAttribute;
			Entity entity = new Entity();
			entity.Name = ((entityAttribute == null) ? type.Name : entityAttribute.EntityName);
			entity.Fields = list.ToArray();
			return entity;
		}
	}
}
