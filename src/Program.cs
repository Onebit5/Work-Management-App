using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

class Program
{
    private static List<Worker> workers = new List<Worker>();
    private const string dataFilePath = "workers_data.csv";

    static void Main()
    {
        LoadData();  // Load existing data

        while (true)
        {
            Console.Clear();
            Console.WriteLine("*************************************");
            Console.WriteLine("*    Welcome to Worker Management    *");
            Console.WriteLine("*************************************");
            Console.WriteLine("1. See workers");
            Console.WriteLine("2. Add worker");
            Console.WriteLine("3. Remove worker");
            Console.WriteLine("4. Exit and Save data");
            Console.Write("Enter your choice: ");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Press any key to continue...");
                Console.ReadKey();
                continue;
            }

            switch (choice)
            {
                case 1:
                    SeeAllWorkers();
                    break;

                case 2:
                    AddWorker();
                    break;

                case 3:
                    RemoveWorker();
                    break;

                case 4:
                    SaveData();  // Save data before exiting
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    static void SeeAllWorkers()
    {
        Console.Clear();
        Console.WriteLine("*************************************");
        Console.WriteLine("*         List of All Workers        *");
        Console.WriteLine("*************************************");

        if (workers.Count == 0)
        {
            Console.WriteLine("No workers found.");
        }
        else
        {
            foreach (var worker in workers)
            {
                Console.WriteLine($"-------------------------------------");
                Console.WriteLine($"Name: {worker.Name}");
                Console.WriteLine($"ID: {worker.ID}");
                Console.WriteLine($"Phone: {worker.PhoneNumber}");
                Console.WriteLine($"Entry Date: {worker.EntryDate.ToShortDateString()}");
                Console.WriteLine($"Salary (month): ${worker.Salary}");
                Console.WriteLine($"On Vacation: {worker.OnVacation}");
                Console.WriteLine($"Work Type: {worker.WorkType}");
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void RemoveWorker()
    {
        Console.Clear();
        Console.WriteLine("*************************************");
        Console.WriteLine("*        Remove Worker Menu          *");
        Console.WriteLine("*************************************");

        Console.Write("Enter the ID of the worker to remove: ");
        int idToRemove;
        if (!int.TryParse(Console.ReadLine(), out idToRemove))
        {
            Console.WriteLine("Invalid input. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        var workerToRemove = workers.Find(w => w.ID == idToRemove);

        if (workerToRemove != null)
        {
            workers.Remove(workerToRemove);
            Console.WriteLine($"Worker with ID {idToRemove} removed successfully.");
        }
        else
        {
            Console.WriteLine($"Worker with ID {idToRemove} not found.");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void AddWorker()
    {
        Console.Clear();
        Console.WriteLine("*************************************");
        Console.WriteLine("*          Add Worker Menu           *");
        Console.WriteLine("*************************************");

        Worker newWorker = new Worker();

        Console.Write("Enter Name: ");
        newWorker.Name = Console.ReadLine();

        newWorker.ID = GenerateRandomID();

        Console.Write("Enter Phone Number: ");
        newWorker.PhoneNumber = Console.ReadLine();

        newWorker.EntryDate = DateTime.Now;

        Console.Write("Enter Salary (e.g., $1,000): ");
        string salaryInput = Console.ReadLine();

        // Try parsing the salary using the currency format
        if (decimal.TryParse(salaryInput, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal salary))
        {
            newWorker.Salary = (double)salary;
        }
        else
        {
            Console.WriteLine("Invalid salary format. Please enter the salary in the form of '$1,000'.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            return; // Do not add the worker if the salary is invalid
        }

        Console.Write("Is on Vacation (true/false): ");
        bool onVacation;
        if (!bool.TryParse(Console.ReadLine(), out onVacation))
        {
            Console.WriteLine("Invalid input. Press any key to continue...");
            Console.ReadKey();
            return;
        }
        newWorker.OnVacation = onVacation;

        Console.Write("Enter Work Type: ");
        newWorker.WorkType = Console.ReadLine();

        workers.Add(newWorker);

        Console.WriteLine("Worker added successfully. Press any key to continue...");
        Console.ReadKey();
    }

    static void SaveData()
    {
        using (StreamWriter writer = new StreamWriter(dataFilePath))
        {
            foreach (var worker in workers)
            {
                writer.WriteLine($"{worker.Name},{worker.ID},{worker.PhoneNumber},{worker.EntryDate},{worker.Salary},{worker.OnVacation},{worker.WorkType}");
            }
        }

        Console.WriteLine("Data saved successfully.");
    }

    static void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            workers.Clear(); // Clear existing data

            using (StreamReader reader = new StreamReader(dataFilePath))
            {
                while (!reader.EndOfStream)
                {
                    string[] parts = reader.ReadLine().Split(',');
                    if (parts.Length == 7)
                    {
                        Worker worker = new Worker
                        {
                            Name = parts[0],
                            ID = int.Parse(parts[1]),
                            PhoneNumber = parts[2],
                            EntryDate = DateTime.Parse(parts[3]),
                            Salary = double.Parse(parts[4]),
                            OnVacation = bool.Parse(parts[5]),
                            WorkType = parts[6]
                        };

                        workers.Add(worker);
                    }
                }
            }

            Console.WriteLine("Data loaded successfully.");
        }
        else
        {
            Console.WriteLine("No existing data found.");
        }
    }

    static int GenerateRandomID()
    {
        // Generate a random 12-digit ID
        Random random = new Random();
        string randomIDString = $"{random.Next(10000000, 99999999)}";

        // Parse the concatenated string into an int
        if (int.TryParse(randomIDString, out int randomID))
        {
            return randomID;
        }
        else
        {
            // Handle the case where parsing fails
            throw new InvalidOperationException("Failed to generate a valid random ID.");
        }
    }
}
