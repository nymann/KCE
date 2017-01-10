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
        
    }
}