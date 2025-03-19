using System.Runtime.CompilerServices;
using System.Timers;
using CheckersBot.engine.threads;
using CheckersBot.logic;
using Timer = System.Timers.Timer;

namespace CheckersBot.engine;

/// <summary>
/// Main Thread, which working tasks are calling back to when finished
/// </summary>
/// <param name="baseBoard"> Starting board position</param>
/// <param name="maxDepth"> Maximum depth to search for </param>
/// <param name="maxTimeToCalculate"> Maximum time Thread is allowed to run</param>
public class MasterThread(Board baseBoard, int maxDepth, long maxTimeToCalculate)
{
    /// <summary>
    /// Event, which is being called after right sequence
    /// of moves is found, or time has run out
    /// </summary>
    public event ReportMove ReportMove = null!;

    /// <summary>
    /// Current max depth for working tasks
    /// which is increasing by one 1 every iteration
    /// </summary>
    private volatile int _currDepth;

    /// <summary>
    /// Maximum depth to search on
    /// </summary>
    private int MaxDepth { get; } = maxDepth;

    /// <summary>
    /// Amount of milliseconds thread is allowed to run
    /// </summary>
    private long MaxTimeToCalculate { get; } = maxTimeToCalculate;

    /// <summary>
    /// Best move sequence found so far after at least one full iteration
    /// </summary>
    private volatile MoveSequence _currBestMoveSequence = null!;

    /// <summary>
    /// New best move candidate inside one iteration
    /// </summary>
    private MoveSequence NewBestMoveSequence { get; set; } = null!;

    /// <summary>
    /// New best evaluation inside one iteration
    /// </summary>
    private double NewBestEval { get; set; }

    /// <summary>
    /// holds amount of moves thread is waiting to be processed
    /// </summary>
    private int _movesToGoThrough;

    /// <summary>
    /// If main Thread is working
    /// </summary>
    private bool _working;

    /// <summary>
    /// Starts creating Tasks and starts timer to limit max time spent
    /// </summary>
    public void StartCalculation()
    {
        Timer timer = new Timer(MaxTimeToCalculate);
        timer.Elapsed += ReturnPrematurely!;
        timer.AutoReset = false;
        timer.Start();
        _working = true;
        StartNewDepthIteration();
    }

    /// <summary>
    /// To allow Calculation to stop prematurely, maximum depth on
    /// secondary thread are increasing by 1 every time
    /// </summary>
    private void StartNewDepthIteration()
    {
        Interlocked.Increment(ref _currDepth);
        if (!_working) return;
        if (_currDepth > MaxDepth)
        {
            ReportMove(_currBestMoveSequence, NewBestEval);
            return;
        }

        NewBestEval = baseBoard.ColorToMove.Equals(PieceColor.White) ? double.MaxValue : double.MinValue;
        List<Move> moves = new List<Move>();
        moves.AddRange(baseBoard.GetActualValidMoves());
        _movesToGoThrough = moves.Count;
        foreach (Move move in moves)
        {
            Board boardInThread = baseBoard.Clone();
            boardInThread.MakeAMove(move);
            WorkingTask workingTask = new WorkingTask(boardInThread, boardInThread.ColorToMove, _currDepth - 1, move);
            workingTask.ReportResult += TryNewBestMove;
            WorkingThreadPolling.ScheduleNewWorkingTask(workingTask);
        }
    }

    /// <summary>
    /// Once task has finished it calls back this function,
    /// which compares returned values with current ones
    /// </summary>
    /// <param name="moveSequence"> Best sequence of moves in one task</param>
    /// <param name="eval"> board evaluation value </param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private void TryNewBestMove(MoveSequence moveSequence, double eval)
    {
        if (baseBoard.ColorToMove.Equals(PieceColor.Black))
        {
            if (NewBestEval <= eval)
            {
                NewBestEval = eval;
                NewBestMoveSequence = moveSequence;
            }
        }
        else
        {
            if (NewBestEval >= eval)
            {
                NewBestEval = eval;
                NewBestMoveSequence = moveSequence;
            }
        }

        _movesToGoThrough--;
        if (_movesToGoThrough == 0)
        {
            _currBestMoveSequence = NewBestMoveSequence;
            StartNewDepthIteration();
        }
    }

    /// <summary>
    /// Called when timer runs out and needs to stop all
    /// thread and return best move sequence found so far
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReturnPrematurely(object sender, ElapsedEventArgs e)
    {
        _working = false;
        ReportMove(_currBestMoveSequence, NewBestEval);
        WorkingThreadPolling.InterruptAllThreads();
    }
}