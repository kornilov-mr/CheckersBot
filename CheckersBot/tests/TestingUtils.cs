using CheckersBot.engine;
using CheckersBot.engine.threads;
using CheckersBot.logic;

namespace CheckersBot.tests;

public class TestingUtils
{
    public static Task<(MoveSequence, double)> WaitForResultReport(WorkingTask task)
    {
        var tcs = new TaskCompletionSource<(MoveSequence, double)>();

        ReportMove handler = null!;
        handler = (move, eval) =>
        {
            task.ReportResult -= handler;
            tcs.SetResult((move,eval));
        };
        task.ReportResult += handler;
        return tcs.Task;
    }
    public static Task<(MoveSequence, double)> WaitForResultReport(MasterThread thread)
    {
        var tcs = new TaskCompletionSource<(MoveSequence, double)>();

        ReportMove handler = null!;
        handler = (move, eval) =>
        {
            thread.ReportMove -= handler;
            tcs.SetResult((move,eval));
        };
        thread.ReportMove += handler;
        return tcs.Task;
    }

    public static bool CompareMoveSequenceWithEval((MoveSequence, double) sequence1, (MoveSequence, double) sequence2)
    {
        return sequence2.Item1.Equals(sequence1.Item1) &&
               Math.Abs(Math.Round(sequence1.Item2, 6) - Math.Round(sequence2.Item2, 6)) < 0.00001;
    }
}