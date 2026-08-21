using InfectionVet.Models;

namespace InfectionVet.Services;

public class PatientService
{
    /// <summary>
    /// Registers a new patient and its owner.
    /// </summary>
    /// <param name="patients">Represents the patients' class as a list.</param>
    public void RegisterPatient(List<Patient> patients, Dictionary<int, Patient> patientDictionary)
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
            string symptom = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(symptom))
            {
                Console.WriteLine("Sympton cannot be empty.");
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
                Id = patients.Count + 1,
                Name = name,
                Age = age,
                Symptom = symptom,
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
                    $"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Owner: Not registered.");
                continue;
            }
            Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone}");
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
        
        Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} |  Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone} | Address: {patient.Owner.Address}");
    }

    public void FindPatientById(Dictionary<int, Patient> patientDictionary, int id)
    {
        if (!patientDictionary.TryGetValue(id, out Patient? patient))
        {
            Console.WriteLine("Patient not found.");
            return;
        }
        
        Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom} | Owner: {patient.Owner.Name} | Phone: {patient.Owner.Phone}");
    }
}