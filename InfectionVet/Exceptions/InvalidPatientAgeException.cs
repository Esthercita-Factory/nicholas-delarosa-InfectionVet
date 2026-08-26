namespace InfectionVet.Exceptions;

/// <summary>
/// Represents an error caused by an invalid patient age.
/// </summary>
public class InvalidPatientAgeException : Exception
{
    /// <summary>
    /// Initializes a new exception for an invalid patient age.
    /// </summary>
    /// <param name="age">The invalid age.</param>
    public InvalidPatientAgeException(int age)
        : base($"Patient age cannot be negative. Received: {age}")
    {
        
    }
}