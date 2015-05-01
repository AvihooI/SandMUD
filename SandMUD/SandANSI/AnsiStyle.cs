namespace SandDataGenerator
{
    public enum AnsiColor
    {
        Black = 0,
        Red = 1,
        Green = 2,
        Yellow = 3,
        Blue = 4,
        Magenta = 5,
        Cyan = 6,
        White = 7,
        Default = 8
    }

    public struct AnsiStyle
    {
        public AnsiColor BackgroundColor;
        public bool Blink;
        public bool Bold;
        public AnsiColor ForegroundColor;
        public bool Italics;
        public bool Underline;

        public AnsiStyle(AnsiColor foregroundColor = AnsiColor.White, AnsiColor backgroundColor = AnsiColor.Black,
            bool blink = false, bool bold = false, bool italics = false, bool underline = false)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
            Blink = blink;
            Bold = bold;
            Italics = italics;
            Underline = underline;
        }
    }
}