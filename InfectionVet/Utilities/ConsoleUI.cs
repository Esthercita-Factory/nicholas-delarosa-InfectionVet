namespace InfectionVet.Utilities;

/// <summary>
/// Centralizes console styling and validated input prompts so every screen of the clinic system
/// shares the same look and feel instead of scattering Console.ForegroundColor calls across services.
/// </summary>
public static class ConsoleUI
{
    /// <summary>
    /// Prepares the console window: sets the title and switches to UTF-8 so the box-drawing
    /// characters used in the banner render correctly.
    /// </summary>
    /// <remarks>
    /// This deliberately does not call Console.SetWindowSize/SetBufferSize. Those legacy console
    /// APIs are unreliable on modern ConPTY-based terminal hosts (Windows Terminal, VS Code's
    /// integrated terminal, Rider, etc.) and have been observed to hang instead of throwing, so
    /// there is no safe way to guard them with a try-catch. Leaving the window size alone is a
    /// purely cosmetic trade-off, not a functional one.
    /// </remarks>
    public static void Initialize()
    {
        Console.Title = "Infection Vet";

        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
        }
        catch (IOException)
        {
            // Output is redirected (e.g. running inside a test runner), so the encoding cannot be changed. Safe to ignore.
        }
    }

    /// <summary>
    /// Clears the console. Wrapped in a try-catch because Console.Clear() throws when the output
    /// stream has been redirected (e.g. piped into a file), which should never crash the menu loop.
    /// </summary>
    public static void ClearScreen()
    {
        try
        {
            Console.Clear();
        }
        catch (IOException)
        {
            // Redirected output cannot be cleared; simply continue without clearing.
        }
    }

    /// <summary>
    /// Draws a small double-bordered banner. Used sparingly, only for the application title,
    /// so it reads as an intentional header rather than a repeated separator.
    /// </summary>
    public static void WriteBanner(string title, string subtitle)
    {
        int innerWidth = Math.Max(title.Length, subtitle.Length) + 4;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', innerWidth) + "╗");
        Console.WriteLine("║" + CenterText(title, innerWidth) + "║");
        Console.WriteLine("║" + CenterText(subtitle, innerWidth) + "║");
        Console.WriteLine("╚" + new string('═', innerWidth) + "╝");
        Console.ResetColor();
        Console.WriteLine();
    }

    /// <summary>
    /// Writes a colored category heading (e.g. a group of menu options) with breathing room around it.
    /// </summary>
    public static void WriteSectionTitle(string title)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine($" {title}");
        Console.ResetColor();
    }

    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  ✔ {message}");
        Console.ResetColor();
    }

    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✖ {message}");
        Console.ResetColor();
    }

    public static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  ⚠ {message}");
        Console.ResetColor();
    }

    public static void WriteInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  {message}");
        Console.ResetColor();
    }

    /// <summary>
    /// Prompts for a string and keeps re-asking until the user provides a non-empty value.
    /// Looping instead of aborting keeps the surrounding flow (e.g. patient registration) from
    /// being interrupted by a single accidental blank entry.
    /// </summary>
    public static string ReadRequiredString(string label)
    {
        while (true)
        {
            string input = Prompt(label).Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            WriteWarning("This field cannot be empty.");
        }
    }

    /// <summary>
    /// Prompts for a string that is allowed to be left blank, such as an unknown breed.
    /// </summary>
    public static string ReadOptionalString(string label)
    {
        return Prompt(label).Trim();
    }

    /// <summary>
    /// Prompts for a whole number and keeps re-asking until the input can be parsed.
    /// TryParse is used instead of int.Parse + catch so a typo never throws or aborts the
    /// surrounding flow; domain rules (e.g. age cannot be negative) are still enforced separately
    /// by the caller through custom exceptions where a thrown exception genuinely signals a business error.
    /// </summary>
    public static int ReadInt(string label)
    {
        while (true)
        {
            string input = Prompt(label);

            if (int.TryParse(input, out int value))
            {
                return value;
            }

            WriteWarning("Please enter a whole number.");
        }
    }

    private static string Prompt(string label)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"{label}: ");
        Console.ResetColor();

        return Console.ReadLine() ?? string.Empty;
    }

    private static string CenterText(string text, int width)
    {
        int totalPadding = width - text.Length;
        int leftPadding = totalPadding / 2;
        int rightPadding = totalPadding - leftPadding;

        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }
}