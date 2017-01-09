using System;
using System.Collections.Generic;
using KCE.BoardRepresentation.PieceRules;

namespace KCE.BoardRepresentation
{
    public class MoveGenerator
    {
        private Helper helper;
        private BoardState _boardState;
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

        public MoveGenerator(BoardState boardState)
        {
            helper = new Helper();
            _boardState = boardState;
        }

        public List<Ply> AllLegalMoves()
        {
            List<Ply> legalMoves = new List<Ply>();

            #region black to move and not in check
            if (_boardState.SideToMove == Definitions.Black && !IsSquareAttacked(_boardState.KingSquares[0]))
            {
                foreach (int square in _board64)
                {
                    // Loop through the BoardState, find blackpieces, and generate moves for them.
                    switch (_boardState.BoardRepresentation[square])
                    {
                        case Definitions.BlackPawn:
                            Pawn blackPawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.Black, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);
                                
                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.BlackKnight:
                            Knight blackKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.BlackBishop:
                            Bishop blackBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.BlackRook:
                            Rook blackRook = new Rook(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.BlackQueen:
                            Queen blackQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.BlackKing:
                            King blackKing = new King(_boardState.BoardRepresentation, square, Definitions.Black,
                                _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion

            #region white to move and not in check
            // If it's black's turn and he is not in check.
            else if (_boardState.SideToMove == Definitions.White && !IsSquareAttacked(_boardState.KingSquares[1]))
            {
                foreach (int square in _board64)
                {
                    // Loop through the BoardState, find whitepieces, and generate moves for them.
                    switch (_boardState.BoardRepresentation[square])
                    {
                        case Definitions.WhitePawn:
                            Pawn whitePawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.White, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.WhiteKnight:
                            Knight whiteKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.WhiteBishop:
                            Bishop whiteBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.WhiteRook:
                            Rook whiteRook = new Rook(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.WhiteQueen:
                            Queen whiteQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        case Definitions.WhiteKing:
                            King whiteKing = new King(_boardState.BoardRepresentation, square, Definitions.White,
                                _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion

            #region side to move is in double check
            // if it's blacks or whites turn and he is in double check.
            else if (helper.DoubleCheckedFEN(_boardState.BoardRepresentation, _boardState.SideToMove,
                _boardState.KingSquares))
            {
                King king = new King(_boardState.BoardRepresentation, _boardState.KingSquares,_boardState.SideToMove,
                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                    _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide);
                var psuedolegalMoves = king.MoveGeneration();
                
                var square = 0;
                if (_boardState.SideToMove == Definitions.White)
                {
                    square = 1;
                }

                foreach (var psuedoLegalMove in psuedolegalMoves)
                {
                    Ply ply = helper.MakePly(_boardState.BoardRepresentation, _boardState.KingSquares[square], psuedoLegalMove,
                                    _boardState.EnPasSquare);

                    if (!helper.IsSquareAttacked(_boardState.SideToMove, ply.GetBoard(), psuedoLegalMove))
                    {
                        legalMoves.Add(ply);
                    }
                }
            }
            
            #endregion

            #region side to move is in check
            else
            {
                // Current sides king is in check, but it can possibly be guarded by other pieces or the checking piece could possibly be captued.
            }
            #endregion

            return legalMoves;
        }

        private bool IsSquareAttacked(int square)
        {
            Knight imaginaryKnight = new Knight(_boardState.BoardRepresentation, square, _boardState.SideToMove);

            var listOfKnightMoves = imaginaryKnight.MoveGeneration();
            foreach (int possibleAttackedFromSquares in listOfKnightMoves)
            {
                if (_boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.WhiteKnight ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.BlackKnight)
                {
                    return true;
                }
            }

            Bishop imaginaryBishop = new Bishop(_boardState.BoardRepresentation, square, _boardState.SideToMove);
            var listOfBishopMoves = imaginaryBishop.MoveGeneration();

            foreach (int possibleAttackedFromSquares in listOfBishopMoves)
            {
                if (_boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.WhiteQueen ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.BlackQueen ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.WhiteBishop ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.BlackBishop)
                {
                    return true;
                }
            }

            if (_boardState.SideToMove == Definitions.WhiteToMove)
            {
                // Check if black has pawns that can attack us.
                if (_boardState.BoardRepresentation[square + 11] == Definitions.BlackPawn ||
                    _boardState.BoardRepresentation[square + 9] == Definitions.BlackPawn)
                {
                    return true;
                }
            }
            else
            {
                // Check if white has pawns that can attack us.
                if (_boardState.BoardRepresentation[square - 11] == Definitions.WhitePawn ||
                    _boardState.BoardRepresentation[square - 9] == Definitions.WhitePawn)
                {
                    return true;
                }
            }

            Rook imaginaryRook = new Rook(_boardState.BoardRepresentation, square, _boardState.SideToMove);
            var listOfRookMoves = imaginaryRook.MoveGeneration();

            foreach (int possibleAttackedFromSquares in listOfRookMoves)
            {
                if (_boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.WhiteQueen ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.BlackQueen ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.WhiteRook ||
                    _boardState.BoardRepresentation[possibleAttackedFromSquares] == Definitions.BlackRook)
                {
                    return true;
                }
            }

            return false;
        }

    }
}