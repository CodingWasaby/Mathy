// Dandelion.Serialization.Deserializer
using System;
using System.Text;
namespace Mathy.Utils.Dandelion.Serialization
{
	public abstract class Deserializer
	{
		public abstract object Deserialize(byte[] data, Encoding encoding, Type type);

		public abstract object Deserialize(byte[] data, Encoding encoding);
	}
}
