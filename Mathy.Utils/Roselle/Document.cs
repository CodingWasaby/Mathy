// 警告: 某些程序集引用无法自动解析。这可能会导致某些部分反编译错误,
// 比如属性getter/setter 访问。要获得最佳反编译结果, 请手动将缺少的引用添加到加载的程序集列表中。
// Roselle.Document
using Roselle;
using System.Collections.Generic;
namespace Roselle
{
	public class Document
	{
		public List<IDocumentElement> Elements = new List<IDocumentElement>();

		public Document()
		{
			Elements = new List<IDocumentElement>();
		}
	}
}
