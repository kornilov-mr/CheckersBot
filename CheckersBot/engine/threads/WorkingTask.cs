using CheckersBot.logic;
using CheckersBot.utils;
using Microsoft.Extensions.Logging;

namespace CheckersBot.engine.threads;

public delegate void CallBackOnExit();

public delegate void ReportMove(MoveSequence moveSequence, double eval);

/// <summary>
/// Separated Thread, which goes through all move on one branch via DPS
/// All Threads are using different board, to achieve Thread-safety
/// </summary>
public class WorkingTask
{
    /// <summary>
    /// logger, that write to the console
    /// </summary>
    private readonly ILogger _consoleLogger = LoggerSingleton.CreateLogger<WorkingTask>();

    /// <summary>
    /// Maximum depth of DFS
    /// </summary>
    private int MaxDepth { get; }

    /// <summary>
    /// Event that fires before exiting
    /// </summary>
    public event CallBackOnExit OnExit = delegate { };

    /// <summary>
    /// Event that fires after going through all moves
    /// </summary>
    public event ReportMove ReportResult = null!;

    /// <summary>
    /// Board object, which is used only in one thread to achieve thread-safety
    /// </summary>
    private Board BoardInThread { get; }

    /// <summary>
    /// Next player's color to move
    /// </summary>
    private PieceColor ColorToMove { get; }

    /// <summary>
    /// Starting move sequence (empty if only using one Thread, because it searches from the root)
    /// </summary>
    private MoveSequence MoveSequence { get; }

    public WorkingTask(Board boardInThread, PieceColor colorToMove, int maxDepth)
    {
        BoardInThread = boardInThread;
        ColorToMove = colorToMove;
        MaxDepth = maxDepth;
        MoveSequence = new MoveSequence();
    }

    public WorkingTask(Board boardInThread, PieceColor colorToMove, int maxDepth, Move startingMove) : this(
        boardInThread, colorToMove, maxDepth)
    {
        MoveSequence.PushMoveToBack(startingMove);
    }

    /// <summary>
    /// Start calculating all moves via DFS
    /// </summary>
    public void Run()
    {
        try
        {
            _consoleLogger.LogInformation("Started new WorkingTask \n" +
                                          "Starting Color: " + ColorToMove + "\n" +
                                          "maxDepth: " + MaxDepth);
            if (LoggerSingleton.DetailedLogger)
                _consoleLogger.LogInformation("Board: " + BoardInThread);
            (MoveSequence, double) result;

            result = DfsEvaluation(BoardInThread, 0, null);

            ReportResult.Invoke(MoveSequence.JoinSequence(result.Item1), result.Item2);
            OnExit.Invoke();
        }
        catch (ThreadInterruptedException)
        {
            _consoleLogger.LogInformation($"Thread {Thread.CurrentThread.ManagedThreadId} was interrupted!");
            return;
        }
    }

    /// <summary>
    /// Main DFS body
    /// </summary>
    /// <param name="boardInThread"> Board, which handles all pieces on the board and available moves</param>
    /// <param name="currentDepth"> Current depth of the DFS</param>
    /// <param name="prevMove"> Previous move, null at the root </param>
    /// <returns></returns>
    private (MoveSequence, double) DfsEvaluation(Board boardInThread, int currentDepth, Move? prevMove)
    {
        if (prevMove != null)
            boardInThread.MakeAMove(prevMove);
        if (boardInThread.WhoWonTheGame() != null)
        {
            return (new MoveSequence(),
                boardInThread.WhoWonTheGame().Equals(PieceColor.White) ? double.MinValue : double.MaxValue);
        }

        if (currentDepth >= MaxDepth)
        {
            if (LoggerSingleton.DetailedLogger)
                _consoleLogger.LogInformation("DFS Reached bottom Board: " + BoardInThread);

            return (new MoveSequence(), boardInThread.EvaluateBoard());
        }

        double result = boardInThread.ColorToMove.Equals(PieceColor.White) ? double.MaxValue : double.MinValue;
        MoveSequence bestMoveSequence = null!;
        List<Move> moves = new List<Move>();
        moves.AddRange(boardInThread.GetActualValidMoves());
        foreach (var move in moves)
        {
            var temp = DfsEvaluation(boardInThread, currentDepth + 1, move);
            if (boardInThread.ColorToMove.Equals(PieceColor.White))
            {
                if (result <= temp.Item2)
                {
                    result = temp.Item2;
                    bestMoveSequence = new MoveSequence().PushMoveToBack(move).JoinSequence(temp.Item1);
                }
            }
            else
            {
                if (result >= temp.Item2)
                {
                    result = temp.Item2;
                    bestMoveSequence = new MoveSequence().PushMoveToBack(move).JoinSequence(temp.Item1);
                }
            }

            boardInThread.UndoLastMove();
        }

        return (bestMoveSequence, result);
    }
}