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

        public List<int> MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_board[_square - 21] == Definitions.EmptySquare || _board[_square - 21] < 6)
                validMoves.Add(_square - 21);

            if (_board[_square - 19] == Definitions.EmptySquare || _board[_square - 19] < 6)
                validMoves.Add(_square - 19);

            if (_board[_square - 12] == Definitions.EmptySquare || _board[_square - 12] < 6)
                validMoves.Add(_square - 12);

            if (_board[_square - 8] == Definitions.EmptySquare || _board[_square - 8] < 6)
                validMoves.Add(_square - 8);

            if (_board[_square + 8] == Definitions.EmptySquare || _board[_square + 8] < 6)
                validMoves.Add(_square + 8);

            if (_board[_square + 12] == Definitions.EmptySquare || _board[_square + 12] < 6)
                validMoves.Add(_square + 12);

            if (_board[_square + 19] == Definitions.EmptySquare || _board[_square + 19] < 6)
                validMoves.Add(_square + 19);

            if (_board[_square + 21] == Definitions.EmptySquare || _board[_square + 21] < 6)
                validMoves.Add(_square + 21);

            return validMoves;
        }

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_board[_square - 21] == 0 ||
                _board[_square - 21] > 6 && _board[_square - 21] < 13)
                validMoves.Add(_square - 21);

            if (_board[_square - 19] == 0 ||
                _board[_square - 19] > 6 && _board[_square - 19] < 13)
                validMoves.Add(_square - 19);

            if (_board[_square - 12] == 0 ||
                _board[_square - 12] > 6 && _board[_square - 12] < 13)
                validMoves.Add(_square - 12);

            if (_board[_square - 8] == 0 ||
                _board[_square - 8] > 6 && _board[_square - 8] < 13)
                validMoves.Add(_square - 8);

            if (_board[_square + 21] == 0 ||
                _board[_square + 21] > 6 && _board[_square + 21] < 13)
                validMoves.Add(_square + 21);

            if (_board[_square + 19] == 0 ||
                _board[_square + 19] > 6 && _board[_square + 19] < 13)
                validMoves.Add(_square + 19);

            if (_board[_square + 12] == 0 ||
                _board[_square + 12] > 6 && _board[_square + 12] < 13)
                validMoves.Add(_square + 12);

            if (_board[_square + 8] == 0 ||
                _board[_square + 8] > 6 && _board[_square + 8] < 13)
                validMoves.Add(_square + 8);

            return validMoves;
        }
    }
}