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
        public void KingSquareUpdateTest()
        {
            KCE.BoardRepresentation.PieceRules.Helper helper = new KCE.BoardRepresentation.PieceRules.Helper();
            BoardState bs = helper.BoardsetupFromFen("k7/8/K7/8/8/8/8/8 b - - 0 1");
            var ply = helper.MakePly(bs.BoardRepresentation, Definitions.AlgebraicToIndex["a8"], Definitions.AlgebraicToIndex["b8"],
               Definitions.NoEnPassantSquare,
               false, false, false, false, Definitions.White, bs.KingSquares, -1, Definitions.AlgebraicToIndex["b8"]);
            KCE.BoardRepresentation.MoveGenerator moveGenerator = new KCE.BoardRepresentation.MoveGenerator(bs);
            moveGenerator.MakeMove(ply);

            int[] expected = {97, 78};
            Assert.AreEqual(expected[0], bs.KingSquares[0]);
            Assert.AreEqual(expected[1], bs.KingSquares[1]);

        }

    }
}