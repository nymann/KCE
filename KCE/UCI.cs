using System;
using System.Net.Mime;
using System.Runtime.Remoting.Messaging;
using KCE.BoardRepresentation;
using KCE.Engine;
using KCE.Engine.Search;

namespace KCE
{
    public class UCI
    {
        /*
         * This class is called when the engine receives "uci", which tells the engine to use the UCI interface.
         * UCI
         * Commands to support:
         * "isready" = IsReady();
         * "register" 
         */


        private static BoardState bs;
        private Helper helper;
        private Search search;
        private SearchInfo sInfo;

        public UCI()
        {
            helper = new Helper();
            search = new Search();
        }

        private void MainLoop()
        {
            while (true)
            {
                string input = Console.ReadLine();

                if (input.Contains("position"))
                {
                    Position(input);
                }

                if (input.Contains("go"))
                {
                    Go(input);
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
                //Console.WriteLine(command);
            }

            string fen = command;
            int index;
            string[] moves = null;

            if (command.Contains("moves"))
            {
                index = command.IndexOf("moves") - 1;
                fen = command.Substring(0, index);
                index = command.IndexOf("moves") + 6;
                string temp = command.Substring(index);
                moves = temp.Split(' ');
            }

            index = fen.IndexOf("fen") + 4;
            fen = fen.Substring(index);
            //Console.WriteLine(fen);

            // Setup new BoardState.
            bs = helper.BoardsetupFromFen(fen);

            if (moves != null)
            {
                MoveGenerator mg = new MoveGenerator(bs);
                foreach (var move in moves)
                {
                    // perform those moves                 
                    var validMoves = mg.AllLegalMoves();
                    foreach (var validMove in validMoves)
                    {
                        if (validMove.GetAlgebraicPly() == move)
                        {
                            mg.MakeMove(validMove);
                            break;
                        }
                    }
                    
                }
            }

            //helper.PrintBoardWhitePerspective(bs.BoardRepresentation);
        }

        private void Go(string command)
        {
            sInfo = new SearchInfo();
            if (command.Contains("infinite"))
            {
                sInfo.TimeLeft = Definitions.InfiniteTime;
            }
        
            else if (command.Contains("wtime"))
            {
                long timeLeft;
                int index;

                if (bs.SideToMove == Definitions.White)
                {
                    // We are white.
                    index = command.IndexOf("wtime") + 6;
                    string temp = command.Substring(index);
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                    timeLeft = long.Parse(temp);
                    
                }
                else
                {
                    // we are black.
                    index = command.IndexOf("btime") + 6;
                    string temp = command.Substring(index);
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                    timeLeft = long.Parse(temp);
                }
                sInfo.TimeLeft = timeLeft;
            }

            else if (command.Contains("movetime"))
            {
                long timeLeft;
                int index = command.IndexOf("movetime") + 9;
                string temp = command.Substring(index);
                if (temp.Contains(" "))
                {
                    index = temp.IndexOf(' ');
                    temp = temp.Substring(0, index);
                }

                timeLeft = long.Parse(temp);
                sInfo.TimeLeft = timeLeft;
            }

            search.SearchPosition(bs, sInfo);
        }

        // Engine to GUI;
    }
}