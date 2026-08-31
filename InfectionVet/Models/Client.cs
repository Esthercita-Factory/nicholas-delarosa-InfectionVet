using InfectionVet.Interfaces;
using InfectionVet.Utilities;

namespace InfectionVet.Models;

/// <summary>
/// Client represents the owner of one or more patients.
/// Patients are the pets registered in the clinic, so Client does not inherit from Patient or Animal.
/// It implements IRegistrable because an owner is also registered in the clinic's system, independently of their pets.
/// </summary>
public class Client : IRegistrable
{
    private string _phone = string.Empty;

    private readonly List<Patient> _patients = new();

    public int Id { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// Gets the client's phone number.
    /// The value can only be modified inside the Client class.
    /// </summary>
    public string Phone
    {
        get => _phone;
        private set => _phone = value;
    }

    public string Address { get; set; }

    /// <summary>
    /// Provides read-only access to the patients owned by this client.
    /// </summary>
    public IReadOnlyList<Patient> Patients => _patients;

    /// <summary>
    /// Initializes a new client with the required information.
    /// </summary>
    /// <param name="id">The client's ID.</param>
    /// <param name="name">The client's name.</param>
    /// <param name="phone">The client's phone number.</param>
    /// <param name="address">The client's address.</param>
    public Client(
        int id,
        string name,
        string phone,
        string address)
    {
        Id = id;
        Name = name;
        Phone = phone;
        Address = address;
    }

    /// <summary>
    /// Adds a patient to this client's collection of patients.
    /// </summary>
    /// <param name="patient">The patient to add.</param>
    public void AddPatient(Patient patient)
    {
        _patients.Add(patient);
    }

    /// <summary>
    /// Removes a patient from this client's collection.
    /// Kept in sync whenever a patient is deleted from the clinic, otherwise the owner would keep
    /// referencing a pet that no longer exists.
    /// </summary>
    /// <param name="patient">The patient to remove.</param>
    public void RemovePatient(Patient patient)
    {
        _patients.Remove(patient);
    }

    /// <summary>
    /// Displays all patients owned by this client.
    /// </summary>
    public void DisplayPatients()
    {
        Console.WriteLine($"\nPatients owned by {Name}");

        if (_patients.Count == 0)
        {
            Console.WriteLine("No patients registered.");
            return;
        }

        foreach (Patient patient in _patients)
        {
            Console.WriteLine($"- {patient.Name} ({patient.Species}, {patient.Age} years old)");
        }
    }

    /// <summary>
    /// Displays the client's information in the console.
    /// </summary>
    public void DisplayInformation()
    {
        Console.WriteLine($@"Client ID: {Id}
Name: {Name}
Phone: {Phone}
Address: {Address}");
    }

    /// <summary>
    /// Registers the client as an owner in the clinic's system.
    /// </summary>
    public void Register()
    {
        ConsoleUI.WriteSuccess($"Client {Name} registered successfully.");
    }
}