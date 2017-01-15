/*using System;
using System.Collections.Generic;
using System.Linq;
using KCE;
using KCE.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class Moves
    {
        private BoardState _bs;
        private readonly Helper _helper = new Helper();

        [TestMethod]
        public void TestMoves()
        {
            Position("position startpos moves e2e4 e7e5");
            var item1 = new Tuple<int, int>(Definitions.AlgebraicToIndex["e2"], Definitions.AlgebraicToIndex["e4"]);
            var item2 = new Tuple<int, int>(Definitions.AlgebraicToIndex["e7"], Definitions.AlgebraicToIndex["e5"]);

            var expected = new List<Tuple<int, int>> {item1, item2};

            Assert.AreEqual(expected.Count, _bs.Moves.Count);
        }

        [TestMethod]
        public void TestRepetition()
        {
            Position("position startpos moves g1f3 b8c6 f3g1 c6b8");
            var expected = true;
            var actual = IsRepetition();

            Assert.AreEqual(expected, actual);

            /*foreach (var move in _bs.Moves)
            {
                //Console.WriteLine("\n{1}{2}", Definitions.IndexToAlgebraic[move.Item1], Definitions.IndexToAlgebraic[move.Item2]);
            }#1#
        }


        private void Position(string command)
        {
            if (command.Contains("startpos"))
                command = command.Replace("startpos", "fen " + Definitions.STDSETUP);

            var fen = command;
            int index;
            string[] moves = null;

            if (command.Contains("moves"))
            {
                index = command.IndexOf("moves") - 1;
                fen = command.Substring(0, index);
                index = command.IndexOf("moves") + 6;
                var temp = command.Substring(index);
                moves = temp.Split(' ');
            }

            index = fen.IndexOf("fen") + 4;
            fen = fen.Substring(index);
            //Console.WriteLine(fen);

            // Setup new BoardState.
            _bs = _helper.BoardsetupFromFen(fen);

            if (moves != null)
            {
                var mg = new KCE.Engine.MoveGenerator(_bs);
                foreach (var move in moves)
                {
                    //Console.WriteLine(move); //Correct.
                    // perform those moves                 
                    var validMoves = mg.AllLegalMoves();
                    foreach (var validMove in validMoves)
                        if (validMove.GetAlgebraicPly() == move)
                        {
                            //Console.WriteLine("{0}", move);
                            mg.MakeMove(validMove);
                            //Console.WriteLine(_bs.Moves.Last());
                            break;
                        }
                }
            }
        }

        private bool IsRepetition()
        {
            int lengthOfList = _bs.Moves.Count;

            if (lengthOfList < 4) return false;

            Tuple<int, int> lastMove = _bs.Moves[lengthOfList - 1];
            int index = lengthOfList - 3; // - 2 - 1 (zero indexed).

            if (_bs.Moves[index].Item1 == lastMove.Item2 && _bs.Moves[index].Item2 == lastMove.Item1)
            {
                index -= 1;
                if (_bs.Moves[index].Item1 == lastMove.Item2 && _bs.Moves[index].Item2 == lastMove.Item1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}*/