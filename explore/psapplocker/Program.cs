using System;
using System.Management.Automation;
using System.Collections.ObjectModel;

class Program
{
    static void Main()
    {
        // loop to prompt the user for commands until "exit" is typed by user
        while (true)
        {
            // Prompt for PowerShell command
            Console.Write("Enter a PowerShell command (or type 'exit' to quit): ");
            string userInput = Console.ReadLine();

            // Check if 'exit'
            if (userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;  // Exit the loop and terminate the program
            }

            // Execute PowerShell command
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(userInput); // User's command

                try
                {
                    // Execute the command and capture the results
                    Collection<PSObject> results = ps.Invoke();

                    // Output the results of the PowerShell command
                    if (results.Count > 0)
                    {
                        foreach (var result in results)
                        {
                            Console.WriteLine(result.ToString());
                        }
                    }
                    else
                    {
                        Console.WriteLine("No output from the command.");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during command execution
                    Console.WriteLine("Error executing command: " + ex.Message);
                }
            }

            // Ask the user to press Enter to continue
            Console.WriteLine("\nPress Enter to continue or type 'exit' to quit...");
            string continueInput = Console.ReadLine();

            // uer types "exit" break the loop and exit the program
            if (continueInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }
        }
    }
}
