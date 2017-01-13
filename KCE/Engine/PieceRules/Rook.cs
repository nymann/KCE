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

            while (_board[_square - 10 * i] == 0)
            {
                validMoves.Add(_square - 10 * i);
                i++;
            }

            if (_board[_square - 10 * i] > 6 && _board[_square - 10 * i] < 13)
                validMoves.Add(_square - 10 * i);

            i = 1;

            while (_board[_square + 10 * i] == 0)
            {
                validMoves.Add(_square + 10 * i);
                i++;
            }

            if (_board[_square + 10 * i] > 6 && _board[_square + 10 * i] < 13)
                validMoves.Add(_square + 10 * i);

            i = 1;

            while (_board[_square + 1 * i] == 0)
            {
                validMoves.Add(_square + 1 * i);
                i++;
            }

            if (_board[_square + 1 * i] > 6 && _board[_square + 1 * i] < 13)
                validMoves.Add(_square + 1 * i);

            i = 1;

            while (_board[_square - 1 * i] == 0)
            {
                validMoves.Add(_square - 1 * i);
                i++;
            }

            if (_board[_square - 1 * i] > 6 && _board[_square - 1 * i] < 13)
                validMoves.Add(_square - 1 * i);

            return validMoves;
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board[_square - 10 * i] == 0)
            {
                validMoves.Add(_square - 10 * i);
                i++;
            }

            if (_board[_square - 10 * i] < 6)
                validMoves.Add(_square - 10 * i);

            i = 1;

            while (_board[_square + 10 * i] == 0)
            {
                validMoves.Add(_square + 10 * i);
                i++;
            }

            if (_board[_square + 10 * i] < 6)
                validMoves.Add(_square + 10 * i);

            i = 1;

            while (_board[_square + 1 * i] == 0)
            {
                validMoves.Add(_square + 1 * i);
                i++;
            }

            if (_board[_square + 1 * i] < 6)
                validMoves.Add(_square + 1 * i);

            i = 1;

            while (_board[_square - 1 * i] == 0)
            {
                validMoves.Add(_square - 1 * i);
                i++;
            }

            if (_board[_square - 1 * i] < 6)
                validMoves.Add(_square - 1 * i);

            return validMoves;
        }
    }
}