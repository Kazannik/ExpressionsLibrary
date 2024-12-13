namespace ExpressionsLibrary
{
	/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="ICell"]/*' />
	public interface ICell
	{
		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="Format"]/*' />
		string Format { get; set; }

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="FormulaFormat"]/*' />
		string FormulaFormat { get; set; }

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="Key"]/*' />
		string Key { get; }

		/// <include file='XmlDocs/CommonXmlDocComments.xml' path='CommonXmlDocComments/Cells/Member[@name="SetValue"]/*' />
		void SetValue(decimal value);

		decimal Value { get; }
	}
}
