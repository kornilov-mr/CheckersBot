using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using CheckersBot.UI.resources;

namespace CheckersBot.UI.components.board;

public class SquareLayer: UniformGrid 
{
    private int _squareSize = int.Parse(
        Environment.GetEnvironmentVariable("PLAYING_SQUARE_SIZE")!);

    private Color _playingFieldColor1 = Resource.GetColor("PlayingField1"); 
    private Color _playingFieldColor2 = Resource.GetColor("PlayingField2");

    public SquareLayer()
    {
    }

    protected override void OnInitialized(EventArgs e)
    {
        Height = _squareSize*8;
        Width = _squareSize*8;
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var rect = new Rectangle
                {
                    Fill = (row + col) % 2 == 0 ? new SolidColorBrush(_playingFieldColor1) :
                        new SolidColorBrush(_playingFieldColor2)
                };
                Children.Add(rect);
            }
        }
    }
}