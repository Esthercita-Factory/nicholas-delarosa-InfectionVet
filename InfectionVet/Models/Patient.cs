namespace InfectionVet.Models;

/// <summary>
/// Represents a veterinary patient.
/// In InfectionVet, a patient is a pet owned by a client.
/// </summary>
public class Patient
{
    // Patient data and owner relationship are kept together because the patient represents the pet registered at the clinic.
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Symptom { get; set; }
    public string Species { get; set; }
    public Client Owner { get; set; }

    /// <summary>
    /// Initializes a new veterinary patient.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="age"></param>
    /// <param name="symptom"></param>
    /// <param name="species"></param>
    /// <param name="owner"></param>
    public Patient(
        int id,
        string name,
        int age,
        string symptom,
        string species,
        Client owner)
    {
        Id = id;
        Name = name;
        Age = age;
        Symptom = symptom;
        Species = species;
        Owner = owner;
    }

    /// <summary>
    /// Displays the patient's information in the console.
    /// </summary>
    public void DisplayInformation()
    {
        Console.WriteLine($@"Patient ID: {Id}
Name: {Name}
Age: {Age}
Symptom: {Symptom}
Species: {Species}
Owner: {Owner.Name}");
    }
}