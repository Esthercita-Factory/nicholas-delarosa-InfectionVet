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
}