using KCE.BoardRepresentation;
using KCE.Engine.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Search
{
    [TestClass]
    public class Mates
    {
        [TestMethod]
        public void MateIn1()
        {
            Assert.AreEqual("g6g8", PerformMateTest("4k3/8/4K1R1/8/8/8/8/8 w - - 0 1"));
        }

        [TestMethod]
        public void MateIn2()
        {
            Assert.AreEqual("f3f7", PerformMateTest("2bqkbn1/2pppp2/np2N3/r3P1p1/p2N2B1/5Q2/PPPPKPP1/RNB2r2 w - - 0 1"));
        }

        [TestMethod]
        public void MateIn3()
        {
            Assert.AreEqual("b2b4",PerformMateTest("r3Bb1r/pQ6/2p2p1p/k3q3/4P3/2N1B3/PP4PP/6K1 w - - 0 1"));
        }

        [TestMethod]
        public void MateIn4()
        {
            Assert.AreEqual("g5f6", PerformMateTest("r4r1k/1bpq1p1n/p1np4/1p1Bb1BQ/P7/6R1/1P3PPP/1N2R1K1 w - - 0 1"));
        }

        private string PerformMateTest(string fen)
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs = helper.BoardsetupFromFen(fen);
            KCE.Engine.Search.Search search = new KCE.Engine.Search.Search();
            var sInfo = new SearchInfo();
            search.SearchPosition(bs, sInfo);

            return bs.BestPly.GetAlgebraicPly();
        }

    }
}