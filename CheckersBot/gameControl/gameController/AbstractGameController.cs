using CheckersBot.gameControl.notifier;
using CheckersBot.logic;

namespace CheckersBot.gameControl.gameController;

public delegate void OnMove(Move move);
public abstract class AbstractGameController(Board board)
{
    protected PieceColor ColorToMove { get; private set; } =PieceColor.Black;
    public Board Board { get; } = board;
    private HashSet<INotifyState> StatesSubs { get; } = new HashSet<INotifyState>();
    public event OnMove OnMovePlayed = null!;
    private bool _gameStarted;

    public void StartGame()
    {
        _gameStarted = true;
        StartGameInternal();
    }

    protected abstract void StartGameInternal();

    public virtual void MakeAMove(Move move)
    {
        Board.MakeAMove(move);
        ColorToMove = MoveUtils.SwitchColor(ColorToMove);
        foreach (var sub in StatesSubs)
        {
            sub.NotifyChangeColor(Board.ColorToMove);
            sub.NotifyWin(Board.WhoWonTheGame());
        }
        OnMovePlayed(move);
    }

    public bool IsMoveValid(Move move)
    {
        return Board.IsMoveValid(move) && _gameStarted;
    }

    public void AddStateNotifier(INotifyState sub)
    {
        StatesSubs.Add(sub);
    }
}