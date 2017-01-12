using System;
using System.Collections.Generic;
using KCE.BoardRepresentation.PieceRules;
using KCE.Engine;

namespace KCE.BoardRepresentation
{
    public class MoveGenerator
    {
        #region variables
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

        #endregion
        public MoveGenerator(BoardState boardState)
        {
            helper = new Helper();
            _boardState = boardState;
        }

        public List<Ply> AllLegalMoves()
        {
            var legalMoves = new List<Ply>();

            #region side to move is in double check
            // if it's blacks or whites turn and he is in double check.
            if (helper.DoubleCheckedFen(_boardState.BoardRepresentation, _boardState.SideToMove,
                _boardState.KingSquares))
            {
                //Console.WriteLine("Check");
                // Player is in check disable all castling.
                var king = new King(_boardState.BoardRepresentation, _boardState.KingSquares, _boardState.SideToMove,
                    false, false,
                    false, false);
                var psuedolegalMoves = king.MoveGeneration();

                var square = 0;
                if (_boardState.SideToMove == Definitions.White)
                    square = 1;

                foreach (var psuedoLegalMove in psuedolegalMoves)
                {
                    var ply = helper.MakePly(_boardState.BoardRepresentation, _boardState.KingSquares[square], psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                    if (!helper.IsSquareAttacked(_boardState.SideToMove, ply.GetBoard(), psuedoLegalMove))
                        legalMoves.Add(ply);
                }
            }

            #endregion

            #region black to move
            else if (_boardState.SideToMove == Definitions.Black)
            {
                foreach (var square in _board64)
                    switch (_boardState.BoardRepresentation[square])
                    {
                            #region black pawn
                        case Definitions.BlackPawn:
                            var blackPawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.Black, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);
                                
                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                    if (!helper.IsPieceOnSecondRank(square))
                                    {
                                        legalMoves.Add(ply);
                                    }
                                    else
                                    {
                                        var castlePerms = helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            _boardState.SideToMove);
                                        var boardQueen = (int[]) _boardState.BoardRepresentation.Clone();
                                        boardQueen[square] = Definitions.EmptySquare;
                                        boardQueen[psuedoLegalMove] = Definitions.BlackQueen;


                                        var queen = new Ply(boardQueen, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "q",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(queen);
                                        
                                        var boardRook = (int[]) boardQueen.Clone();
                                        boardRook[psuedoLegalMove] = Definitions.BlackRook;
                                        var rook = new Ply(boardRook, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "r",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(rook);

                                        var boardbishop = (int[])boardQueen.Clone();
                                        boardbishop[psuedoLegalMove] = Definitions.BlackBishop;
                                        var bishop = new Ply(boardbishop, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "b",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(bishop);

                                        var boardknight = (int[])boardQueen.Clone();
                                        boardknight[psuedoLegalMove] = Definitions.BlackKnight;
                                        var knight = new Ply(boardknight, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "n",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(knight);
                                    }
                            }
                            break;
                            #endregion

                            #region black knight
                        case Definitions.BlackKnight:
                            var blackKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region black bishop
                        case Definitions.BlackBishop:
                            var blackBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region black rook
                        case Definitions.BlackRook:
                            var blackRook = new Rook(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region black queen
                        case Definitions.BlackQueen:
                            var blackQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region black king
                        case Definitions.BlackKing:
                            King blackKing;
                            if (helper.IsSquareAttacked(Definitions.Black, _boardState.BoardRepresentation,
                                _boardState.KingSquares[0]))
                                blackKing = new King(_boardState.BoardRepresentation, square, Definitions.Black,
                                    false, false,
                                    false, false);
                            else
                                blackKing = new King(_boardState.BoardRepresentation, square, Definitions.Black,
                                    _boardState.WCKS, _boardState.WCQS,
                                    _boardState.BCKS, _boardState.BCQS);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();

                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_boardState.BCKS && psuedoLegalMove == 92)
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, 
                                        _boardState.BCKS,_boardState.BCQS,
                                        _boardState.WCKS, _boardState.WCQS,
                                        _boardState.SideToMove, 0);

                                else if (_boardState.BCQS && psuedoLegalMove == 96)
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, 
                                        _boardState.BCKS,_boardState.BCQS,
                                        _boardState.WCKS, _boardState.WCQS,
                                        _boardState.SideToMove, 1);

                                else
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, 
                                        _boardState.BCKS,_boardState.BCQS,
                                        _boardState.WCKS, _boardState.WCQS,
                                        _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), psuedoLegalMove))
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
            else if (_boardState.SideToMove == Definitions.White)
            {
                foreach (var square in _board64)
                    switch (_boardState.BoardRepresentation[square])
                    {
                            #region white pawn
                        case Definitions.WhitePawn:
                            var whitePawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.White, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                var castlePerms = helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                    _boardState.WCKS, _boardState.WCQS, 
                                    _boardState.BCKS, _boardState.BCQS,
                                    _boardState.SideToMove);
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (
                                    !helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                        _boardState.KingSquares[1]))
                                    if (!helper.IsPieceOnSeventhRank(square))
                                    {
                                        legalMoves.Add(ply);
                                    }
                                    else
                                    {
                                        var boardQueen = (int[]) _boardState.BoardRepresentation.Clone();
                                        boardQueen[square] = Definitions.EmptySquare;
                                        boardQueen[psuedoLegalMove] = Definitions.WhiteQueen;
                                        var queen = new Ply(boardQueen, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "Q",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(queen);

                                        var boardRook = (int[]) boardQueen.Clone();
                                        boardRook[psuedoLegalMove] = Definitions.WhiteRook;
                                        var rook = new Ply(boardRook, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "R",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(rook);

                                        var boardbishop = (int[]) boardQueen.Clone();
                                        boardbishop[psuedoLegalMove] = Definitions.WhiteBishop;
                                        var bishop = new Ply(boardbishop, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "B",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(bishop);

                                        var boardknight = (int[]) boardQueen.Clone();
                                        boardknight[psuedoLegalMove] = Definitions.WhiteKnight;
                                        var knight = new Ply(boardknight, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "K",
                                            _boardState.WCKS, _boardState.WCQS,
                                            _boardState.BCKS, _boardState.BCQS,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(knight);
                                    }
                            }
                            break;
                            #endregion

                            #region white knight
                        case Definitions.WhiteKnight:
                            var whiteKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region white bishop
                        case Definitions.WhiteBishop:
                            var whiteBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                    legalMoves.Add(ply);
                            }
                            break;

                            #endregion

                            #region white rook
                        case Definitions.WhiteRook:
                            var whiteRook = new Rook(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region white queen
                        case Definitions.WhiteQueen:
                            var whiteQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                var ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BCKS, _boardState.BCQS,
                                    _boardState.WCKS, _boardState.WCQS, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                    legalMoves.Add(ply);
                            }
                            break;
                            #endregion

                            #region white king
                        case Definitions.WhiteKing:
                            King whiteKing;
                            if (helper.IsSquareAttacked(Definitions.White, _boardState.BoardRepresentation,
                                _boardState.KingSquares[1]))
                                whiteKing = new King(_boardState.BoardRepresentation, square, Definitions.White,
                                    false, false,
                                    false, false);
                            else
                                whiteKing = new King(_boardState.BoardRepresentation, square, Definitions.White,
                                    _boardState.WCKS, _boardState.WCQS,
                                    _boardState.BCKS, _boardState.BCQS);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (var psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply;
                                if (_boardState.WCKS && psuedoLegalMove == 22)
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, 
                                        _boardState.BCKS,_boardState.BCQS,
                                        false, false,
                                        _boardState.SideToMove, 2);

                                else if (_boardState.WCQS && psuedoLegalMove == 26)
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, _boardState.BCKS,
                                        _boardState.BCQS,
                                        false, false,
                                        _boardState.SideToMove, 3);

                                else
                                    ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                        _boardState.EnPasSquare, _boardState.BCKS,
                                        _boardState.BCQS,
                                        false, false,
                                        _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), psuedoLegalMove))
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

                Console.WriteLine("Perft: {0}: {1}", legalMoves[i].GetAlgebraicPly(), nodes);
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
            _boardState.EnPasSquare = makePly.GetEnPas();
            _boardState.BoardRepresentation = (int[]) makePly.GetBoard().Clone();
            _boardState.SideToMove = !_boardState.SideToMove;

            _boardState.WCKS = makePly.GetWCCKS();
            _boardState.WCQS = makePly.GetWCCQS();
            _boardState.BCKS = makePly.GetBCCKS();
            _boardState.BCQS = makePly.GetBCCQS();
            _boardState.Ply++;
            UpdateKingSquares();
        }

        public void UndoMove(Ply undoPly)
        {
            _boardState.EnPasSquare = undoPly.GetHisEnPas();
            _boardState.BoardRepresentation = (int[]) undoPly.GetHisBoard().Clone();
            _boardState.SideToMove = !_boardState.SideToMove;

            _boardState.WCKS = undoPly.GetHisWCCKS();
            _boardState.WCQS = undoPly.GetHisWCCQS();
            _boardState.BCKS = undoPly.GetHisBCCKS();
            _boardState.BCQS = undoPly.GetHisBCCQS();
            _boardState.Ply--;
            UpdateKingSquares();

        }

        private void UpdateKingSquares()
        {
            if (_boardState.BoardRepresentation[_boardState.KingSquares[0]] != Definitions.BlackKing ||
                _boardState.BoardRepresentation[_boardState.KingSquares[1]] != Definitions.WhiteKing)
                foreach (var square in _board64)
                    if (_boardState.BoardRepresentation[square] == Definitions.BlackKing)
                        _boardState.KingSquares[0] = square;
                    else if (_boardState.BoardRepresentation[square] == Definitions.WhiteKing)
                        _boardState.KingSquares[1] = square;
        }
    }
}