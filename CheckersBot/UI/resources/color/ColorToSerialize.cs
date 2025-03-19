
using System.Windows.Media;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CheckersBot.UI.resources.color

{
    internal class ColorToSerialize
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public Color ToColor()
        {
            var color = System.Drawing.Color.FromArgb(R, G, B);
            Console.WriteLine(Color.FromRgb(color.R, color.G, color.B));
            return Color.FromRgb(color.R,color.G,color.B);
        }
    }
}
