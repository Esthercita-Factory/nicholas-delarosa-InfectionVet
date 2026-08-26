namespace InfectionVet.Interfaces;

/// <summary>
/// Defines the contract for entities that can send notifications.
/// </summary>
public interface INotifiable
{
    /// <summary>
    /// Sends a notification related to the entity.
    /// </summary>
    void SendNotification();
}