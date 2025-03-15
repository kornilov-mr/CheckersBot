using CheckersBot.logic;

namespace CheckersBot.gameControl.notifier;

public interface INotifyState
{
    public void NotifyChangeColor(PieceColor? color);
    public void NotifyWin(PieceColor? color);

}