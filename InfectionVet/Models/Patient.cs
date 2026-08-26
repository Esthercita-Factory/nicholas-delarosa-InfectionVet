namespace InfectionVet.Models;

/// <summary>
/// Represents a veterinary patient.
/// In InfectionVet, a patient is a pet owned by a client.
/// </summary>
public class Patient : Animal
{
    // Patient data and owner relationship are kept together because the patient represents the pet registered at the clinic.
    public int Id { get; private set; }
    public string Symptom { get; set; }
    public Client Owner { get; private set; }

    /// <summary>
    /// Initializes a new veterinary patient.
    /// </summary>
    /// <param name="id">The patient's ID.</param>
    /// <param name="name">The patient's name.</param>
    /// <param name="age">The patient's age.</param>
    /// <param name="symptom">The patient's symptom.</param>
    /// <param name="species">The patient's species.</param>
    /// <param name="owner">The client's owner.</param>
    public Patient(
        int id,
        string name,
        int age,
        string symptom,
        string species,
        Client owner)
        : base(name, species, age)
    {
        Id = id;
        Symptom = symptom;
        Owner = owner;
    }

    /// <summary>
    /// Updates the patient's name.
    /// </summary>
    /// <param name="name">The new name of the patient.</param>
    public void UpdateName(string name)
    {
        _name = name;
    }

    /// <summary>
    /// Updates the patient's age.
    /// </summary>
    /// <param name="age">The new age of the patient.</param>
    public void UpdateAge(int age)
    {
        _age = age;
    }

    /// <summary>
    /// Updates the patient's species.
    /// </summary>
    /// <param name="species">The new species of the patient.</param>
    public void UpdateSpecies(string species)
    {
        _species = species;
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

    /// <summary>
    /// Produces a sound based on the patient's species.
    /// </summary>
    public override void MakeSound()
    {
        switch (Species.ToLower())
        {
            case "dog":
                Console.WriteLine("Woof!");
                break;
            
            case "cat":
                Console.WriteLine("Meow!");
                break;
            
            default:
                Console.WriteLine("The animal makes a sound.");
                break;
        }
    }
}