using CheckersBot.logic;

namespace CheckersBot.gameControl.notifier;

public interface INotifyMove
{
    public void ReportMove(Move move);
}