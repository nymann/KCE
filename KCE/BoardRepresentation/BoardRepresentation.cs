using System;
using System.Collections.Generic;
using System.Text;
using KCE.BoardRepresentation.PieceRules;

namespace KCE.BoardRepresentation
{
    public class BoardRepresentation
    {
        private BoardState boardState;
        //private bool userColor;
        private bool gameIsOver = false;
        private string gameOverMessage = "";

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
            Helper helper = new Helper();;
            boardState = helper.BoardsetupFromFen(fen);
            PrintBoardWhitePerspective();
            MoveGenerator moveGenerator = new MoveGenerator(boardState);
            List<Ply> legalMoves = moveGenerator.AllLegalMoves();
            var counter = 1;
            Console.WriteLine("\n{0} legal moves.\n", legalMoves.Count);
            foreach (Ply legalMove in legalMoves)
            {
                Console.WriteLine("{0}: {1}.", counter, legalMove.GetAlgebraicPly());
                counter++;
            }

            /*boardState = BoardsetupFromFen(fen);

            while (!gameIsOver)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
                string enPasSquareAlgebraic = "";
                string check = "";

                if (boardState.SideToMove == Definitions.WhiteToMove && IsWhiteKingInCheck())
                {
                    check = "White is in check! ";
                }
                else if (boardState.SideToMove == Definitions.BlackToMove && IsBlackKingInCheck())
                {
                    check = "Black is in check! ";
                }

                if (Definitions.IndexToAlgebraic.ContainsKey(boardState.EnPasSquare))
                {
                    enPasSquareAlgebraic = Definitions.IndexToAlgebraic[boardState.EnPasSquare];
                }
                Console.WriteLine("{3}Last move was: {0}, EnPassantSquare: {1}, FiftyMoveCounter: {2}.\n", boardState.LastMove, enPasSquareAlgebraic, boardState.FiftyMoveRule, check);

                if (boardState.SideToMove == Definitions.WhiteToMove)
                {
                    PrintBoardWhitePerspective();
                }
                else
                {
                    PrintBoardBlackPerspective();
                }

                MoveGenerator moveGenerator = new MoveGenerator(boardState);
                Console.WriteLine("Number of moves: {0}", moveGenerator.AllLegalMoves().Count);
                //Console.WriteLine(BoardState.SideToMove ? "\nWhite to move:" : "\nBlack to move:");

                string userMove = Console.ReadLine();
                userMove = userMove?.ToLower();
                if (UserMove(userMove))
                {
                    MakeMove(userMove);
                    boardState.LastMove = userMove;
                    boardState.SideToMove = !boardState.SideToMove;
                }
            }

            Console.WriteLine(gameOverMessage);*/

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
                switch (boardState.BoardRepresentation[square])
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
                switch (boardState.BoardRepresentation[_board64[i]])
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

        /*private bool UserMove(string longAlgebraicNotation)
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

            if (boardState.BoardRepresentation[fromSquareInt] == Definitions.EmptySquare ||
                boardState.SideToMove == Definitions.BlackToMove && boardState.BoardRepresentation[fromSquareInt] < 6 ||
                boardState.SideToMove == Definitions.WhiteToMove && boardState.BoardRepresentation[fromSquareInt] > 6 && boardState.BoardRepresentation[fromSquareInt] < 13)
            {
                Console.WriteLine("Reached. Side to move was {0}", boardState.SideToMove);
                return false;
            }

            switch (boardState.BoardRepresentation[fromSquareInt])
            {
                case Definitions.WhitePawn:
                case Definitions.BlackPawn:
                    Pawn pawn = new Pawn(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove, boardState.EnPasSquare);
                    return pawn.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteBishop:
                case Definitions.BlackBishop:
                    Bishop bishop = new Bishop(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove);
                    return bishop.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteKnight:
                case Definitions.BlackKnight:
                    Knight knight = new Knight(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove);
                    return knight.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteRook:
                case Definitions.BlackRook:
                    Rook rook = new Rook(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove);
                    return rook.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteQueen:
                case Definitions.BlackQueen:
                    Queen queen = new Queen(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove);
                    return queen.MoveGeneration().Contains(toSquareInt);

                case Definitions.WhiteKing:
                case Definitions.BlackKing:
                    King king = new King(boardState.BoardRepresentation, fromSquareInt, boardState.SideToMove);
                    return king.MoveGeneration().Contains(toSquareInt);

                default:
                    return false;
            }

        }*/

        /*private void MakeMove(string longAlgebraicNotation)
        {
            var fromSquareString = longAlgebraicNotation.Substring(0, 2);
            var fromSquareInt = Definitions.AlgebraicToIndex[fromSquareString];

            var toSquareString = longAlgebraicNotation.Substring(2, 2);
            var toSquareInt = Definitions.AlgebraicToIndex[toSquareString];

            if (boardState.BoardRepresentation[fromSquareInt] == Definitions.WhiteKing)
            {
                boardState.KingSquares[1] = toSquareInt;
                boardState.WhiteCanCastleQueenSide = false;
                boardState.WhiteCanCastleKingSide = false;

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
            else if (boardState.BoardRepresentation[fromSquareInt] == Definitions.BlackKing)
            {
                boardState.KingSquares[0] = toSquareInt;
                boardState.BlackCanCastleKingSide = false;
                boardState.BlackCanCastleQueenSide = false;

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
                boardState.WhiteCanCastleKingSide = false;
            }

            else if (fromSquareInt == 28) // Is rook on a1 moving? Then disable castling.
            {
                boardState.WhiteCanCastleQueenSide = false;
            }

            else if (fromSquareInt == 91) // Is rook on h8 moving? Then disable castling.
            {
                boardState.BlackCanCastleKingSide = false;
            }

            else if (fromSquareInt == 98) // Is rook on a8 moving? Then disable castling.
            {
                boardState.BlackCanCastleQueenSide = false;
            }

            else if (toSquareInt == boardState.EnPasSquare && boardState.BoardRepresentation[fromSquareInt] == Definitions.WhitePawn)
            {
                boardState.BoardRepresentation[toSquareInt - 10] = Definitions.EmptySquare;
            }

            else if (toSquareInt == boardState.EnPasSquare && boardState.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn)
            {
                boardState.BoardRepresentation[toSquareInt + 10] = Definitions.EmptySquare;
            }

            boardState.EnPasSquare = Definitions.NoEnPassantSquare;

            // En passant.
            if (boardState.BoardRepresentation[fromSquareInt] == Definitions.WhitePawn && (toSquareInt - fromSquareInt) == 20)
            {
                boardState.EnPasSquare = toSquareInt - 10;
            }

            if (boardState.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn && (fromSquareInt - toSquareInt) == 20)
            {
                boardState.EnPasSquare = fromSquareInt - 10;
            }

            boardState.FiftyMoveRule += 1;
            if (boardState.FiftyMoveRule == 100) // 100 halfmoves = 50 full moves.
            {
                gameIsOver = true;
                gameOverMessage = "Game drawn due to 50 moves rule.";
            }
            // Fifty move rule, was a piece captured or a pawn moved? If so, reset the fifty move counter to 0.
            if (boardState.BoardRepresentation[fromSquareInt] == Definitions. WhitePawn || boardState.BoardRepresentation[fromSquareInt] == Definitions.BlackPawn || boardState.BoardRepresentation[toSquareInt] != Definitions.EmptySquare)
            {
                boardState.FiftyMoveRule = 0;
            }

            boardState.BoardRepresentation[toSquareInt] = boardState.BoardRepresentation[fromSquareInt];
            boardState.BoardRepresentation[fromSquareInt] = Definitions.EmptySquare;
        }*/

        /*private void MoveRookCastling(int fromSquare, int toSquare)
        {
            boardState.BoardRepresentation[toSquare] = boardState.BoardRepresentation[fromSquare];
            boardState
            fromSquare] = Definitions.EmptySquare;
        }*/
    }
}