namespace InfectionVet.Interfaces;

/// <summary>
/// Defines the contract for services that can attend a veterinary case.
/// </summary>
public interface IAtendible
{
    /// <summary>
    /// Performs the veterinary service.
    /// </summary>
    void Attend();
}