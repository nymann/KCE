using System;
using System.Linq;
using KCE.Engine.PieceRules;

namespace KCE.Engine
{
    public class MoveGenerator
    {
        #region variables
        private readonly Helper _helper = new Helper();
        private readonly BoardState _bs;
        //private readonly Zobrist _zobrist = new Zobrist();

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

        #endregion

        public MoveGenerator(BoardState bs)
        {
            _bs = bs;
        }

        public Ply[] AllLegalMoves()
        {
            var legalMoves = new Ply[256];
            var index = 0;

            #region side to move is in double check

            // if it's blacks or whites turn and he is in double check.
            if (_helper.DoubleCheckedFen(_bs.BoardRepresentation, _bs.SideToMove,
                _bs.KingSquares))
            {
                //Console.WriteLine("Check");
                // Player is in check disable all castling.
                var king = new King(_bs.BoardRepresentation, _bs.KingSquares, _bs.SideToMove,
                    false, false,
                    false, false);
                var psuedolegalMoves = king.MoveGeneration();

                var square = 0;
                if (_bs.SideToMove == Definitions.White)
                {
                    square = 1;
                }

                foreach (var psuedoLegalMove in psuedolegalMoves)
                {
                    var ply = _helper.MakePly(_bs.BoardRepresentation, _bs.KingSquares[square], psuedoLegalMove,
                        _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                        _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                    if (!_helper.IsSquareAttacked(_bs.SideToMove, ply.GetBoard(), psuedoLegalMove))
                    {
                        legalMoves[index] = ply;
                        index++;
                    }
                }
            }

            #endregion

            #region black to move

            else if (_bs.SideToMove == Definitions.Black)
            {
                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                            #region black pawn

                        case Definitions.BlackPawn:
                            var blackPawn = new Pawn(_bs.BoardRepresentation, square, Definitions.Black, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                                {
                                    continue;
                                }
                                if (!_helper.IsPieceOnSecondRank(square))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                                else
                                {
                                    /*var castlePerms = _helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.SideToMove);*/
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[psuedoLegalMove] = Definitions.BlackQueen;


                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs, new int[] {square, psuedoLegalMove}) {Promotion = "q"};
                                    legalMoves[index] = queen;
                                    index++;

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[psuedoLegalMove] = Definitions.BlackRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new int[] {square, psuedoLegalMove}) {Promotion = "r"};
                                    legalMoves[index] = rook;
                                    index++;

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[psuedoLegalMove] = Definitions.BlackBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new int[] {square, psuedoLegalMove}) {Promotion = "b"};
                                    legalMoves[index] = bishop;
                                    
                                    index++;

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[psuedoLegalMove] = Definitions.BlackKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new int[] {square, psuedoLegalMove}) {Promotion = "n"};
                                    legalMoves[index] = knight;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region black knight

                        case Definitions.BlackKnight:
                            var blackKnight = new Knight(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region black bishop

                        case Definitions.BlackBishop:
                            var blackBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region black rook

                        case Definitions.BlackRook:
                            var blackRook = new Rook(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region black queen

                        case Definitions.BlackQueen:
                            var blackQueen = new Queen(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region black king

                        case Definitions.BlackKing:
                            King blackKing;
                            if (_helper.IsSquareAttacked(Definitions.Black, _bs.BoardRepresentation,
                                _bs.KingSquares[0]))
                            {
                                blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black,
                                    false, false,
                                    false, false);
                            }
                            else
                            {
                                blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs);
                            }
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();

                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_bs.Bcks && psuedoLegalMove == 92)
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove, 0);
                                }

                                else if (_bs.Bcqs && psuedoLegalMove == 96)
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove, 1);
                                }

                                else
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove);
                                }

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                        default:
                            break;
                    }
            }
            #endregion

            #region white to move

            else if (_bs.SideToMove == Definitions.White)
            {
                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                            #region white pawn

                        case Definitions.WhitePawn:
                            var whitePawn = new Pawn(_bs.BoardRepresentation, square, Definitions.White, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                /*var castlePerms = _helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    _bs.SideToMove);*/
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                    _bs.KingSquares[1]))
                                {
                                    continue;
                                }

                                if (!_helper.IsPieceOnSeventhRank(square))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                                else
                                {
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[psuedoLegalMove] = Definitions.WhiteQueen;
                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs, new int[] {square, psuedoLegalMove}) {Promotion = "q"};
                                    legalMoves[index] = queen;
                                    index++;

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[psuedoLegalMove] = Definitions.WhiteRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new int[] {square, psuedoLegalMove}) {Promotion = "r"};
                                    ;
                                    legalMoves[index] = rook;
                                    index++;

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[psuedoLegalMove] = Definitions.WhiteBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, psuedoLegalMove}) {Promotion = "b"};
                                    ;
                                    legalMoves[index] = bishop;
                                    index++;

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[psuedoLegalMove] = Definitions.WhiteKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, psuedoLegalMove}) {Promotion = "n"};
                                    ;
                                    legalMoves[index] = knight;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region white knight

                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                legalMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white bishop

                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                legalMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white rook

                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                legalMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white queen

                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                legalMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white king

                        case Definitions.WhiteKing:
                            King whiteKing;
                            if (_helper.IsSquareAttacked(Definitions.White, _bs.BoardRepresentation,
                                _bs.KingSquares[1]))
                            {
                                whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White,
                                    false, false,
                                    false, false);
                            }
                            else
                            {
                                whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs);
                            }
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_bs.Wcks && psuedoLegalMove == 22)
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove, 2);
                                }

                                else if (_bs.Wcqs && psuedoLegalMove == 26)
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare, _bs.Bcks,
                                        _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove, 3);
                                }

                                else
                                {
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare, _bs.Bcks,
                                        _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove);
                                }

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves[index] = ply;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                        default:
                            break;
                    }
            }

            #endregion

            var finalArray = new Ply[index];
            for (var i = 0; i < index; i++)
            {
                finalArray[i] = legalMoves[i];
            }

            return finalArray;
        }

        public Ply[] AllCapMoves()
        {
            var capMoves = new Ply[128];
            var index = 0;

            #region Side to move is in double check.

            if (_helper.DoubleCheckedFen(_bs.BoardRepresentation, _bs.SideToMove,
                _bs.KingSquares))
            {
                return null;
            }

            #endregion

            #region Side to move is in check.

            // Find all the moves that can capture the piece that set us in check.

            #region white to move

            if (_bs.SideToMove == Definitions.White)
            {
                // Find the square the white king is attacked by.
                var attackedBySquare = _helper.AttackedBySquare(Definitions.White, _bs.BoardRepresentation,
                    _bs.KingSquares[1]);

                if (attackedBySquare == 99)
                {
                    return null;
                }

                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                        case Definitions.WhitePawn:
                            var whitePawn = new Pawn(_bs.BoardRepresentation, square, Definitions.White, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            if (psuedoLegalPawnMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }

                            break;

                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            if (psuedoLegalKnightMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            if (psuedoLegalBishopMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            if (psuedoLegalRookMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            if (psuedoLegalQueenMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.WhiteKing:
                            var whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            if (psuedoLegalKingMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        default:
                            break;
                    }
            }

            #endregion

            #region black to move

            else
            {
                // Find the square the black king is attacked by.
                var attackedBySquare = _helper.AttackedBySquare(Definitions.Black, _bs.BoardRepresentation,
                    _bs.KingSquares[0]);

                if (attackedBySquare == 99)
                {
                    return null;
                }

                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                        case Definitions.BlackPawn:
                            var blackPawn = new Pawn(_bs.BoardRepresentation, square, Definitions.Black, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            if (psuedoLegalPawnMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }

                            break;

                        case Definitions.BlackKnight:
                            var blackKnight = new Knight(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            if (psuedoLegalKnightMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.BlackBishop:
                            var blackBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            if (psuedoLegalBishopMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.BlackRook:
                            var blackRook = new Rook(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            if (psuedoLegalRookMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.BlackQueen:
                            var blackQueen = new Queen(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            if (psuedoLegalQueenMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        case Definitions.BlackKing:
                            var blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();
                            if (psuedoLegalKingMoves.Contains(attackedBySquare))
                            {
                                capMoves[index] = _helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);
                                index++;
                            }
                            break;

                        default:
                            break;
                    }
            }

            #endregion

            #endregion

            #region White to move

            if (_bs.SideToMove == Definitions.White)
            {
                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                            #region white pawn

                        case Definitions.WhitePawn:
                            var whitePawn = new Pawn(_bs.BoardRepresentation, square, Definitions.White, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            foreach (var toSquare in psuedoLegalPawnMoves)
                            {
                                // If not a capture move, continue to the next psuedo cap move.
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare &&
                                    toSquare != _bs.EnPasSquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                // If in check after move, continue to the next psuedo cap move.
                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                    _bs.KingSquares[1]))
                                {
                                    continue;
                                }

                                // If move is not a promoting move, add it.
                                if (!_helper.IsPieceOnSeventhRank(square))
                                {
                                    capMoves[index] = ply;
                                    index++;
                                }
                                else
                                {
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[toSquare] = Definitions.WhiteQueen;
                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, toSquare}) {Promotion = "q"};
                                    capMoves[index] = queen;
                                    index++;

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[toSquare] = Definitions.WhiteRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, toSquare}) {Promotion = "r"};
                                    capMoves[index] = rook;
                                    index++;

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[toSquare] = Definitions.WhiteBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, toSquare}) {Promotion = "b"};
                                    ;
                                    capMoves[index] = bishop;
                                    index++;

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[toSquare] = Definitions.WhiteKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                        Definitions.NoEnPassantSquare,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        new[] {square, toSquare}) {Promotion = "n"};

                                    capMoves[index] = knight;
                                    index++;
                                }
                            }
                            break;

                            #endregion

                            #region white knight

                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            foreach (var toSquare in psuedoLegalKnightMoves)
                            {
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                capMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white bishop

                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            foreach (var toSquare in psuedoLegalBishopMoves)
                            {
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                capMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white rook

                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            foreach (var toSquare in psuedoLegalRookMoves)
                            {
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                capMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white queen

                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            foreach (var toSquare in psuedoLegalQueenMoves)
                            {
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                capMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                            #region white king

                        case Definitions.WhiteKing:
                            var whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (var toSquare in psuedoLegalKingMoves)
                            {
                                if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                                {
                                    continue;
                                }
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                {
                                    continue;
                                }
                                capMoves[index] = ply;
                                index++;
                            }
                            break;

                            #endregion

                        default:
                            break;
                    }
            }

            #endregion

            #region Black to move

            foreach (var square in _board64)
                switch (_bs.BoardRepresentation[square])
                {
                        #region black pawn

                    case Definitions.BlackPawn:
                        var blackPawn = new Pawn(_bs.BoardRepresentation, square, Definitions.Black, _bs.EnPasSquare);
                        var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                        foreach (var toSquare in psuedoLegalPawnMoves)
                        {
                            // If not a capture move, continue to the next psuedo cap move.
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare &&
                                toSquare != _bs.EnPasSquare)
                            {
                                continue;
                            }

                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare,
                                _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            // If in check after move, continue to the next psuedo cap move.
                            if (_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(),
                                _bs.KingSquares[0]))
                            {
                                continue;
                            }

                            // If move is not a promoting move, add it.
                            if (!_helper.IsPieceOnSecondRank(square))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                            else
                            {
                                var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                boardQueen[square] = Definitions.EmptySquare;
                                boardQueen[toSquare] = Definitions.BlackQueen;
                                var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                    Definitions.NoEnPassantSquare,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    new[] {square, toSquare}) {Promotion = "q"};
                                capMoves[index] = queen;
                                index++;

                                var boardRook = (int[]) boardQueen.Clone();
                                boardRook[toSquare] = Definitions.BlackRook;
                                var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                    Definitions.NoEnPassantSquare,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    new[] {square, toSquare}) {Promotion = "r"};
                                capMoves[index] = rook;
                                index++;

                                var boardbishop = (int[]) boardQueen.Clone();
                                boardbishop[toSquare] = Definitions.BlackBishop;
                                var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                    Definitions.NoEnPassantSquare,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    new[] {square, toSquare}
                                ) {Promotion = "b"};
                                capMoves[index] = bishop;
                                index++;

                                var boardknight = (int[]) boardQueen.Clone();
                                boardknight[toSquare] = Definitions.BlackKnight;
                                var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                    Definitions.NoEnPassantSquare,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    new[] {square, toSquare}) {Promotion = "n"};
                                capMoves[index] = knight;
                                index++;
                            }
                        }
                        break;

                        #endregion

                        #region black knight

                    case Definitions.BlackKnight:
                        var blackKnight = new Knight(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                        for (var i = 0; i < psuedoLegalKnightMoves.Length; i++)
                        {
                            var toSquare = psuedoLegalKnightMoves[i];
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                            {
                                continue;
                            }
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                        }
                        break;

                        #endregion

                        #region black bishop

                    case Definitions.BlackBishop:
                        var blackBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                        foreach (var toSquare in psuedoLegalBishopMoves)
                        {
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                            {
                                continue;
                            }
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                        }
                        break;

                        #endregion

                        #region black rook

                    case Definitions.BlackRook:
                        var blackRook = new Rook(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalRookMoves = blackRook.MoveGeneration();
                        for (var i = 0; i < psuedoLegalRookMoves.Length; i++)
                        {
                            var toSquare = psuedoLegalRookMoves[i];
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                            {
                                continue;
                            }
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                        }
                        break;

                        #endregion

                        #region black queen

                    case Definitions.BlackQueen:
                        var blackQueen = new Queen(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                        foreach (var toSquare in psuedoLegalQueenMoves)
                        {
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                            {
                                continue;
                            }
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                        }
                        break;

                        #endregion

                        #region black king

                    case Definitions.BlackKing:
                        var blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalKingMoves = blackKing.MoveGeneration();
                        foreach (var toSquare in psuedoLegalKingMoves)
                        {
                            if (_bs.BoardRepresentation[toSquare] == Definitions.EmptySquare)
                            {
                                continue;
                            }
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0]))
                            {
                                capMoves[index] = ply;
                                index++;
                            }
                        }
                        break;

                        #endregion

                    default:
                        break;
                }

            #endregion

            var finalArray = new Ply[index];
            //Console.Write("Cap moves:");
            for (var i = 0; i < index; i++)
            {
                finalArray[i] = capMoves[i];
                //Console.Write(" {0}{1}", Definitions.IndexToAlgebraic[finalArray[i].GetFromToSquare()[0]], Definitions.IndexToAlgebraic[finalArray[i].GetFromToSquare()[1]]);
            }
            //Console.Write(".\n");

            return finalArray;
        }

        public ulong Perft(int depth)
        {
            ulong nodes = 0;

            if (depth == 1)
            {
                return (ulong) AllLegalMoves().Length;
            }

            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Length;

            for (var i = 0; i < nMoves; i++)
            {
                MakeMove(legalMoves[i]);
                nodes += Perft(depth - 1);
                UndoMove(legalMoves[i]);
            }

            return nodes;
        }

        public ulong Divide(int depth)
        {
            ulong totalNodes = 0;

            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Length;

            foreach (var legalMove in legalMoves)
            {
                MakeMove(legalMove);

                var childNotes = depth == 1 ? 1 : Perft(depth - 1);
                totalNodes += childNotes;
                UndoMove(legalMove);
                Console.WriteLine("{0}{1}: {2}", Definitions.IndexToAlgebraic[legalMove.GetFromToSquare()[0]], Definitions.IndexToAlgebraic[legalMove.GetFromToSquare()[1]], childNotes);
            }

            Console.WriteLine("\nTotal moves: {0}, Total nodes: {1}", nMoves, totalNodes);

            return totalNodes;
        }

        public void MakeMove(Ply makePly)
        {
            _bs.HisEnPas = _bs.EnPasSquare;
            _bs.EnPasSquare = makePly.GetEnPas();
            _bs.BoardRepresentation = (int[]) makePly.GetBoard().Clone();
            _bs.SideToMove = !_bs.SideToMove;

            _bs.HisWcks = _bs.Wcks;
            _bs.HisWcqs = _bs.Wcqs;
            _bs.HisBcks = _bs.Bcks;
            _bs.HisBcqs = _bs.Bcqs;

            _bs.Wcks = makePly.GetWCCKS();
            _bs.Wcqs = makePly.GetWCCQS();
            _bs.Bcks = makePly.GetBCCKS();
            _bs.Bcqs = makePly.GetBCCQS();
            //_bs.Moves[0, _bs.Ply] = makePly.GetFromToSquare()[0];
            //_bs.Moves[1, _bs.Ply] = makePly.GetFromToSquare()[1];
            _bs.Ply++;
            UpdateKingSquares();
        }

        public void UndoMove(Ply undoPly)
        {
            _bs.EnPasSquare = _bs.HisEnPas;
            _bs.BoardRepresentation = (int[]) undoPly.GetHisBoard().Clone();
            _bs.SideToMove = !_bs.SideToMove;
            _bs.Wcks = _bs.HisWcks;
            _bs.Wcqs = _bs.HisWcqs;
            _bs.Bcks = _bs.HisWcks;
            _bs.Bcqs = _bs.HisWcqs;
            //_bs.Moves[0, _bs.Ply] = 0;
            //_bs.Moves[1, _bs.Ply] = 0;
            _bs.Ply--;
            UpdateKingSquares();
        }

        private void UpdateKingSquares()
        {
            if (_bs.BoardRepresentation[_bs.KingSquares[0]] != Definitions.BlackKing ||
                _bs.BoardRepresentation[_bs.KingSquares[1]] != Definitions.WhiteKing)
            {
                foreach (var square in _board64)
                    if (_bs.BoardRepresentation[square] == Definitions.BlackKing)
                    {
                        _bs.KingSquares[0] = square;
                    }
                    else if (_bs.BoardRepresentation[square] == Definitions.WhiteKing)
                    {
                        _bs.KingSquares[1] = square;
                    }
            }
        }

        /*private void UpdatePosKey(Ply ply)
        {
            //_bs.PieceKeys[piece,square]
            /* position startpos
             * e2e4
             * XOR out the pawn at e2.
             * XOR in the pawn at e4.
             #1#

            /* position startpos moves e2e4 d7d5
             * e4d5
             * XOR out the black pawn at d5.
             * XOR in the white pawn at d5.
             * XOR out the white pawn from e4.
             * XOR in nothing at the source square.
             #1#
            var toSquare = ply.GetFromToSquare()[1];
            var fromSquare = ply.GetFromToSquare()[0];

            var opponentPieceOnToSquare = _bs.BoardRepresentation[toSquare];
            var pieceOnFromSquare = _bs.BoardRepresentation[fromSquare];
            /*_bs.PosKey ^= _bs.PieceKeys[opponentPieceOnToSquare, toSquare];
            Console.WriteLine("XOR'ed out oppoentPieceOnToSquare: {0:X}.", _bs.PosKey);#1#
            _bs.PosKey ^= _bs.PieceKeys[pieceOnFromSquare, toSquare];
            Console.WriteLine("XOR'ed in piece on from square: {0:X}.", _bs.PosKey);
            _bs.PosKey ^= _bs.PieceKeys[pieceOnFromSquare, fromSquare];
            Console.WriteLine("XOR'ed out the piece on from square: {0:X}.", _bs.PosKey);
            /*_bs.PosKey ^= _bs.PieceKeys[Definitions.EmptySquare, fromSquare];
            Console.WriteLine("XOR'ed in nothing at the from square: {0:X}.", _bs.PosKey);#1#
        }*/
    }
}