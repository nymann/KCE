using System;
using KCE.BoardRepresentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.MoveGenerator
{
    [TestClass]
    public class BoardStateTest
    {
        [TestMethod]
        public void CastlePermissionsSetupTest()
        {
            KCE.BoardRepresentation.PieceRules.Helper helper = new KCE.BoardRepresentation.PieceRules.Helper();

            BoardState bs = helper.BoardsetupFromFen("r3k3/1K6/8/8/8/8/8/8 w q - 0 1");
            
            Assert.AreEqual(false, bs.BlackCanCastleKingSide);
            Assert.AreEqual(true, bs.BlackCanCastleQueenSide);
            Assert.AreEqual(false, bs.WhiteCanCastleKingSide);
            Assert.AreEqual(false, bs.WhiteCanCastleQueenSide);
        }

        [TestMethod]
        public void KingSquareMakeUnmakeTest()
        {
            KCE.BoardRepresentation.PieceRules.Helper helper = new KCE.BoardRepresentation.PieceRules.Helper();
            BoardState bs = helper.BoardsetupFromFen("k7/8/K7/P7/8/8/8/8 w - - 0 1");
            var ply = helper.MakePly(bs.BoardRepresentation, Definitions.AlgebraicToIndex["a6"], Definitions.AlgebraicToIndex["b6"],
               Definitions.NoEnPassantSquare,
               false, false, false, false, bs.SideToMove, -1);
            KCE.BoardRepresentation.MoveGenerator moveGenerator = new KCE.BoardRepresentation.MoveGenerator(bs);
            moveGenerator.MakeMove(ply);

            int[] expected = {Definitions.AlgebraicToIndex["a8"], Definitions.AlgebraicToIndex["b6"]};
            Assert.AreEqual(expected[0], bs.KingSquares[0]);
            Assert.AreEqual(expected[1], bs.KingSquares[1]);
            moveGenerator.UndoMove(ply);
            expected[1] = Definitions.AlgebraicToIndex["a6"];
            Assert.AreEqual(expected[0], bs.KingSquares[0]);
            Assert.AreEqual(expected[1], bs.KingSquares[1]);
        }

        [TestMethod]
        public void KingSquare3PlysMakeTest()
        {
            KCE.BoardRepresentation.PieceRules.Helper helper = new KCE.BoardRepresentation.PieceRules.Helper();
            BoardState bs = helper.BoardsetupFromFen("k7/8/K7/P7/8/8/8/8 w - - 0 1");
            helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
            Console.WriteLine("\n");
            var firstPly = helper.MakePly(bs.BoardRepresentation,
                Definitions.AlgebraicToIndex["a6"], Definitions.AlgebraicToIndex["b6"],
               Definitions.NoEnPassantSquare,
               false, false, 
               false, false, 
               bs.SideToMove, -1);

            KCE.BoardRepresentation.MoveGenerator moveGenerator = new KCE.BoardRepresentation.MoveGenerator(bs);

            moveGenerator.MakeMove(firstPly);
            helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
            Console.WriteLine("\n");

            int[] expected = { Definitions.AlgebraicToIndex["a8"], Definitions.AlgebraicToIndex["b6"] };
            Assert.AreEqual(expected[0], bs.KingSquares[0]);
            Assert.AreEqual(expected[1], bs.KingSquares[1]);

            var secondPly = helper.MakePly(bs.BoardRepresentation, 
                Definitions.AlgebraicToIndex["a8"],Definitions.AlgebraicToIndex["b8"],
                Definitions.NoEnPassantSquare,
                false, false,
                false, false, bs.SideToMove);

            moveGenerator.MakeMove(secondPly);
            helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
            Console.WriteLine("\n");

            expected[0] = Definitions.AlgebraicToIndex["b8"];
            expected[1] = Definitions.AlgebraicToIndex["b6"];
            Assert.AreEqual(expected[0], bs.KingSquares[0]);
            Assert.AreEqual(expected[1], bs.KingSquares[1]);

            var legalMoves = moveGenerator.AllLegalMoves();
            foreach (Ply legalMove in legalMoves)
            {
                Console.WriteLine("{0}.", legalMove.GetAlgebraicPly());
            }

        }
    }
}