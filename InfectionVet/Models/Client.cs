namespace InfectionVet.Models;

/// <summary>
/// Represents a client who owns one or more patients in the veterinary clinic.
/// </summary>
public class Client
{
    public int Id  { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public string Phone { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
}