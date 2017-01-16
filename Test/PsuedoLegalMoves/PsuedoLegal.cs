using System;
using KCE.Engine;
using KCE.Engine.PieceRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.PsuedoLegalMoves
{
    [TestClass]
    public class PsuedoLegal
    {
        Helper helper = new Helper();

        [TestMethod]
        public void PsuedoLegalKingMovesTest()
        {
            BoardState bs = SetupBs("k7/8/K7/P7/8/8/8/8 w - - 0 1");
            var expected = 4;
            var actual = new King(bs.BoardRepresentation, bs.KingSquares[1], bs.SideToMove).MoveGeneration();

            foreach (var move in actual)
            {
                Console.WriteLine(Definitions.IndexToAlgebraic[move]);
            }

            Assert.AreEqual(expected, actual.Length);
        }

        [TestMethod]
        public void PsuedoLegalKnightTest()
        {
            
            BoardState bs = SetupBs("rnbqkb1r/pppppppp/7n/8/8/2N5/PPPPPPPP/R1BQKBNR w KQkq - 0 1");
            var expected = 5;
            var actual = new Knight(bs.BoardRepresentation, Definitions.AlgebraicToIndex["c3"], bs.SideToMove).MoveGeneration();

            foreach (var move in actual)
            {
                Console.WriteLine(Definitions.IndexToAlgebraic[move]);
            }

            Assert.AreEqual(expected, actual.Length);
        }

        [TestMethod]
        public void LegalKingMOvesTest()
        {
            BoardState bs = SetupBs("k7/8/K7/P7/8/8/8/8 w - - 0 1");
            KCE.Engine.MoveGenerator mg = new KCE.Engine.MoveGenerator(bs);
            var expected = 2;
            var actual = mg.AllLegalMoves();

            foreach (var move in actual)
            {
                Console.WriteLine(move.GetAlgebraicPly());
            }

            Assert.AreEqual(expected, actual.Length);
        }

        private BoardState SetupBs(string fen)
        {
            return helper.BoardsetupFromFen(fen);
        }
    }

    
}