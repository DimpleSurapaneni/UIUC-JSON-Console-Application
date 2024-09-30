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

##OR

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
