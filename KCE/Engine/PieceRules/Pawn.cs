using System;
using System.Collections.Generic;
using KCE.Engine;

namespace KCE.BoardRepresentation.PieceRules
{
    public class Pawn : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;
        private readonly int _enPasSquare;
        private Helper helper;

        public Pawn(int[] board, int square, bool sideToMove, int enPasSquare)
        {
            helper = new Helper();
            _square = square;
            _board = board;
            _sideToMove = sideToMove;
            _enPasSquare = enPasSquare;
        }

        public List<int> MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            if (PawnIsOnSeventhRank() && _board[_square - 10] == Definitions.EmptySquare &&
                _board[_square - 20] == Definitions.EmptySquare)
            {
                validMoves.Add(_square - 20);
            }

            if (_board[_square - 10] == Definitions.EmptySquare)
            {
                /*if (!PawnIsOnSecondRank())
                {
                    validMoves.Add(_square - 10);
                }

                // promotion
                // set flag?*/

                validMoves.Add(_square - 10);
            }

            #region attacking moves.
            if (_board[_square - 11] != 0 && _board[_square - 11] < 7)
            {
                validMoves.Add(_square - 11);
            }

            if (_board[_square - 9] != 0 && _board[_square - 9] < 7)
            {
                validMoves.Add(_square - 9);
            }

            if (_enPasSquare == _square - 11)
            {
                validMoves.Add(_square - 11);
            }

            if (_enPasSquare == _square - 9)
            {
                validMoves.Add(_square - 9);
            }
            #endregion

            return validMoves;
        }

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            if (PawnIsOnSecondRank() && _board[_square + 10] == Definitions.EmptySquare &&
                _board[_square + 20] == Definitions.EmptySquare)
            {
                validMoves.Add(_square + 20);
            }

            if (_board[_square + 10] == 0)
            {
                /*if (!PawnIsOnSeventhRank())
                {
                    validMoves.Add(_square + 10);
                }
                else
                {
                    // promotion
                }*/

                validMoves.Add(_square + 10);
            }


            #region attacking moves.
            if (_board[_square + 11] > 6 && _board[_square + 11] < 13)
            {
                validMoves.Add(_square + 11);
            }

            if (_board[_square + 9] > 6 && _board[_square + 9] < 13)
            {
                validMoves.Add(_square + 9);
            }

            if (_enPasSquare == _square + 11)
            {
                validMoves.Add(_square + 11);
            }

            if (_enPasSquare == _square + 9)
            {
                validMoves.Add(_square + 9);
            }
            #endregion

            return validMoves;
        }

        private bool PawnIsOnSeventhRank()
        {
            return _square/10 == 8;
        }

        private bool PawnIsOnSecondRank()
        {
            return _square/10 == 3;
        }
    }
}