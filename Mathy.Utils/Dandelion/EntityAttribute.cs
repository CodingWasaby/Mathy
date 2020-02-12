// Dandelion.EntityAttribute
using System;
namespace Mathy.Utils.Dandelion
{
	public class EntityAttribute : Attribute
	{
		public string EntityName
		{
			get;
			private set;
		}

		public EntityAttribute()
		{
		}

		public EntityAttribute(string entityName)
		{
			EntityName = entityName;
		}
	}
}
