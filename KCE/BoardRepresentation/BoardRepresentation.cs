using System;
using System.Text;
using KCE.BoardRepresentation.MoveGeneration;

namespace KCE.BoardRepresentation
{
    public class BoardRepresentation
    {
        private Board board;
        private bool userColor;
        private string lastMove = "";

        private int[] _kingSquares = {24, 94};

        private int[] _board =
        {
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 4, 2, 3, 6, 5, 3, 2, 4, 99,
            99, 1, 1, 1, 1, 1, 1, 1, 1, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 7, 7, 7, 7, 7, 7, 7, 7, 99,
            99, 10, 8, 9, 12, 11, 9, 8, 10, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
        };

        private readonly int[] _board64 =
        {
            21, 22, 23, 24, 25, 26, 27, 28,
            31, 32, 33, 34, 35, 36, 37, 38,
            41, 42, 43, 44, 45, 46, 47, 48,
            51, 52, 53, 54, 55, 56, 57, 58,
            61, 62, 63, 64, 65, 66, 67, 68,
            71, 72, 73, 74, 75, 76, 77, 78,
            81, 82, 83, 84, 85, 86, 87, 88,
            91, 92, 93, 94, 95, 96, 97, 98
        };

        public BoardRepresentation()
        {
            board = new Board(_board, true, _kingSquares, Definitions.NoEnPassantSquare, 0);
            
            Console.WriteLine("Type 'w' if you want to be white, or 'b' if you want to be black.");
            String userColorString = Console.ReadLine();
            switch (userColorString)
            {
                case "w":
                    userColor = Definitions.White;
                    break;

                case "b":
                    userColor = Definitions.Black;
                    break;

                default:
                    Console.WriteLine("I didn't understand that, so I assigned you the white pieces!");
                    userColor = Definitions.White;
                    break;
            }
            while (true)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                Console.WriteLine("Last move was: {0}\n", lastMove);
                PrintBoard();
                Console.WriteLine(board.SideToMove ? "\nWhite to move:" : "\nBlack to move:");

                string userMove = Console.ReadLine();
                userMove = userMove?.ToLower();
                if (UserMove(userMove))
                {
                    MakeMove(userMove);
                    lastMove = userMove;
                    board.SideToMove = !board.SideToMove;
                }
            }

        }

        private void PrintBoard()
        {
            var counter = 1;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   h   g   f   e   d   c   b   a");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            foreach (var square in _board64)
            {
                Console.Write(" | ");
                switch (_board[square])
                {
                    case 0:
                        // Empty Square
                        Console.Write(' ');
                        break;
                    case 1:
                        // White Pawn
                        Console.Write('\u2659');
                        break;

                    case 2:
                        // White Knight
                        Console.Write('\u2658');
                        break;

                    case 3:
                        // White Bishop
                        Console.Write('\u2657');
                        break;

                    case 4:
                        // White Rook
                        Console.Write('\u2656');
                        break;

                    case 5:
                        // White Queen
                        Console.Write('\u2655');
                        break;

                    case 6:
                        // White King
                        Console.Write('\u2654');
                        break;

                    case 7:
                        // Black Pawn
                        Console.Write('\u265f');
                        break;
                    case 8:
                        // Black Knight
                        Console.Write('\u265e');
                        break;

                    case 9:
                        // Black Bishop
                        Console.Write('\u265d');
                        break;

                    case 10:
                        // Black Rook
                        Console.Write('\u265c');
                        break;

                    case 11:
                        // Black Queen
                        Console.Write('\u265b');
                        break;

                    case 12:
                        // Black King
                        Console.Write('\u265a');
                        break;

                    default:
                        Console.WriteLine("THIS SHOULDN'T HAPPEN.");
                        break;
                }

                if (square%10 != 8) continue;
                Console.WriteLine(" | {0}", counter);
                Console.WriteLine(" +---+---+---+---+---+---+---+---+");
                counter++;
            }
        }

        private bool UserMove(string longAlgebraicNotation)
        {
            string fromSquareString = longAlgebraicNotation.Substring(0, 2);
            if (!Definitions.AlgebraicToIndex.ContainsKey(fromSquareString))
            {
                return false;
            }

            int fromSquareInt = Definitions.AlgebraicToIndex[fromSquareString];

            string toSquareString = longAlgebraicNotation.Substring(2, 2);
            if (!Definitions.AlgebraicToIndex.ContainsKey(toSquareString))
            {
                return false;
            }
            int toSquareInt = Definitions.AlgebraicToIndex[toSquareString];

            if (board.BoardRepresentation[fromSquareInt] == Definitions.EmptySquare ||
                board.SideToMove == Definitions.BlackToMove && board.BoardRepresentation[fromSquareInt] < 6 ||
                board.SideToMove == Definitions.WhiteToMove && board.BoardRepresentation[fromSquareInt] > 6 && board.BoardRepresentation[fromSquareInt] < 13)
            {
                Console.WriteLine("Reached. Side to move was {0}", board.SideToMove);
                return false;
            }

            switch (_board[fromSquareInt])
            {
                case Definitions.WhitePawn:
                case Definitions.BlackPawn:
                    Pawn pawn = new Pawn(fromSquareInt, board);
                    return pawn.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteBishop:
                case Definitions.BlackBishop:
                    Bishop bishop = new Bishop(board, fromSquareInt);
                    return bishop.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteKnight:
                case Definitions.BlackKnight:
                    Knight knight = new Knight(board, fromSquareInt);
                    return knight.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteRook:
                case Definitions.BlackRook:
                    Rook rook = new Rook(fromSquareInt, board);
                    return rook.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteQueen:
                case Definitions.BlackQueen:
                    Queen queen = new Queen(fromSquareInt, board);
                    return queen.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteKing:
                case Definitions.BlackKing:
                    King king = new King(board, fromSquareInt);
                    return king.MoveGeneration().Contains(toSquareInt);

                default:
                    return false;
            }

        }

        private void MakeMove(string longAlgebraicNotation)
        {
            string fromSquareString = longAlgebraicNotation.Substring(0, 2);
            int fromSquareInt = Definitions.AlgebraicToIndex[fromSquareString];

            string toSquareString = longAlgebraicNotation.Substring(2, 2);
            int toSquareInt = Definitions.AlgebraicToIndex[toSquareString];

            // TODO: Update king square, castling, etc.

            board.BoardRepresentation[toSquareInt] = board.BoardRepresentation[fromSquareInt];
            board.BoardRepresentation[fromSquareInt] = Definitions.EmptySquare;
        }
    }
}