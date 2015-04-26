using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandDataGenerator
{ 
    /*TODO:
     * a) implement all possible ANSI stuff
     * b) add direct property access for style elements within the generator
    */
    public class DataGenerator
    {
        private List<byte> _currentBuffer;
        private AnsiStyle _currentStyle;


        public DataGenerator()
        {
            _currentBuffer = new List<byte>();
            _currentStyle = new AnsiStyle();
        }

        static private IEnumerable<byte> generateControlCode(int code)
        {
            var result = new List<byte>();

            result.Add(27); //ASCII for Escape
            result.Add(91); //ASCII for '['

            var codeBytes =  Encoding.ASCII.GetBytes(code.ToString()); //Create the appropriate code number (ascii equivalent - one or two bytes possible)
            result.AddRange(codeBytes); //Add to the result
           
            result.Add(109); //ASCII for 'm'

            return result;
        }

        static private IEnumerable<byte> generateControlCodes(AnsiStyle oldStyle, AnsiStyle newStyle)
        {
            var result = new List<byte>();

            //Foreground color
            if (oldStyle.ForegroundColor != newStyle.ForegroundColor)
            {
                int code = 30 + (int)newStyle.ForegroundColor;
                result.AddRange(generateControlCode(code));
            }

            //Background color
            if (oldStyle.BackgroundColor != newStyle.BackgroundColor)
            {
                int code = 40 + (int)newStyle.BackgroundColor;
                result.AddRange(generateControlCode(code));
            }

            //Blink
            if (oldStyle.Blink != newStyle.Blink)
            {
                if (newStyle.Blink)
                    result.AddRange(generateControlCode(5));
                else
                    result.AddRange(generateControlCode(25));
            }

            //Bold
            if (oldStyle.Bold != newStyle.Bold)
            {
                if (newStyle.Bold)
                    result.AddRange(generateControlCode(1));
                else
                    result.AddRange(generateControlCode(22));
            }

            //Italics
            if (oldStyle.Italics != newStyle.Italics)
            {
                if (newStyle.Italics)
                    result.AddRange(generateControlCode(3));
                else
                    result.AddRange(generateControlCode(23));
            }

            //Underline
            if (oldStyle.Underline != newStyle.Underline)
            {
                if (newStyle.Underline)
                    result.AddRange(generateControlCode(4));
                else
                    result.AddRange(generateControlCode(24));
            }

            return result;
        }

        public void SetStyle(AnsiStyle style)
        {

            var newControlCodes = generateControlCodes(_currentStyle, style);

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
