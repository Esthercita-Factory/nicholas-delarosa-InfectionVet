namespace InfectionVet.Models;

/// <summary>
/// Represents a client who owns one or more patients in the veterinary clinic.
/// </summary>
public class Client
{
    public int Id  { get; set; }
    
    public string Name { get; set; }
    
    public string Phone { get; set; }
    
    public string Address { get; set; }

    /// <summary>
    /// Contains all patients owned by this client.
    /// </summary>
    public List<Patient> Patients { get; } = new();

    /// <summary>
    /// Initializes a new client with the required information.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="phone"></param>
    /// <param name="address"></param>
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
    /// <param name="patient"></param>
    public void AddPatient(Patient patient)
    {
        Patients.Add(patient);
    }

    /// <summary>
    /// Displays all patienst owned by this client
    /// </summary>
    public void DisplayPatients()
    {
        Console.WriteLine($"\nPatients owned by {Name}");

        if (Patients.Count == 0)
        {
            Console.WriteLine("No patients registered.");
            return;
        }

        foreach (Patient patient in Patients)
        {
            Console.WriteLine($"- {patient.Name} ({patient.Species}, {patient.Age} years old)");
        }
    }

    /// <summary>
    /// Display the client's information in the console.
    /// </summary>
    public void DisplayInformation()
    {
        Console.WriteLine($@"Client ID: {Id}
Name: {Name}
Phone: {Phone}
Address: {Address}");
    }
}