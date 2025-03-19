using CheckersBot.engine;
using CheckersBot.logic;

namespace CheckersBot.gameControl.gameController;

/// <summary>
/// Controller for playing with a bot
/// </summary>
/// <param name="board"> starting board </param>
/// <param name="playerColor"> Color of which player wants to play with </param>
/// <param name="maxTimerForEngine"> Max Time before engine terminates </param>
public class BotGameController(Board board, PieceColor playerColor, long maxTimerForEngine)
    : AbstractGameController(board)
{
    private readonly int _maxSearchDepth = int.Parse(
        Environment.GetEnvironmentVariable("MAX_SEARCH_DEPTH")!);
    private PieceColor PlayerColor { get; set; } = playerColor;
    private long MaxTimerForEngine { get; } = maxTimerForEngine;


    protected override void StartGameInternal()
    {
    }
    /// <summary>
    /// After making a move starts the engine 
    /// </summary>
    /// <param name="move"></param>
    public override void MakeAMove(Move move)
    {
        if (!PlayerColor.Equals(ColorToMove)) return;
        base.MakeAMove(move);
        if (PlayerColor.Equals(MoveUtils.SwitchColor(ColorToMove)))
        {
            MasterThread masterThread = new MasterThread(Board, _maxSearchDepth, MaxTimerForEngine);
            masterThread.ReportMove += WaitForBotSequence;
            Thread thread = new Thread(masterThread.StartCalculation);
            thread.Start();
        }
    }
    /// <summary>
    /// Function, which is passed as event to the engine and fires after engine stops
    /// </summary>
    /// <param name="moveSequence"> Sequence of best Moves the engine calculated </param>
    /// <param name="eval"> evaluation of the position </param>
    private void WaitForBotSequence(MoveSequence moveSequence, double eval)
    {
        Console.WriteLine(moveSequence.GetFirstMove());
        base.MakeAMove(moveSequence.GetFirstMove());
    }
}