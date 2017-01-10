using System;
using KCE.BoardRepresentation;
using KCE.BoardRepresentation.PieceRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test.Helper;

namespace Test.MoveGenerator
{
    [TestClass]
    public class Perft
    {
        private BoardState _boardState;
        public const int MaxDepth = 3;

        [TestMethod]
        public void PerftStartPositionTest()
        {
            ulong[] startPositionExpectedNodes = {20, 400, 8902, 197281, 4865609, 119060324, 3195901860};
            string startPositionFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            PerftTestHelper(startPositionFEN, startPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerftGoodTestPosition()
        {
            ulong[] goodTestPositionExpectedNodes = {48, 2039, 97862, 4085603, 193690690, 8031647685};
            string goodTestPositionFEN = "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1";
            PerftTestHelper(goodTestPositionFEN, goodTestPositionExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void PerftDiscoverPromotionBugs()
        {
            ulong[] discoverPromotionBugsExpectedNodes = {24, 496, 9483, 182838, 3605103, 71179139};
            string discoverPromotionBugsFen = "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1";
            PerftTestHelper(discoverPromotionBugsFen, discoverPromotionBugsExpectedNodes, MaxDepth);
        }

        [TestMethod]
        public void KingPawnEndGame()
        {
            ulong[] expectedNodes = {2, 4, 24, 103, 703, 2945};
            string fen = "k7/8/K7/P7/8/8/8/8 w - - 0 1";
            PerftTestHelper(fen, expectedNodes, MaxDepth);
        }

        private void PerftTestHelper(string FEN, ulong[] expectedValues, int depthMax)
        {
            for (int depth = 1; depth <= depthMax; depth++)
            {
                var actual = SetupPerftTest(FEN, depth);
                var expected = expectedValues[depth - 1];
                Console.WriteLine("{0}, D{1}, expected: {2}, actual: {3}.", FEN, depth, expected, actual);
                Assert.AreEqual(expected, actual);
            }
        }

        private ulong SetupPerftTest(string FEN, int depth)
        {
            KCE.Engine.Helper helper = new KCE.Engine.Helper();
            _boardState = helper.BoardsetupFromFen(FEN);
            KCE.BoardRepresentation.MoveGenerator moveGenerator = new KCE.BoardRepresentation.MoveGenerator(_boardState);
            return moveGenerator.Perft(depth);
        }

        [TestMethod]
        public void PerftSuiteTest()
        {
            var input = new ReadFileLineByLine().ReadFile("C://perftsuite.epd");
            foreach (var line in input)
            {
                Tuple<string, ulong[]> data = GetSuiteData(line);
                PerftTestHelper(data.Item1, data.Item2, MaxDepth);
            }
            Console.WriteLine("\n\n\n");
        }

        Tuple<string, ulong[]> GetSuiteData(string line)
        {
            string[] lineArray = line.Split(';');
            string fen = lineArray[0].TrimEnd();

            ulong[] expectedNodes = {0, 0, 0, 0, 0, 0};

            for (int i = 1; i < lineArray.Length; i++)
            {
                lineArray[i] = lineArray[i].Remove(0, 3).TrimEnd();
                expectedNodes[i - 1] = Convert.ToUInt64(lineArray[i]);
            }

            return new Tuple<string, ulong[]>(fen, expectedNodes);
        }
    }
}