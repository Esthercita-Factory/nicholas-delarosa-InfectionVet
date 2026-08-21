namespace InfectionVet.Models;

/// <summary>
/// Represents a veterinary patient.
/// A patient is a pet that receives medical care at the clinic.
/// </summary>
public class Patient
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Symptom { get; set; }
    public Client Owner { get; set; }
}