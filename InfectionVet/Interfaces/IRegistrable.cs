namespace InfectionVet.Interfaces;

/// <summary>
/// Defines the contract for entities that can be registered in the clinic.
/// An interface is used because different types of entities can provide registration behavior without belonging to the same inheritance hierarchy.
/// </summary>
public interface IRegistrable
{
    /// <summary>
    /// Registers the entity.
    /// </summary>
    void Register();
}