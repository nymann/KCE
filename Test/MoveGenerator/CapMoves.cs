using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.MoveGenerator
{
    [TestClass]
    public class CapMoves
    {

        [TestMethod]
        public void CapMovesTest()
        {
            var expected = 0;
            var actual = PerformCapMoves(Definitions.STDSETUP);

            Assert.AreEqual(expected, actual);
        }

        private int PerformCapMoves(string fen)
        {
            var helper = new Helper();
            var bs = helper.BoardsetupFromFen(fen);
            var mg = new KCE.Engine.MoveGenerator(bs);

            return mg.AllCapMoves().Length;
        }
        
    }
}