using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using cYo.Common.Text.FunctionParser;

namespace cYo.Common.Text
{
    public static class ExtendedStringFormater
    {
        private const string Error = "#ERROR";

        public static string Format(string format, Func<string, object> getValue)
        {
            bool success;
            try
            {
                return Format(format, getValue, out success);
            }
            catch (Exception e)
            {
                success = false;
                return $"{Error} - {e.Message}";
            }
        }

        public static string Format(string format, IDictionary<string, object> values)
        {
            try
            {
                return Format(format, (string s) => GetValue(values, s));
            }
            catch (Exception e)
            {
                return $"{Error} - {e.Message}";
            }
        }

        private static object GetValue(IDictionary<string, object> values, string key)
        {
            if (!values.TryGetValue(key, out var value))
            {
                return null;
            }
            return value;
        }

        private static string Format(string format, Func<string, object> getValue, out bool success, bool escapeComma = false)
        {
            StringBuilder stringBuilder = new StringBuilder();
            success = true;
            for (int i = 0; i < format.Length; i++)
            {
                char c = format[i];
                switch (c)
                {
                    case '[':
                        {
                            bool success2;
                            string value2 = Format(GetPart(format, ref i, '[', ']'), getValue, out success2, escapeComma);
                            if (success2)
                            {
                                stringBuilder.Append(value2);
                            }
                            success |= success2;
                            break;
                        }
                    case '$':
                        {
                            bool success3;
                            string value3 = Format(GetPart(format, ref i, '$', '>', ['<', '>', '$']), getValue, out success3, true);
                            string result = ParseFunction(value3, out success3);
							stringBuilder.Append(result);
                            success &= success3 && !string.IsNullOrEmpty(result);
                            break;
                        }
                     case '{':
                        {
                            string value = FormatValue(GetPart(format, ref i, '{', '}'), getValue);
                            success &= !string.IsNullOrEmpty(value);
                            if (escapeComma && value.Contains(',')) //Escape commas when using functions, so they aren't considered parameters
                                value = value.Replace(",", "\\,");

                            stringBuilder.Append(value);
                            break;
                        }
                    case '\\':
                        stringBuilder.Append(format[++i]);
                        break;
                    default:
                        stringBuilder.Append(c);
                        break;
                }
            }
            return stringBuilder.ToString();
        }

        private static string ParseFunction(string value, out bool success)
        {
            string name = string.Empty;
            success = false;
            string[] param = Array.Empty<string>();

            string result = string.Empty;
            Regex regex = new Regex(@"(?<function>[^<]+?)<(?<params>.+)$");
            name = regex.Match(value).Groups["function"].Value.ToLower();
            string paramsValue = regex.Match(value).Groups["params"]?.Value;

			param = string.IsNullOrEmpty(paramsValue)
				? Array.Empty<string>()  // Return an empty array if null or empty
				: Regex.Split(paramsValue, @"(?<!\\),")
					.Select(x => x.Trim().Replace(@"\,", ",")) // Remove escaping backslash
					.ToArray();

			IFunction func = FunctionFactory.Functions.CreateFunction(name);
            func.SetParameters(param);
            result = func.ResultAsText;
            success = true;

            return result;
        }

        private static string FormatValue(string format, Func<string, object> getValue)
        {
            string[] array = format.Split(':');
            object obj = getValue(array[0]);
            if (obj == null)
            {
                return string.Empty;
            }
            if (array.Length == 1)
            {
                return obj.ToString();
            }
            try
            {
                obj = Convert.ToDouble(obj);
            }
            catch
            {
            }
            try
            {
                return string.Format("{0:" + array[1] + "}", obj);
            }
            catch (Exception)
            {
                return obj.ToString();
            }
        }

        private static string GetPart(string text, ref int index, char openChar, char closeChar, char[] charToEscape = null)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            bool flag = openChar == closeChar;
            charToEscape ??= Array.Empty<char>();
            while (index < text.Length)
            {
                char c = text[index];
                char n = index + 1 < text.Length ? text[index + 1] : '\0';
                if(c == '\\' && charToEscape.Contains(n))
                {
                    index += 2;
                    continue;
                }

                if (c == openChar && (!flag || num3 <= 0))
                {
                    if (num3++ == 0)
                    {
                        num = index + 1;
                    }
                }
                else if (c == closeChar && --num3 == 0)
                {
                    num2 = index;
                    break;
                }
                index++;
            }
            return text.Substring(num, num2 - num);
        }
    }
}
