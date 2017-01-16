using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public class Bishop : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;

        public Bishop(int[] board, int square, bool sideToMove)
        {
            _board = board;
            _square = square;
            _sideToMove = sideToMove;
        }

        public int[] MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private int[] WhiteMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxBishopMoves];
            var index = 0;
            var i = 1;

            while (_board[_square - 9 * i] == 0)
            {
                validMoves[index] = _square - 9 * i;
                i++;
                index++;
            }

            if (_board[_square - 9 * i] > 6 && _board[_square - 9 * i] < 13)
            {
                validMoves[index] = _square - 9 * i;
                index++;
            }
            i = 1;

            while (_board[_square + 9 * i] == Definitions.EmptySquare)
            {
                validMoves[index] = _square + 9 * i;
                index++;
                i++;
            }

            if (_board[_square + 9 * i] > 6 && _board[_square + 9 * i] < 13)
            {
                validMoves[index] = (_square + 9 * i);
                index++;
            }
            i = 1;

            while (_board[_square - 11 * i] == Definitions.EmptySquare)
            {
                validMoves[index] = (_square - 11 * i);
                index++;
                i++;
            }

            if (_board[_square - 11 * i] > 6 && _board[_square - 11 * i] < 13)
            {
                validMoves[index] = (_square - 11 * i);
                index++;
            }
            i = 1;

            while (_board[_square + 11 * i] == 0)
            {
                validMoves[index] = (_square + 11 * i);
                index++;
                i++;
            }

            if (_board[_square + 11 * i] > 6 && _board[_square + 11 * i] < 13)
            {
                validMoves[index] = (_square + 11 * i);
                index++;
            }

            var finalArray = new int[index];
            for (var p = 0; p < index; p++)
            {
                finalArray[p] = validMoves[p];
            }

            return finalArray;
        }

        private int[] BlackMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxBishopMoves];

            var i = 1;
            var index = 0;

            while (_board[_square - 9 * i] == 0)
            {
                validMoves[index] = (_square - 9 * i);
                index++;
                i++;
            }

            if (_board[_square - 9 * i] < 6)
            {
                validMoves[index] = (_square - 9 * i);
                index++;
            }
            i = 1;

            while (_board[_square + 9 * i] == 0)
            {
                validMoves[index] = (_square + 9 * i);
                index++;
                i++;
            }

            if (_board[_square + 9 * i] < 6)
            {
                validMoves[index] = (_square + 9 * i);
                index++;
            }
            i = 1;

            while (_board[_square - 11 * i] == 0)
            {
                validMoves[index] = (_square - 11 * i);
                index++;
                i++;
            }

            if (_board[_square - 11 * i] < 6)
            {
                validMoves[index] = (_square - 11 * i);
                index++;
            }

            i = 1;

            while (_board[_square + 11 * i] == 0)
            {
                validMoves[index] = (_square + 11 * i);
                index++;
                i++;
            }

            if (_board[_square + 11 * i] < 6)
            {
                validMoves[index] = (_square + 11 * i);
                index++;
            }
            var finalArray = new int[index];
            for (var p = 0; p < index; p++)
            {
                finalArray[p] = validMoves[p];
            }

            return finalArray;
        }
    }
}