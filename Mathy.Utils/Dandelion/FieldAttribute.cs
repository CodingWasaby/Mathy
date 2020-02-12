// Dandelion.FieldAttribute
using System;

namespace Mathy.Utils.Dandelion
{
	public class FieldAttribute : Attribute
	{
		public string FieldName
		{
			get;
			private set;
		}

		public FieldAttribute()
		{
		}

		public FieldAttribute(string fieldName)
		{
			FieldName = fieldName;
		}
	}
}
