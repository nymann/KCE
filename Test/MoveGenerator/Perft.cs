using System;
using System.Diagnostics;
using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.HelperKek;

namespace Test.MoveGenerator
{
    [TestClass]
    public class Perft
    {
        private BoardState _boardState;
        public const int MaxDepth = 5;

        [TestMethod]
        public void PerftStartPositionTest()
        {
            ulong[] startPositionExpectedNodes = {20, 400, 8902, 197281, 4865609, 119060324, 3195901860};
            var startPositionFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            PerftTestHelper(startPositionFEN, startPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerftGoodTestPosition()
        {
            ulong[] goodTestPositionExpectedNodes = {48, 2039, 97862, 4085603, 193690690, 8031647685};
            var goodTestPositionFEN = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
            PerftTestHelper(goodTestPositionFEN, goodTestPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerftDiscoverPromotionBugs()
        {
            ulong[] discoverPromotionBugsExpectedNodes = {24, 496, 9483, 182838, 3605103, 71179139};
            var discoverPromotionBugsFen = "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1";
            PerftTestHelper(discoverPromotionBugsFen, discoverPromotionBugsExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void KingPawnEndGame()
        {
            ulong[] expectedNodes = {2, 4, 24, 103, 703, 2945};
            var fen = "k7/8/K7/P7/8/8/8/8 w - - 0 1";
            PerftTestHelper(fen, expectedNodes, MaxDepth);
        }

        private void PerftTestHelper(string FEN, ulong[] expectedValues, int depthMax)
        {
            var stopwatch = new Stopwatch();
            
            for (var depth = 1; depth <= depthMax; depth++)
            {
                stopwatch.Start();
                var actual = SetupPerftTest(FEN, depth);
                stopwatch.Stop();
                var expected = expectedValues[depth - 1];
                
                Console.WriteLine("{0}, D{1}, expected: {2}, actual: {3}, Time: {4} ms.", FEN, depth, expected, actual, stopwatch.ElapsedMilliseconds);
                Assert.AreEqual(expected, actual);
                stopwatch.Restart();
            }
        }

        private ulong SetupPerftTest(string FEN, int depth)
        {
            var helper = new Helper();
            _boardState = helper.BoardsetupFromFen(FEN);
            var moveGenerator = new KCE.Engine.MoveGenerator(_boardState);
            return moveGenerator.Perft(depth);
        }

        [TestMethod]
        public void PerftSuiteTest()
        {
            var input = new ReadFileLineByLine().ReadFile("C://perftsuite.epd");
            foreach (var line in input)
            {
                var data = GetSuiteData(line);
                PerftTestHelper(data.Item1, data.Item2, MaxDepth);
            }
            Console.WriteLine("\n\n\n");
        }

        private Tuple<string, ulong[]> GetSuiteData(string line)
        {
            var lineArray = line.Split(';');
            var fen = lineArray[0].TrimEnd();

            ulong[] expectedNodes = {0, 0, 0, 0, 0, 0};

            for (var i = 1; i < lineArray.Length; i++)
            {
                lineArray[i] = lineArray[i].Remove(0, 3).TrimEnd();
                expectedNodes[i - 1] = Convert.ToUInt64(lineArray[i]);
            }

            return new Tuple<string, ulong[]>(fen, expectedNodes);
        }
    }
}