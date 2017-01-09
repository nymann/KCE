using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using KCE.BoardRepresentation.PieceRules;

namespace KCE.BoardRepresentation
{
    public class BoardRepresentation
    {
        private BoardState boardState;
        public BoardRepresentation(string fen)
        {
            Helper helper = new Helper();
            ;
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
            int depth = 5;
            UInt64 calcNodes = moveGenerator.Perft(depth);
            stopwatch.Stop();
            Console.WriteLine("Depth: {0}, total legal moves {1}, calculated in {2} ms.", depth, calcNodes,
                stopwatch.ElapsedMilliseconds);

        }
    }
}