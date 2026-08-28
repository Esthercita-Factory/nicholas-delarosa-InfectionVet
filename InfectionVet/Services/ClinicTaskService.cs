using InfectionVet.Models;

namespace InfectionVet.Services;

/// <summary>
/// Simulates independent clinic processes running asynchronously.
/// </summary>
public class ClinicTaskService
{
    /// <summary>
    /// Simulates a patient record processing operation.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous patient record processing operation.</returns>
    private Task ProcessPatientRecordAsync()
    {
        return Task.Run(async () =>
        {
            Console.WriteLine("Patient record processing started.");
            
            await Task.Delay(3000);
            
            Console.WriteLine("Patient record processing finished.");
        });
    }
    
    /// <summary>
    /// Simulates a medical analysis operation.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous medical analysis operation.</returns>
    private Task PerformMedicalAnalysisAsync()
    {
        return Task.Run(async () =>
        {
            Console.WriteLine("Medical analysis started.");
            
            await Task.Delay(5000);
            
            Console.WriteLine("Medical analysis finished.");
        });
    }
    
    /// <summary>
    /// Simulates an owner notification operation.
    /// </summary>
    /// <returns>A <see cref="Task"/> that represents the asynchronous owner notification operation.</returns>
    private Task NotifyOwnerAsync()
    {
        return Task.Run(async () =>
        {
            Console.WriteLine("Owner notification started.");
            
            await Task.Delay(2000);
            
            Console.WriteLine("Owner notification finished.");
        });
    }

    /// <summary>
    /// Runs all clinic processes concurrently and waits until every process is completed.
    /// Task.WhenAll is useful when the application needs the results of every operation before continuing.
    /// </summary>
    public async Task RunAllClinicProcessesAsync()
    {
        Console.WriteLine("\nStarting all clinic processes...");

        Task patientTask = ProcessPatientRecordAsync();
        Task analysisTask = PerformMedicalAnalysisAsync();
        Task notificationTask = NotifyOwnerAsync();

        await Task.WhenAll(
            patientTask,
            analysisTask,
            notificationTask);
        
        Console.WriteLine("All clinic processes completed.");
    }

    /// <summary>
    /// Runs all clinic processes concurrently and continues when the first process completes.
    /// Task.WhenAny is useful when the application can continue after receiving the first completed operation while the remaining tasks continue running.
    /// </summary>
    public async Task RunFirstCompletedClinicProcessAsync()
    {
        Console.WriteLine("\nStarting clinic processes and waiting for the first one...");
        
        Task patientTask = ProcessPatientRecordAsync();
        Task analysisTask = PerformMedicalAnalysisAsync();
        Task notificationTask = NotifyOwnerAsync();

        Task completedTask = await Task.WhenAny(
            patientTask,
            analysisTask,
            notificationTask);
        
        Console.WriteLine("The first clinic process has completed.");

        await completedTask;
    }

    /// <summary>
    /// Simulates processing a single patient asynchronously.
    /// </summary>
    /// <param name="patient">The patient being processed.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    private async Task ProcessSinglePatientAsync(Patient patient)
    {
        int processingTime = patient.Id % 2 == 0 ? 2000 : 4000;

        Console.WriteLine($"Processing patient {patient.Name} ({processingTime / 1000}s)...");
        
        await Task.Delay(processingTime);
        
        Console.WriteLine($"Finished processing patient: {patient.Name}");
    }

    /// <summary>
    /// Simulates processing multiple patients concurrently.
    /// </summary>
    /// <param name="patients">The patients to process.</param>
    /// <returns>A <see cref="Task"/> that completes when all patients have been processed.</returns>
    public async Task ProcessPatientsConcurrentlyAsync(List<Patient> patients)
    {
        Console.WriteLine("\nStarting concurrent patient processing...");

        List<Task> patientTasks = [];

        foreach (Patient patient in patients)
        {
            patientTasks.Add(ProcessSinglePatientAsync(patient));
        }

        await Task.WhenAll(patientTasks);
        
        Console.WriteLine("All patients have been processed.");
    }
}