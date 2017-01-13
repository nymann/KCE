using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.MoveGenerator
{
    [TestClass]
    public class UpdateCastlePermissionTest
    {

        [TestMethod]
        public void WhiteCapturesRookOnA8()
        {
            var helper = new Helper();
            var actual = helper.UpdateCastlePermissions(Definitions.AlgebraicToIndex["b7"],
                Definitions.AlgebraicToIndex["a8"], false, false, false, true, Definitions.White);
            bool[] expected = {false, false, false, false};

            for (var i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }

        [TestMethod]
        public void MakePlyWhiteCapturesRookOnA8()
        {
            var helper = new Helper();

            var bs = helper.BoardsetupFromFen("r3k3/1K6/8/8/8/8/8/8 w q - 0 1");
            var ply = helper.MakePly(bs.BoardRepresentation, Definitions.AlgebraicToIndex["b7"], Definitions.AlgebraicToIndex["a8"],
                Definitions.NoEnPassantSquare,
                false, true, false, false, Definitions.White);

            Assert.AreEqual(false, ply.GetBCCQS());
        }
    }
}