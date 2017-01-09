using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KCE.BoardRepresentation.PieceRules
{
    public class Helper
    {

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

        public Helper()
        {
            
        }

        public bool IsPieceBound(int square, int piece, bool side, int[] board)
        {
            return false;
        }

        public bool IsSquareAttacked(bool attackingSide, int[] board, int squareInQuestion)
        {
            Knight imaginaryKnight = new Knight(board, squareInQuestion, attackingSide);
            var listOfAttackingSquares = imaginaryKnight.MoveGeneration();
            foreach (int square in listOfAttackingSquares)
            {
                if (board[square] == Definitions.WhiteKnight || board[square] == Definitions.BlackKnight)
                {
                    return true;
                }
            }

            Bishop imaginaryBishop = new Bishop(board, squareInQuestion, attackingSide);
            var listOfBishopMoves = imaginaryBishop.MoveGeneration();

            foreach (int possibleAttackedFromSquares in listOfBishopMoves)
            {
                if (board[possibleAttackedFromSquares] == Definitions.WhiteQueen ||
                    board[possibleAttackedFromSquares] == Definitions.BlackQueen ||
                    board[possibleAttackedFromSquares] == Definitions.WhiteBishop ||
                    board[possibleAttackedFromSquares] == Definitions.BlackBishop)
                {
                    return true;
                }
            }

            if (attackingSide == Definitions.WhiteToMove)
            {
                // Check if black has pawns that can attack us.
                if (board[squareInQuestion + 11] == Definitions.BlackPawn ||
                    board[squareInQuestion + 9] == Definitions.BlackPawn)
                {
                    return true;
                }
            }
            else
            {
                // Check if white has pawns that can attack us.
                if (board[squareInQuestion - 11] == Definitions.WhitePawn ||
                    board[squareInQuestion - 9] == Definitions.WhitePawn)
                {
                    return true;
                }
            }

            Rook imaginaryRook = new Rook(board, squareInQuestion, attackingSide);
            var listOfRookMoves = imaginaryRook.MoveGeneration();

            foreach (int possibleAttackedFromSquares in listOfRookMoves)
            {
                if (board[possibleAttackedFromSquares] == Definitions.WhiteQueen ||
                    board[possibleAttackedFromSquares] == Definitions.BlackQueen ||
                    board[possibleAttackedFromSquares] == Definitions.WhiteRook ||
                    board[possibleAttackedFromSquares] == Definitions.BlackRook)
                {
                    return true;
                }
            }

            return false;
        }

        public Ply MakePly(int[] hisBoard, int fromSquare, int toSquare, int hisEnPas)
        {
            int enPas = Definitions.NoEnPassantSquare;
            int[] board = (int[]) hisBoard.Clone(); // 7 hour bug.
            
            int pieceOnFromSquare = board[fromSquare];
            board[fromSquare] = Definitions.EmptySquare;
            board[toSquare] = pieceOnFromSquare;
            string algebraicPly = Definitions.IndexToAlgebraic[fromSquare] + Definitions.IndexToAlgebraic[toSquare];

            // En passant.
            if (hisBoard[fromSquare] == Definitions.WhitePawn && (toSquare - fromSquare) == 20)
            {
                enPas = toSquare - 10;
            }

            if (hisBoard[fromSquare] == Definitions.BlackPawn && (fromSquare - toSquare) == 20)
            {
                enPas = fromSquare - 10;
            }

            return new Ply(board, hisBoard, hisEnPas, enPas, algebraicPly);
        }

        /*private bool DoubleChecked()
        {
            // If we are in check by the start in a fen string, then we are doomed.
            string lastMoveToSquare = _boardState.LastMove.Substring(2, 2);

            if (Definitions.AlgebraicToIndex.ContainsKey(lastMoveToSquare))
            {
                int lastMove = Definitions.AlgebraicToIndex[lastMoveToSquare];
                if (_boardState.BoardRepresentation[lastMove] == Definitions.WhiteKnight ||
                    _boardState.BoardRepresentation[lastMove] == Definitions.WhitePawn ||
                    _boardState.BoardRepresentation[lastMove] == Definitions.BlackKnight ||
                    _boardState.BoardRepresentation[lastMove] == Definitions.BlackPawn)
                {
                    if (_boardState.SideToMove == Definitions.WhiteToMove)
                    {
                        Knight whiteKnight = new Knight(_boardState, _boardState.KingSquares[1]);
                        var whiteKnightMoves = whiteKnight.MoveGeneration();
                        if (whiteKnightMoves.Contains(lastMove))
                        {
                            return true;
                        }

                        Pawn whitePawn = new Pawn(_boardState.KingSquares[1], _boardState);
                        var whitePawnMoves = whitePawn.MoveGeneration();
                        if (whitePawnMoves.Contains(lastMove))
                        {
                            return true;
                        }

                        return false;
                    }

                    Knight blackKnight = new Knight(_boardState, _boardState.KingSquares[0]);
                    var blackKnightMoves = blackKnight.MoveGeneration();
                    if (blackKnightMoves.Contains(lastMove))
                    {
                        return true;
                    }

                    Pawn blackPawn = new Pawn(_boardState.KingSquares[0], _boardState);
                    var blackPawnMoves = blackPawn.MoveGeneration();
                    if (blackPawnMoves.Contains(lastMove))
                    {
                        return true;
                    }

                    return false;
                }
            }

            return false;
        }*/

        public void PrintBoardWhitePerspective(int[] board)
        {
            var counter = 8;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   a   b   c   d   e   f   g   h");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            for (int i = 63; i >= 0; i--)
            {
                Console.Write(" | ");
                switch (board[_board64[i]])
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
    }
}