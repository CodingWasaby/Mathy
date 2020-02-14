
using System;
namespace Mathy.Utils.Dandelion.Serialization
{
	public class JsonDeserializer : StringDeserializer
	{
		public override object DeserializeString(string s, Type type)
		{
			return new JsonDeserializerContext().DeserializeString(s, type);
		}

		public override object DeserializeString(string s)
		{
			return new JsonDeserializerContext().DeserializeString(s);
		}
	}
}
