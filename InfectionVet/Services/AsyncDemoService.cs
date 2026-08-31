using System.Diagnostics;
using InfectionVet.Utilities;

namespace InfectionVet.Services;

/// <summary>
/// Demonstrates the practical difference between synchronous and asynchronous execution.
/// Async programming avoids blocking the calling thread while an operation is waiting on something
/// external (a delay, a disk read, a network call), which keeps the application responsive. It is most
/// valuable when the wait itself is the bottleneck, not the CPU work.
/// </summary>
public class AsyncDemoService
{
    private const int SimulatedDelayMilliseconds = 2000;

    /// <summary>
    /// Simulates a synchronous operation. The calling thread is blocked for the entire duration.
    /// </summary>
    public void RunSynchronousExample()
    {
        ConsoleUI.WriteInfo("Synchronous operation started. The thread is now blocked...");

        Thread.Sleep(SimulatedDelayMilliseconds);

        ConsoleUI.WriteSuccess("Synchronous operation completed.");
    }

    /// <summary>
    /// Simulates an asynchronous operation. Awaiting Task.Delay frees the calling thread while the
    /// delay elapses instead of holding it hostage, which is what makes the application stay responsive.
    /// </summary>
    public async Task RunAsynchronousExampleAsync()
    {
        ConsoleUI.WriteInfo("Asynchronous operation started. The thread is free to do other work...");

        await Task.Delay(SimulatedDelayMilliseconds);

        ConsoleUI.WriteSuccess("Asynchronous operation completed.");
    }

    /// <summary>
    /// Runs the synchronous and asynchronous examples back to back and times each one, turning the
    /// difference between the two execution models into something the user can actually see.
    /// </summary>
    public async Task CompareExecutionModesAsync()
    {
        ConsoleUI.WriteSectionTitle("Synchronous execution");

        Stopwatch synchronousStopwatch = Stopwatch.StartNew();

        RunSynchronousExample();

        synchronousStopwatch.Stop();
        ConsoleUI.WriteInfo($"Elapsed time: {synchronousStopwatch.ElapsedMilliseconds} ms.");

        ConsoleUI.WriteSectionTitle("Asynchronous execution");

        Stopwatch asynchronousStopwatch = Stopwatch.StartNew();
        Task demoTask = RunAsynchronousExampleAsync();

        ConsoleUI.WriteInfo("The menu thread could keep doing other work here instead of waiting immediately.");

        await demoTask;

        asynchronousStopwatch.Stop();
        ConsoleUI.WriteInfo($"Elapsed time: {asynchronousStopwatch.ElapsedMilliseconds} ms.");
    }
}