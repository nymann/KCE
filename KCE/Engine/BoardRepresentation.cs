using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using KCE.BoardRepresentation.PieceRules;
using KCE.Engine;

namespace KCE.BoardRepresentation
{
    public class BoardRepresentation
    {
        private BoardState boardState;
        public BoardRepresentation(string fen, int depth)
        {
            Helper helper = new Helper();
            boardState = helper.BoardsetupFromFen(fen);
            helper.PrintBoardWhitePerspective(boardState.BoardRepresentation);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            MoveGenerator moveGenerator = new MoveGenerator(boardState);
            UInt64 calcNodes = moveGenerator.Divide(depth);
            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            double nodesSec = calcNodes/(elapsedTime*0.001);
            Console.WriteLine("Depth: {0}, Nodes: {1}, Time: {2} ms, Nodes/Sec: {3}", depth, calcNodes,
                stopwatch.ElapsedMilliseconds, Math.Round(nodesSec));
        }
    }
}