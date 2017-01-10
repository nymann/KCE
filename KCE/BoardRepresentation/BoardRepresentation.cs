using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using KCE.BoardRepresentation.PieceRules;

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
            /*List<Ply> legalMoves = moveGenerator.AllLegalMoves();
            stopwatch.Stop();
            var counter = 1;
            Console.WriteLine("\n{0} legal moves found in {1} ms.\n", legalMoves.Count, stopwatch.ElapsedMilliseconds);
            foreach (Ply legalMove in legalMoves)
            {
                Console.WriteLine("{0}: {1}.", counter, legalMove.GetAlgebraicPly());
                counter++;
            }*/
            UInt64 calcNodes = moveGenerator.Divide(depth);
            stopwatch.Stop();
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            double nodesSec = (double) calcNodes/(elapsedTime*0.001);
            Console.WriteLine("Depth: {0}, Nodes: {1}, Time: {2} ms, Nodes/Sec: {3}", depth, calcNodes,
                stopwatch.ElapsedMilliseconds, Math.Round(nodesSec));

        }
    }
}