
using System;
using System.Globalization;
using System.Text;

namespace Common.Serialization
{
    internal class JsonString
    {
        private string _s;
        private int _index;

        internal JsonString(string s)
        {
            _s = s;
        }

        internal Nullable<char> GetNextNonEmptyChar()
        {
            while (_s.Length > _index)
            {
                char c = _s[_index++];
                if (!Char.IsWhiteSpace(c))
                {
                    return c;
                }
            }

            return null;
        }

        internal Nullable<char> MoveNext()
        {
            if (_s.Length > _index)
            {
                return _s[_index++];
            }

            return null;
        }

        internal string MoveNext(int count)
        {
            if (_s.Length >= _index + count)
            {
                string result = _s.Substring(_index, count);
                _index += count;

                return result;
            }

            return null;
        }

        internal void MovePrev()
        {
            if (_index > 0)
            {
                _index--;
            }
        }

        internal void MovePrev(int count)
        {
            while (_index > 0 && count > 0)
            {
                _index--;
                count--;
            }
        }

        private static void AppendCharAsUnicode(StringBuilder builder, char c)
        {
            builder.Append("\\u");
            builder.AppendFormat(CultureInfo.InvariantCulture, "{0:x4}", (int)c);
        }

        internal static string QuoteString(string value)
        {
            StringBuilder b = null;

            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            int startIndex = 0;
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                char c = value[i];

                if (c == '\r' || c == '\t' || c == '\"' || c == '\'' || 
                    //c == '<' || c == '>' || // 不用 Unicode 输出
                    c == '\\' || c == '\n' || c == '\b' || c == '\f' || c < ' ')
                {
                    if (b == null)
                    {
                        b = new StringBuilder(value.Length + 5);
                    }

                    if (count > 0)
                    {
                        b.Append(value, startIndex, count);
                    }

                    startIndex = i + 1;
                    count = 0;
                }

                switch (c)
                {
                    case '\r':
                        b.Append("\\r");
                        break;
                    case '\t':
                        b.Append("\\t");
                        break;
                    case '\"':
                        b.Append("\\\"");
                        break;
                    case '\\':
                        b.Append("\\\\");
                        break;
                    case '\n':
                        b.Append("\\n");
                        break;
                    case '\b':
                        b.Append("\\b");
                        break;
                    case '\f':
                        b.Append("\\f");
                        break;
                    case '\'':
                    //case '>': // 不晓得微软为何要把
                    //case '<': //   这两个字符用unicode输出，导致ContentService输出的html变得很大
                        AppendCharAsUnicode(b, c);
                        break;
                    default:
                        if (c < ' ')
                        {
                            AppendCharAsUnicode(b, c);
                        }
                        else
                        {
                            count++;
                        }
                        break;
                }
            }

            if (b == null)
            {
                return value;
            }

            if (count > 0)
            {
                b.Append(value, startIndex, count);
            }

            return b.ToString();
        }

        public override string ToString()
        {
            if (_s.Length > _index)
            {
                return _s.Substring(_index);
            }

            return String.Empty;
        }

        internal string GetDebugString(string message)
        {
            bool showJson = false; // 默认false
            return string.Format("{0} ({1}) {2}", message, _index, showJson ? _s : "需要显示详细信息，请设置Carpa.ShowJsonDebug为true");
        }
    }
}