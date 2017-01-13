
using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.MoveGenerator
{
    [TestClass]
    public class AllMoves
    {

        [TestMethod]
        public void AllMovesTest()
        {
            var expected = 0;
            var actual = PerformAllMoves("8/8/8/8/8/k2b4/P7/K7 w - - 0 1");

            Assert.AreEqual(expected, actual);
        }

        private int PerformAllMoves(string fen)
        {
            var helper = new Helper();
            var bs = helper.BoardsetupFromFen(fen);
            var mg = new KCE.Engine.MoveGenerator(bs);
            var list = mg.AllLegalMoves();
            return list.Count;
        }

    }
}