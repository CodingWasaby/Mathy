
using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public abstract class Serializer
	{
		public DateParser DateParser;

		public bool SerializeEnumAsInteger
		{
			get;
			set;
		}

		public abstract byte[] Serialize(object data, Encoding encoding);
	}
}
