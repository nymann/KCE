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

        /*public int AlphaBeta(int alpha, int beta, int depthLeft, BoardState bs)
        {
            bs.Nodes++;
            if (depthLeft == 0)
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
                var score = -AlphaBeta(-beta, -alpha, depthLeft - 1, bs);
                moveGenerator.UndoMove(legalMove);
                if (legalMove.GetAlgebraicPly() == "e2e4" /*|| legalMove.GetAlgebraicPly() == "e2e4"#1#)
                {
                    Console.WriteLine("Move: {0}, Score: {1}, Depth: {2}.", legalMove.GetAlgebraicPly(), score, depthLeft);
                }

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
            //Console.WriteLine("Score: {0}, Move: {1}, Depth: {2}", alpha, bs.BestPly.GetAlgebraicPly(), depthLeft);

            if (numberOfLegalMoves != 0) return alpha;
            if (helper.IsKingInCheck(bs.SideToMove, bs.BoardRepresentation, bs.KingSquares))
            {
                return -Definitions.MATE;
            }

            return 0; // stalemate
        }*/


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
            int score = -Definitions.INFINITE;
            int nMoves = legalMoves.Count;
            int oldAlpha = alpha;

            for (var moveNum = 0; moveNum < legalMoves.Count; moveNum++)
            {
                // PickNextMove(moveNum, legalMoves)

                mg.MakeMove(legalMoves[moveNum]);
                score = -AlphaBeta(-beta, -alpha, depth - 1, bs, sInfo);
                mg.UndoMove(legalMoves[moveNum]);

                if (sInfo.Stopped)
                {
                    return 0;
                }

                if (score > alpha)
                {
                    if (score >= beta)
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

                    alpha = score; // alpha acts like max in minimax.
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

        public Ply Temp(int depth, BoardState bs, SearchInfo sInfo)
        {
            Helper helper = new Helper();
            Ply bestPly = null;
            int bestScore = (-1) * Definitions.INFINITE + depth;
            MoveGenerator mg = new MoveGenerator(bs);

            var legalMoves = mg.AllLegalMoves();
            sInfo.Nodes += (ulong) legalMoves.Count;
            Evaluate evaluate = new Evaluate();
            if (depth == 1)
            {
                foreach (Ply legalMove in legalMoves)
                {
                    mg.MakeMove(legalMove);
                    legalMove.Score = evaluate.EvalPosition(bs);
                    mg.UndoMove(legalMove);

                    if (legalMove.Score > bestScore)
                    {
                        Console.WriteLine("\nMove: {0}, Score: {1}, Nodes: {2}.", legalMove.GetAlgebraicPly(), legalMove.Score, sInfo.Nodes);
                        helper.PrintBoardWhitePerspective(legalMove.GetBoard());
                        bestPly = legalMove;
                        bestScore = legalMove.Score;
                    }
                }
            }
            else
            {

                foreach (Ply legalMove in legalMoves)
                {
                    mg.MakeMove(legalMove);
                    Ply currentPly = Temp(depth - 1, bs, sInfo);
                    mg.UndoMove(legalMove);

                    if (currentPly != null)
                    {
                        bestPly = currentPly;
                    }
                }
            }
            return bestPly;
        }

        public void SearchPosition(BoardState bs)
        {
            SearchInfo sInfo = new SearchInfo();
            int depth = 1;

            while (!sInfo.IsTimeUp())
            {
                var bestScore = AlphaBeta(-Definitions.INFINITE, Definitions.INFINITE, depth, bs, sInfo);
                Console.WriteLine("Move: {3}, Score: {0}, Depth: {1}, Timeleft: {2} ms. FH: {4}, FHF: {5}", 
                    bestScore, depth, sInfo.TimeLeft(), bs.BestPly.GetAlgebraicPly(),
                    sInfo.Fh, sInfo.Fhf);
                depth++;
            }
        }
    }
}