using InfectionVet.Interfaces;
using InfectionVet.Utilities;

namespace InfectionVet.Services;

/// <summary>
/// Represents a general veterinary consultation.
/// </summary>
public class GeneralConsultation : VeterinaryService, IAtendible
{
    /// <summary>
    /// Initializes a new general consultation.
    /// </summary>
    public GeneralConsultation()
        : base("General Consultation")
    {
    }

    /// <summary>
    /// Performs the general consultation.
    /// </summary>
    public override void Attend()
    {
        ConsoleUI.WriteSuccess("Performing a general veterinary consultation.");
    }
}