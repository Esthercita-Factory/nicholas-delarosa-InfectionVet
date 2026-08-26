namespace InfectionVet.Models;

/// <summary>
/// Represents a client who owns one or more patients in the veterinary clinic.
/// </summary>
public class Client
{
    private string _phone;
    
    private readonly List<Patient> _patients = new();
    
    public int Id  { get; set; }
    
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
        _patients.Add(patient);
    }

    /// <summary>
    /// Displays all patienst owned by this client
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