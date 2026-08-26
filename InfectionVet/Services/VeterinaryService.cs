namespace InfectionVet.Services;

/// <summary>
/// Represents a general veterinary service.
/// </summary>
public abstract class VeterinaryService
{
    public string Name { get; private set; }

    /// <summary>
    /// Initializes a new veterinary service.
    /// </summary>
    /// <param name="Name">The name of the service.</param>
    protected VeterinaryService(string name)
    {
        Name = name;
    }

    public abstract void Attend();
}