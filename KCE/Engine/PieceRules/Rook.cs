using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public class Rook : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;

        public Rook(int[] board, int square, bool sideToMove)
        {
            _square = square;
            _board = board;
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
            var validMoves = new int[Definitions.MaxRookMoves];

            var i = 1;
            var index = 0;

            while (_board[_square - 10 * i] == 0)
            {
                validMoves[index] = _square - 10 * i;
                index++;
                i++;
            }

            if (_board[_square - 10 * i] > 6 && _board[_square - 10 * i] < 13)
            {
                validMoves[index] = _square - 10 * i;
                index++;
            }

            i = 1;

            while (_board[_square + 10 * i] == 0)
            {
                validMoves[index] = _square + 10 * i;
                index++;
                i++;
            }

            if (_board[_square + 10 * i] > 6 && _board[_square + 10 * i] < 13)
            {
                validMoves[index] = _square + 10 * i;
                index++;
            }

            i = 1;

            while (_board[_square + 1 * i] == 0)
            {
                validMoves[index] = (_square + 1 * i);
                index++;
                i++;
            }

            if (_board[_square + 1 * i] > 6 && _board[_square + 1 * i] < 13)
            {
                validMoves[index] = (_square + 1 * i);
                index++;
            }

            i = 1;

            while (_board[_square - 1 * i] == 0)
            {
                validMoves[index] = (_square - 1 * i);
                index++;
                i++;
            }

            if (_board[_square - 1 * i] > 6 && _board[_square - 1 * i] < 13)
            {
                validMoves[index] = (_square - 1 * i);
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
            var validMoves = new int[Definitions.MaxRookMoves];
            var index = 0;
            var i = 1;

            while (_board[_square - 10 * i] == 0)
            {
                validMoves[index] = (_square - 10 * i);
                index++;
                i++;
            }

            if (_board[_square - 10 * i] < 6)
            {
                validMoves[index] = (_square - 10 * i);
                index++;
            }

            i = 1;

            while (_board[_square + 10 * i] == 0)
            {
                validMoves[index] = (_square + 10 * i);
                index++;
                i++;
            }

            if (_board[_square + 10 * i] < 6)
            {
                validMoves[index] = (_square + 10 * i);
                index++;
            }

            i = 1;

            while (_board[_square + 1 * i] == 0)
            {
                validMoves[index] = (_square + 1 * i);
                index++;
                i++;
            }

            if (_board[_square + 1 * i] < 6)
            {
                validMoves[index] = (_square + 1 * i);
                index++;
            }

            i = 1;

            while (_board[_square - 1 * i] == 0)
            {
                validMoves[index] = (_square - 1 * i);
                index++;
                i++;
            }

            if (_board[_square - 1 * i] < 6)
            {
                validMoves[index] = (_square - 1 * i);
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