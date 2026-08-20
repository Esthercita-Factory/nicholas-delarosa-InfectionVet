using InfectionVet.Models;

Patient patient = new Patient
{
    Id = 1,
    Name = "John Smith",
    Age = 32,
    Symptom = "Fever"
};

Console.WriteLine($@"Patient: {patient.Name}.
Age: {patient.Age}.
Symptom: {patient.Symptom}");