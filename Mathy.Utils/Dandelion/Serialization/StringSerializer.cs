using System.Text;

namespace Mathy.Utils.Dandelion.Serialization
{
	public abstract class StringSerializer : Serializer
	{
		public abstract string SerializeToString(object data);

		public override byte[] Serialize(object data, Encoding encoding)
		{
			return encoding.GetBytes(SerializeToString(data));
		}
	}
}
