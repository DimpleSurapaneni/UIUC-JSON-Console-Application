# UIUC JSON Console Application

## Overview
The **UIUC JSON Console Application** is a C# console application designed to process training completion data from a JSON file. This application allows organizations to analyze training records, track completions, and manage certifications effectively.

## Features
This application performs the following tasks:
1. **Count Unique Completions**: Lists each training with the number of unique individuals who completed it.
2. **Fiscal Year Analysis**: For specified trainings, lists individuals who completed the training within the defined fiscal year (July 1 of the previous year to June 30 of the current year).
3. **Expiration Check**: Identifies individuals whose trainings have expired or will expire within one month of a given date, indicating whether each training is expired or expiring soon.

## Getting Started

### Requirements
Before you begin, ensure you have the following:
- [.NET SDK](https://dotnet.microsoft.com/download) (version 6.0 or later)
- Visual Studio or any other compatible C# development environment

### Setup Instructions
1. **Clone the Repository**:
   Open your terminal (Command Prompt or PowerShell) and run:
   ```bash
   git clone https://github.com/DimpleSurapaneni/UIUC-JSON-Console-Application.git
   cd UIUC-JSON-Console-Application

### OR

1. **Open the Project**:
   - Launch Visual Studio.
   - Open the project file (`.csproj`).

2. **Install Required Packages**:
   - Ensure you have the necessary NuGet packages installed. Visual Studio should automatically restore these packages when you open the project. If the packages are not restored automatically, you can do it manually by following these steps:
     - Go to **Tools > NuGet Package Manager > Package Manager Console**.
     - In the Package Manager Console, run the following command:
       ```powershell
       dotnet restore
       ```
## Usage

1. **Input File**: 
   - Ensure the `trainings.txt` JSON file is located in the user's **Downloads** folder.
   - Alternatively, update the file path in the code to point to the actual location of `trainings.txt` as per your setup if necessary.

2. **Run the Application**:
   - Open a terminal in the project directory.
   - Execute the following command to build and run the application:
     ```bash
     dotnet run
     ```

3. **Check the Output Files**:
   - After the application has completed execution, the following output files will be generated in the same directory as the input file:
     - **CountOfPeopleCompletedTrainings.json**: Lists the number of unique individuals who completed each training.
     - **PeopleCompletedTrainingsFiscalYear.json**: Lists individuals who completed specified trainings during the fiscal year 2024.
     - **PeopleCompletedExpiredTrainings.json**: Identifies individuals whose trainings have expired or will expire soon.
## Project Structure
The project is organized as follows:
 ```bash
   UIUC-JSON-Console-Application/
   │
   ├── Program.cs                  # Main application logic
   ├── models/                     # Contains data models
   │   ├── Person.cs               # Model representing a person
   │   └── TrainingCompletion.cs    # Model representing a training completion
   ├── trainings.txt               # Input JSON file with training data
   ├── CountOfPeopleCompletedTrainings.json  # Output for Task 1
   ├── PeopleCompletedTrainingsFiscalYear.json  # Output for Task 2
   ├── PeopleCompletedExpiredTrainings.json     # Output for Task 3
   └── UIUC-JSON-Console-Application.csproj    # Project file
```
## How It Works
1. **Input**: The application reads the `trainings.txt` file for training completion data.
2. **Processing**: It processes the data through three main tasks:
   - **Task 1**: Counts unique completions for each training and saves this information.
   - **Task 2**: Lists individuals who completed specified trainings during the fiscal year (July 1, 2023, to June 30, 2024).
   - **Task 3**: Identifies individuals with trainings that have expired or will expire soon.
3. **Output**: The results are saved as JSON files in the same directory as the input file for easy access.
## Output Formats
The application generates output in the following formats:

1. **Count of People Completed Trainings**: 
   - **File**: `CountOfPeopleCompletedTrainings.json`
   - **Description**: This file lists each training along with the number of unique individuals who have completed it.
   - **Sample Output**:
     ```json
     [
       {
         "training": "Electrical Safety for Labs",
         "count": 118
       },
       {
         "training": "Safe Handling of Human Cell Lines/Materials in a Research Laboratory",
         "count": 99
       }
     ]
     ```

2. **People Completed Trainings Given Year**:
   - **File**: `PeopleCompletedTrainingsFiscalYear.json`
   - **Description**: This file contains a list of individuals who completed specific trainings within the defined fiscal year (2024).
   - **Sample Output**:
     ```json
     [
       {
         "training": "Electrical Safety for Labs",
         "fiscalYear": 2024,
         "people": ["Allie Barnes", "Brogan Stein", "Lexie Ho"]
       },
       {
         "training": "X-Ray Safety",
         "fiscalYear": 2024,
         "people": ["Asia Duke", "Anabelle Braun", "Cloe Williamson"]
       }
     ]
     ```

3. **People Completed Expired Trainings**:
   - **File**: `PeopleCompletedExpiredTrainings.json`
   - **Description**: This file identifies individuals with trainings that have expired or will expire soon.
   - **Sample Output**:
     ```json
     [
       {
         "name": "Lexie Mckinney",
         "expiredTrainings": [
           {
             "training": "IRB Quiz",
             "status": "Expired"
           }
         ]
       },
       {
         "name": "Gretchen Boyer",
         "expiredTrainings": [
           {
             "training": "Safe Handling of Human Cell Lines/Materials in a Research Laboratory",
             "status": "Expired"
           }
         ]
       }
     ]
     ```


## Contributions
Contributions to this project are welcome! You can help improve it by:
1. Forking the repository.
2. Creating a new branch for your feature (`git checkout -b feature-branch`).
3. Making your changes and committing them (`git commit -m 'Add your feature'`).
4. Pushing to the branch (`git push origin feature-branch`).
5. Opening a pull request for review.

