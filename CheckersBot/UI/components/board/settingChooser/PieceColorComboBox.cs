using System.Windows.Controls;
using CheckersBot.logic;

namespace CheckersBot.UI.components.board.settingChooser;

public class PieceColorComboBox : ComboBox
{
    public PieceColorComboBox()
    {
        ItemsSource= Enum.GetValues(typeof(PieceColor));
        SelectedItem = PieceColor.Black;
    }
}