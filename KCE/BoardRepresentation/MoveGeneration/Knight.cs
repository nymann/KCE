using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Knight : IMoveGeneration
    {
        private readonly Board _board;
        private readonly int _square;

        public Knight(Board board, int square)
        {
            _board = board;
            _square = square;
        }

        public List<int> MoveGeneration()
        {
            return _board.SideToMove
                ? WhiteMoveGeneration()
                : BlackMoveGeneration();
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_board.BoardRepresentation[_square - 21] == 0 || _board.BoardRepresentation[_square - 21] < 6)
            {
                validMoves.Add(_square - 21);
            }

            if (_board.BoardRepresentation[_square - 19] == 0 || _board.BoardRepresentation[_square - 19] < 6)
            {
                validMoves.Add(_square - 19);
            }

            if (_board.BoardRepresentation[_square - 12] == 0 || _board.BoardRepresentation[_square - 12] < 6)
            {
                validMoves.Add(_square - 12);
            }

            if (_board.BoardRepresentation[_square - 8] == 0 || _board.BoardRepresentation[_square - 8] < 6)
            {
                validMoves.Add(_square - 8);
            }

            if (_board.BoardRepresentation[_square + 8] == 0 || _board.BoardRepresentation[_square + 8] < 6)
            {
                validMoves.Add(_square + 8);
            }

            if (_board.BoardRepresentation[_square + 12] == 0 || _board.BoardRepresentation[_square + 12] < 6)
            {
                validMoves.Add(_square + 12);
            }

            if (_board.BoardRepresentation[_square + 19] == 0 || _board.BoardRepresentation[_square + 19] < 6)
            {
                validMoves.Add(_square + 19);
            }

            if (_board.BoardRepresentation[_square + 21] == 0 || _board.BoardRepresentation[_square + 21] < 6)
            {
                validMoves.Add(_square + 21);
            }

            return validMoves;
        }

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            if (_board.BoardRepresentation[_square - 21] == 0 ||
                _board.BoardRepresentation[_square - 21] > 6 && _board.BoardRepresentation[_square - 21] < 13)
            {
                validMoves.Add(_square - 21);
            }

            if (_board.BoardRepresentation[_square - 19] == 0 ||
                _board.BoardRepresentation[_square - 19] > 6 && _board.BoardRepresentation[_square - 19] < 13)
            {
                validMoves.Add(_square - 19);
            }

            if (_board.BoardRepresentation[_square - 12] == 0 ||
                _board.BoardRepresentation[_square - 12] > 6 && _board.BoardRepresentation[_square - 12] < 13)
            {
                validMoves.Add(_square - 12);
            }

            if (_board.BoardRepresentation[_square - 8] == 0 ||
                _board.BoardRepresentation[_square - 8] > 6 && _board.BoardRepresentation[_square - 8] < 13)
            {
                validMoves.Add(_square - 8);
            }

            if (_board.BoardRepresentation[_square + 21] == 0 ||
                _board.BoardRepresentation[_square + 21] > 6 && _board.BoardRepresentation[_square + 21] < 13)
            {
                validMoves.Add(_square + 21);
            }

            if (_board.BoardRepresentation[_square + 19] == 0 ||
                _board.BoardRepresentation[_square + 19] > 6 && _board.BoardRepresentation[_square + 19] < 13)
            {
                validMoves.Add(_square + 19);
            }

            if (_board.BoardRepresentation[_square + 12] == 0 ||
                _board.BoardRepresentation[_square + 12] > 6 && _board.BoardRepresentation[_square + 12] < 13)
            {
                validMoves.Add(_square + 12);
            }

            if (_board.BoardRepresentation[_square + 8] == 0 ||
                _board.BoardRepresentation[_square + 8] > 6 && _board.BoardRepresentation[_square + 8] < 13)
            {
                validMoves.Add(_square + 8);
            }

            return validMoves;
        }
    }
}