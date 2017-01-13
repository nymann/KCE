using KCE.Engine;
using KCE.Engine.Search;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Search
{
    [TestClass]
    public class BestMove
    {
        [TestMethod]
        public void StartPosition()
        {
            Assert.AreEqual("e2e4", PerformBestMove(Definitions.STDSETUP));
        }

        private string PerformBestMove(string fen)
        {
            var helper = new Helper();
            var bs =
                helper.BoardsetupFromFen(fen);
            var search = new KCE.Engine.Search.Search();
            var sInfo = new SearchInfo();
            search.SearchPosition(bs, sInfo);
            sInfo.TimeLeft = 30000;
            return bs.BestPly.GetAlgebraicPly();
        }

    }
}