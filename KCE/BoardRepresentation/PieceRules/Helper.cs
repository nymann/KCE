using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

            foreach (int possibleAttackedFromSquare in listOfBishopMoves)
            {
                if (board[possibleAttackedFromSquare] == Definitions.WhiteQueen ||
                    board[possibleAttackedFromSquare] == Definitions.BlackQueen ||
                    board[possibleAttackedFromSquare] == Definitions.WhiteBishop ||
                    board[possibleAttackedFromSquare] == Definitions.BlackBishop)
                {
                    return true;
                }
            }

            King imaginaryKing = new King(board, squareInQuestion, attackingSide);
            var listOfKingMoves = imaginaryKing.MoveGeneration();

            foreach (int possibleAttackedFromSquare in listOfKingMoves)
            {
                if (board[possibleAttackedFromSquare] == Definitions.WhiteKing ||
                    board[possibleAttackedFromSquare] == Definitions.BlackKing)
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

        public Ply MakePly(int[] hisBoard,
            int fromSquare, int toSquare,
            int hisEnPas,
            bool hisBCCKS, bool hisBCCQS, bool hisWCCKS, bool hisWCCQS,
            bool sideToMove,
            int performCastling = -1)
        {
            int enPas = Definitions.NoEnPassantSquare;
            int[] board = (int[]) hisBoard.Clone(); // 7 hour bug.

            int pieceOnFromSquare = board[fromSquare];
            board[fromSquare] = Definitions.EmptySquare;
            board[toSquare] = pieceOnFromSquare;


            string algebraicPly = Definitions.IndexToAlgebraic[fromSquare] + Definitions.IndexToAlgebraic[toSquare];

            // are we castling?
            if (performCastling != -1)
            {
                if (performCastling == 0)
                {
                    // Black kingside castling
                    board[91] = Definitions.EmptySquare;
                    board[93] = Definitions.BlackRook;
                }

                else if (performCastling == 1)
                {
                    // Black queenside castling.
                    board[98] = Definitions.EmptySquare;
                    board[95] = Definitions.BlackRook;
                }

                else if (performCastling == 2)
                {
                    // White kingside castling
                    board[21] = Definitions.EmptySquare;
                    board[23] = Definitions.WhiteRook;
                }

                else if (performCastling == 3)
                {
                    // White queenside castling
                    board[28] = Definitions.EmptySquare;
                    board[25] = Definitions.WhiteRook;
                }
            }

            #region En Passsant

            // Capture En Passant
            if (sideToMove == Definitions.White && hisBoard[fromSquare] == Definitions.WhitePawn &&
                hisEnPas == toSquare)
            {
                // example, toSquare = enpassquare = e6 (74), fromSquare = d5 (65), black pawn on e5 (64) 
                if (fromSquare - toSquare == -9)
                {
                    board[fromSquare - 1] = Definitions.EmptySquare;
                }

                // example, toSquare = enpassquare = c6 (76), fromSquare = d5 (65), black pawn on c5 (66) 
                else if (fromSquare - toSquare == -11)
                {
                    board[fromSquare + 1] = Definitions.EmptySquare;
                }
            }
            else if (sideToMove == Definitions.Black && hisBoard[fromSquare] == Definitions.BlackPawn &&
                     hisEnPas == toSquare)
            {
                // example, fromsquare = enpassquare = d3 (45), fromSquare = e4 (54), white pawn on d4 (55) 
                if (fromSquare - toSquare == 9)
                {
                    board[fromSquare + 1] = Definitions.EmptySquare;
                }

                else if (fromSquare - toSquare == 11)
                {
                    board[fromSquare - 1] = Definitions.EmptySquare;
                }
            }

            // Set En passant square.
            if (hisBoard[fromSquare] == Definitions.WhitePawn && (toSquare - fromSquare) == 20)
            {
                enPas = toSquare - 10;
            }

            if (hisBoard[fromSquare] == Definitions.BlackPawn && (fromSquare - toSquare) == 20)
            {
                enPas = fromSquare - 10;
            }

            #endregion

            bool[] castle = UpdateCastlePermissions(fromSquare, toSquare, hisWCCKS, hisWCCQS, hisBCCKS, hisBCCQS,
                sideToMove);

            return new Ply(board, hisBoard, hisEnPas, enPas, algebraicPly,
                castle[Definitions.WCCKS], castle[Definitions.WCCQS],
                castle[Definitions.BCCKS], castle[Definitions.BCCQS],
                hisWCCKS, hisWCCQS, hisBCCKS, hisBCCQS);
        }

        public bool DoubleCheckedFen(int[] board, bool sideToMove, int[] kingSquare)
        {
            bool possibleDoubleCheck = false;

            #region white side

            if (sideToMove == Definitions.White)
            {
                Knight whiteKnight = new Knight(board, kingSquare[1], Definitions.White);
                var listOfWhiteMoves = whiteKnight.MoveGeneration();

                foreach (var square in listOfWhiteMoves)
                {
                    if (board[square] == Definitions.BlackKnight)
                    {
                        possibleDoubleCheck = true;
                        break;
                    }
                }

                if (!possibleDoubleCheck)
                {
                    return false;
                }

                Bishop whiteBishop = new Bishop(board, kingSquare[1], Definitions.White);
                listOfWhiteMoves = whiteBishop.MoveGeneration();
                foreach (var square in listOfWhiteMoves)
                {
                    if (board[square] == Definitions.BlackBishop)
                    {
                        return true;
                    }
                }


                Rook whiteRook = new Rook(board, kingSquare[1], Definitions.White);
                listOfWhiteMoves = whiteRook.MoveGeneration();
                foreach (var square in listOfWhiteMoves)
                {
                    if (board[square] == Definitions.BlackRook)
                    {
                        return true;
                    }
                }
            }
            #endregion

            #region black side

            else
            {
                Knight blackKnight = new Knight(board, kingSquare[0], Definitions.Black);
                var listOfBlackMoves = blackKnight.MoveGeneration();

                foreach (var square in listOfBlackMoves)
                {
                    if (board[square] == Definitions.WhiteKnight)
                    {
                        possibleDoubleCheck = true;
                        break;
                    }
                }

                if (!possibleDoubleCheck)
                {
                    return false;
                }

                Bishop blackBishop = new Bishop(board, kingSquare[0], Definitions.Black);
                listOfBlackMoves = blackBishop.MoveGeneration();
                foreach (var square in listOfBlackMoves)
                {
                    if (board[square] == Definitions.WhiteBishop)
                    {
                        return true;
                    }
                }

                Rook blackRook = new Rook(board, kingSquare[0], Definitions.Black);
                listOfBlackMoves = blackRook.MoveGeneration();
                foreach (var square in listOfBlackMoves)
                {
                    if (board[square] == Definitions.WhiteRook)
                    {
                        return true;
                    }
                }
            }

            #endregion

            return false;
        }

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

        private void PrintBoardBlackPerspective(int[] board)
        {
            var counter = 1;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   h   g   f   e   d   c   b   a");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            foreach (var square in _board64)
            {
                Console.Write(" | ");
                switch (board[square])
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

                if (square % 10 != 8) continue;
                Console.WriteLine(" | {0}", counter);
                Console.WriteLine(" +---+---+---+---+---+---+---+---+");
                counter++;
            }
        }

        public BoardState BoardsetupFromFen(string fen)
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

            return new BoardState(boardRepresentation, sideToMove, kingSquares, enPasSquare, fiftyMoveRule, WCCKS, WCCQS,
                BCCKS, BCCQS);
        }

        public bool IsPieceOnSecondRank(int square)
        {
            return square / 10 == 3;
        }

        public bool IsPieceOnSeventhRank(int square)
        {
            return square / 10 == 8;
        }

        public bool[] UpdateCastlePermissions(int squareFrom, int squareTo, bool wccks, bool wccqs, bool bccks,
            bool bccqs, bool sideToMove)
        {
            bool WCCKS = false;
            bool WCCQS = false;
            bool BCCKS = false;
            bool BCCQS = false;

            if (sideToMove == Definitions.White)
            {
                if (wccks && squareFrom != 24 && squareFrom != 21)
                {
                    WCCKS = true;
                }

                if (wccqs && squareFrom != 24 && squareFrom != 28)
                {
                    WCCQS = true;
                }

                if (bccks && squareTo != 91)
                {
                    BCCKS = true;
                }

                if (bccqs && squareTo != 98)
                {
                    BCCQS = true;
                }

                return new[] {BCCKS, BCCQS, WCCKS, WCCQS};
            }

            if (bccks && squareFrom != 94 && squareFrom != 91)
            {
                BCCKS = true;
            }

            if (bccqs && squareFrom != 94 && squareFrom != 98)
            {
                BCCQS = true;
            }

            if (wccks && squareTo != 21)
            {
                WCCKS = true;
            }

            if (wccqs && squareTo != 28)
            {
                WCCQS = true;
            }

            return new[] {BCCKS, BCCQS, WCCKS, WCCQS};
        }
    }
}