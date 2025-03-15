using CheckersBot.gameControl.notifier;
using CheckersBot.logic;

namespace CheckersBot.gameControl.gameController;

public abstract class AbstractGameController(Board board)
{
    public Board Board { get; } = board;
    private HashSet<INotifyState> StatesSubs { get; } = new HashSet<INotifyState>();
    private HashSet<INotifyMove> MoveSubs { get; } = new HashSet<INotifyMove>();
    private bool _gameStarted;

    public void StartGame()
    {
        _gameStarted = true;
        StartGameInternal();
    }

    protected abstract void StartGameInternal();

    public void MakeAMove(Move move)
    {
        Board.MakeAMove(move);
        foreach (var sub in StatesSubs)
        {
            sub.NotifyChangeColor(Board.ColorToMove);
            sub.NotifyWin(Board.WhoWonTheGame());
        }

        foreach (var sub in MoveSubs)
        {
            sub.ReportMove(move);
        }
    }

    public bool IsMoveValid(Move move)
    {
        return Board.IsMoveValid(move) && _gameStarted;
    }

    public void AddStateNotifier(INotifyState sub)
    {
        StatesSubs.Add(sub);
    }

    public void AddMoveNotifier(INotifyMove sub)
    {
        MoveSubs.Add(sub);
    }
}