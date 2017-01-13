using System.Diagnostics;

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
        private readonly Stopwatch _sWatch;
        public float Fh { get; set; } = 0;
        public float Fhf { get; set; } = 0;
        public long TimeLeft { get; set; } = Definitions.StdTimePrPly;

        public SearchInfo()
        {
            _sWatch = new Stopwatch();
            _sWatch.Start();
        }

        public bool IsTimeUp()
        {
            var elapsedTime = _sWatch.ElapsedMilliseconds;
            return elapsedTime > TimeLeft;
        }

        public long ElapsedTime()
        {
            return _sWatch.ElapsedMilliseconds;
        }
    }
}