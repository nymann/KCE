using System;

namespace KCE.Engine.Search
{

    public class Search
    {
        private readonly Evaluate _eval = new Evaluate();
        private readonly Helper _helper = new Helper();

        public int Quiescene(int alpha, int beta, BoardState bs, SearchInfo sInfo)
        {
            if (sInfo.IsTimeUp())
            {
                sInfo.Stopped = true;
            }

            sInfo.Nodes++;

            /*if (_helper.IsRepetition(bs) || bs.FiftyMoveRule >= 100)
            {
                //if (bs.BestPly != null) bs.BestPly.Score = 0;
                return 0;
            }*/

            var score = _eval.EvalPosition(bs);

            if (score >= beta)
            {
                return beta;
            }

            if (score > alpha)
            {
                alpha = score;
            }

            var mg = new MoveGenerator(bs);

            var capMoves = mg.AllCapMoves();

            Ply bestMove = null;
            var nMoves = 0;
            if (capMoves != null)
            {
                nMoves = capMoves.Length;
            }
            var oldAlpha = alpha;
            var moveNum = 0;

            if (bs.BestPlyAtLowerDepth != null && !bs.HaveSearched)
            {
                mg.MakeMove(bs.BestPlyAtLowerDepth);
                bs.BestPlyAtLowerDepth.Score = -Quiescene(-beta, -alpha, bs, sInfo);
                mg.UndoMove(bs.BestPlyAtLowerDepth);
                bs.HaveSearched = true;
                bestMove = bs.BestPlyAtLowerDepth;
                alpha = bestMove.Score;
                //Console.WriteLine("This never happens.");
            }

            for (moveNum = 0; moveNum < nMoves; moveNum++)
            {
                mg.MakeMove(capMoves[moveNum]);
                capMoves[moveNum].Score = -Quiescene(-beta, -alpha, bs, sInfo);
                mg.UndoMove(capMoves[moveNum]);

                if (sInfo.Stopped)
                {
                    return Definitions.Stopped;
                }

                if (capMoves[moveNum].Score <= alpha)
                {
                    continue;
                }

                if (capMoves[moveNum].Score >= beta)
                {
                    if (nMoves == 1)
                    {
                        sInfo.Fhf++;
                    }
                    sInfo.Fh++;

                    return beta; // Fail hard beta-cutoff.
                }

                alpha = capMoves[moveNum].Score; // alpha acts like max in minimax.
                bestMove = capMoves[moveNum];
            }

            if (alpha != oldAlpha)
            {
                //Console.WriteLine("New best ply!");
                bs.BestPly = bestMove;
            }
            return alpha;
        }

        // Inspiration from VICE Chess Engine.
        public int AlphaBeta(int alpha, int beta, int depth, BoardState bs, SearchInfo sInfo)
        {
            if (depth == 0)
            {
                //return _eval.EvalPosition(bs);
                return Quiescene(alpha, beta, bs, sInfo);
            }

            // Check if time is up or interrupted by the GUI.
            if (sInfo.IsTimeUp())
            {
                sInfo.Stopped = true;
            }

            sInfo.Nodes++;

            /*if (bs.BestPly != null)
            {
                if (bs.FiftyMoveRule >= 100 || _helper.IsRepetition(bs))
                {
                    return 0;
                    //Console.WriteLine("bestmove: {0}{1}, score {2}", Definitions.IndexToAlgebraic[bs.BestPly.GetFromToSquare()[0]], Definitions.IndexToAlgebraic[bs.BestPly.GetFromToSquare()[1]], bs.BestPly.Score);
                }
            }*/

            var mg = new MoveGenerator(bs);

            var legalMoves = mg.AllLegalMoves();

            Ply bestMove = null;
            var nMoves = legalMoves.Length;

            var oldAlpha = alpha;
            var moveNum = 0;

            if (bs.BestPlyAtLowerDepth != null && !bs.HaveSearched)
            {
                mg.MakeMove(bs.BestPlyAtLowerDepth);
                bs.BestPlyAtLowerDepth.Score = -AlphaBeta(-beta, -alpha, depth - 1, bs, sInfo);
                mg.UndoMove(bs.BestPlyAtLowerDepth);
                bs.HaveSearched = true;
                bestMove = bs.BestPlyAtLowerDepth;
                alpha = bestMove.Score;
            }

            for (moveNum = 0; moveNum < legalMoves.Length; moveNum++)
            {
                mg.MakeMove(legalMoves[moveNum]);
                legalMoves[moveNum].Score = -AlphaBeta(-beta, -alpha, depth - 1, bs, sInfo);
                mg.UndoMove(legalMoves[moveNum]);

                if (sInfo.Stopped)
                {
                    return Definitions.Stopped;
                }

                if (legalMoves[moveNum].Score >= beta)
                {
                    if (nMoves == 1)
                    {
                        sInfo.Fhf++;
                    }
                    sInfo.Fh++;

                    return beta; // Fail hard beta-cutoff.
                }
                if (legalMoves[moveNum].Score > alpha)
                {
                    alpha = legalMoves[moveNum].Score; // alpha acts like max in minimax.
                    bestMove = legalMoves[moveNum];
                }
            }

            if (nMoves == 0)
            {
                if (_helper.IsKingInCheck(bs.SideToMove, bs.BoardRepresentation, bs.KingSquares))
                {
                    return -Definitions.MATE + bs.Ply;
                }

                // Stalemate.
                return 0;
            }

            if (alpha != oldAlpha)
            {
                bs.BestPly = bestMove;
            }

            return alpha;
        }

        public void SearchPosition(BoardState bs, SearchInfo sInfo)
        {
            var move = "";
            var depth = 1;
            var bestScore = - Definitions.INFINITE;
            while (!sInfo.IsTimeUp() || bestScore < Definitions.MATE - 10)
            {
                bestScore = AlphaBeta(-Definitions.INFINITE, Definitions.INFINITE, depth, bs, sInfo);

                if (bestScore == Definitions.Stopped)
                {
                    bs.BestPly = bs.BestPlyAtLowerDepth;
                    break;
                }

                if (bestScore > Definitions.MATE - 20)
                {
                    Console.WriteLine("info depth {1} nodes {2} time {3} score mate {0}", Definitions.MATE - (bestScore / 2 + 1),
                        depth, sInfo.Nodes, sInfo.ElapsedTime());
                    break;
                }

                if (bestScore < -Definitions.MATE + 20)
                {
                    Console.WriteLine("info depth {1} nodes {2} time {3} score mate -{0}", Definitions.MATE - (bestScore/2 +1),
                        depth, sInfo.Nodes, sInfo.ElapsedTime());
                }
                else
                {
                    Console.WriteLine("info depth {1} nodes {2} time {3} score cp {0}", bestScore, depth, sInfo.Nodes,
                        sInfo.ElapsedTime());
                }

                bs.BestPlyAtLowerDepth = bs.BestPly;
                /*Console.WriteLine("Move: {3}, Score: {0}, Depth: {1}, Nodes: {4}, Time: {2} ms, Ordering: {5}/{6}.",
                    bestScore, depth, sInfo.ElapsedTime(), bs.BestPly.GetAlgebraicPly(),
                    sInfo.Nodes, sInfo.Fhf, sInfo.Fh);*/

                move = Definitions.IndexToAlgebraic[bs.BestPly.GetFromToSquare()[0]] +
                              Definitions.IndexToAlgebraic[bs.BestPly.GetFromToSquare()[1]];
                Console.WriteLine("info currmove {0}{1}", move, bs.BestPly.Promotion);
                depth++;
                bs.HaveSearched = false;
            }
            Console.WriteLine("bestmove: {0}{1}, score: {2}", move, bs.BestPly.Promotion, bs.BestPly.Score);
        }
    }
}