namespace InfectionVet.Services;

/// <summary>
/// Represents the common structure of a veterinary service.
/// An abstract class is used because all veterinary services share common state and behavior, such as the service name.
/// Derived services must provide their own implementation of Attend(). 
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

    // Each veterinary service performs the attendance differently, so the specific implementation is delegated to the derived class.
    public abstract void Attend();
}