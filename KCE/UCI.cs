using System;
using KCE.Engine;
using KCE.Engine.Search;

namespace KCE
{
    public class UCI
    {
        private static BoardState _bs;
        private readonly Helper _helper;
        private readonly Search _search;
        private SearchInfo _sInfo;

        public UCI()
        {
            _helper = new Helper();
            _search = new Search();
        }

        private void MainLoop()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (input.Contains("position"))
                    Position(input);

                if (input.Contains("go"))
                    Go(input);

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
                var mg = new MoveGenerator(_bs);
                foreach (var move in moves)
                {
                    // perform those moves                 
                    var validMoves = mg.AllLegalMoves();
                    foreach (var validMove in validMoves)
                        if (validMove.GetAlgebraicPly() == move)
                        {
                            mg.MakeMove(validMove);
                            break;
                        }
                }
            }

            //helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
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