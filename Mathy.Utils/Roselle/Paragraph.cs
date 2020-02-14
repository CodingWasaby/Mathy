// 警告: 某些程序集引用无法自动解析。这可能会导致某些部分反编译错误,
// 比如属性getter/setter 访问。要获得最佳反编译结果, 请手动将缺少的引用添加到加载的程序集列表中。
// Roselle.Paragraph
using Roselle;

public class Paragraph : IDocumentElement
{
	public string Text
	{
		get;
		set;
	}

	public FontSize FontSize
	{
		get;
		set;
	}

	public TextStyle[] Styles
	{
		get;
		set;
	}

	public bool IsCentered
	{
		get;
		set;
	}

	public Paragraph()
	{
		FontSize = FontSize.Normal;
	}

	public Paragraph(string text)
	{
		FontSize = FontSize.Normal;
		Text = (text ?? string.Empty);
	}
}
