using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class CodingSession
    {
        public Int64 Id {  get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration { get; set; }

        public CodingSession(System.Int64 id, System.String startTime, System.String endTime)
        {
            Id = id;
            StartTime = DateTime.ParseExact(startTime, "dd-MM-yy HH:mm", null);
            EndTime = DateTime.ParseExact(endTime, "dd-MM-yy HH:mm", null);
            Duration = EndTime - StartTime;
        }
    }
}
