using InfectionVet.Models;

namespace InfectionVet.Services;

public class PatientService
{
    public void RegisterPatient(List<Patient> patients)
    {
        Console.Write("Enter patient name: ");
        string name = Console.ReadLine() ?? "";
        
        Console.Write("Enter patient age: ");
        int age = int.Parse(Console.ReadLine() ?? "");
        
        Console.Write("Enter symptom: ");
        string symptom = Console.ReadLine() ?? "";

        Patient patient = new Patient
        {
            Id = patients.Count + 1,
            Name = name,
            Age = age,
            Symptom = symptom
        };
        
        patients.Add(patient);
        
        Console.WriteLine($"\nPatient registered successfully.");
    }

    public void ListPatients(List<Patient> patients)
    {
        foreach (Patient patient in patients)
        {
            Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom}");
        }
    }

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
        
        Console.WriteLine($"ID: {patient.Id} | Name: {patient.Name} |  Age: {patient.Age} | Symptom: {patient.Symptom}");
    }
}