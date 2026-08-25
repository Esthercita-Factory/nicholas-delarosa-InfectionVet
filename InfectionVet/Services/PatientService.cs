using InfectionVet.Models;

namespace InfectionVet.Services;

public class PatientService
{
    /// <summary>
    /// Registers a new patient and its owner.
    /// </summary>
    /// <param name="patients">Represents the patients' class as a list.</param>
    /// <param name="patientDictionary">Represents the patients' class as a dictionary</param>
    /// <param name="id"></param>
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

            if (age <= 0)
            {
                Console.WriteLine("Patient age must be greater than zero.");
                return;
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

            Client owner = new Client
            {
                Id = patients.Count + 1,
                Name = ownerName,
                Phone = ownerPhone,
                Address = ownerAddress
            };

            Patient patient = new Patient
            {
                Id = id,
                Name = name,
                Age = age,
                Symptom = symptom,
                Species = species,
                Owner = owner
            };

            patients.Add(patient);
            patientDictionary.Add(patient.Id, patient);
            
            Console.WriteLine($"\nPatient registered successfully.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid age. Please enter a whole number.");
        }
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
                Console.WriteLine(
                    $"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Species: {patient.Species} | Owner: Not registered.");
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
        
        patient.Name = name;
        patient.Age = age;
        patient.Symptom = symptom;
        patient.Species = species;
        
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
    }
}