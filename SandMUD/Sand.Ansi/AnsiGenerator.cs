using System.Collections.Generic;
using System.Text;

namespace Sand.Ansi
{
    public class AnsiGenerator
    {
        private readonly List<byte> _currentBuffer;
        private AnsiStyle _currentStyle;

        public AnsiGenerator()
        {
            _currentBuffer = new List<byte>();
            _currentStyle = new AnsiStyle();
        }

        //Added properties
        public AnsiColor ForegroundColor
        {
            get { return _currentStyle.ForegroundColor; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.ForegroundColor = value;

                SetStyle(newStyle);
            }
        }

        public AnsiColor BackgroundColor
        {
            get { return _currentStyle.BackgroundColor; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.BackgroundColor = value;

                SetStyle(newStyle);
            }
        }

        public bool Blink
        {
            get { return _currentStyle.Blink; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.Blink = value;

                SetStyle(newStyle);
            }
        }

        public bool Bold
        {
            get { return _currentStyle.Bold; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.Bold = value;

                SetStyle(newStyle);
            }
        }

        public bool Italics
        {
            get { return _currentStyle.Italics; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.Italics = value;

                SetStyle(newStyle);
            }
        }

        public bool Underline
        {
            get { return _currentStyle.Underline; }
            set
            {
                var newStyle = _currentStyle;
                newStyle.Underline = value;

                SetStyle(newStyle);
            }
        }

        private static IEnumerable<byte> GenerateControlCode(int code)
        {
            //ASCII for Escape + ASCII for '['

            var result = new List<byte> {27, 91};

            //Create the appropriate code number (ascii equivalent - one or two bytes possible)
            result.AddRange(Encoding.ASCII.GetBytes(code.ToString())); //Add to the result

            result.Add(109); //ASCII for 'm'

            return result;
        }

        private static IEnumerable<byte> GenerateControlCodes(AnsiStyle oldStyle, AnsiStyle newStyle)
        {
            var result = new List<byte>();

            //Foreground color
            if (oldStyle.ForegroundColor != newStyle.ForegroundColor)
            {
                var code = 30 + (int) newStyle.ForegroundColor;
                result.AddRange(GenerateControlCode(code));
            }

            //Background color
            if (oldStyle.BackgroundColor != newStyle.BackgroundColor)
            {
                var code = 40 + (int) newStyle.BackgroundColor;
                result.AddRange(GenerateControlCode(code));
            }

            //Blink
            if (oldStyle.Blink != newStyle.Blink)
            {
                result.AddRange(newStyle.Blink ? GenerateControlCode(5) : GenerateControlCode(25));
            }

            //Bold
            if (oldStyle.Bold != newStyle.Bold)
            {
                result.AddRange(newStyle.Bold ? GenerateControlCode(1) : GenerateControlCode(22));
            }

            //Italics
            if (oldStyle.Italics != newStyle.Italics)
            {
                result.AddRange(newStyle.Italics ? GenerateControlCode(3) : GenerateControlCode(23));
            }

            //Underline
            if (oldStyle.Underline != newStyle.Underline)
            {
                result.AddRange(newStyle.Underline ? GenerateControlCode(4) : GenerateControlCode(24));
            }

            return result;
        }

        public void SetStyle(AnsiStyle style)
        {
            var newControlCodes = GenerateControlCodes(_currentStyle, style);

            _currentBuffer.AddRange(newControlCodes);

            _currentStyle = style;
        }

        public void AddText(string text)
        {
            _currentBuffer.AddRange(Encoding.ASCII.GetBytes(text));
        }

        public byte[] GetData()
        {
            return _currentBuffer.ToArray();
        }

        public void Clear()
        {
            _currentBuffer.Clear();
        }
    }
}