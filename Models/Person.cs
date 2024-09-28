using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIUC_JSON_Console_Application.Models
{
    public class Person
    {
        public string? Name { get; set; }
        public List<TrainingCompletion>? Completions { get; set; }
    }
}
