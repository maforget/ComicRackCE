using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cYo.Common.Text.FunctionParser.Functions.Text
{
	public record EscapeFunctionParameter(string Text) : FunctionParameter;

	[FunctionDefinition("escape")]
	public class EscapeFunction(string Name) : FunctionBase<EscapeFunctionParameter, string>(Name)
	{
		protected override Func<EscapeFunctionParameter, string> Function => param =>
		{
			var stringBuilder = new StringBuilder();
			foreach (char c in param.Text)
			{
				string text = c.ToString();
				switch (c)
				{
					case '\'': text = text.Replace("'", "\\'"); break;
					case '\"': text = text.Replace("\"", "\\\""); break;
					case '\\': text = text.Replace("\\", "\\\\"); break;
					case '\0': text = text.Replace("\0", "\\0"); break;
					case '\a': text = text.Replace("\a", "\\a"); break;
					case '\b': text = text.Replace("\b", "\\b"); break;
					case '\f': text = text.Replace("\f", "\\f"); break;
					case '\n': text = text.Replace("\n", "\\n"); break;
					case '\r': text = text.Replace("\r", "\\r"); break;
					case '\t': text = text.Replace("\t", "\\t"); break;
					case '\v': text = text.Replace("\v", "\\v"); break;
					case ',': text = text.Replace(",", "\\,"); break;
					case '<': text = text.Replace("<", "\\<"); break;
					case '>': text = text.Replace(">", "\\>"); break;
					case '$': text = text.Replace("$", "\\$"); break;
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		};
	}
}
