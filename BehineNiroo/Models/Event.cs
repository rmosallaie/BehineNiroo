using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BehineNiroo.Models
{
    public class Event
    {
        public Int16 SerialNum { get; set; }

        public UInt32 EventNumber { get; set; }

        [Required]
        public string EventValue { get; set; }

        public string EventTime { get; set; }

        public int MilliTime { get; set; }

        [Required]
        public string EventReason { get; set; }

        public int EventType { get; set; }

        public string CityName { get; set; }

        public string PostName { get; set; }

        public string FeederName { get; set; }

    }
}
