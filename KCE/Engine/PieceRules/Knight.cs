using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public class Knight : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;

        public Knight(int[] board, int square, bool sideToMove)
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

        private int[] BlackMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxKnightMoves];
            var index = 0;

            if (_board[_square - 21] == Definitions.EmptySquare || _board[_square - 21] < 6)
            {
                validMoves[index] = _square - 21;
                index++;
            }

            if (_board[_square - 19] == Definitions.EmptySquare || _board[_square - 19] < 6)
            {
                validMoves[index] = _square - 19;
                index++;
            }

            if (_board[_square - 12] == Definitions.EmptySquare || _board[_square - 12] < 6)
            {
                validMoves[index] = _square - 12;
                index++;
            }

            if (_board[_square - 8] == Definitions.EmptySquare || _board[_square - 8] < 6)
            {
                validMoves[index] = _square - 8;
                index++;
            }

            if (_board[_square + 8] == Definitions.EmptySquare || _board[_square + 8] < 6)
            {
                validMoves[index] = _square + 8;
                index++;
            }

            if (_board[_square + 12] == Definitions.EmptySquare || _board[_square + 12] < 6)
            {
                validMoves[index] = _square + 12;
                index++;
            }

            if (_board[_square + 19] == Definitions.EmptySquare || _board[_square + 19] < 6)
            {
                validMoves[index] = _square + 19;
                index++;
            }

            if (_board[_square + 21] == Definitions.EmptySquare || _board[_square + 21] < 6)
            {
                validMoves[index] = _square + 21;
                index++;
            }

            var finalArray = new int[index];
            for (var p = 0; p < index; p++)
            {
                finalArray[p] = validMoves[p];
            }

            return finalArray;
        }

        private int[] WhiteMoveGeneration()
        {
            var validMoves = new int[Definitions.MaxKnightMoves];
            var index = 0;

            if (_board[_square - 21] == 0 ||
                _board[_square - 21] > 6 && _board[_square - 21] < 13)
            {
                validMoves[index] = _square - 21;
                index++;
            }

            if (_board[_square - 19] == 0 ||
                _board[_square - 19] > 6 && _board[_square - 19] < 13)
            {
                validMoves[index] = _square - 19;
                index++;
            }

            if (_board[_square - 12] == 0 ||
                _board[_square - 12] > 6 && _board[_square - 12] < 13)
            {
                validMoves[index] = _square - 12;
                index++;
            }

            if (_board[_square - 8] == 0 ||
                _board[_square - 8] > 6 && _board[_square - 8] < 13)
            {
                validMoves[index] = _square - 8;
                index++;
            }

            if (_board[_square + 21] == 0 ||
                _board[_square + 21] > 6 && _board[_square + 21] < 13)
            {
                validMoves[index] = _square + 21;
                index++;
            }

            if (_board[_square + 19] == 0 ||
                _board[_square + 19] > 6 && _board[_square + 19] < 13)
            {
                validMoves[index] = _square + 19;
                index++;
            }

            if (_board[_square + 12] == 0 ||
                _board[_square + 12] > 6 && _board[_square + 12] < 13)
            {
                validMoves[index] = _square + 12;
                index++;
            }

            if (_board[_square + 8] == 0 ||
                _board[_square + 8] > 6 && _board[_square + 8] < 13)
            {
                validMoves[index] = _square + 8;
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