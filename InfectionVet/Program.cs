using InfectionVet.Exceptions;
using InfectionVet.Interfaces;
using InfectionVet.Models;
using InfectionVet.Services;

// Store all registered patients.
List<Patient> patients = [];
Dictionary<int, Patient> patientDictionary = [];
int nextPatientId = 1;
PatientService patientService = new PatientService();

// Temporary exception handling test for S4 TASK 5.
try
{
    int firstNumber = 10;
    int secondNumber = 0;

    int result = firstNumber / secondNumber;

    Console.WriteLine($"Result: {result}");
}
catch (DivideByZeroException)
{
    Console.WriteLine("Cannot divide by zero.");
}
finally
{
    Console.WriteLine("Exception handling test completed.");
}

// Temporary debugging test for S4 TASK 4
// int firstNumber = 10;
// int secondNumber = 2;
//
// int result = firstNumber / secondNumber;
//
// Console.WriteLine($"Result: {result}");

// Temporary multiple-interface test for S4 TASK 3
// Patient testPatient = new Patient(
//     999,
//     "Notification Test",
//     5,
//     "None",
//     "Dog",
//     new Client(
//         999,
//         "Test Owner",
//         "0000000000",
//         "Test Address")
// );
//
// IRegistrable registrable = testPatient;
// INotifiable notifiable = testPatient;
//
// registrable.Register();
// notifiable.SendNotification();

// // Temporary IAtendible test for S4 TASK 2.
// IAtendible consultation = new
//     GeneralConsultation();
// IAtendible vaccination = new 
//     Vaccination();
//
// consultation.Attend();
// vaccination.Attend();

// Temporary polymorphism test for S3 TASK 5.
// Animal dog = new Patient(
//     100,
//     "Test Dog",
//     5,
//     "None",
//     "Dog",
//     new Client(
//         100,
//         "Test Owner",
//         "0000000000",
//         "Test Address")
// );
//
// Animal cat = new Patient(
//     101,
//     "Test Cat",
//     5,
//     "None",
//     "Cat",
//     new Client(
//         100,
//         "Test Owner",
//         "0000000000",
//         "Test Address")
// );
//
// dog.MakeSound();
// cat.MakeSound();

// S2 Exclusive
// Patient testPatient = new Patient
// {
//     Id = 999,
//     Name = "Test Patient",
//     Age = 5,
//     Symptom = "Fever",
//     Species = "Dog"
// };
//
// Console.WriteLine($"Created patient: {testPatient.Name}");

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
7. Exit.
8. List patients by species.
9. Show patients statistics.
10. Display first client's patient.");
    Console.Write("Choose an option: ");

    string option = Console.ReadLine() ?? "";

    switch (option)
    {
        case "1":
            try
            {
                patientService.RegisterPatient(patients, patientDictionary, nextPatientId);
                nextPatientId++;
            }
            catch (InvalidPatientAgeException ex)
            {
                Console.WriteLine($"Registration failed: {ex.Message}");
            }
            
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
        
        case "8":
            Console.WriteLine("Enter species: ");
            string species = Console.ReadLine() ?? "";
            
            patientService.ShowPatientsBySpecies(patients, species);
            
            break;
        
        case "9":
            patientService.RunPatientStatistics(patients);
            
            break;
        
        case "10":
            if (patients.Count == 0)
            {
                Console.WriteLine("There are no patients.");
                break;
            }

            Client firstClient = patients[0].Owner;
            
            firstClient.DisplayPatients();
            break;

        default:
            Console.WriteLine("Invalid option.");
            break;
    }
    
    // Just for Linq Consults
    // patientService.RunLinqExamples(patients);
}