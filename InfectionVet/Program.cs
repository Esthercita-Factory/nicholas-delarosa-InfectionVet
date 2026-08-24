using InfectionVet.Models;
using InfectionVet.Services;

// Store all registered patients.
List<Patient> patients = [];
Dictionary<int, Patient> patientDictionary = [];
int nextPatientId = 1;
PatientService patientService = new PatientService();

bool running = true;

while (running)
{
    Console.WriteLine($@"Infection Vet
1. Register patient.
2. List patients.
3. Find patient by name.
4. Find patient by id.
5. Update patient.
6. Delete patient.
7. Exit");
    Console.Write("Choose an option: ");

    string option = Console.ReadLine() ?? "";

    switch (option)
    {
        case "1":
            patientService.RegisterPatient(patients, patientDictionary, nextPatientId);
            nextPatientId++;
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
            Console.Write("Enter patient ID: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                patientService.FindPatientById(patientDictionary, id);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            
            break;
        
        case "5":
            Console.Write("Enter patient ID: ");

            if (int.TryParse(Console.ReadLine(), out int updateId))
            {
                patientService.UpdatePatient(patientDictionary, updateId);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            
            break;
        
        case "6":
            Console.Write("Enter patient ID: ");

            if (int.TryParse(Console.ReadLine(), out int deleteId))
            {
                patientService.DeletePatient(patients, patientDictionary, deleteId);
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            
            break;
        
        case "7":
            running = false;
            Console.WriteLine("Goodbye!");
            break;
        
        default:
            Console.WriteLine("Invalid option.");
            break;
    }
}