using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models.Statistics
{
    public class BlockStatisticsDTO
    {
        public Guid BlockId { get; set; }

        public string BlockName { get; set; }

        public int FullyCountTask { get; set; }

        public int FullyDurationNeeded { get; set; }

        public int CompletedTaskCount { get; set; }

        public int DurationCompletedTask { get; set; }

        public double PercentCompletedTask { get; set; }

        public double PercentDurationCompletedTask { get; set; }
    }
}
