using InfectionVet.Exceptions;
using InfectionVet.Models;
using InfectionVet.Services;
using InfectionVet.Utilities;

ConsoleUI.Initialize();

List<Patient> patients = [];
Dictionary<int, Patient> patientDictionary = [];
int nextPatientId = 1;

PatientService patientService = new();
ClinicTaskService clinicTaskService = new();
AsyncDemoService asyncDemoService = new();

bool running = true;

while (running)
{
    ConsoleUI.ClearScreen();
    ConsoleUI.WriteBanner("INFECTION VET", "Veterinary Clinic Management System");

    PrintMainMenu();

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("Select an option: ");
    Console.ResetColor();

    string option = (Console.ReadLine() ?? string.Empty).Trim();

    if (option == "0")
    {
        running = false;
        ConsoleUI.WriteSuccess("Goodbye, and thank you for using Infection Vet!");
        break;
    }

    try
    {
        switch (option)
        {
            case "1":
                Task registrationTask = patientService.RegisterPatientAsync(patients, patientDictionary, nextPatientId);

                ConsoleUI.WriteInfo("The menu thread stays responsive while registration is being processed...");

                await registrationTask;

                Logger.LogInfo($"Patient '{patients[^1].Name}' registered successfully with ID {nextPatientId}.");
                nextPatientId++;

                break;

            case "2":
                patientService.ListPatients(patients);
                break;

            case "3":
                string searchName = ConsoleUI.ReadRequiredString("Enter patient name");
                patientService.FindPatientByName(patients, searchName);
                break;

            case "4":
                int searchId = ConsoleUI.ReadInt("Enter patient ID");
                patientService.FindPatientById(patientDictionary, searchId);
                break;

            case "5":
                int updateId = ConsoleUI.ReadInt("Enter the ID of the patient to update");
                patientService.UpdatePatient(patientDictionary, updateId);
                Logger.LogInfo($"Patient with ID {updateId} updated successfully.");
                break;

            case "6":
                int deleteId = ConsoleUI.ReadInt("Enter the ID of the patient to delete");
                patientService.DeletePatient(patients, patientDictionary, deleteId);
                Logger.LogInfo($"Patient with ID {deleteId} deleted successfully.");
                break;

            case "7":
                string species = ConsoleUI.ReadRequiredString("Enter species");
                patientService.ShowPatientsBySpecies(patients, species);
                break;

            case "8":
                patientService.RunPatientStatistics(patients);
                break;

            case "9":
                patientService.DemonstrateLinqQueries(patients);
                break;

            case "10":
                int ownerLookupId = ConsoleUI.ReadInt("Enter the ID of one of the client's patients");

                if (!patientDictionary.TryGetValue(ownerLookupId, out Patient? ownerLookupPatient))
                {
                    throw new PatientNotFoundException($"ID {ownerLookupId}");
                }

                ownerLookupPatient.Owner.DisplayInformation();
                ownerLookupPatient.Owner.DisplayPatients();

                break;

            case "11":
                await clinicTaskService.ProcessPatientsConcurrentlyAsync(patients);
                break;

            case "12":
                await clinicTaskService.RunAllClinicProcessesAsync();
                break;

            case "13":
                await clinicTaskService.RunFirstCompletedClinicProcessAsync();
                break;

            case "14":
                await asyncDemoService.CompareExecutionModesAsync();
                break;

            case "15":
                AttendVeterinaryService();
                break;

            default:
                ConsoleUI.WriteWarning("Invalid option. Please choose a number from the menu.");
                break;
        }
    }
    catch (InvalidPatientAgeException ex)
    {
        ConsoleUI.WriteError(ex.Message);
        Logger.LogError(ex.Message);
    }
    catch (PatientNotFoundException ex)
    {
        ConsoleUI.WriteError(ex.Message);
        Logger.LogError(ex.Message);
    }
    catch (Exception ex)
    {
        // A last-resort guard so an unforeseen error never crashes the menu loop; the full
        // exception is logged for troubleshooting while the user only sees a friendly summary.
        ConsoleUI.WriteError($"An unexpected error occurred: {ex.Message}");
        Logger.LogError($"Unexpected error: {ex}");
    }
    finally
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  Press Enter to return to the menu...");
        Console.ResetColor();
        Console.ReadLine();
    }
}

/// <summary>
/// Prints the main menu, grouped by the kind of work each option performs.
/// </summary>
static void PrintMainMenu()
{
    ConsoleUI.WriteSectionTitle("Patient Records");
    Console.WriteLine("   1. Register a new patient");
    Console.WriteLine("   2. List all patients");
    Console.WriteLine("   3. Find patient by name");
    Console.WriteLine("   4. Find patient by ID");
    Console.WriteLine("   5. Update patient information");
    Console.WriteLine("   6. Delete patient");

    ConsoleUI.WriteSectionTitle("Insights & Queries");
    Console.WriteLine("   7. List patients by species");
    Console.WriteLine("   8. View patient statistics");
    Console.WriteLine("   9. Run LINQ demonstrations");
    Console.WriteLine("  10. View a client's registered patients");

    ConsoleUI.WriteSectionTitle("Asynchronous Operations");
    Console.WriteLine("  11. Process all patients concurrently");
    Console.WriteLine("  12. Run independent clinic processes (Task.WhenAll)");
    Console.WriteLine("  13. Run independent clinic processes (Task.WhenAny)");
    Console.WriteLine("  14. Compare synchronous vs asynchronous execution");

    ConsoleUI.WriteSectionTitle("Veterinary Services");
    Console.WriteLine("  15. Attend a veterinary service");

    Console.WriteLine();
    Console.WriteLine("   0. Exit");
    Console.WriteLine();
}

/// <summary>
/// Lets the user pick a veterinary service and attends it, demonstrating the abstract
/// VeterinaryService hierarchy and the IAtendible interface through real polymorphic dispatch.
/// </summary>
static void AttendVeterinaryService()
{
    Console.WriteLine("   1. General consultation");
    Console.WriteLine("   2. Vaccination");

    Console.ForegroundColor = ConsoleColor.Gray;
    Console.Write("Select a service: ");
    Console.ResetColor();

    string serviceOption = (Console.ReadLine() ?? string.Empty).Trim();

    VeterinaryService? service = serviceOption switch
    {
        "1" => new GeneralConsultation(),
        "2" => new Vaccination(),
        _ => null
    };

    if (service is null)
    {
        ConsoleUI.WriteWarning("Invalid service option.");
        return;
    }

    ConsoleUI.WriteInfo($"Attending: {service.Name}");
    service.Attend();
}