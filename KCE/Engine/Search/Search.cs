using KCE.BoardRepresentation;
using System;
using System.Collections.Generic;

namespace KCE.Engine.Search
{
    public class Search
    {
        private Evaluate eval = new Evaluate();
        public Search()
        {
            
        }

        // Inspiration from VICE Chess Engine.
        public int AlphaBeta(int alpha, int beta, int depth, BoardState bs, SearchInfo sInfo)
        {
            if (depth == 0)
            {
                return new Evaluate().EvalPosition(bs);
            }

            // Check if time is up or interrupted by the GUI.
            if (sInfo.IsTimeUp())
            {
                sInfo.Stopped = true;
            }

            sInfo.Nodes++;

            // TODO: Check if repetition aswell.
            if (bs.FiftyMoveRule >= 100)
            {
                return 0;
            }

            MoveGenerator mg = new MoveGenerator(bs);

            List<Ply> legalMoves = mg.AllLegalMoves();

            Ply BestMove = null;
            //int score = -Definitions.INFINITE;
            int nMoves = legalMoves.Count;
            int oldAlpha = alpha;
            int moveNum = 0;
            

            for (moveNum = 0; moveNum < legalMoves.Count; moveNum++)
            {
                //legalMoves = PickNextMove(moveNum, legalMoves);

                mg.MakeMove(legalMoves[moveNum]);
                legalMoves[moveNum].Score = -AlphaBeta(-beta, -alpha, depth - 1, bs, sInfo);
                mg.UndoMove(legalMoves[moveNum]);

                if (sInfo.Stopped)
                {
                    return Definitions.Stopped;
                }

                if (legalMoves[moveNum].Score > alpha)
                {
                    if (legalMoves[moveNum].Score >= beta)
                    {
                        if (nMoves == 1)
                        {
                            sInfo.Fhf++;
                        }
                        sInfo.Fh++;

                        /*if (!(list->moves[MoveNum].move & MFLAGCAP))
                        {
                            pos->searchKillers[1][pos->ply] = pos->searchKillers[0][pos->ply];
                            pos->searchKillers[0][pos->ply] = list->moves[MoveNum].move;
                        }*/

                        return beta; // Fail hard beta-cutoff.
                    }

                    alpha = legalMoves[moveNum].Score; // alpha acts like max in minimax.
                    BestMove = legalMoves[moveNum];

                    /*if (!(list->moves[MoveNum].move & MFLAGCAP))
                    {
                        pos->searchHistory[pos->pieces[FROMSQ(BestMove)]][TOSQ(BestMove)] += depth;
                    }*/
                }
            }

            if (nMoves == 0)
            {
                if (new Helper().IsKingInCheck(bs.SideToMove, bs.BoardRepresentation, bs.KingSquares))
                {
                    // We are mate in X plys (bs.Ply).
                    return -Definitions.MATE + bs.Ply;
                }

                // Stalemate.
                return 0;
            }

            if (alpha != oldAlpha)
            {
                bs.BestPly = BestMove;
                // StorePvMove(pos, BestMove);
            }

            return alpha;
        }

        public void SearchPosition(BoardState bs, SearchInfo sInfo)
        {
            
            int depth = 1;

            while (!sInfo.IsTimeUp())
            {
                var bestScore = AlphaBeta(-Definitions.INFINITE, Definitions.INFINITE, depth, bs, sInfo);
                
                if (bestScore == Definitions.Stopped)
                {
                    bs.BestPly = bs.BestPlyAtLowerDepth;
                    break;
                }
                bs.BestPlyAtLowerDepth = bs.BestPly;
                /*Console.WriteLine("Move: {3}, Score: {0}, Depth: {1}, Nodes: {4}, Time: {2} ms, Ordering: {5}/{6}.",
                    bestScore, depth, Definitions.StdTimePrPly - sInfo.TimeLeft(), bs.BestPly.GetAlgebraicPly(),
                    sInfo.Nodes, sInfo.Fhf, sInfo.Fh);*/
                Console.WriteLine("info depth {1} nodes {2} time {3} score cp {0}", bestScore, depth, sInfo.Nodes, sInfo.ElapsedTime());
                Console.WriteLine("info currmove {0}", bs.BestPly.GetAlgebraicPly());
                depth++;
            }

            Console.WriteLine("bestmove {0}", bs.BestPly.GetAlgebraicPly());
            //new Helper().PrintBoardWhitePerspective(bs.BoardRepresentation);
        }

        private List<Ply> PickNextMove(int moveNum, List<Ply> legalMoves)
        {
            Ply temp;
            int index = 0;
            int bestScore = 0;
            int bestNum = moveNum;

            for (index = moveNum; index < legalMoves.Count; index++)
            {
                if (legalMoves[index].Score != - Definitions.INFINITE)
                {
                    Console.WriteLine("Changing indexes.");
                    bestScore = legalMoves[index].Score;
                    bestNum = index;
                }
            }

            temp = legalMoves[moveNum];
            legalMoves[moveNum] = legalMoves[bestNum];
            legalMoves[bestNum] = temp;

            return legalMoves;
        }
    }
}