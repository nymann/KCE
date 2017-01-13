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

        public List<int> MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board[_square - 9 * i] == 0)
            {
                validMoves.Add(_square - 9 * i);
                i++;
            }

            if (_board[_square - 9 * i] > 6 && _board[_square - 9 * i] < 13)
                validMoves.Add(_square - 9 * i);

            i = 1;

            while (_board[_square + 9 * i] == 0)
            {
                validMoves.Add(_square + 9 * i);
                i++;
            }

            if (_board[_square + 9 * i] > 6 && _board[_square + 9 * i] < 13)
                validMoves.Add(_square + 9 * i);

            i = 1;

            while (_board[_square - 11 * i] == 0)
            {
                validMoves.Add(_square - 11 * i);
                i++;
            }

            if (_board[_square - 11 * i] > 6 && _board[_square - 11 * i] < 13)
                validMoves.Add(_square - 11 * i);

            i = 1;

            while (_board[_square + 11 * i] == 0)
            {
                validMoves.Add(_square + 11 * i);
                i++;
            }

            if (_board[_square + 11 * i] > 6 && _board[_square + 11 * i] < 13)
                validMoves.Add(_square + 11 * i);

            return validMoves;
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board[_square - 9 * i] == 0)
            {
                validMoves.Add(_square - 9 * i);
                i++;
            }

            if (_board[_square - 9 * i] < 6)
                validMoves.Add(_square - 9 * i);

            i = 1;

            while (_board[_square + 9 * i] == 0)
            {
                validMoves.Add(_square + 9 * i);
                i++;
            }

            if (_board[_square + 9 * i] < 6)
                validMoves.Add(_square + 9 * i);

            i = 1;

            while (_board[_square - 11 * i] == 0)
            {
                validMoves.Add(_square - 11 * i);
                i++;
            }

            if (_board[_square - 11 * i] < 6)
                validMoves.Add(_square - 11 * i);

            i = 1;

            while (_board[_square + 11 * i] == 0)
            {
                validMoves.Add(_square + 11 * i);
                i++;
            }

            if (_board[_square + 11 * i] < 6)
                validMoves.Add(_square + 11 * i);

            return validMoves;
        }
    }
}