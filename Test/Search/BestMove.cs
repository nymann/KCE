using KCE.BoardRepresentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Search
{
    [TestClass]
    public class BestMove
    {
        [TestMethod]
        public void StartPosition()
        {
            Assert.AreEqual("d2d4", PerformBestMove(Definitions.STDSETUP));
        }

        private string PerformBestMove(string fen)
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs =
                helper.BoardsetupFromFen(fen);
            KCE.Engine.Search.Search search = new KCE.Engine.Search.Search();
            search.SearchPosition(bs);
            return bs.BestPly.GetAlgebraicPly();
        }

    }
}