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
            Assert.AreEqual(30, PerformEvaluate("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1"));
        }

        private int PerformEvaluate(string fen)
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            BoardState bs = helper.BoardsetupFromFen(fen);
            return new KCE.BoardRepresentation.Evaluate().EvalPosition(bs);
        }
    }
}