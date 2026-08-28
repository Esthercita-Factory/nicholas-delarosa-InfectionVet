using InfectionVet.Exceptions;
using InfectionVet.Interfaces;
using InfectionVet.Models;

namespace InfectionVet.Services;

public class PatientService
{
    /// <summary>
    /// Registers a new patient and its owner.
    /// </summary>
    /// <param name="patients">Represents the patients' class as a list.</param>
    /// <param name="patientDictionary">Represents the patients' class as a dictionary</param>
    /// <param name="id">The ID assigned to the new patient.</param>
    public void RegisterPatient(List<Patient> patients, Dictionary<int, Patient> patientDictionary, int id)
    {
        try
        {
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Patient name cannot be empty.");
                return;
            }

            Console.Write("Enter patient age: ");
            int age = int.Parse(Console.ReadLine() ?? "");

            if (age < 0)
            {
                throw new InvalidPatientAgeException(age);
            }

            Console.Write("Enter symptom: ");
            string symptom = Console.ReadLine() ?? "" ;

            if (string.IsNullOrWhiteSpace(symptom))
            {
                Console.WriteLine("Symptom cannot be empty.");
                return;
            }

            Console.Write("Enter species: ");
            string species = Console.ReadLine() ?? "" ;

            if (string.IsNullOrWhiteSpace(species))
            {
                Console.WriteLine("Species cannot be empty.");
                return;
            }
            
            Console.Write("Enter owner's name: ");
            string ownerName = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(ownerName))
            {
                Console.WriteLine("Owner name cannot be empty.");
                return;
            }
            
            Console.Write("Enter owner's phone: ");
            string ownerPhone = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(ownerPhone))
            {
                Console.WriteLine("Owner phone cannot be empty.");
                return;
            }
            
            Console.Write("Enter owner's address: ");
            string ownerAddress = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(ownerAddress))
            {
                Console.WriteLine("Owner address cannot be empty.");
                return;
            }

            Client? owner = patients
                .Select(patient => patient.Owner)
                .FirstOrDefault(client =>
                    client.Name.Equals(
                        ownerName,
                        StringComparison.OrdinalIgnoreCase)
                    &&
                    client.Phone == ownerPhone
                );

            if (owner == null)
            {
                int ownerId = patients
                    .Select(patient => patient.Owner.Id)
                    .DefaultIfEmpty(0)
                    .Max() + 1;
                
                owner = new Client(
                    ownerId,
                    ownerName,
                    ownerPhone,
                    ownerAddress
                );
            }

            Patient patient = new Patient(
                id,
                name,
                age,
                symptom,
                species,
                owner
            );
            
            owner.AddPatient(patient);
            
            patients.Add(patient);
            patientDictionary.Add(patient.Id, patient);
            
            Console.WriteLine($"\nPatient registered successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid age. Please enter a whole number.");
        }
    }

    /// <summary>
    /// Registers a patient asynchronously and simulates a delayed operation.
    /// Using await prevents the application from blocking while the operation is waiting to complete.
    /// </summary>
    /// <param name="patients">The collection of registered patients.</param>
    /// <param name="patientDictionary">The dictionary of registered patients.</param>
    /// <param name="nextPatientId">The ID assigned to the new patient.</param>
    public async Task RegisterPatientAsync(
        List<Patient> patients,
        Dictionary<int, Patient> patientDictionary,
        int nextPatientId)
    {
        Console.WriteLine("Starting patient registration...");

        Console.Write("Enter patient name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Enter patient age: ");

        if (!int.TryParse(Console.ReadLine(), out int age))
        {
            Console.WriteLine("Invalid age.");
            return;
        }

        if (age < 0)
        {
            throw new InvalidPatientAgeException(age);
        }

        Console.Write("Enter symptom: ");
        string symptom = Console.ReadLine() ?? "";

        Console.Write("Enter species: ");
        string species = Console.ReadLine() ?? "";

        Console.Write("Enter owner's name: ");
        string ownerName = Console.ReadLine() ?? "";

        Console.Write("Enter owner's phone: ");
        string ownerPhone = Console.ReadLine() ?? "";

        Console.Write("Enter owner's address: ");
        string ownerAddress = Console.ReadLine() ?? "";

        Console.WriteLine("Processing patient registration...");

        await Task.Delay(2000);

        Client owner = new Client(
            nextPatientId,
            ownerName,
            ownerPhone,
            ownerAddress);

        Patient patient = new Patient(
            nextPatientId,
            name,
            age,
            symptom,
            species,
            owner);

        owner.AddPatient(patient);

        patients.Add(patient);
        patientDictionary.Add(patient.Id, patient);

        Console.WriteLine("Patient registered successfully.");
    }
    
    public void ListPatients(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            Console.WriteLine("No patients registered.");
        }
        
        foreach (Patient patient in patients)
        {
            if (patient == null)
            {
                Console.WriteLine("Invalid patient record.");
                continue;
            }
            Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Species: {patient.Species} | Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone}");
        }
    }
    
    /// <summary>
    /// Finds a patient by name and displays its information.
    /// </summary>
    /// <param name="patients">Represents the patients' class as a list.</param>
    /// <param name="name">Represents the patients' name.</param>
    public void FindPatientByName(List<Patient> patients, string name)
    {
        Patient? patient = patients.FirstOrDefault(
            p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
        );

        if (patient == null)
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        
        Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Species: {patient.Species} |  Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone} | Address: {patient.Owner.Address}");
    }

    public void FindPatientById(Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        
        Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Species: {patient.Species} | Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone}");
    }

    /// <summary>
    /// Updates the information of an existing patient.
    /// </summary>
    /// <param name="patientDictionary"></param>
    /// <param name="id"></param>
    public void UpdatePatient(Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        
        Console.Write("Enter new patient name: ");
        string name = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Patient name cannot be empty.");
            return;
        }
        
        Console.Write("Enter new patient age: ");

        if (!int.TryParse(Console.ReadLine(), out int age) || age <= 0)
        {
            Console.WriteLine("Invalid age.");
            return;
        }
        
        Console.Write("Enter new symptom: ");
        string symptom = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(symptom))
        {
            Console.WriteLine("Symptom cannot be empty.");
            return;
        }
        
        Console.Write("Enter new species: ");
        string species = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(species))
        {
            Console.WriteLine("Species cannot be empty.");
            return;
        }
        
        patient.UpdateName(name);
        patient.UpdateAge(age);
        patient.Symptom = symptom;
        patient.UpdateSpecies(species);
        
        Console.WriteLine("Patient updated successfully.");
    }

    /// <summary>
    /// Deletes a patient from the patient collection and dictionary.
    /// </summary>
    /// <param name="patients"></param>
    /// <param name="patientDictionary"></param>
    /// <param name="id"></param>
    public void DeletePatient(List<Patient> patients, Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        
        patientDictionary.Remove(id);
        patients.Remove(patient);
        
        Console.WriteLine("Patient deleted successfully.");
    }

    /// <summary>
    /// Demonstrates LINQ queries using method and query syntax.
    /// </summary>
    /// <param name="patients"></param>
    public void RunLinqExamples(List<Patient> patients)
    {
        // Where
        var olderPatients = patients
            .Where(patient => patient.Age > 5);
        
        Console.WriteLine("\nPatients older than 5:");

        foreach (Patient patient in olderPatients)
        {
            Console.WriteLine(patient.Name);
        }

        // Select
        var patientNames = patients
            .Select(patient => patient.Name);
        
        Console.WriteLine("\nPatient names:");

        foreach (string name in patientNames)
        {
            Console.WriteLine(name);
        }

        // OrderBy
        var patientsByName = patients
            .OrderBy(patient => patient.Name);
        
        Console.WriteLine("\nPatients ordered by name:");

        foreach (Patient patient in patientsByName)
        {
            Console.WriteLine(patient.Name);
        }
        
        // OrderByDescending
        var patientsByAgeDescending = patients
            .OrderByDescending(patient => patient.Age);
        
        Console.WriteLine("\nPatients ordered by age:");

        foreach (Patient patient in patientsByAgeDescending)
        {
            Console.WriteLine($"Name: {patient.Name} | Age: {patient.Age}");
        }
        
        // GroupBy
        var patientsBySpecies = patients
            .GroupBy(patient => patient.Species);
        
        Console.WriteLine("\nPatients grouped by species:");

        foreach (var group in patientsBySpecies)
        {
            Console.WriteLine($"Species: {group.Key}");

            foreach (Patient patient in group)
            {
                Console.WriteLine($"- {patient.Name}");
            }
        }
        
        // First
        if (patients.Count > 0)
        {
            Patient firstPatient = patients.First();
            
            Console.WriteLine($"\nFirst patient: {firstPatient.Name}");
        }
        
        // FirstOrDefault
        Patient? firstOlderPatient = patients
            .FirstOrDefault(patient => patient.Age > 5);

        if (firstOlderPatient != null)
        {
            Console.WriteLine($"\nFirst patient older than 5: {firstOlderPatient.Name}");
        }
        else
        {
            Console.WriteLine("\nNo patient older than 5 was found");
        }
        
        // Any
        bool hasFeverPatient = patients.Any(
            patient => patient.Symptom.Equals(
                "Fever",
                StringComparison.OrdinalIgnoreCase
            )
        );
        
        Console.WriteLine($"\nHas patient with fever: {hasFeverPatient}");
        
        // All
        bool allPatientsHaveOwners = patients.All(
            patient => patient.Owner != null
        );
        
        Console.WriteLine($"All patients have owners: {allPatientsHaveOwners}");
        
        // Count
        int patientCount = patients.Count();
        
        Console.WriteLine($"\nTotal patients: {patientCount}");

        int olderPatientCount = patients.Count(
            patient => patient.Age < 5
        );
        
        Console.WriteLine($"Patient older than 5: {olderPatientCount}");
        
        // Query Syntax
        Console.WriteLine("Query Syntax");
        
        // Where + Select
        var queryNames =
            from patient in patients
            where patient.Age > 5
            select patient.Name;

        foreach (string name in queryNames)
        {
            Console.WriteLine(name);
        }
        
        // OrderBy
        var queryOrderedPatients =
            from patient in patients
            orderby patient.Name
            select patient;

        foreach (Patient patient in queryOrderedPatients)
        {
            Console.WriteLine(patient.Name);
        }
        
        // GroupBy
        var queryGroupedPatients =
            from patient in patients
            group patient by patient.Name;

        foreach (var group in queryGroupedPatients)
        {
            Console.WriteLine($"Species: {group.Key}");

            foreach (Patient patient in group)
            {
                Console.WriteLine($"- {patient.Name}");
            }
        }
    }

    /// <summary>
    /// Finds patients of a specific species, orders them by age, and displays the patient's name and owner's phone.
    /// </summary>
    /// <param name="patients"></param>
    /// <param name="species"></param>
    public void ShowPatientsBySpecies(List<Patient> patients, string species)
    {
        var result = patients
            .Where(patient =>
                patient.Species.Equals(
                    species,
                    StringComparison.OrdinalIgnoreCase
                )
            )
            .OrderBy(patient => patient.Age)
            .Select(patient => new
            {
                PatientName = patient.Name,
                OwnerPhone = patient.Owner.Phone
            });

        if (!result.Any())
        {
            Console.WriteLine($"No patients of species: {species} were found.");
            return;
        }

        foreach (var patient in result)
        {
            Console.WriteLine($"Patient: {patient.PatientName} | Owner Phone: {patient.OwnerPhone}");
        }
    }

    /// <summary>
    /// Demonstrate practical LINQ queries used to analyze patient data
    /// </summary>
    /// <param name="patients"></param>
    public void RunPatientStatistics(List<Patient> patients)
    {
        if (patients.Count == 0)
        {
            Console.WriteLine("There are no patients to analyse.");
            return;
        }
        
        // Find the youngest patient.
        Patient? youngestPatient = patients
            .OrderBy(patient => patient.Age)
            .FirstOrDefault();

        if (youngestPatient != null)
        {
            Console.WriteLine($"Youngest patient: {youngestPatient.Name}, {youngestPatient.Age} years old");
        }
        
        // Find oldest patient
        Patient? oldestPatient = patients
            .OrderByDescending(patient => patient.Age)
            .FirstOrDefault();

        if (oldestPatient != null)
        {
            Console.WriteLine($"Oldest patient: {oldestPatient.Name}, {oldestPatient.Age} years old");
        }
        
        // Count the number of patients for each species
        var countBySpecies = patients
            .GroupBy(patient => patient.Species);
        
        Console.WriteLine("\nPatients by species:");

        foreach (var group in countBySpecies)
        {
            Console.WriteLine($"{group.Key}: {group.Count()}");
        }
        
        // Check whether at least one patient has fever
        bool hasFeverPatient = patients.Any(patient =>
            patient.Symptom.Equals(
                "Fever",
                StringComparison.OrdinalIgnoreCase
            )
        );
        
        Console.WriteLine($"\nHas patient with fever: {hasFeverPatient}");
        
        // Get all patient names in uppercase and alphabetic order
        var patientNames = patients
            .Select(patient => patient.Name.ToUpper())
            .OrderBy(name => name);
        
        Console.WriteLine("\nPatient names:");

        foreach (string name in patientNames)
        {
            Console.WriteLine(name);
        }
    }

    /// <summary>
    /// Demonstrates polymorphism using the IRegistrable interface.
    /// </summary>
    public void RunRegistrationTest()
    {
        IRegistrable registrable = new Patient(
            999,
            "Registration Test",
            3,
            "None",
            "Dog",
            new Client(
                999,
                "Test Owner",
                "0000000000",
                "Test Address"));
        
        registrable.Register();
    }
}