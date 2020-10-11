using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace cYo.Common.Text
{
	public class Tokenizer
	{
		public class TextPosition
		{
			public int Line
			{
				get;
				set;
			}

			public int Column
			{
				get;
				set;
			}
		}

		public class ParseException : Exception
		{
			public Token Token
			{
				get;
				private set;
			}

			public ParseException(string message, Token token)
				: base(message)
			{
				Token = token;
			}
		}

		public class Token
		{
			public string Text
			{
				get;
				set;
			}

			public string Source
			{
				get;
				set;
			}

			public int Index
			{
				get;
				set;
			}

			public int Length
			{
				get;
				set;
			}

			public TextPosition Position
			{
				get
				{
					StringUtility.ConvertIndexToLineAndColumn(Source, Index, out var line, out var column);
					return new TextPosition
					{
						Line = line,
						Column = column
					};
				}
			}

			public bool Is(params string[] p)
			{
				return p.Any((string s) => string.Equals(Text, s, StringComparison.OrdinalIgnoreCase));
			}

			public void ThrowEndException()
			{
			}

			public void ThrowParserException(string format)
			{
				throw new ParseException(string.Format(format, Text), this);
			}

			public override string ToString()
			{
				return Text;
			}
		}

		private readonly string source;

		private readonly MatchCollection matches;

		private int position;

		private bool trim;

		public int Count => matches.Count;

		public string Text
		{
			get
			{
				if (Current == null)
				{
					return null;
				}
				return Current.Text;
			}
		}

		public Token Current
		{
			get
			{
				if (position >= matches.Count)
				{
					return null;
				}
				return Get(position);
			}
		}

		public Token Last
		{
			get
			{
				if (position - 1 >= matches.Count)
				{
					return null;
				}
				return Get(position - 1);
			}
		}

		public Token Next
		{
			get
			{
				if (position + 1 >= matches.Count)
				{
					return null;
				}
				return Get(position + 1);
			}
		}

		public bool EndReached => Current == null;

		public Tokenizer(Regex tokenizer, string text, bool trim = true)
		{
			source = text;
			matches = tokenizer.Matches(text);
			position = 0;
			this.trim = trim;
		}

		public IEnumerable<Token> GetAll()
		{
			while (true)
			{
				Token token;
				Token t = (token = Take());
				if (token != null)
				{
					yield return t;
					continue;
				}
				break;
			}
		}

		public Token Get(int n)
		{
			Match match = matches[n];
			string text = match.Value;
			if (trim)
			{
				text = text.Trim();
			}
			return new Token
			{
				Source = source,
				Text = text,
				Index = match.Index,
				Length = match.Length
			};
		}

		public Token Take(string startsWith = null, string endsWith = null, bool trimStartEnd = true)
		{
			Token current = Current;
			position++;
			string text = ((current == null) ? string.Empty : current.Text);
			if (!string.IsNullOrEmpty(startsWith))
			{
				if (current == null)
				{
					ThrowEndException(current);
				}
				if (!text.StartsWith(startsWith))
				{
					current.ThrowParserException($"'{current.Text}' must start with '{startsWith}'");
				}
				if (trimStartEnd)
				{
					text = text.Substring(startsWith.Length);
				}
			}
			if (!string.IsNullOrEmpty(endsWith))
			{
				if (current == null)
				{
					ThrowEndException(current);
				}
				if (!text.EndsWith(endsWith))
				{
					current.ThrowParserException($"'{current.Text}' must end with '{endsWith}'");
				}
				if (trimStartEnd)
				{
					text = text.Substring(0, text.Length - endsWith.Length);
				}
			}
			if (current != null)
			{
				current.Text = text;
			}
			return current;
		}

		public Token TakeString()
		{
			Token token = Take("\"", "\"");
			token.Text = token.Text.Unescape();
			return token;
		}

		public Token Expect(params string[] expect)
		{
			if (Current == null || !Current.Is(expect))
			{
				ThrowExpectedException(Current, expect);
			}
			return Take();
		}

		public bool Is(params string[] p)
		{
			if (Current == null)
			{
				ThrowExpectedException(Current, p);
			}
			return IsOptional(p);
		}

		public bool IsOptional(params string[] p)
		{
			return p.Any((string s) => string.Equals(Text, s, StringComparison.OrdinalIgnoreCase));
		}

		public void Skip(int count = 1)
		{
			position += count;
		}

		private void ThrowEndException(Token token)
		{
			if (token == null)
			{
				throw new ParseException("Unexpected end reached.", null);
			}
		}

		private void ThrowExpectedException(Token token, params string[] expected)
		{
			string arg = ((expected.Length == 1) ? expected[0] : string.Format("one of ({0})", expected.ToListString(", ")));
			if (token == null)
			{
				throw new Exception("Unexpected end reached");
			}
			token.ThrowParserException($"Expected '{arg}', but found '{token.Text}'");
		}
	}
}
