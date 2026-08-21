using InfectionVet.Models;
using InfectionVet.Services;

// Store all registered patients.
List<Patient> patients = [];
PatientService patientService = new PatientService();

bool running = true;

while (running)
{
    Console.WriteLine($@"Infection Vet
1. Register patient.
2. List patients.
3. Find patient.
4. Exit.");
    Console.Write("Choose an option: ");

    string option = Console.ReadLine() ?? "";

    switch (option)
    {
        case "1":
            patientService.RegisterPatient(patients);
            break;
        
        case "2":
            patientService.ListPatients(patients);
            break;
        
        case "3":
            Console.Write("Enter patient name: ");
            string name = Console.ReadLine() ?? "";
            
            patientService.FindPatientByName(patients, name);
            break;
        
        case "4":
            running = false;
            Console.WriteLine("Goodbye!");
            break;
        
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}