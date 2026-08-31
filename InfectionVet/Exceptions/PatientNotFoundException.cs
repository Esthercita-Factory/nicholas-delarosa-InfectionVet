namespace InfectionVet.Exceptions;

/// <summary>
/// Represents an error raised when no patient matches the identifier or name used to look it up.
/// </summary>
public class PatientNotFoundException : Exception
{
    /// <summary>
    /// Initializes a new exception describing which patient lookup failed.
    /// </summary>
    /// <param name="searchCriteria">A human-readable description of what was searched for, e.g. "ID 12" or "name 'Rex'".</param>
    public PatientNotFoundException(string searchCriteria)
        : base($"No patient was found matching {searchCriteria}.")
    {
    }
}