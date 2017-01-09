using System;
using System.Collections.Generic;
using KCE.BoardRepresentation.PieceRules;

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
            List<Ply> legalMoves = new List<Ply>();

            #region side to move is in double check
            // if it's blacks or whites turn and he is in double check.
            if (helper.DoubleCheckedFEN(_boardState.BoardRepresentation, _boardState.SideToMove,
                _boardState.KingSquares))
            {
                King king = new King(_boardState.BoardRepresentation, _boardState.KingSquares, _boardState.SideToMove,
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
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                    if (!helper.IsSquareAttacked(_boardState.SideToMove, ply.GetBoard(), psuedoLegalMove))
                    {
                        legalMoves.Add(ply);
                    }
                }
            }

            #endregion

            #region black to move
            else if (_boardState.SideToMove == Definitions.Black)
            {
                foreach (int square in _board64)
                {
                    // Loop through the BoardState, find blackpieces, and generate moves for them.
                    switch (_boardState.BoardRepresentation[square])
                    {
                        #region black pawn
                        case Definitions.BlackPawn:
                            Pawn blackPawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.Black, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = blackPawn.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);
                                
                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    if (!helper.IsPieceOnSecondRank(square))
                                    {
                                        legalMoves.Add(ply);
                                    }
                                    else
                                    {
                                        bool[] castlePerms = helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            _boardState.SideToMove);
                                        int[] boardQueen = (int[]) _boardState.BoardRepresentation.Clone();
                                        boardQueen[square] = Definitions.EmptySquare;
                                        boardQueen[psuedoLegalMove] = Definitions.BlackQueen;


                                        Ply queen = new Ply(boardQueen, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "q",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(queen);
                                        
                                        int[] boardRook = (int[]) boardQueen.Clone();
                                        boardRook[psuedoLegalMove] = Definitions.BlackRook;
                                        Ply rook = new Ply(boardRook, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "r",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(rook);

                                        int[] boardbishop = (int[])boardQueen.Clone();
                                        boardbishop[psuedoLegalMove] = Definitions.BlackBishop;
                                        Ply bishop = new Ply(boardbishop, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "b",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(bishop);

                                        int[] boardknight = (int[])boardQueen.Clone();
                                        boardknight[psuedoLegalMove] = Definitions.BlackKnight;
                                        Ply knight = new Ply(boardknight, _boardState.BoardRepresentation, _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] + Definitions.IndexToAlgebraic[psuedoLegalMove] + "n",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(knight);
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region black knight
                        case Definitions.BlackKnight:
                            Knight blackKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalKnightMoves = blackKnight.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region black bishop
                        case Definitions.BlackBishop:
                            Bishop blackBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalBishopMoves = blackBishop.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region black rook
                        case Definitions.BlackRook:
                            Rook blackRook = new Rook(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalRookMoves = blackRook.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region black queen
                        case Definitions.BlackQueen:
                            Queen blackQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.Black);
                            var psuedoLegalQueenMoves = blackQueen.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), _boardState.KingSquares[0]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region black king
                        case Definitions.BlackKing:
                            King blackKing = new King(_boardState.BoardRepresentation, square, Definitions.Black,
                                _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide);
                            var psuedoLegalKingMoves = blackKing.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.Black, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion
                        default:
                            break;
                    }
                }
            }
            #endregion

            #region white to move
            else if (_boardState.SideToMove == Definitions.White)
            {
                foreach (int square in _board64)
                {
                    // Loop through the BoardState, find whitepieces, and generate moves for them.
                    switch (_boardState.BoardRepresentation[square])
                    {
                        #region white pawn
                        case Definitions.WhitePawn:
                            Pawn whitePawn = new Pawn(_boardState.BoardRepresentation, square, Definitions.White, _boardState.EnPasSquare);
                            var psuedoLegalPawnMoves = whitePawn.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalPawnMoves)
                            {
                                bool[] castlePerms = helper.UpdateCastlePermissions(square, psuedoLegalMove,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, 
                                    _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.SideToMove);
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (
                                    !helper.IsSquareAttacked(Definitions.White, ply.GetBoard(),
                                        _boardState.KingSquares[1]))
                                {
                                    if (!helper.IsPieceOnSeventhRank(square))
                                    {
                                        legalMoves.Add(ply);
                                    }
                                    else
                                    {
                                        int[] boardQueen = (int[]) _boardState.BoardRepresentation.Clone();
                                        boardQueen[square] = Definitions.EmptySquare;
                                        boardQueen[psuedoLegalMove] = Definitions.WhiteQueen;
                                        Ply queen = new Ply(boardQueen, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "Q",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]);
                                        legalMoves.Add(queen);

                                        int[] boardRook = (int[]) boardQueen.Clone();
                                        boardRook[psuedoLegalMove] = Definitions.WhiteRook;
                                        Ply rook = new Ply(boardRook, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "R",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(rook);

                                        int[] boardbishop = (int[]) boardQueen.Clone();
                                        boardbishop[psuedoLegalMove] = Definitions.WhiteBishop;
                                        Ply bishop = new Ply(boardbishop, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "B",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(bishop);

                                        int[] boardknight = (int[]) boardQueen.Clone();
                                        boardknight[psuedoLegalMove] = Definitions.WhiteKnight;
                                        Ply knight = new Ply(boardknight, _boardState.BoardRepresentation,
                                            _boardState.EnPasSquare, Definitions.NoEnPassantSquare,
                                            Definitions.IndexToAlgebraic[square] +
                                            Definitions.IndexToAlgebraic[psuedoLegalMove] + "K",
                                            _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                            _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                            castlePerms[Definitions.BCCKS], castlePerms[Definitions.BCCQS],
                                            castlePerms[Definitions.WCCKS], castlePerms[Definitions.WCCQS]); ;
                                        legalMoves.Add(knight);
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region white knight
                        case Definitions.WhiteKnight:
                            Knight whiteKnight = new Knight(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalKnightMoves = whiteKnight.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKnightMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region white bishop
                        case Definitions.WhiteBishop:
                            Bishop whiteBishop = new Bishop(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalBishopMoves = whiteBishop.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalBishopMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;

                        #endregion

                        #region white rook
                        case Definitions.WhiteRook:
                            Rook whiteRook = new Rook(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalRookMoves = whiteRook.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalRookMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region white queen
                        case Definitions.WhiteQueen:
                            Queen whiteQueen = new Queen(_boardState.BoardRepresentation, square, Definitions.White);
                            var psuedoLegalQueenMoves = whiteQueen.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalQueenMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), _boardState.KingSquares[1]))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        #region white king
                        case Definitions.WhiteKing:
                            King whiteKing = new King(_boardState.BoardRepresentation, square, Definitions.White,
                                _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide,
                                _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide);
                            var psuedoLegalKingMoves = whiteKing.MoveGeneration();
                            foreach (int psuedoLegalMove in psuedoLegalKingMoves)
                            {
                                Ply ply = helper.MakePly(_boardState.BoardRepresentation, square, psuedoLegalMove,
                                    _boardState.EnPasSquare, _boardState.BlackCanCastleKingSide, _boardState.BlackCanCastleQueenSide,
                                    _boardState.WhiteCanCastleKingSide, _boardState.WhiteCanCastleQueenSide, _boardState.SideToMove);

                                if (!helper.IsSquareAttacked(Definitions.White, ply.GetBoard(), psuedoLegalMove))
                                {
                                    legalMoves.Add(ply);
                                }
                            }
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            #endregion

            return legalMoves;
        }

        public UInt64 Perft(int depth)
        {
            UInt64 nodes = 0;

            if (depth == 1)
            {
                return (ulong) AllLegalMoves().Count;
            }

            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Count;

            for (var i = 0; i < nMoves; i++)
            {
                MakeMove(legalMoves[i]);
                nodes += Perft(depth - 1);
                UndoMove(legalMoves[i]);
            }

            return nodes;
        }

        public UInt64 Divide(int depth)
        {
            UInt64 nodes = 0;
            UInt64 totalNodes = 0;
           
            var legalMoves = AllLegalMoves();
            var nMoves = legalMoves.Count;

            

            foreach (var legalMove in legalMoves)
            {
                MakeMove(legalMove);
                ulong childNotes;

                if (depth == 1)
                {
                    childNotes = (ulong) nMoves;
                }
                else
                {
                    childNotes = Perft(depth - 1);
                }
                totalNodes += childNotes;
                UndoMove(legalMove);
                Console.WriteLine("{0}: {1}", legalMove.GetAlgebraicPly(), childNotes);
            }

            Console.WriteLine("\nTotal moves: {0}, Total nodes: {1}", nMoves, totalNodes);

            return totalNodes;
        }

        void MakeMove(Ply makePly)
        {
            _boardState.EnPasSquare = makePly.GetEnPas();
            _boardState.BoardRepresentation = (int[]) makePly.GetBoard().Clone();
            _boardState.SideToMove = !_boardState.SideToMove;

            _boardState.WhiteCanCastleKingSide = makePly.GetWCCKS();
            _boardState.WhiteCanCastleQueenSide = makePly.GetWCCQS();
            _boardState.BlackCanCastleKingSide = makePly.GetBCCKS();
            _boardState.BlackCanCastleQueenSide = makePly.GetBCCQS();
        }

        void UndoMove(Ply undoPly)
        {
            _boardState.EnPasSquare = undoPly.GetHisEnPas();
            _boardState.BoardRepresentation = (int[]) undoPly.GetHisBoard().Clone();
            _boardState.SideToMove = !_boardState.SideToMove;
            _boardState.WhiteCanCastleKingSide = undoPly.GetHisWCCKS();
            _boardState.WhiteCanCastleQueenSide = undoPly.GetHisWCCQS();
            _boardState.BlackCanCastleKingSide = undoPly.GetHisBCCKS();
            _boardState.BlackCanCastleQueenSide = undoPly.GetHisBCCQS();
        }
    }
}