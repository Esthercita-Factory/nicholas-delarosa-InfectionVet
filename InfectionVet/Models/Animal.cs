namespace InfectionVet.Models;

/// <summary>
/// Represents the common characteristics of an animal. 
/// </summary>
public class Animal
{
    protected string _name;
    protected string _species;
    protected int _age;

    public string Name => _name;
    public string Species => _species;
    public int Age => _age;

    /// <summary>
    /// Initializes a new animal.
    /// </summary>
    /// <param name="name">The animal's name.</param>
    /// <param name="species">The animal's species.</param>
    /// <param name="age">The animal's age.</param>
    protected Animal(string name ,string species, int age)
    {
        _name = name;
        _species = species;
        _age = age;
    }

    /// <summary>
    /// Produces the default sound of an animal.
    /// </summary>
    public virtual void MakeSound()
    {
        Console.WriteLine("The animal makes a sound.");
    }
}