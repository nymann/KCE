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
        private bool gameIsOver = false;
        private string gameOverMessage = "";

        private int[] _kingSquares = {94, 24};

        /*private int[] _board =
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
        };*/

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

        public BoardRepresentation(string fen)
        {
            //board = new Board(_board, true, _kingSquares, Definitions.NoEnPassantSquare, 0, true, true, true, true);

            board = BoardsetupFromFen(fen);

            Console.WriteLine("Type 'w' if you want to be white, or 'b' if you want to be black.");
            var userColorString = Console.ReadLine();
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
            while (!gameIsOver)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                string enPasSquareAlgebraic = "";

                if (Definitions.IndexToAlgebraic.ContainsKey(board.EnPasSquare))
                {
                    enPasSquareAlgebraic = Definitions.IndexToAlgebraic[board.EnPasSquare];
                }
                Console.WriteLine("Last move was: {0}, EnPassantSquare: {1}, FiftyMoveCounter: {2}.\n", lastMove, enPasSquareAlgebraic, board.FiftyMoveRule);
                if (board.SideToMove == Definitions.WhiteToMove)
                {
                    PrintBoardWhitePerspective();
                }
                else
                {
                    PrintBoardBlackPerspective();
                }
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

            Console.WriteLine(gameOverMessage);

        }

        private void PrintBoardBlackPerspective()
        {
            var counter = 1;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   h   g   f   e   d   c   b   a");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            foreach (var square in _board64)
            {
                Console.Write(" | ");
                switch (board.BoardRepresentation[square])
                {
                    case Definitions.EmptySquare:
                        // Empty Square
                        Console.Write(' ');
                        break;

                    case Definitions.WhitePawn:
                        // White Pawn
                        Console.Write('\u265f');
                        break;
                    case Definitions.WhiteKnight:
                        // White Knight
                        Console.Write('\u265e');
                        break;

                    case Definitions.WhiteBishop:
                        // White Bishop
                        Console.Write('\u265d');
                        break;

                    case Definitions.WhiteRook:
                        // White Rook
                        Console.Write('\u265c');
                        break;

                    case Definitions.WhiteQueen:
                        // White Queen
                        Console.Write('\u265b');
                        break;

                    case Definitions.WhiteKing:
                        // White King
                        Console.Write('\u265a');
                        break;

                    case Definitions.BlackPawn:
                        // Black Pawn
                        Console.Write('\u2659');
                        break;

                    case Definitions.BlackKnight:
                        // Black Knight
                        Console.Write('\u2658');
                        break;

                    case Definitions.BlackBishop:
                        // Black Bishop
                        Console.Write('\u2657');
                        break;

                    case Definitions.BlackRook:
                        // Black Rook
                        Console.Write('\u2656');
                        break;

                    case Definitions.BlackQueen:
                        // Black Queen
                        Console.Write('\u2655');
                        break;

                    case Definitions.BlackKing:
                        // Black King
                        Console.Write('\u2654');
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

        private void PrintBoardWhitePerspective()
        {
            var counter = 8;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   a   b   c   d   e   f   g   h");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            for (int i = 63; i >= 0; i-- )
            {
                Console.Write(" | ");
                switch (board.BoardRepresentation[_board64[i]])
                {
                    case Definitions.EmptySquare:
                        // Empty Square
                        Console.Write(' ');
                        break;

                    case Definitions.WhitePawn:
                        // White Pawn
                        Console.Write('\u265f');
                        break;
                    case Definitions.WhiteKnight:
                        // White Knight
                        Console.Write('\u265e');
                        break;

                    case Definitions.WhiteBishop:
                        // White Bishop
                        Console.Write('\u265d');
                        break;

                    case Definitions.WhiteRook:
                        // White Rook
                        Console.Write('\u265c');
                        break;

                    case Definitions.WhiteQueen:
                        // White Queen
                        Console.Write('\u265b');
                        break;

                    case Definitions.WhiteKing:
                        // White King
                        Console.Write('\u265a');
                        break;

                    case Definitions.BlackPawn:
                        // Black Pawn
                        Console.Write('\u2659');
                        break;

                    case Definitions.BlackKnight:
                        // Black Knight
                        Console.Write('\u2658');
                        break;

                    case Definitions.BlackBishop:
                        // Black Bishop
                        Console.Write('\u2657');
                        break;

                    case Definitions.BlackRook:
                        // Black Rook
                        Console.Write('\u2656');
                        break;

                    case Definitions.BlackQueen:
                        // Black Queen
                        Console.Write('\u2655');
                        break;

                    case Definitions.BlackKing:
                        // Black King
                        Console.Write('\u2654');
                        break;

                    default:
                        Console.WriteLine("THIS SHOULDN'T HAPPEN.");
                        break;
                }

                if (_board64[i] % 10 != 1) continue;
                Console.WriteLine(" | {0}", counter);
                Console.WriteLine(" +---+---+---+---+---+---+---+---+");
                counter--;
            }

        }

        private bool UserMove(string longAlgebraicNotation)
        {
            var fromSquareString = longAlgebraicNotation.Substring(0, 2);
            if (!Definitions.AlgebraicToIndex.ContainsKey(fromSquareString))
            {
                return false;
            }

            var fromSquareInt = Definitions.AlgebraicToIndex[fromSquareString];

            var toSquareString = longAlgebraicNotation.Substring(2, 2);
            if (!Definitions.AlgebraicToIndex.ContainsKey(toSquareString))
            {
                return false;
            }
            var toSquareInt = Definitions.AlgebraicToIndex[toSquareString];

            if (board.BoardRepresentation[fromSquareInt] == Definitions.EmptySquare ||
                board.SideToMove == Definitions.BlackToMove && board.BoardRepresentation[fromSquareInt] < 6 ||
                board.SideToMove == Definitions.WhiteToMove && board.BoardRepresentation[fromSquareInt] > 6 && board.BoardRepresentation[fromSquareInt] < 13)
            {
                Console.WriteLine("Reached. Side to move was {0}", board.SideToMove);
                return false;
            }

            switch (board.BoardRepresentation[fromSquareInt])
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
            var fromSquareString = longAlgebraicNotation.Substring(0, 2);
            var fromSquareInt = Definitions.AlgebraicToIndex[fromSquareString];

            var toSquareString = longAlgebraicNotation.Substring(2, 2);
            var toSquareInt = Definitions.AlgebraicToIndex[toSquareString];

            // TODO: Update king square, castling, etc.
            if (board.BoardRepresentation[fromSquareInt] == Definitions.WhiteKing)
            {
                board.KingSquares[1] = toSquareInt;
                board.WhiteCanCastleQueenSide = false;
                board.WhiteCanCastleKingSide = false;

                if (longAlgebraicNotation.Equals("e1g1"))
                {
                    // just move the rook. H1 to F1.
                    MoveRookCastling(21, 23);
                }

                else if (longAlgebraicNotation.Equals("e1c1"))
                {
                    // just move the rook. A1 to D1.
                    MoveRookCastling(28, 25);
                }
            }
            else if (board.BoardRepresentation[fromSquareInt] == Definitions.BlackKing)
            {
                board.KingSquares[0] = toSquareInt;
                board.BlackCanCastleKingSide = false;
                board.BlackCanCastleQueenSide = false;

                if (longAlgebraicNotation.Equals("e8g8"))
                {
                    // just move the rook. H8 to F8.
                    MoveRookCastling(91, 93);
                }

                else if (longAlgebraicNotation.Equals("e8c8"))
                {
                    // just move the rook. A8 to D8
                    MoveRookCastling(98, 95);
                }
            }

            else if (fromSquareInt == 21) // Is rook on h1 moving? Then disable castling.
            {
                board.WhiteCanCastleKingSide = false;
            }

            else if (fromSquareInt == 28) // Is rook on a1 moving? Then disable castling.
            {
                board.WhiteCanCastleQueenSide = false;
            }

            else if (fromSquareInt == 91) // Is rook on h8 moving? Then disable castling.
            {
                board.BlackCanCastleKingSide = false;
            }

            else if (fromSquareInt == 98) // Is rook on a8 moving? Then disable castling.
            {
                board.BlackCanCastleQueenSide = false;
            }

            else if (toSquareInt == board.EnPasSquare && board.BoardRepresentation[fromSquareInt] == Definitions.WhitePawn)
            {
                board.BoardRepresentation[toSquareInt - 10] = Definitions.EmptySquare;
            }

            else if (toSquareInt == board.EnPasSquare && board.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn)
            {
                board.BoardRepresentation[toSquareInt + 10] = Definitions.EmptySquare;
            }

            board.EnPasSquare = Definitions.NoEnPassantSquare;

            // En passant.
            if (board.BoardRepresentation[fromSquareInt] == Definitions.WhitePawn && (toSquareInt - fromSquareInt) == 20)
            {
                board.EnPasSquare = toSquareInt - 10;
            }

            if (board.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn && (fromSquareInt - toSquareInt) == 20)
            {
                board.EnPasSquare = fromSquareInt - 10;
            }

            board.FiftyMoveRule += 1;
            if (board.FiftyMoveRule == 100) // 100 halfmoves = 50 full moves.
            {
                gameIsOver = true;
                gameOverMessage = "Game drawn due to 50 moves rule.";
            }
            // Fifty move rule, was a piece captured or a pawn moved? If so, reset the fifty move counter to 0.
            if (board.BoardRepresentation[fromSquareInt] == Definitions. WhitePawn || board.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn || board.BoardRepresentation[toSquareInt] != Definitions.EmptySquare)
            {
                board.FiftyMoveRule = 0;
            }

            board.BoardRepresentation[toSquareInt] = board.BoardRepresentation[fromSquareInt];
            board.BoardRepresentation[fromSquareInt] = Definitions.EmptySquare;
        }

        private void MoveRookCastling(int fromSquare, int toSquare)
        {
            board.BoardRepresentation[toSquare] = board.BoardRepresentation[fromSquare];
            board.BoardRepresentation[fromSquare] = Definitions.EmptySquare;
        }

        private Board BoardsetupFromFen(string fen)
        {
            int[] boardRepresentation =
            {
                99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
                99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
                99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
                99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            };
            bool sideToMove;
            bool WCCKS = false;
            bool WCCQS = false;
            bool BCCKS = false;
            bool BCCQS = false;
            int enPasSquare;
            int fiftyMoveRule;
            int fullMoves;
            int[] kingSquares = {99, 99};

            int index = 63;
            string[] pieces = fen.Split(' ');

            #region pieceSetup

            foreach (char c in pieces[0])
            {
                if (c > 'a' && c < 'z')
                {
                    switch (c)
                    {
                        case 'p':
                            boardRepresentation[_board64[index]] = Definitions.BlackPawn;
                            break;

                        case 'r':
                            boardRepresentation[_board64[index]] = Definitions.BlackRook;
                            break;

                        case 'n':
                            boardRepresentation[_board64[index]] = Definitions.BlackKnight;
                            break;

                        case 'b':
                            boardRepresentation[_board64[index]] = Definitions.BlackBishop;
                            break;

                        case 'q':
                            boardRepresentation[_board64[index]] = Definitions.BlackQueen;
                            break;

                        case 'k':
                            boardRepresentation[_board64[index]] = Definitions.BlackKing;
                            kingSquares[0] = _board64[index];
                            break;

                        default:
                            Console.WriteLine("This can't happen.");
                            break;
                    }

                    index--;
                }

                else if (c > 'A' && c < 'Z')
                {
                    // piece is white
                    switch (c)
                    {
                        case 'P':
                            boardRepresentation[_board64[index]] = Definitions.WhitePawn;
                            break;

                        case 'R':
                            boardRepresentation[_board64[index]] = Definitions.WhiteRook;
                            break;

                        case 'N':
                            boardRepresentation[_board64[index]] = Definitions.WhiteKnight;
                            break;

                        case 'B':
                            boardRepresentation[_board64[index]] = Definitions.WhiteBishop;
                            break;

                        case 'Q':
                            boardRepresentation[_board64[index]] = Definitions.WhiteQueen;
                            break;

                        case 'K':
                            boardRepresentation[_board64[index]] = Definitions.WhiteKing;
                            kingSquares[1] = _board64[index];
                            break;

                        default:
                            Console.WriteLine("This can't happen.");
                            break;
                    }

                    index--;
                }

                else if (c > '0' && c < '9')
                {
                    // the next 'c' squares are Empty.
                    int n = Convert.ToInt16(c) - '0';
                    index -= n;
                }
            }

            #endregion

            #region side to move

            if (pieces[1] == "w")
            {
                sideToMove = Definitions.WhiteToMove;
            }

            else if (pieces[1] == "b")
            {
                sideToMove = Definitions.BlackToMove;
            }

            else
            {
                sideToMove = Definitions.WhiteToMove;
                Console.WriteLine("Couldn't detect starting site in FEN String. Value was '{0}'.", pieces[1]);
            }

            #endregion

            #region castling rights

            foreach (char c in pieces[2])
            {
                switch (c)
                {
                    case 'K':
                        // White can castle kingside
                        WCCKS = true;
                        break;

                    case 'Q':
                        // White can castle queenside
                        WCCQS = true;
                        break;

                    case 'k':
                        // Black can castle kingside
                        BCCKS = true;
                        break;

                    case 'q':
                        // Black can caslte queenside
                        BCCQS = true;
                        break;

                    default:
                        // '-' no side can castle.
                        break;
                }
            }

            #endregion

            #region en passant square
            if (pieces[3].Equals("-"))
            {
                enPasSquare = Definitions.NoEnPassantSquare;
            }
            else if (Definitions.AlgebraicToIndex.ContainsKey(pieces[3]))
            {
                enPasSquare = Definitions.AlgebraicToIndex[pieces[3]];
            }
            else
            {
                enPasSquare = Definitions.NoEnPassantSquare;
                Console.WriteLine("Failed to determine EnPassant square, value was: '{0}'.", pieces[3]);
            }

            #endregion

            #region fifty move rule

            fiftyMoveRule = Convert.ToInt16(pieces[4]);

            #endregion

            #region full moves

            fullMoves = Convert.ToInt16(pieces[5]);

            #endregion

            return new Board(boardRepresentation, sideToMove, kingSquares, enPasSquare, fiftyMoveRule, WCCKS, WCCQS, BCCKS, BCCQS);
        }
    }
}