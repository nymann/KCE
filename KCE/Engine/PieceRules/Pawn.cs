using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public class Pawn : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;
        private readonly int _enPasSquare;

        public Pawn(int[] board, int square, bool sideToMove, int enPasSquare)
        {
            _square = square;
            _board = board;
            _sideToMove = sideToMove;
            _enPasSquare = enPasSquare;
        }

        public int[] MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private int[] BlackMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxPawnMoves];
            var index = 0;

            if (PawnIsOnSeventhRank() && _board[_square - 10] == Definitions.EmptySquare &&
                _board[_square - 20] == Definitions.EmptySquare)
            {
                validMoves[index] = _square - 20;
                index++;
            }

            if (_board[_square - 10] == Definitions.EmptySquare)
            {
                validMoves[index] = _square - 10;
                index++;
            }

            #region attacking moves.

            if (_board[_square - 11] != Definitions.EmptySquare && _board[_square - 11] < 7)
            {
                validMoves[index] = _square - 11;
                index++;
            }

            if (_board[_square - 9] != Definitions.EmptySquare && _board[_square - 9] < 7)
            {
                validMoves[index] = _square - 9;
                index++;
            }

            if (_enPasSquare == _square - 11)
            {
                validMoves[index] = _square - 11;
                index++;
            }

            if (_enPasSquare == _square - 9)
            {
                validMoves[index] = _square - 9;
                index++;
            }

            #endregion

            var finalArray = new int[index];
            for (var p = 0; p < index; p++)
            {
                finalArray[p] = validMoves[p];
            }

            return finalArray;
        }

        private int[] WhiteMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxPawnMoves];
            var index = 0;

            if (PawnIsOnSecondRank() && _board[_square + 10] == Definitions.EmptySquare &&
                _board[_square + 20] == Definitions.EmptySquare)
            {
                validMoves[index] = _square + 20;
                index++;
            }

            if (_board[_square + 10] == 0)
            {
                validMoves[index] = _square + 10;
                index++;
            }

            #region attacking moves.

            if (_board[_square + 11] > 6 && _board[_square + 11] < 13)
            {
                validMoves[index] = _square + 11;
                index++;
            }

            if (_board[_square + 9] > 6 && _board[_square + 9] < 13)
            {
                validMoves[index] = _square + 9;
                index++;
            }

            if (_enPasSquare == _square + 11)
            {
                validMoves[index] = _square + 11;
                index++;
            }

            if (_enPasSquare == _square + 9)
            {
                validMoves[index] = _square + 9;
                index++;
            }

            #endregion

            var finalArray = new int[index];
            for (var p = 0; p < index; p++)
            {
                finalArray[p] = validMoves[p];
            }

            return finalArray;
        }

        private bool PawnIsOnSeventhRank()
        {
            return _square / 10 == 8;
        }

        private bool PawnIsOnSecondRank()
        {
            return _square / 10 == 3;
        }
    }
}