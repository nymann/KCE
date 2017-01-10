using System;
using KCE.BoardRepresentation.PieceRules;
using KCE.Engine;

namespace KCE.BoardRepresentation
{
    public class Search
    {
        private Evaluate eval = new Evaluate();
        public Search()
        {
            
        }

        public void IsTimeUp()
        {
            
        }

        public int Quiescence(int alpha, int beta, BoardState bs)
        {
            return 0;
        }

        public int AlphaBeta(int alpha, int beta, int depth, BoardState bs)
        {
            bs.Nodes++;
            if (depth == 0)
            {
                return eval.EvalPosition(bs);
            }

            if (bs.FiftyMoveRule >= 100)
            {
                return 0;
            }

            
            MoveGenerator moveGenerator = new MoveGenerator(bs);
            Helper helper = new Helper();

            var allLegalMoves = moveGenerator.AllLegalMoves();
            int numberOfLegalMoves = allLegalMoves.Count;

            foreach (var legalMove in allLegalMoves)
            {
                moveGenerator.MakeMove(legalMove);
                var score = -AlphaBeta(-beta, -alpha, depth - 1, bs);
                moveGenerator.UndoMove(legalMove);

                if (score == Definitions.MATE)
                {
                    bs.BestPly = legalMove;
                    return Definitions.MATE;
                }

                if (score > alpha)
                {
                    if (score >= beta)
                    {
                        return beta;
                    }
                }
                alpha = score;
                bs.BestPly = legalMove;
            }

            if (numberOfLegalMoves != 0) return alpha;
            if (helper.IsKingInCheck(bs.SideToMove, bs.BoardRepresentation, bs.KingSquares))
            {
                return -Definitions.MATE;
            }

            return 0; // stalemate
        }

        public void SearchPosition(BoardState bs)
        {
            int bestScore = - Definitions.INFINITE;
            int currentDepth = 0;

            for (currentDepth = 1; currentDepth <= Definitions.MAXDEPTH; currentDepth++)
            {                              // alpha                 // beta
                bestScore = AlphaBeta(-Definitions.INFINITE, Definitions.INFINITE, currentDepth, bs);
                if (bestScore == Definitions.MATE || bestScore == -Definitions.MATE)
                {
                    break;
                }
            }

            Console.WriteLine("Depth: {0}, Score: {1}, Move: {2}, Nodes Searched: {3}", currentDepth, bestScore, bs.BestPly.GetAlgebraicPly(), bs.Nodes);
        }
    }
}