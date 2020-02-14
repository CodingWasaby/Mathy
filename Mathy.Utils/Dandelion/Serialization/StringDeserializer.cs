using System;
using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public abstract class StringDeserializer : Deserializer
	{
		public abstract object DeserializeString(string s, Type type);

		public abstract object DeserializeString(string s);

		public override object Deserialize(byte[] data, Encoding encoding, Type type)
		{
			return DeserializeString(encoding.GetString(data), type);
		}

		public override object Deserialize(byte[] data, Encoding encoding)
		{
			return DeserializeString(encoding.GetString(data));
		}
	}
}
