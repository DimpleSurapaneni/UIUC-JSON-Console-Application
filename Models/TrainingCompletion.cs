using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIUC_JSON_Console_Application.Models
{
    public class TrainingCompletion
    {
        public string? Name { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? Expires { get; set; }
    }
}
