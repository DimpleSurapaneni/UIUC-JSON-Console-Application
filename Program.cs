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
          
            string jsonFile = @"C:\Users\dimpl\OneDrive\Desktop\GIT Projects\UIUC JSON Console Application\trainings.txt";

            string jsonData = File.ReadAllText(jsonFile);

           
            var people = JsonConvert.DeserializeObject<List<Person>>(jsonData);

            
            string outputDirectory = Path.GetDirectoryName(jsonFile);

            
            Task1(people, outputDirectory); // Count how many people completed each training
            Task2(people, outputDirectory); // List people who completed specified trainings in fiscal year 2024
            Task3(people, outputDirectory); // Identify people with expired or soon-to-expire trainings
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

       
        static void Task2(List<Person> people, string outputDirectory)
        {
           
            DateTime fiscalYearStart = new DateTime(2023, 7, 1);
            DateTime fiscalYearEnd = new DateTime(2024, 6, 30);

            
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
                    fiscalYear = 2024, 
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

       
        static void Task3(List<Person> people, string outputDirectory)
        {
           
            DateTime specifiedDate = new DateTime(2023, 10, 1);
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
