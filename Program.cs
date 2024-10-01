using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UIUC_JSON_Console_Application.Models;

namespace UIUC_JSON_Console_Application
{
    class Program
    {
        static void Main(string[] args)
        {

            // Path to the input file --> the below path is given in perspective to the download of this zip file
            //Please change the file path accordingly if necessary

            string jsonFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "UIUC-JSON-Console-Application-main", "trainings.txt");
            Console.WriteLine(jsonFile);

            if (!File.Exists(jsonFile))
            {
                Console.WriteLine("File not found: {0}", jsonFile);
                return;
            }

            string jsonData = File.ReadAllText(jsonFile);
            var people = JsonConvert.DeserializeObject<List<Person>>(jsonData);

            //Change this Output Directory as Necessary in the below
            string outputDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "UIUC-JSON-Console-Application-main");
            Console.WriteLine(outputDirectory);


            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            Task1(people, outputDirectory); // Count how many people completed each training
            int fiscalYear = 2024;
            Task2(people, outputDirectory, fiscalYear); // List people who completed specified trainings in fiscal year 2024
            DateTime specifiedDate = new DateTime(2023, 10, 1);
            Task3(people, outputDirectory, specifiedDate); // Identify people with expired or soon-to-expire trainings
        }

        static void Task1(List<Person> people, string outputDirectory)
        {
            var mostRecentCompletions = people
                .SelectMany(p => p.Completions
                    .GroupBy(c => c.Name)
                    .Select(g => new
                    {
                        TrainingName = g.Key,
                        MostRecentCompletion = g.OrderByDescending(c => c.Timestamp).FirstOrDefault(),
                        PersonName = p.Name
                    }))
                .ToList();

            var trainingCounts = mostRecentCompletions
                .GroupBy(c => c.TrainingName)
                .Select(g => new
                {
                    training = g.Key,
                    count = g.Select(x => x.PersonName).Distinct().Count()
                });

            string outputFilePath = Path.Combine(outputDirectory, "CountOfPeopleCompletedTrainings.json");

            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(trainingCounts, Formatting.Indented));
            Console.WriteLine("Task 1 completed: CountOfPeopleCompletedTrainings.json generated.");
        }

        static void Task2(List<Person> people, string outputDirectory, int fiscalYear)
        {
            DateTime fiscalYearStart = new DateTime(fiscalYear - 1, 7, 1);
            DateTime fiscalYearEnd = new DateTime(fiscalYear, 6, 30);

            var specifiedTrainings = new List<string>
            {
                "Electrical Safety for Labs",
                "X-Ray Safety",
                "Laboratory Safety Training"
            };

            var task2Result = specifiedTrainings
                .Select(training => new
                {
                    training,
                    fiscalYear,
                    people = people
                        .Select(p => new
                        {
                            Name = p.Name,
                            MostRecentCompletion = p.Completions
                                .Where(c => c.Name == training)
                                .OrderByDescending(c => c.Timestamp)
                                .FirstOrDefault()
                        })
                        .Where(p => p.MostRecentCompletion != null &&
                                    p.MostRecentCompletion.Timestamp >= fiscalYearStart &&
                                    p.MostRecentCompletion.Timestamp <= fiscalYearEnd)
                        .Select(p => p.Name)
                        .ToList()
                })
                .Where(result => result.people.Count > 0);

            string outputFilePath = Path.Combine(outputDirectory, "PeopleCompletedTrainingsFiscalYear.json");

            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(task2Result, Formatting.Indented));
            Console.WriteLine("Task 2 completed: PeopleCompletedTrainingsFiscalYear.json generated.");
        }

        static void Task3(List<Person> people, string outputDirectory, DateTime specifiedDate)
        {
            DateTime expiryThreshold = specifiedDate.AddMonths(1);

            var task3Result = people
                .Select(p => new
                {
                    name = p.Name,
                    expiredTrainings = p.Completions
                        .GroupBy(c => c.Name)
                        .Select(g => g.OrderByDescending(c => c.Timestamp).FirstOrDefault())
                        .Where(c => c.Expires.HasValue &&
                                    (c.Expires.Value < specifiedDate || c.Expires.Value <= expiryThreshold))
                        .Select(c => new
                        {
                            training = c.Name,
                            status = c.Expires.Value < specifiedDate ? "Expired" : "Expires Soon"
                        })
                        .ToList()
                })
                .Where(p => p.expiredTrainings.Count > 0);

            string outputFilePath = Path.Combine(outputDirectory, "PeopleCompletedExpiredTrainings.json");

            File.WriteAllText(outputFilePath, JsonConvert.SerializeObject(task3Result, Formatting.Indented));
            Console.WriteLine("Task 3 completed: PeopleCompletedExpiredTrainings.json generated.");
        }
    }
}
