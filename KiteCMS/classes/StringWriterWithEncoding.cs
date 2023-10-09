    using System;
    using System.Text;
    using System.IO;

namespace KiteCMS
{
        /// <summary>
        /// StringWriterWithEncoding
        /// </summary>
        /// <remarks>
        ///If you want to use XmlTextWriter to write XML into a StringBuilder
        ///you can create the XmlTextWriter like this:
        ///StringBuilder builder = new StringBuilder();
        ///XmlWriter writer = new XmlTextWriter(new StringWriter(builder));
        ///But this generates a declaration on the resulting XML with the encoding of UTF-16
        ///(the encoding of a .Net String).
        ///There doesn't seem to be a straightforward way of making this declaration UTF-8 in this set up.
        ///You can, of course, use a MemoryStream instead of a StringWriter,
        ///and then use Encoding.UTF8.GetString(...) to convert the bytes to a string,
        ///but doing this made the resulting string have non-printable characters in it,
        ///which we don't want.
        /// </remarks>
        public class StringWriterWithEncoding : StringWriter
        {
            private Encoding _encoding;

            public StringWriterWithEncoding()
                : base() { }

            public StringWriterWithEncoding(IFormatProvider formatProvider)
                : base(formatProvider) { }

            public StringWriterWithEncoding(StringBuilder sb)
                : base(sb) { }

            public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider)
                : base(sb, formatProvider) { }


            public StringWriterWithEncoding(Encoding encoding)
                : base()
            {
                _encoding = encoding;
            }

            public StringWriterWithEncoding(IFormatProvider formatProvider, Encoding encoding)
                : base(formatProvider)
            {
                _encoding = encoding;
            }

            public StringWriterWithEncoding(StringBuilder sb, Encoding encoding)
                : base(sb)
            {
                _encoding = encoding;
            }

            public StringWriterWithEncoding(StringBuilder sb, IFormatProvider formatProvider, Encoding encoding)
                : base(sb, formatProvider)
            {
                _encoding = encoding;
            }

            public override Encoding Encoding
            {
                get
                {
                    return (null == _encoding) ? base.Encoding : _encoding;
                }
            }
        }
    }
