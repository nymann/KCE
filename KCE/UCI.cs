using System;
using KCE.Engine;
using KCE.Engine.Search;

namespace KCE
{
    public class UCI
    {
        private static BoardState _bs;
        private readonly Helper _helper = new Helper();
        private readonly Search _search = new Search();
        private SearchInfo _sInfo;
        //private readonly Zobrist _zobrist = new Zobrist();

        public UCI()
        {
            
        }

        private void MainLoop()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (input.Contains("position"))
                {
                    Position(input);
                }

                else if (input.Contains("go"))
                {
                    Go(input);
                }

                else if (input.Contains("divide"))
                {
                    var command = input.Split(' ');
                    var depth = Convert.ToInt32(command[1]);

                    var mg = new MoveGenerator(_bs);
                    mg.Divide(depth);
                }

                else if (input.Contains("perft"))
                {
                    var command = input.Split(' ');
                    var depth = Convert.ToInt32(command[1]);

                    var mg = new MoveGenerator(_bs);
                    Console.WriteLine("Nodes: {0}.", mg.Perft(depth));
                }

                else if (input.Equals("d"))
                {
                    _helper.PrintBoardWhitePerspective(_bs.BoardRepresentation);
                }

                switch (input)
                {
                    case "isready":
                        IsReady();
                        break;

                    case "quit":
                        Quit();
                        break;

                    case "stop":
                        break;

                    case "z":
                        //Console.WriteLine("Pos key: {0:X}.", _bs.PosKey);
                        break;
                    default:
                        break;
                }
            }
        }

        public void Identify()
        {
            Console.WriteLine("id name KCE");
            Console.WriteLine("id author Kristian Nymann Jakobsen");
            Console.WriteLine("uciok");
            MainLoop();
        }

        private void Quit()
        {
            Environment.Exit(Definitions.QuitCode);
        }

        private void IsReady()
        {
            Console.WriteLine("readyok");
        }

        private void Register()
        {
            Console.WriteLine("register name Kristian NJ code 1");
        }

        private void Position(string command)
        {
            if (command.Contains("startpos"))
            {
                command = command.Replace("startpos", "fen " + Definitions.STDSETUP);
            }

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
            /*_bs.PieceKeys = _zobrist.InitZobrist(_bs);
            _bs.PosKey = _zobrist.Hash(_bs);*/
            if (moves == null)
            {
                return;
            }

            var mg = new MoveGenerator(_bs);
            foreach (var move in moves)
            {
                // perform those moves                 
                var validMoves = mg.AllLegalMoves();
                foreach (var validMove in validMoves)
                {
                    var algebraic = Definitions.IndexToAlgebraic[validMove.GetFromToSquare()[0]] +
                                    Definitions.IndexToAlgebraic[validMove.GetFromToSquare()[1]];
                    if (algebraic == move)
                    {
                        mg.MakeMove(validMove);
                        break;
                    }
                }
            }

            //_helper.PrintBoardWhitePerspective(_bs.BoardRepresentation);
        }

        private void Go(string command)
        {
            _sInfo = new SearchInfo();
            if (command.Contains("infinite"))
            {
                _sInfo.TimeLeft = Definitions.InfiniteTime;
            }
        
            else if (command.Contains("wtime"))
            {
                long timeLeft;
                int index;

                if (_bs.SideToMove == Definitions.White)
                {
                    // We are white.
                    index = command.IndexOf("wtime") + 6;
                    var temp = command.Substring(index);
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                    timeLeft = long.Parse(temp);
                    timeLeft = timeLeft / 30;
                }
                else
                {
                    // we are black.
                    index = command.IndexOf("btime") + 6;
                    var temp = command.Substring(index);
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                    timeLeft = long.Parse(temp);
                    timeLeft = timeLeft / 30;
                }
                _sInfo.TimeLeft = timeLeft;
            }

            else if (command.Contains("movetime"))
            {
                long timeLeft;
                var index = command.IndexOf("movetime") + 9;
                var temp = command.Substring(index);
                if (temp.Contains(" "))
                {
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                }

                timeLeft = long.Parse(temp);
                _sInfo.TimeLeft = timeLeft;
            }

            _search.SearchPosition(_bs, _sInfo);
        }

        // Engine to GUI;
    }
}