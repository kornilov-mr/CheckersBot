using System.Runtime.CompilerServices;
using CheckersBot.utils;
using Microsoft.Extensions.Logging;

namespace CheckersBot.engine.threads;

/// <summary>
/// Static object, which handles Thread allocation for the engine
/// </summary>
public class WorkingThreadPolling
{
    private static readonly ILogger ConsleLogger = LoggerSingleton.CreateLogger<WorkingThreadPolling>();

    /// <summary>
    /// Thread that are available for allocation (comes from maximum thread minus threads for system)
    /// </summary>
    public static int CurrAvailableThreads = Math.Max(
        int.Parse(Environment.GetEnvironmentVariable("MAX_THREADS_COUNT")!)
        , Environment.ProcessorCount - int.Parse(Environment.GetEnvironmentVariable("THREAD_LEFT_FOR_SYSTEM")!));

    /// <summary>
    /// Stack for tasks, that are waiting for allocation
    /// </summary>
    private static Stack<WorkingTask> WaitingTasks { get; } = new Stack<WorkingTask>();

    /// <summary>
    /// Currently running threads
    /// </summary>
    private static HashSet<Thread> RunningThreads { get; } = new HashSet<Thread>();

    /// <summary>
    /// Allocates task to one Thread if possible, or set it to the waiting stack
    /// </summary>
    /// <param name="task"> Task, that goes through one DFS Tree </param>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static void ScheduleNewWorkingTask(WorkingTask task)
    {
        ConsleLogger.LogInformation("Trying to schedule new task" + task);
        if (CurrAvailableThreads == 0)
        {
            ConsleLogger.LogInformation("Pushed task to waiting stack:" + task);
            WaitingTasks.Push(task);
            return;
        }

        CurrAvailableThreads = Interlocked.Decrement(ref CurrAvailableThreads);
        task.OnExit += ThreadExit;
        Thread thread = new Thread(task.Run);
        RunningThreads.Add(thread);
        ConsleLogger.LogInformation("Allocated new Thread for task:" + task);
        thread.Start();
    }

    /// <summary>
    /// When Thread has exited, it calls back to the Pool for notification
    /// </summary>
    [MethodImpl(MethodImplOptions.Synchronized)]
    private static void ThreadExit()
    {
        ConsleLogger.LogInformation("One thread Finished, waiting for next task" + "\n" +
                                    "Current available threads:" + CurrAvailableThreads);
        Interlocked.Increment(ref CurrAvailableThreads);
        if (WaitingTasks.Count != 0)
            ScheduleNewWorkingTask(WaitingTasks.Pop());
    }

    /// <summary>
    /// Stops all Threads
    /// </summary>
    public static void InterruptAllThreads()
    {
        foreach (var thread in RunningThreads)
        {
            thread.Interrupt();
        }

        CurrAvailableThreads = Math.Max(int.Parse(Environment.GetEnvironmentVariable("MAX_THREADS_COUNT")!),
            Environment.ProcessorCount - int.Parse(Environment.GetEnvironmentVariable("THREAD_LEFT_FOR_SYSTEM")!));
    }
}