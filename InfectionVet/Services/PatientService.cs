using InfectionVet.Exceptions;
using InfectionVet.Models;
using InfectionVet.Utilities;

namespace InfectionVet.Services;

/// <summary>
/// Provides the CRUD operations, owner management, and LINQ-based reporting used to run the clinic's
/// patient records. Console interaction lives here rather than in a separate presentation layer,
/// consistent with the rest of this project's structure.
/// </summary>
public class PatientService
{
    /// <summary>
    /// Registers a new patient (and its owner, when new) asynchronously. Awaiting the simulated delay
    /// keeps the calling thread free instead of blocking it while the "save" operation completes.
    /// </summary>
    /// <param name="patients">The collection of registered patients.</param>
    /// <param name="patientDictionary">The dictionary of registered patients, keyed by ID for fast lookup.</param>
    /// <param name="nextPatientId">The ID assigned to the new patient.</param>
    public async Task RegisterPatientAsync(
        List<Patient> patients,
        Dictionary<int, Patient> patientDictionary,
        int nextPatientId)
    {
        ConsoleUI.WriteInfo("Starting patient registration...");

        string name = ConsoleUI.ReadRequiredString("Enter patient name");
        int age = ConsoleUI.ReadInt("Enter patient age");

        if (age < 0)
        {
            throw new InvalidPatientAgeException(age);
        }

        string symptom = ConsoleUI.ReadRequiredString("Enter symptom");
        string species = ConsoleUI.ReadRequiredString("Enter species");
        string breed = ConsoleUI.ReadOptionalString("Enter breed (press Enter if unknown)");

        string ownerName = ConsoleUI.ReadRequiredString("Enter owner's name");
        string ownerPhone = ConsoleUI.ReadRequiredString("Enter owner's phone");
        string ownerAddress = ConsoleUI.ReadRequiredString("Enter owner's address");

        ConsoleUI.WriteInfo("Processing patient registration...");

        // Simulates a slower operation, such as a database write, without blocking the menu thread.
        await Task.Delay(1500);

        Client? owner = patients
            .Select(patient => patient.Owner)
            .FirstOrDefault(client =>
                client.Name.Equals(ownerName, StringComparison.OrdinalIgnoreCase) &&
                client.Phone == ownerPhone);

        bool isNewOwner = owner is null;

        if (owner is null)
        {
            int ownerId = patients
                .Select(patient => patient.Owner.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            owner = new Client(ownerId, ownerName, ownerPhone, ownerAddress);
        }

        Patient patient = new Patient(
            nextPatientId,
            name,
            age,
            symptom,
            species,
            breed,
            owner);

        owner.AddPatient(patient);

        patients.Add(patient);
        patientDictionary.Add(patient.Id, patient);

        // Patient implements both IRegistrable and INotifiable, so a single registration exercises
        // both contracts. Client implements IRegistrable too, but only needs to register once.
        patient.Register();
        patient.SendNotification();

        if (isNewOwner)
        {
            owner.Register();
        }
    }

    /// <summary>
    /// Lists every registered patient as an aligned table.
    /// </summary>
    /// <param name="patients">The patients to list.</param>
    public void ListPatients(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            ConsoleUI.WriteWarning("No patients registered.");
            return;
        }

        ConsoleUI.WriteSectionTitle("Registered patients");
        PrintPatientTableHeader();

        foreach (Patient patient in patients)
        {
            PrintPatientRow(patient);
        }
    }

    /// <summary>
    /// Finds a patient by name and displays its information.
    /// </summary>
    /// <param name="patients">The patients to search.</param>
    /// <param name="name">The name to search for.</param>
    /// <exception cref="PatientNotFoundException">Thrown when no patient matches the given name.</exception>
    public void FindPatientByName(List<Patient> patients, string name)
    {
        Patient patient = patients.FirstOrDefault(
            p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new PatientNotFoundException($"name '{name}'");

        ConsoleUI.WriteSectionTitle("Patient found");
        PrintPatientTableHeader();
        PrintPatientRow(patient);
    }

    /// <summary>
    /// Finds a patient by ID and displays its information.
    /// </summary>
    /// <param name="patientDictionary">The patient lookup dictionary.</param>
    /// <param name="id">The ID to search for.</param>
    /// <exception cref="PatientNotFoundException">Thrown when no patient matches the given ID.</exception>
    public void FindPatientById(Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            throw new PatientNotFoundException($"ID {id}");
        }

        ConsoleUI.WriteSectionTitle("Patient found");
        PrintPatientTableHeader();
        PrintPatientRow(patient);
    }

    /// <summary>
    /// Updates the information of an existing patient.
    /// </summary>
    /// <param name="patientDictionary">The patient lookup dictionary.</param>
    /// <param name="id">The ID of the patient to update.</param>
    /// <exception cref="PatientNotFoundException">Thrown when no patient matches the given ID.</exception>
    /// <exception cref="InvalidPatientAgeException">Thrown when the new age is negative.</exception>
    public void UpdatePatient(Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            throw new PatientNotFoundException($"ID {id}");
        }

        string name = ConsoleUI.ReadRequiredString("Enter new patient name");
        int age = ConsoleUI.ReadInt("Enter new patient age");

        if (age < 0)
        {
            throw new InvalidPatientAgeException(age);
        }

        string symptom = ConsoleUI.ReadRequiredString("Enter new symptom");
        string species = ConsoleUI.ReadRequiredString("Enter new species");
        string breed = ConsoleUI.ReadOptionalString("Enter new breed (press Enter if unknown)");

        patient.UpdateName(name);
        patient.UpdateAge(age);
        patient.UpdateSpecies(species);
        patient.Symptom = symptom;
        patient.Breed = breed;

        ConsoleUI.WriteSuccess("Patient updated successfully.");
    }

    /// <summary>
    /// Deletes a patient from both the patient collection and the owner's list of patients, keeping
    /// the two in sync.
    /// </summary>
    /// <param name="patients">The patient collection.</param>
    /// <param name="patientDictionary">The patient lookup dictionary.</param>
    /// <param name="id">The ID of the patient to delete.</param>
    /// <exception cref="PatientNotFoundException">Thrown when no patient matches the given ID.</exception>
    public void DeletePatient(List<Patient> patients, Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            throw new PatientNotFoundException($"ID {id}");
        }

        patientDictionary.Remove(id);
        patients.Remove(patient);
        patient.Owner.RemovePatient(patient);

        ConsoleUI.WriteSuccess("Patient deleted successfully.");
    }

    /// <summary>
    /// Finds patients of a specific species, orders them by age, and projects only the patient's name
    /// and the owner's phone number — an example of chaining Where, OrderBy, and Select in one query.
    /// </summary>
    /// <param name="patients">The patients to search.</param>
    /// <param name="species">The species to filter by.</param>
    public void ShowPatientsBySpecies(List<Patient> patients, string species)
    {
        var result = patients
            .Where(patient => patient.Species.Equals(species, StringComparison.OrdinalIgnoreCase))
            .OrderBy(patient => patient.Age)
            .Select(patient => new
            {
                PatientName = patient.Name,
                OwnerPhone = patient.Owner.Phone
            })
            .ToList();

        if (result.Count == 0)
        {
            ConsoleUI.WriteWarning($"No patients of species '{species}' were found.");
            return;
        }

        ConsoleUI.WriteSectionTitle($"Patients of species '{species}', ordered by age");

        foreach (var entry in result)
        {
            ConsoleUI.WriteInfo($"{entry.PatientName} — owner phone: {entry.OwnerPhone}");
        }
    }

    /// <summary>
    /// Answers a handful of practical questions about the patient collection using LINQ: the youngest
    /// and oldest patients, how many patients exist per species, whether any patient has no defined
    /// breed, and every patient's name in uppercase, alphabetically ordered.
    /// </summary>
    /// <param name="patients">The patients to analyze.</param>
    public void RunPatientStatistics(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            ConsoleUI.WriteWarning("There are no patients to analyze.");
            return;
        }

        ConsoleUI.WriteSectionTitle("Patient statistics");

        Patient youngestPatient = patients.OrderBy(patient => patient.Age).First();
        Patient oldestPatient = patients.OrderByDescending(patient => patient.Age).First();

        ConsoleUI.WriteInfo($"Youngest patient: {youngestPatient.Name}, {youngestPatient.Age} years old.");
        ConsoleUI.WriteInfo($"Oldest patient: {oldestPatient.Name}, {oldestPatient.Age} years old.");

        ConsoleUI.WriteInfo("Patients by species:");

        foreach (var group in patients.GroupBy(patient => patient.Species))
        {
            ConsoleUI.WriteInfo($"  {group.Key}: {group.Count()}");
        }

        bool hasPatientWithoutBreed = patients.Any(patient => string.IsNullOrWhiteSpace(patient.Breed));
        ConsoleUI.WriteInfo($"Has at least one patient without a defined breed: {hasPatientWithoutBreed}");

        ConsoleUI.WriteInfo("Patient names (uppercase, alphabetical order):");

        foreach (string name in patients.Select(patient => patient.Name.ToUpper()).OrderBy(name => name))
        {
            ConsoleUI.WriteInfo($"  {name}");
        }
    }

    /// <summary>
    /// Walks through the core LINQ methods — Where, Select, OrderBy, OrderByDescending, GroupBy,
    /// First, FirstOrDefault, Any, All, and Count — using both method syntax and query syntax, so the
    /// two styles can be compared side by side.
    /// </summary>
    /// <param name="patients">The patients to query.</param>
    public void DemonstrateLinqQueries(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            ConsoleUI.WriteWarning("There are no patients to run LINQ demonstrations on.");
            return;
        }

        const int ageThreshold = 5;

        ConsoleUI.WriteSectionTitle("Method syntax");

        ConsoleUI.WriteInfo($"Where — patients older than {ageThreshold}:");
        foreach (Patient patient in patients.Where(patient => patient.Age > ageThreshold))
        {
            ConsoleUI.WriteInfo($"  {patient.Name}");
        }

        ConsoleUI.WriteInfo("Select — patient names:");
        foreach (string name in patients.Select(patient => patient.Name))
        {
            ConsoleUI.WriteInfo($"  {name}");
        }

        ConsoleUI.WriteInfo("OrderBy — patients ordered by name:");
        foreach (Patient patient in patients.OrderBy(patient => patient.Name))
        {
            ConsoleUI.WriteInfo($"  {patient.Name}");
        }

        ConsoleUI.WriteInfo("OrderByDescending — patients ordered by age:");
        foreach (Patient patient in patients.OrderByDescending(patient => patient.Age))
        {
            ConsoleUI.WriteInfo($"  {patient.Name} ({patient.Age})");
        }

        ConsoleUI.WriteInfo("GroupBy — patients grouped by species:");
        foreach (var group in patients.GroupBy(patient => patient.Species))
        {
            ConsoleUI.WriteInfo($"  {group.Key}: {string.Join(", ", group.Select(patient => patient.Name))}");
        }

        Patient firstPatient = patients.First();
        ConsoleUI.WriteInfo($"First — {firstPatient.Name}");

        Patient? firstOlderPatient = patients.FirstOrDefault(patient => patient.Age > ageThreshold);
        ConsoleUI.WriteInfo(firstOlderPatient is null
            ? $"FirstOrDefault — no patient older than {ageThreshold} was found."
            : $"FirstOrDefault — {firstOlderPatient.Name} is the first patient older than {ageThreshold}.");

        bool anyWithFever = patients.Any(patient => patient.Symptom.Equals("Fever", StringComparison.OrdinalIgnoreCase));
        ConsoleUI.WriteInfo($"Any — at least one patient has a fever: {anyWithFever}");

        bool allHaveOwners = patients.All(patient => patient.Owner is not null);
        ConsoleUI.WriteInfo($"All — every patient has an owner: {allHaveOwners}");

        int youngPatientCount = patients.Count(patient => patient.Age <= ageThreshold);
        ConsoleUI.WriteInfo($"Count — {patients.Count} total patients, {youngPatientCount} aged {ageThreshold} or younger.");

        ConsoleUI.WriteSectionTitle("Query syntax (equivalent results)");

        var queryNames =
            from patient in patients
            where patient.Age > ageThreshold
            select patient.Name;

        ConsoleUI.WriteInfo($"Where + Select — patients older than {ageThreshold}:");
        foreach (string name in queryNames)
        {
            ConsoleUI.WriteInfo($"  {name}");
        }

        var queryOrderedPatients =
            from patient in patients
            orderby patient.Name
            select patient;

        ConsoleUI.WriteInfo("OrderBy — patients ordered by name:");
        foreach (Patient patient in queryOrderedPatients)
        {
            ConsoleUI.WriteInfo($"  {patient.Name}");
        }

        var queryGroupedPatients =
            from patient in patients
            group patient by patient.Species;

        ConsoleUI.WriteInfo("GroupBy — patients grouped by species:");
        foreach (var group in queryGroupedPatients)
        {
            ConsoleUI.WriteInfo($"  {group.Key}: {string.Join(", ", group.Select(patient => patient.Name))}");
        }
    }

    /// <summary>
    /// Prints the column headings shared by every tabular patient listing.
    /// </summary>
    private static void PrintPatientTableHeader()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($"  {"ID",-4} {"Name",-14} {"Age",-4} {"Species",-10} {"Breed",-12} {"Symptom",-14} {"Owner",-14} {"Phone"}");
        Console.ResetColor();
    }

    /// <summary>
    /// Prints a single patient as an aligned table row.
    /// </summary>
    /// <param name="patient">The patient to print.</param>
    private static void PrintPatientRow(Patient patient)
    {
        string breedDisplay = string.IsNullOrWhiteSpace(patient.Breed) ? "—" : patient.Breed;

        Console.WriteLine($"  {patient.Id,-4} {patient.Name,-14} {patient.Age,-4} {patient.Species,-10} {breedDisplay,-12} {patient.Symptom,-14} {patient.Owner.Name,-14} {patient.Owner.Phone}");
    }
}