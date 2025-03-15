using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CheckersBot.gameControl.gameController;
using CheckersBot.logic;

namespace CheckersBot;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly Board _defaultBoard = new Board(new BoardPositionSetting(
        PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
    public MainWindow()
    {
        ConsoleGameController controller = new ConsoleGameController(_defaultBoard);
        controller.StartGame();
        InitializeComponent();
    }
}