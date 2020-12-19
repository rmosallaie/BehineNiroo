using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BehineNiroo.Models
{
    public class Setting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Items { get; set; }
        public string Type { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
