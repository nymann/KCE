using System;
using System.Diagnostics;
using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.MoveGenerator
{
    [TestClass]
    public class Divide
    {
        private BoardState _boardState;
        private int MaxDepth = 4;

        [TestMethod]
        public void PerformTest()
        {
            ulong[] startPositionExpectedNodes = { 20, 400, 8902, 197281, 4865609, 119060324, 3195901860 };
            var startPositionFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            DivideTestHelper(startPositionFEN, startPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerformTest2()
        {
            
            ulong[] startPositionExpectedNodes = { 22, 438, 10546, 232214, 4865609, 6064002, 147592672 };
            var startPositionFEN = "rnbqkb1r/pppppppp/7n/8/8/2N5/PPPPPPPP/R1BQKBNR w KQkq - 0 1";
            DivideTestHelper(startPositionFEN, startPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerformTest3()
        {
            var fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
            var depth = 4;
            ulong[] expected = { 48, 2039, 97862, 4085603 };
            DivideTestHelper(fen, expected, depth);
        }

        public void PerformTest4()
        {
            var fen = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
            var depth = 3;
        }

        [TestMethod]
        public void PerformTest5()
        {
            ulong[] discoverPromotionBugsExpectedNodes = { 24, 496, 9483, 182838, 3605103, 71179139 };
            var discoverPromotionBugsFen = "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1";

            DivideTestHelper(discoverPromotionBugsFen, discoverPromotionBugsExpectedNodes, 3);
        }

        private void DivideTestHelper(string FEN, ulong[] expectedValues, int depthMax)
        {
            //var stopwatch = new Stopwatch();

            for (var depth = 1; depth <= depthMax; depth++)
            {
                //stopwatch.Start();
                var actual = DividePerftTest(FEN, depth);
                //stopwatch.Stop();
                var expected = expectedValues[depth - 1];

                //Console.WriteLine("{0}, D{1}, expected: {2}, actual: {3}, Time: {4} ms.", FEN, depth, expected, actual, stopwatch.ElapsedMilliseconds);
                Assert.AreEqual(expected, actual);
                //stopwatch.Restart();
            }
        }

        private ulong DividePerftTest(string FEN, int depth)
        {
            var helper = new Helper();
            _boardState = helper.BoardsetupFromFen(FEN);
            var moveGenerator = new KCE.Engine.MoveGenerator(_boardState);
            return moveGenerator.Divide(depth);
        }
    }
}