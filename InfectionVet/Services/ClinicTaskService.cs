using InfectionVet.Models;
using InfectionVet.Utilities;

namespace InfectionVet.Services;

/// <summary>
/// Simulates independent clinic processes running concurrently, showcasing Task.Run, Task.WhenAll,
/// and Task.WhenAny in scenarios that resemble real clinic work.
/// </summary>
public class ClinicTaskService
{
    /// <summary>
    /// Simulates a patient record processing operation.
    /// </summary>
    private Task ProcessPatientRecordAsync()
    {
        return Task.Run(async () =>
        {
            ConsoleUI.WriteInfo("Patient record processing started.");

            await Task.Delay(3000);

            ConsoleUI.WriteSuccess("Patient record processing finished.");
        });
    }

    /// <summary>
    /// Simulates a medical analysis operation.
    /// </summary>
    private Task PerformMedicalAnalysisAsync()
    {
        return Task.Run(async () =>
        {
            ConsoleUI.WriteInfo("Medical analysis started.");

            await Task.Delay(5000);

            ConsoleUI.WriteSuccess("Medical analysis finished.");
        });
    }

    /// <summary>
    /// Simulates an owner notification operation.
    /// </summary>
    private Task NotifyOwnerAsync()
    {
        return Task.Run(async () =>
        {
            ConsoleUI.WriteInfo("Owner notification started.");

            await Task.Delay(2000);

            ConsoleUI.WriteSuccess("Owner notification finished.");
        });
    }

    /// <summary>
    /// Runs all clinic processes concurrently and waits until every one of them is completed.
    /// Task.WhenAll is useful when the application needs the results of every operation before continuing.
    /// </summary>
    public async Task RunAllClinicProcessesAsync()
    {
        ConsoleUI.WriteSectionTitle("Starting all clinic processes (Task.WhenAll)");

        Task patientTask = ProcessPatientRecordAsync();
        Task analysisTask = PerformMedicalAnalysisAsync();
        Task notificationTask = NotifyOwnerAsync();

        await Task.WhenAll(patientTask, analysisTask, notificationTask);

        ConsoleUI.WriteSuccess("All clinic processes completed.");
    }

    /// <summary>
    /// Runs all clinic processes concurrently and continues as soon as the first one completes.
    /// Task.WhenAny is useful when the application can move on after the first result while the
    /// remaining tasks keep running in the background.
    /// </summary>
    public async Task RunFirstCompletedClinicProcessAsync()
    {
        ConsoleUI.WriteSectionTitle("Starting clinic processes and waiting for the first one (Task.WhenAny)");

        Task patientTask = ProcessPatientRecordAsync();
        Task analysisTask = PerformMedicalAnalysisAsync();
        Task notificationTask = NotifyOwnerAsync();

        Task firstCompletedTask = await Task.WhenAny(patientTask, analysisTask, notificationTask);

        ConsoleUI.WriteSuccess("The first clinic process has completed; the other two keep running in the background.");
        ConsoleUI.WriteWarning("Their own completion messages may pop up on a later screen — that is Task.WhenAny in action, not a glitch.");

        await firstCompletedTask;
    }

    /// <summary>
    /// Simulates processing a single patient asynchronously.
    /// </summary>
    /// <param name="patient">The patient being processed.</param>
    private async Task ProcessSinglePatientAsync(Patient patient)
    {
        int processingTime = patient.Id % 2 == 0 ? 2000 : 4000;

        ConsoleUI.WriteInfo($"Processing patient {patient.Name} ({processingTime / 1000}s)...");

        await Task.Delay(processingTime);

        ConsoleUI.WriteSuccess($"Finished processing patient: {patient.Name}");
    }

    /// <summary>
    /// Simulates processing multiple patients concurrently, so the total wait time is bound by the
    /// slowest patient instead of the sum of every patient's processing time.
    /// </summary>
    /// <param name="patients">The patients to process.</param>
    public async Task ProcessPatientsConcurrentlyAsync(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            ConsoleUI.WriteWarning("There are no patients to process.");
            return;
        }

        ConsoleUI.WriteSectionTitle("Starting concurrent patient processing");

        List<Task> patientTasks = patients
            .Select(ProcessSinglePatientAsync)
            .ToList();

        await Task.WhenAll(patientTasks);

        ConsoleUI.WriteSuccess("All patients have been processed.");
    }
}