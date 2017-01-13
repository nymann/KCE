using System;
using KCE.BoardRepresentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class Evaluate
    {
        [TestMethod]
        public void EvaluateTest()
        {
            Assert.AreEqual(0, PerformEvaluate(Definitions.STDSETUP));
            Assert.AreEqual(-30, PerformEvaluate("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1"));
        }

        private int PerformEvaluate(string fen)
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs = helper.BoardsetupFromFen(fen);
            return new KCE.BoardRepresentation.Evaluate().EvalPosition(bs);
        }

        // https://chessprogramming.wikispaces.com/Tapered+Eval
        [TestMethod]
        public void TaperedEvalTest()
        {
            string expected = "Middle";
            string actual = StateOfGame(TaperedEval(0, 1, 2, 2, 0, 0, 2, 2, 2, 0));
            Assert.AreEqual(expected, actual);
        }


        private int TaperedEval(int wP, int wN, int wB, int wR, int wQ, int bP, int bN, int bB, int bR, int bQ)
        {
            int pawnPhase = 0;
            int knightPhase = 1;
            int bishopPhase = 1;
            int rookPhase = 2;
            int queenPhase = 4;
            int totalPhase = pawnPhase * 16 + knightPhase * 4 + bishopPhase * 4 + rookPhase * 4 + queenPhase * 2;
            int phase = totalPhase;

            phase -= wP * pawnPhase;
            phase -= wN * knightPhase;
            phase -= wB * bishopPhase;
            phase -= wR * rookPhase;
            phase -= wQ * queenPhase;

            phase -= bP * pawnPhase;
            phase -= bN * knightPhase;
            phase -= bB * bishopPhase;
            phase -= bR * rookPhase;
            phase -= bQ * queenPhase;

            phase = (phase * 256 + (totalPhase / 2)) / totalPhase;

            int eval = ((100 * (256 - phase)) + (300 * phase)) / 256;

            return eval;
        }

        private string StateOfGame(int eval)
        {
            if (eval > 170 && eval < 230)
            {
                return "Middle";
            }

            if (eval <= 170)
            {
                return "Opening";
            }

            return "Endgame";
        }
    }
}