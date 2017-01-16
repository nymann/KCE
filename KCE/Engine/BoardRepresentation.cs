/*using System;
using System.Diagnostics;

namespace KCE.Engine
{
    public class BoardRepresentation
    {
        public BoardRepresentation(string fen, int depth)
        {
            var helper = new Helper();
            var bs = helper.BoardsetupFromFen(fen);
            helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var moveGenerator = new MoveGenerator(bs);
            var calcNodes = moveGenerator.Divide(depth);
            stopwatch.Stop();
            var elapsedTime = stopwatch.ElapsedMilliseconds;
            var nodesSec = calcNodes/(elapsedTime*0.001);
            Console.WriteLine("Depth: {0}, Nodes: {1}, Time: {2} ms, Nodes/Sec: {3}", depth, calcNodes,
                stopwatch.ElapsedMilliseconds, Math.Round(nodesSec));
        }
    }
}*/