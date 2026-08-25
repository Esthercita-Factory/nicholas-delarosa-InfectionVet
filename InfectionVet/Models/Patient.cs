namespace InfectionVet.Models;

/// <summary>
/// Represents a veterinary patient.
/// In InfectionVet, a patient is a pet owned by a client.
/// </summary>
public class Patient
{
    // Patient data and owner relationship are kept together because the patient represents the pet registered at the clinic.
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Symptom { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public Client Owner { get; set; } = null!;
}