using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CheckersBot.gameControl.gameController;
using CheckersBot.logic;

namespace CheckersBot.UI.components.board;

public partial class PlayingField : UserControl
{
    private AbstractGameController? GameController { get; set; }

    public PlayingField()
    {
        InitializeComponent();
        MaxCalculationTimeTextBox.Text = "5000";
    }

    public void SetUpBoard(AbstractGameController gameController)
    {
        GameController = gameController;
        GameController.StartGame();
        PiecesLayer.SetUpBoard(GameController);
    }
    private void IntegerTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
    }
    private void CreateGame(object sender, RoutedEventArgs e)
    {
        Board board = new Board(new BoardPositionSetting(
            PathResolver.ResolvePathFromSolutionRoot("/tests/startingPositions/defaultPosition.txt")));
        BotGameController gameController = new BotGameController(board, (PieceColor)ColorComboBox.SelectedItem, 
            long.Parse(MaxCalculationTimeTextBox.Text));
        SetUpBoard(gameController);
    }
    
}