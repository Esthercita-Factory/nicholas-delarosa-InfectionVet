# InfectionVet

A console system for managing patients (pets) and their owners at a veterinary clinic.
Written in C# on .NET 10 as a practice project covering OOP, LINQ, exception handling, and
asynchronous programming, spanning User Stories **M5.3S1 through M5.3S5**.

## Domain glossary

The code is in English, but the domain itself is Spanish-speaking. Term mapping:

| In the code | Means |
|---|---|
| `Patient` | The pet / veterinary patient |
| `Client`  | The pet's owner |

## How to run

Requires the .NET 10 SDK.

```bash
dotnet run --project InfectionVet
```

## Project structure

```
InfectionVet/
├── Documentation/      UML class diagram (.drawio) of the domain model
├── Exceptions/         Custom domain exceptions
├── Interfaces/         Contracts: IRegistrable, INotifiable, IAtendible
├── Models/             Animal, Patient, Client
├── Services/           Business logic, LINQ queries, and async demos
├── Utilities/          ConsoleUI (console styling) and Logger (file logging)
├── Logs/               Event/error log (generated at runtime, git-ignored)
└── Program.cs          Entry point and main menu
```

## Main menu

| Option | Action |
|---|---|
| 1 | Register a new patient (async) |
| 2 | List all patients |
| 3 | Find patient by name |
| 4 | Find patient by ID |
| 5 | Update patient information |
| 6 | Delete patient |
| 7 | List patients by species |
| 8 | View patient statistics |
| 9 | Run LINQ demonstrations |
| 10 | View a client's registered patients |
| 11 | Process all patients concurrently |
| 12 | Run independent clinic processes (`Task.WhenAll`) |
| 13 | Run independent clinic processes (`Task.WhenAny`) |
| 14 | Compare synchronous vs. asynchronous execution |
| 15 | Attend a veterinary service |
| 0 | Exit |

## Features by User Story

### M5.3S1 — Basic patient registration
- `Patient` with `Id`, `Name`, `Age`, `Symptom` (automatic properties).
- Menu driven by a `while` loop and `switch-case` navigation.
- `PatientService` centralizes the logic for registering, listing, and searching.
- `List<Patient>` as the primary collection.
- Input validation: `ConsoleUI.ReadRequiredString` retries on empty fields,
  `ConsoleUI.ReadInt` retries on non-numeric input — the program never aborts the flow over
  a typo.

### M5.3S2 — Collections and LINQ queries
- `Dictionary<int, Patient>` kept alongside the list for O(1) lookup by ID.
- `PatientService.DemonstrateLinqQueries` (option 9): walks through `Where`, `Select`,
  `OrderBy`, `OrderByDescending`, `GroupBy`, `First`, `FirstOrDefault`, `Any`, `All`, and
  `Count`, in both method syntax and query syntax, so the two can be compared directly.
- `ShowPatientsBySpecies` (option 7): a chained query — filters by species, orders by age,
  and projects only the patient's name and the owner's phone number.
- `RunPatientStatistics` (option 8): youngest/oldest patient, count per species, whether any
  patient has no breed defined (`Any`), and every patient's name in uppercase, alphabetically
  ordered.

### M5.3S3 — Inheritance and polymorphism
- `Animal` (base class): protected `Name`, `Species`, `Age`, a `protected` constructor, and
  a virtual `MakeSound()`.
- `Patient : Animal` adds `Symptom`, `Breed`, `Owner`, and overrides `MakeSound()` per
  species (polymorphism).
- `Client` maintains the one-to-many relationship: `Client.Patients`
  (`IReadOnlyList<Patient>`), populated through `AddPatient` / `RemovePatient`.
- Encapsulation: `protected`/`private` fields exposed through properties (`Client.Phone` is
  read-only from outside the class).
- `VeterinaryService` (abstract class) with an abstract `Attend()`; `GeneralConsultation`
  and `Vaccination` override it.
- `IRegistrable` with `Register()`, implemented by both `Patient` and `Client`.

### M5.3S4 — Interfaces, debugging, and exceptions
- Three interfaces, each with a distinct responsibility: `IRegistrable`, `INotifiable`,
  `IAtendible`. An abstract class (`VeterinaryService`) is used where state and behavior are
  shared; interfaces are used where the contract needs to cross unrelated hierarchies
  (`Patient` and `Client` don't share a base class, but both register).
- `Patient` implements `IRegistrable` **and** `INotifiable` at once: registering a patient
  (option 1) calls both `patient.Register()` and `patient.SendNotification()` in the same
  operation — multiple-interface usage happens in the real flow, not in a separate test case.
- Domain exceptions: `InvalidPatientAgeException` (negative age) and
  `PatientNotFoundException` (a search/update/delete with no match). Neither is ever
  silenced: both are caught, shown to the user with a clear message, and logged.
- `Program.cs` wraps every menu action in a single `try-catch-finally`: specific catches
  first (`InvalidPatientAgeException`, `PatientNotFoundException`), a generic catch-all as a
  last resort, and a `finally` that always pauses with "Press Enter to return to the
  menu...".

### M5.3S5 — Asynchronous programming
- `RegisterPatientAsync` uses `await Task.Delay(...)` to simulate a slower operation (e.g. a
  database write) without blocking the menu thread.
- `ClinicTaskService` demonstrates `Task.Run`, `Task.WhenAll` (option 12), and
  `Task.WhenAny` (option 13) across three simulated clinic processes with different delays.
- `ProcessPatientsConcurrentlyAsync` (option 11) processes every patient in parallel with
  `Task.WhenAll`, instead of one at a time.
- `AsyncDemoService.CompareExecutionModesAsync` (option 14) runs a synchronous example
  (`Thread.Sleep`) and an asynchronous one (`await Task.Delay`) back to back, timing each
  with a `Stopwatch` to make the difference measurable rather than theoretical.
- No method ever blocks with `.Result` or `.Wait()`. Every asynchronous method uses the
  `Async` suffix.

## Design decisions

- **`ConsoleUI` (Utilities/ConsoleUI.cs)** centralizes colors, the double-bordered banner,
  and the input prompts (`ReadRequiredString`, `ReadInt`, `ReadOptionalString`) that retry
  instead of aborting the flow. Avoids repeating
  `Console.ForegroundColor`/`ResetColor` boilerplate across every service.
- **`Logger` (Utilities/Logger.cs)** writes to `Logs/infectionvet.log` (a plain text file,
  git-ignored). It records both successful operations (a patient created, updated, or
  deleted) and errors — the kind of trail technical support would use in a real environment
  to reconstruct what happened before a bug report.
- **`int.TryParse` instead of `int.Parse` + `catch`** for age: a typo should never throw an
  exception and cut registration short. Business rules (age cannot be negative) are still
  validated through a dedicated exception (`InvalidPatientAgeException`), reserving
  exceptions for domain errors rather than input-format validation.
- **No `Console.SetWindowSize`/`SetBufferSize`**: those APIs don't play well with modern
  ConPTY-based terminals (Windows Terminal, VS Code's integrated terminal, Rider) and have
  been known to hang the console instead of throwing a catchable exception. The window size
  is left to whatever the user's terminal already has.

## Class diagram (UML)

The full domain model — `Animal`/`Patient`/`Client`, the three interfaces, the
`VeterinaryService` hierarchy, and the two domain exceptions — is documented in:
[`Documentation/InfectionVet-Class-Diagram.drawio`](InfectionVet/Documentation/InfectionVet-Class-Diagram.drawio),
[`Documentation/InfectionVet-Class-Diagram.drawio.pdf`](InfectionVet/Documentation/InfectionVet-Class-Diagram.drawio.pdf)

## Known limitations

Out of scope for User Stories M5.3S1–S5, and therefore not implemented:

- **No persistence**: all data lives in memory and is lost when the program exits.
- **No automated tests**: correctness was verified by running the program manually with
  simulated input, not through a test project.