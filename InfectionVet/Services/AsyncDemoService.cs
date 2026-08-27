namespace InfectionVet.Services;

/// <summary>
/// Demonstrate synchronous and asynchronous execution.
/// Async programming is useful for operations that spend time waiting, such as file access, database queries, network request, and API calls.
/// Using async and await helps avoid unnecessarily blocking the application while those operations are waiting to complete.  
/// </summary>
public class AsyncDemoService
{
    /// <summary>
    /// Simulates a synchronous operation that takes two seconds.
    /// The program waits until the operation finishes before continuing.
    /// </summary>
    public void RunSynchronousExample()
    {
        Console.WriteLine("Synchronous operation started.");
        
        Thread.Sleep(2000);
        
        Console.WriteLine("Synchronous operation completed.");
    }

    /// <summary>
    /// Demonstrates an asynchronous operation using Task.Delay.
    /// Unlike Thread.Sleep, Task.Delay does not block the thread while waiting. 
    /// </summary>
    public async Task RunAsynchronousExample()
    {
        Console.WriteLine("Asynchronous operation started.");

        await Task.Delay(2000);
        
        Console.WriteLine("Asynchronous operation completed.");
    }
}