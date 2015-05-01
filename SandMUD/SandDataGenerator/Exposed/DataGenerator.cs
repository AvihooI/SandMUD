using System.Collections.Generic;
using System.Text;

namespace SandDataGenerator
{
    public class DataGenerator
    {
        private readonly List<byte> _currentBuffer;
        private AnsiStyle _currentStyle;

        public DataGenerator()
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
            var result = new List<byte>();

            result.Add(27); //ASCII for Escape
            result.Add(91); //ASCII for '['

            var codeBytes = Encoding.ASCII.GetBytes(code.ToString());
                //Create the appropriate code number (ascii equivalent - one or two bytes possible)
            result.AddRange(codeBytes); //Add to the result

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
                if (newStyle.Blink)
                    result.AddRange(GenerateControlCode(5));
                else
                    result.AddRange(GenerateControlCode(25));
            }

            //Bold
            if (oldStyle.Bold != newStyle.Bold)
            {
                if (newStyle.Bold)
                    result.AddRange(GenerateControlCode(1));
                else
                    result.AddRange(GenerateControlCode(22));
            }

            //Italics
            if (oldStyle.Italics != newStyle.Italics)
            {
                if (newStyle.Italics)
                    result.AddRange(GenerateControlCode(3));
                else
                    result.AddRange(GenerateControlCode(23));
            }

            //Underline
            if (oldStyle.Underline != newStyle.Underline)
            {
                if (newStyle.Underline)
                    result.AddRange(GenerateControlCode(4));
                else
                    result.AddRange(GenerateControlCode(24));
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