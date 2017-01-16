using System;
using System.Collections.Generic;
using System.Linq;
using KCE.Engine.PieceRules;

namespace KCE.Engine
{
    public class MoveGenerator
    {
        #region variables

        private readonly Helper _helper;
        private readonly BoardState _bs;

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
            _helper = new Helper();
            _bs = bs;
        }

        public List<Ply> AllLegalMoves()
        {
            var legalMoves = new List<Ply>();

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
                    square = 1;

                foreach (var psuedoLegalMove in psuedolegalMoves)
                {
                    var ply = _helper.MakePly(_bs.BoardRepresentation, _bs.KingSquares[square], psuedoLegalMove,
                        _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                        _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                    if (!_helper.IsSquareAttacked(_bs.SideToMove, ply.GetBoard(), psuedoLegalMove))
                        legalMoves.Add(ply);
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
                                    continue;
                                if (!_helper.IsPieceOnSecondRank(square))
                                {
                                    legalMoves.Add(ply);
                                }
                                else
                                {
                                    var castlePerms = _helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.SideToMove);
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[psuedoLegalMove] = Definitions.BlackQueen;


                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "q",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    legalMoves.Add(queen);

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[psuedoLegalMove] = Definitions.BlackRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation, /*_bs.EnPasSquare,*/
                                        Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "r",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    legalMoves.Add(rook);

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[psuedoLegalMove] = Definitions.BlackBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation, /*_bs.EnPasSquare,*/
                                        Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "b",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    legalMoves.Add(bishop);

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[psuedoLegalMove] = Definitions.BlackKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation, /*_bs.EnPasSquare,*/
                                        Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "n",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    legalMoves.Add(knight);
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
                                    legalMoves.Add(ply);
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
                                    legalMoves.Add(ply);
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
                                    legalMoves.Add(ply);
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
                                    legalMoves.Add(ply);
                            }
                            break;

                            #endregion

                            #region black king

                        case Definitions.BlackKing:
                            King blackKing;
                            if (_helper.IsSquareAttacked(Definitions.Black, _bs.BoardRepresentation,
                                _bs.KingSquares[0]))
                                blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black,
                                    false, false,
                                    false, false);
                            else
                                blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();

                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_bs.Bcks && psuedoLegalMove == 92)
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove, 0);

                                else if (_bs.Bcqs && psuedoLegalMove == 96)
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove, 1);

                                else
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), psuedoLegalMove))
                                    legalMoves.Add(ply);
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
                                var castlePerms = _helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    _bs.SideToMove);
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                    _bs.KingSquares[1])) continue;

                                if (!_helper.IsPieceOnSeventhRank(square))
                                {
                                    legalMoves.Add(ply);
                                }
                                else
                                {
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[psuedoLegalMove] = Definitions.WhiteQueen;
                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation/*,
                                        _bs.EnPasSquare*/, Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "q",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    legalMoves.Add(queen);

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[psuedoLegalMove] = Definitions.WhiteRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "r",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    ;
                                    legalMoves.Add(rook);

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[psuedoLegalMove] = Definitions.WhiteBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "b",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    ;
                                    legalMoves.Add(bishop);

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[psuedoLegalMove] = Definitions.WhiteKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[psuedoLegalMove] + "n",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, psuedoLegalMove*/);
                                    ;
                                    legalMoves.Add(knight);
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

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                    legalMoves.Add(ply);
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

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                    legalMoves.Add(ply);
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

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                    legalMoves.Add(ply);
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

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1]))
                                    legalMoves.Add(ply);
                            }
                            break;

                            #endregion

                            #region white king

                        case Definitions.WhiteKing:
                            King whiteKing;
                            if (_helper.IsSquareAttacked(Definitions.White, _bs.BoardRepresentation,
                                _bs.KingSquares[1]))
                                whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White,
                                    false, false,
                                    false, false);
                            else
                                whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_bs.Wcks && psuedoLegalMove == 22)
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare,
                                        _bs.Bcks, _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove, 2);

                                else if (_bs.Wcqs && psuedoLegalMove == 26)
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare, _bs.Bcks,
                                        _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove, 3);

                                else
                                    ply = _helper.MakePly(_bs.BoardRepresentation, square, psuedoLegalMove,
                                        _bs.EnPasSquare, _bs.Bcks,
                                        _bs.Bcqs,
                                        false, false,
                                        _bs.SideToMove);

                                if (!_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), psuedoLegalMove))
                                    legalMoves.Add(ply);
                            }
                            break;

                            #endregion

                        default:
                            break;
                    }
            }

            #endregion

            return legalMoves;
        }

        public List<Ply> AllCapMoves()
        {
            var capMoves = new List<Ply>();

            #region Side to move is in double check.

            if (_helper.DoubleCheckedFen(_bs.BoardRepresentation, _bs.SideToMove,
                _bs.KingSquares))
                return capMoves;

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
                    return capMoves;

                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                        case Definitions.WhitePawn:
                            var whitePawn = new Pawn(_bs.BoardRepresentation, square, Definitions.White, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            if (psuedoLegalPawnMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));

                            break;

                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            if (psuedoLegalKnightMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            if (psuedoLegalBishopMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            if (psuedoLegalRookMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            if (psuedoLegalQueenMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.WhiteKing:
                            var whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            if (psuedoLegalKingMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
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
                    return capMoves;

                foreach (var square in _board64)
                    switch (_bs.BoardRepresentation[square])
                    {
                        case Definitions.BlackPawn:
                            var blackPawn = new Pawn(_bs.BoardRepresentation, square, Definitions.Black, _bs.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            if (psuedoLegalPawnMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));

                            break;

                        case Definitions.BlackKnight:
                            var blackKnight = new Knight(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            if (psuedoLegalKnightMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.BlackBishop:
                            var blackBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            if (psuedoLegalBishopMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.BlackRook:
                            var blackRook = new Rook(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            if (psuedoLegalRookMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.BlackQueen:
                            var blackQueen = new Queen(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            if (psuedoLegalQueenMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        case Definitions.BlackKing:
                            var blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();
                            if (psuedoLegalKingMoves.Contains(attackedBySquare))
                                capMoves.Add(_helper.MakePly(_bs.BoardRepresentation, square, attackedBySquare,
                                    _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove));
                            break;

                        default:
                            break;
                    }
            }

            #endregion

            #endregion

            #region White to move

            if (_bs.SideToMove == Definitions.White)
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
                                    toSquare != _bs.EnPasSquare) continue;

                                var castlePerms = _helper.UpdateCastlePermissions(square, toSquare,
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs,
                                    _bs.SideToMove);
                                var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare,
                                    _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                    _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                                // If in check after move, continue to the next psuedo cap move.
                                if (_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                    _bs.KingSquares[1])) continue;

                                // If move is not a promoting move, add it.
                                if (!_helper.IsPieceOnSeventhRank(square))
                                {
                                    capMoves.Add(ply);
                                }
                                else
                                {
                                    var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                    boardQueen[square] = Definitions.EmptySquare;
                                    boardQueen[toSquare] = Definitions.WhiteQueen;
                                    var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[toSquare] + "q",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                    capMoves.Add(queen);

                                    var boardRook = (int[]) boardQueen.Clone();
                                    boardRook[toSquare] = Definitions.WhiteRook;
                                    var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[toSquare] + "r",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                    ;
                                    capMoves.Add(rook);

                                    var boardbishop = (int[]) boardQueen.Clone();
                                    boardbishop[toSquare] = Definitions.WhiteBishop;
                                    var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[toSquare] + "b",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                    ;
                                    capMoves.Add(bishop);

                                    var boardknight = (int[]) boardQueen.Clone();
                                    boardknight[toSquare] = Definitions.WhiteKnight;
                                    var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                        /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                        Definitions.IndexToAlgebraic[square] +
                                        Definitions.IndexToAlgebraic[toSquare] + "n",
                                        _bs.Wcks, _bs.Wcqs,
                                        _bs.Bcks, _bs.Bcqs/*,
                                        castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                        castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                    ;
                                    capMoves.Add(knight);
                                }
                            }
                            break;

                            #endregion

                            #region white knight

                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            capMoves.AddRange(from toSquare in psuedoLegalKnightMoves
                                where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                                select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                                into ply
                                where !_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1])
                                select ply);
                            break;

                            #endregion

                            #region white bishop

                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            capMoves.AddRange(from toSquare in psuedoLegalBishopMoves
                                where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                                select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                                into ply
                                where !_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1])
                                select ply);
                            break;

                            #endregion

                            #region white rook

                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            capMoves.AddRange(from toSquare in psuedoLegalRookMoves
                                where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                                select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                                into ply
                                where !_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1])
                                select ply);
                            break;

                            #endregion

                            #region white queen

                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            capMoves.AddRange(from toSquare in psuedoLegalQueenMoves
                                where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                                select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                                into ply
                                where !_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1])
                                select ply);
                            break;

                            #endregion

                            #region white king

                        case Definitions.WhiteKing:
                            var whiteKing = new King(_bs.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            capMoves.AddRange(from toSquare in psuedoLegalKingMoves
                                where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                                select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                    _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                                into ply
                                where !_helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _bs.KingSquares[1])
                                select ply);
                            break;

                            #endregion

                        default:
                            break;
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
                                toSquare != _bs.EnPasSquare) continue;

                            var castlePerms = _helper.UpdateCastlePermissions(square, toSquare,
                                _bs.Wcks, _bs.Wcqs,
                                _bs.Bcks, _bs.Bcqs,
                                _bs.SideToMove);
                            var ply = _helper.MakePly(_bs.BoardRepresentation, square, toSquare,
                                _bs.EnPasSquare, _bs.Bcks, _bs.Bcqs,
                                _bs.Wcks, _bs.Wcqs, _bs.SideToMove);

                            // If in check after move, continue to the next psuedo cap move.
                            if (_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(),
                                _bs.KingSquares[0])) continue;

                            // If move is not a promoting move, add it.
                            if (!_helper.IsPieceOnSecondRank(square))
                            {
                                capMoves.Add(ply);
                            }
                            else
                            {
                                var boardQueen = (int[]) _bs.BoardRepresentation.Clone();
                                boardQueen[square] = Definitions.EmptySquare;
                                boardQueen[toSquare] = Definitions.BlackQueen;
                                var queen = new Ply(boardQueen, _bs.BoardRepresentation,
                                    /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                    Definitions.IndexToAlgebraic[square] +
                                    Definitions.IndexToAlgebraic[toSquare] + "q",
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs/*,
                                    castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                    castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                capMoves.Add(queen);

                                var boardRook = (int[]) boardQueen.Clone();
                                boardRook[toSquare] = Definitions.BlackRook;
                                var rook = new Ply(boardRook, _bs.BoardRepresentation,
                                    /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                    Definitions.IndexToAlgebraic[square] +
                                    Definitions.IndexToAlgebraic[toSquare] + "r",
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs/*,
                                    castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                    castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                ;
                                capMoves.Add(rook);

                                var boardbishop = (int[]) boardQueen.Clone();
                                boardbishop[toSquare] = Definitions.BlackBishop;
                                var bishop = new Ply(boardbishop, _bs.BoardRepresentation,
                                    /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                    Definitions.IndexToAlgebraic[square] +
                                    Definitions.IndexToAlgebraic[toSquare] + "b",
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs/*,
                                    castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                    castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                ;
                                capMoves.Add(bishop);

                                var boardknight = (int[]) boardQueen.Clone();
                                boardknight[toSquare] = Definitions.BlackKnight;
                                var knight = new Ply(boardknight, _bs.BoardRepresentation,
                                    /*_bs.EnPasSquare,*/ Definitions.NoEnPassantSquare,
                                    Definitions.IndexToAlgebraic[square] +
                                    Definitions.IndexToAlgebraic[toSquare] + "n",
                                    _bs.Wcks, _bs.Wcqs,
                                    _bs.Bcks, _bs.Bcqs/*,
                                    castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                    castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]*//*, square, toSquare*/);
                                ;
                                capMoves.Add(knight);
                            }
                        }
                        break;

                        #endregion

                        #region black knight

                    case Definitions.BlackKnight:
                        var blackKnight = new Knight(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                        capMoves.AddRange(from toSquare in psuedoLegalKnightMoves
                            where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                            select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                            into ply
                            where !_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0])
                            select ply);
                        break;

                        #endregion

                        #region black bishop

                    case Definitions.BlackBishop:
                        var blackBishop = new Bishop(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                        capMoves.AddRange(from toSquare in psuedoLegalBishopMoves
                            where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                            select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                            into ply
                            where !_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0])
                            select ply);
                        break;

                        #endregion

                        #region black rook

                    case Definitions.BlackRook:
                        var blackRook = new Rook(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalRookMoves = blackRook.MoveGeneration();
                        capMoves.AddRange(from toSquare in psuedoLegalRookMoves
                            where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                            select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                            into ply
                            where !_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0])
                            select ply);
                        break;

                        #endregion

                        #region black queen

                    case Definitions.BlackQueen:
                        var blackQueen = new Queen(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                        capMoves.AddRange(from toSquare in psuedoLegalQueenMoves
                            where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                            select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                            into ply
                            where !_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0])
                            select ply);
                        break;

                        #endregion

                        #region black king

                    case Definitions.BlackKing:
                        var blackKing = new King(_bs.BoardRepresentation, square, Definitions.Black);
                        var psuedoLegalKingMoves = blackKing.MoveGeneration();
                        capMoves.AddRange(from toSquare in psuedoLegalKingMoves
                            where _bs.BoardRepresentation[toSquare] != Definitions.EmptySquare
                            select _helper.MakePly(_bs.BoardRepresentation, square, toSquare, _bs.EnPasSquare,
                                _bs.Bcks, _bs.Bcqs, _bs.Wcks, _bs.Wcqs, _bs.SideToMove)
                            into ply
                            where !_helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _bs.KingSquares[0])
                            select ply);
                        break;

                        #endregion

                    default:
                        break;
                }

            #endregion

            return capMoves;
        }

        public ulong Perft(int depth)
        {
            ulong nodes = 0;

            if (depth == 1)
                return (ulong) AllLegalMoves().Count;

            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Count;

            for (var i = 0; i < nMoves; i++)
            {
                MakeMove(legalMoves[i]);
                nodes += Perft(depth - 1);
                UndoMove(legalMoves[i]);

                //Console.WriteLine("Perft: {0}: {1}", legalMoves[i].GetAlgebraicPly(), nodes);
            }

            return nodes;
        }

        public ulong Divide(int depth)
        {
            ulong totalNodes = 0;

            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Count;

            foreach (var legalMove in legalMoves)
            {
                MakeMove(legalMove);

                var childNotes = depth == 1 ? 1 : Perft(depth - 1);
                totalNodes += childNotes;
                UndoMove(legalMove);
                Console.WriteLine("Divide: {0}: {1}", legalMove.GetAlgebraicPly(), childNotes);
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
            
            _bs.Ply++;
            UpdateKingSquares();
            //_bs.Moves.Add(makePly.GetMove());
        }

        public void UndoMove(Ply undoPly)
        {
            _bs.EnPasSquare = _bs.HisEnPas;
            _bs.BoardRepresentation = (int[]) undoPly.GetHisBoard().Clone();
            _bs.SideToMove = !_bs.SideToMove;

            /*_bs.WCKS = undoPly.GetHisWCCKS();
            _bs.WCQS = undoPly.GetHisWCCQS();
            _bs.BCKS = undoPly.GetHisBCCKS();
            _bs.BCQS = undoPly.GetHisBCCQS();*/

            _bs.Wcks = _bs.HisWcks;
            _bs.Wcqs = _bs.HisWcqs;
            _bs.Bcks = _bs.HisWcks;
            _bs.Bcqs = _bs.HisWcqs;

            _bs.Ply--;
            UpdateKingSquares();
            //_bs.Moves.Remove(_bs.Moves.Last());
        }

        private void UpdateKingSquares()
        {
            if (_bs.BoardRepresentation[_bs.KingSquares[0]] != Definitions.BlackKing ||
                _bs.BoardRepresentation[_bs.KingSquares[1]] != Definitions.WhiteKing)
                foreach (var square in _board64)
                    if (_bs.BoardRepresentation[square] == Definitions.BlackKing)
                        _bs.KingSquares[0] = square;
                    else if (_bs.BoardRepresentation[square] == Definitions.WhiteKing)
                        _bs.KingSquares[1] = square;
        }
    }
}