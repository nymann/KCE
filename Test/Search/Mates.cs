using KCE.BoardRepresentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Search
{
    [TestClass]
    public class Mates
    {
        [TestMethod]
        public void MateIn2()
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs =
                helper.BoardsetupFromFen("2bqkbn1/2pppp2/np2N3/r3P1p1/p2N2B1/5Q2/PPPPKPP1/RNB2r2 w - - 0 1");
            KCE.BoardRepresentation.Search search = new KCE.BoardRepresentation.Search();
            search.SearchPosition(bs);
            Assert.AreEqual("f3f7", bs.BestPly.GetAlgebraicPly());
        }

        [TestMethod]
        public void MateIn1()
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs = helper.BoardsetupFromFen("4k3/8/4K1R1/8/8/8/8/8 w - - 0 1");
            KCE.BoardRepresentation.Search search = new KCE.BoardRepresentation.Search();
            search.SearchPosition(bs);
            Assert.AreEqual("g6g8", bs.BestPly.GetAlgebraicPly());
        }
    }
}