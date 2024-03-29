﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Box2CS.Parsers
{
	public class XMLFragmentAttribute
	{
		public string Name
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}
	}

	public class XMLFragmentElement
	{
		List<XMLFragmentElement> _elements = new List<XMLFragmentElement>();
		List<XMLFragmentAttribute> _attributes = new List<XMLFragmentAttribute>();

		public IList<XMLFragmentElement> Elements
		{
			get { return _elements; }
		}

		public IList<XMLFragmentAttribute> Attributes
		{
			get { return _attributes; }
		}

		public string Name
		{
			get;
			set;
		}

		public string Value
		{
			get;
			set;
		}

		public string OuterXml
		{
			get;
			set;
		}

		public string InnerXml
		{
			get;
			set;
		}
	}

	[Serializable]
	public class XMLFragmentException : Exception
	{
		public XMLFragmentException() { }
		public XMLFragmentException(string message) : base(message) { }
		public XMLFragmentException(string message, Exception inner) : base(message, inner) { }
		protected XMLFragmentException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}

	public class FileBuffer
	{
		public string Buffer
		{
			get;
			set;
		}

		public int Position
		{
			get;
			set;
		}

		public int Length
		{
			get { return Buffer.Length; }
		}

		public char Next
		{
			get
			{
				char c = Buffer[Position];
				Position++;
				return c;
			}
		}

		public char Peek
		{
			get
			{
				return Buffer[Position];
			}
		}

		public bool EndOfBuffer
		{
			get
			{
				return Position == Length;
			}
		}

		public FileBuffer(Stream stream)
		{
			using (StreamReader sr = new StreamReader(stream))
				Buffer = sr.ReadToEnd();

			Position = 0;
		}
	}

	public class XMLFragmentParser
	{
		XMLFragmentElement _rootNode;
		FileBuffer _buffer;
		static char[] _punctuation = new char[] { '/', '<', '>', '=' };

		public XMLFragmentElement RootNode
		{
			get { return _rootNode; }
		}

		public XMLFragmentParser(Stream stream)
		{
			Load(stream);
		}

		public XMLFragmentParser(string fileName)
		{
			using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				Load(fs);
		}

		public void Load(Stream stream)
		{
			_buffer = new FileBuffer(stream);
		}

		public static XMLFragmentElement LoadFromFile(string fileName)
		{
			var x = new XMLFragmentParser(fileName);
			x.Parse();
			return x.RootNode;
		}

		public static XMLFragmentElement LoadFromStream(Stream stream)
		{
			var x = new XMLFragmentParser(stream);
			x.Parse();
			return x.RootNode;
		}

		string NextToken()
		{
			string str = "";
			bool _done = false;

			while (true)
			{
				char c = (char)_buffer.Next;

				if (_punctuation.Contains<char>(c))
				{
					if (str != "")
					{
						_buffer.Position--;
						break;
					}

					_done = true;
				}
				else if (char.IsWhiteSpace(c))
				{
					if (str != "")
						break;
					else
						continue;
				}

				str += c;

				if (_done)
					break;
			}

			str = TrimControl(str);

			// Trim quotes from start and end
			if (str[0] == '\"')
				str = str.Remove(0, 1);

			if (str[str.Length - 1] == '\"')
				str = str.Remove(str.Length - 1, 1);

			return str;
		}

		string PeekToken()
		{
			var oldPos = _buffer.Position;
			var str = NextToken();
			_buffer.Position = oldPos;
			return str;
		}

		string ReadUntil(char c)
		{
			string str = "";

			while (true)
			{
				char ch = (char)_buffer.Next;

				if (ch == c)
				{
					_buffer.Position--;
					break;
				}

				str += ch;
			}

			// Trim quotes from start and end
			if (str[0] == '\"')
				str = str.Remove(0, 1);

			if (str[str.Length - 1] == '\"')
				str = str.Remove(str.Length - 1, 1);

			return str;
		}

		string TrimControl(string str)
		{
			string newStr = str;

			// Trim control characters
			int i = 0;
			while (true)
			{
				if (i == newStr.Length)
					break;

				if (char.IsControl(newStr[i]))
					newStr = newStr.Remove(i, 1);
				else
					i++;
			}

			return newStr;
		}

		string TrimTags(string outer)
		{
			int start = outer.IndexOf('>') + 1;
			int end = outer.LastIndexOf('<');

			return TrimControl(outer.Substring(start, end - start));
		}

		public XMLFragmentElement TryParseNode()
		{
			if (_buffer.EndOfBuffer)
				return null;

			int startOuterXml = _buffer.Position;
			var token = NextToken();

			if (token != "<")
				throw new XMLFragmentException("Expected \"<\", got "+token);

			XMLFragmentElement element = new XMLFragmentElement();
			element.Name = NextToken();

			while (true)
			{
				token = NextToken();

				if (token == ">")
					break;
				else if (token == "/") // quick-exit case
				{
					NextToken();

					element.OuterXml = TrimControl(_buffer.Buffer.Substring(startOuterXml, _buffer.Position - startOuterXml)).Trim();
					element.InnerXml = "";

					return element;
				}
				else
				{
					XMLFragmentAttribute attribute = new XMLFragmentAttribute();
					attribute.Name = token;
					if ((token = NextToken()) != "=")
						throw new XMLFragmentException("Expected \"=\", got "+token);
					attribute.Value = NextToken();

					element.Attributes.Add(attribute);
				}
			}

			while (true)
			{
				var oldPos = _buffer.Position; // for restoration below
				token = NextToken();

				if (token == "<")
				{
					token = PeekToken();

					if (token == "/") // finish element
					{
						NextToken(); // skip the / again
						token = NextToken();
						NextToken(); // skip >

						element.OuterXml = TrimControl(_buffer.Buffer.Substring(startOuterXml, _buffer.Position - startOuterXml)).Trim();
						element.InnerXml = TrimTags(element.OuterXml);

						if (token != element.Name)
							throw new XMLFragmentException("Mismatched element pairs: \""+element.Name+"\" vs \""+token+"\"");

						break;
					}
					else
					{
						_buffer.Position = oldPos;
						element.Elements.Add(TryParseNode());
					}
				}
				else
				{
					// value, probably
					_buffer.Position = oldPos;
					element.Value = ReadUntil('<');
				}
			}

			return element;
		}

		public void Parse()
		{
			_rootNode = TryParseNode();

			if (_rootNode == null)
				throw new XMLFragmentException("Unable to load root node");
		}
	}
}
