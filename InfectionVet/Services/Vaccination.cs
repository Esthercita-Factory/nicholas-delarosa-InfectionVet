using InfectionVet.Interfaces;

namespace InfectionVet.Services;

/// <summary>
/// Represents a vaccination veterinary service.
/// </summary>
public class Vaccination : VeterinaryService, IAtendible
{
    /// <summary>
    /// Initializes a new vaccination service.
    /// </summary>
    public Vaccination()
        : base("Vaccination")
    {
        
    }

    /// <summary>
    /// Performs the vaccination service.
    /// </summary>
    public override void Attend()
    {
        Console.WriteLine("Performing a vaccination.");
    }
}