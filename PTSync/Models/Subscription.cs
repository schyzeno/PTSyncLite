using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

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
        public string Cycle { get; set; }
        public JObject Json()
        {
            JObject jsub = new JObject();
            jsub.Add("Name", Name);
            jsub.Add("Source", Source);
            jsub.Add("Destination", Destination);
            jsub.Add("FileName", FileName);
            jsub.Add("Stage", Stage);
            jsub.Add("Type", Type);
            jsub.Add("Cycle", Cycle);
            return jsub;
        }
    }
}

