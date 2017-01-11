using System;
using System.Diagnostics;
using KCE.BoardRepresentation;

namespace KCE.Engine.Search
{
    public class SearchInfo
    {
        public long Stoptime { get; set; }
        public bool TimeSet { get; set; }

        public int Depth { get; set; }
        public bool DepthSet { get; set; }
        public int MovesToGo { get; set; }
        public int Infinite { get; set; }
        public ulong Nodes { get; set; } = 0;
        public bool Quit { get; set; }
        public bool Stopped { get; set; }
        private Stopwatch stopwatch;

        public SearchInfo()
        {
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        public bool IsTimeUp()
        {
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            return elapsedTime > Definitions.StdTimePrPly;
        }

        public long TimeLeft()
        {
            return Definitions.StdTimePrPly - stopwatch.ElapsedMilliseconds;
        }
    }
}