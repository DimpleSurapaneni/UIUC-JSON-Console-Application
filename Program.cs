using Newtonsoft.Json; // Library for working with JSON
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; // For using LINQ methods
using UIUC_JSON_Console_Application.Models; // Assuming you have the models defined

namespace TrainingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the training data JSON file
            string jsonFile = @"C:\Users\dimpl\OneDrive\Desktop\GIT Projects\UIUC JSON Console Application\trainings.txt";

            // Load and read the JSON file
            string jsonData = File.ReadAllText(jsonFile);

            // Deserialize the JSON data into a list of Person objects
            var people = JsonConvert.DeserializeObject<List<Person>>(jsonData);

            // Get the directory of the input file
            string outputDirectory = Path.GetDirectoryName(jsonFile);

            // Call each task to process the data and provide the output file path
            Task1(people, outputDirectory); // Count how many people completed each training
            Task2(people, outputDirectory); // List people who completed specified trainings in fiscal year 2024
            Task3(people, outputDirectory); // Identify people with expired or soon-to-expire trainings
        }

        // Task 1: Count how many people completed each training
        static void Task1(List<Person> people, string outputDirectory)
        {
            // Get the most recent completion for each training per person
            var mostRecentCompletions = people
                .SelectMany(p => p.Completions
                    .GroupBy(c => c.Name) // Group by training name
                    .Select(g => new
                    {
                        TrainingName = g.Key, // Name of the training
                        MostRecentCompletion = g.OrderByDescending(c => c.Timestamp).FirstOrDefault(), // Most recent completion
                        PersonName = p.Name // Name of the person
                    }))
                .ToList();

            // Count distinct people who completed each training
            var trainingCounts = mostRecentCompletions
                .GroupBy(c => c.TrainingName)
                .Select(g => new
                {
                    training = g.Key, // Training name
                    count = g.Select(x => x.PersonName).Distinct().Count() // Count unique people
                });

            // Set the output file path
            string outputFilePath = Path.Combine(outputDirectory, "countOfPeopleCompletedTrainings.json");

            // Write result to JSON file
            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(trainingCounts, Formatting.Indented));

            Console.WriteLine("Task 1 completed: countOfPeopleCompletedTrainings.json generated.");
        }

        // Task 2: List people who completed specific trainings in fiscal year 2024
        static void Task2(List<Person> people, string outputDirectory)
        {
            // Define the fiscal year range (July 1, 2023 to June 30, 2024)
            DateTime fiscalYearStart = new DateTime(2023, 7, 1);
            DateTime fiscalYearEnd = new DateTime(2024, 6, 30);

            // List of trainings to check
            var specifiedTrainings = new List<string>
            {
                "Electrical Safety for Labs",
                "X-Ray Safety",
                "Laboratory Safety Training"
            };

            // Find people who completed the specified trainings within the fiscal year
            var task2Result = specifiedTrainings
                .Select(training => new
                {
                    training, // Training name
                    fiscalYear = 2024, // Fiscal year
                    people = people
                        .Select(p => new
                        {
                            Name = p.Name,
                            MostRecentCompletion = p.Completions
                                .Where(c => c.Name == training) // Filter for specific training
                                .OrderByDescending(c => c.Timestamp) // Order by completion date
                                .FirstOrDefault() // Get the most recent completion
                        })
                        .Where(p => p.MostRecentCompletion != null && // Ensure there is a completion
                                    p.MostRecentCompletion.Timestamp >= fiscalYearStart && // Within the fiscal year
                                    p.MostRecentCompletion.Timestamp <= fiscalYearEnd)
                        .Select(p => p.Name) // Get the names of people
                        .ToList()
                })
                .Where(result => result.people.Count > 0); // Only include results with people

            // Set the output file path
            string outputFilePath = Path.Combine(outputDirectory, "peopleCompletedTrainingsGivenYear.json");

            // Write result to JSON file
            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(task2Result, Formatting.Indented));

            Console.WriteLine("Task 2 completed: peopleCompletedTrainingsGivenYear.json generated.");
        }

        // Task 3: Find people with expired or soon-to-expire trainings
        static void Task3(List<Person> people, string outputDirectory)
        {
            // Date to check for expiration (October 1, 2023)
            DateTime specifiedDate = new DateTime(2023, 10, 1);
            DateTime expiryThreshold = specifiedDate.AddMonths(1); // 1 month from the specified date

            // Find people whose trainings are expired or expiring soon
            var task3Result = people
                .Select(p => new
                {
                    name = p.Name,
                    expiredTrainings = p.Completions
                        .GroupBy(c => c.Name) // Group by training name
                        .Select(g => g.OrderByDescending(c => c.Timestamp).FirstOrDefault()) // Get most recent completion
                        .Where(c => c.Expires.HasValue && // Check expiration
                                    (c.Expires.Value < specifiedDate || c.Expires.Value <= expiryThreshold))
                        .Select(c => new
                        {
                            training = c.Name, // Training name
                            status = c.Expires.Value < specifiedDate ? "Expired" : "Expires Soon" // Status of the training
                        })
                        .ToList()
                })
                .Where(p => p.expiredTrainings.Count > 0); // Only include results with expired/soon-expiring trainings

            // Set the output file path
            string outputFilePath = Path.Combine(outputDirectory, "peopleCompletedExpiredTrainings.json");

            // Write result to JSON file
            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(task3Result, Formatting.Indented));

            Console.WriteLine("Task 3 completed: peopleCompletedExpiredTrainings.json generated.");
        }
    }
}
