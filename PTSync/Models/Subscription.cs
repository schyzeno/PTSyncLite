using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace PTSync.Models
{
    public class Subscription
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string FileName { get; set; }
        public string Stage { get; set; }
        public string Type { get; set; }
    }
}

