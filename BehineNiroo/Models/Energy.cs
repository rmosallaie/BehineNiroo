using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehineNiroo.Models
{
    public class Energy
    {
        public string EnergyTime { get; set; }

        public int EnergyIntensity { get; set; }

        public int ActElmpWh { get; set; }

        public int T1ImpWh { get; set; }

        public int T2ImpWh { get; set; }

        public int T3ImpWh { get; set; }

        public int T4ImpWh { get; set; }

        public int DemandImpW { get; set; }

        public int ActEExpWh { get; set; }

        public int T1ExpWh { get; set; }

        public int T2ExpWh { get; set; }

        public int T3ExpWh { get; set; }

        public int T4ExpWh { get; set; }

        public int DemandExpw { get; set; }

        public int ReactEImpVArh { get; set; }

        public string CityName { get; set; }

        public string PostName { get; set; }

        public string FeederName { get; set; }
    }
}
