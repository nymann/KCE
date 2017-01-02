using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Bishop : IMoveGeneration
    {
        private readonly Board _board;
        private readonly int _square;

        public Bishop(Board board, int square)
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

        private List<int> WhiteMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board.BoardRepresentation[_square - 9 * i] == 0)
            {
                validMoves.Add(_square - 9 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 9 * i] > 6 && _board.BoardRepresentation[_square - 9 * i] < 13)
            {
                validMoves.Add(_square - 9 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 9 * i] == 0)
            {
                validMoves.Add(_square + 9 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 9 * i] > 6 && _board.BoardRepresentation[_square + 9 * i] < 13)
            {
                validMoves.Add(_square + 9 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square - 11 * i] == 0)
            {
                validMoves.Add(_square - 11 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 11 * i] > 6 && _board.BoardRepresentation[_square - 11 * i] < 13)
            {
                validMoves.Add(_square - 11 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 11 * i] == 0)
            {
                validMoves.Add(_square + 11 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 11 * i] > 6 && _board.BoardRepresentation[_square + 11 * i] < 13)
            {
                validMoves.Add(_square + 11 * i);
            }

            return validMoves;
        }

        private List<int> BlackMoveGeneration()
        {
            var validMoves = new List<int>();

            var i = 1;

            while (_board.BoardRepresentation[_square - 9 * i] == 0)
            {
                validMoves.Add(_square - 9 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 9 * i] < 6)
            {
                validMoves.Add(_square - 9 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 9 * i] == 0)
            {
                validMoves.Add(_square + 9 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 9 * i] < 6)
            {
                validMoves.Add(_square + 9 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square - 11 * i] == 0)
            {
                validMoves.Add(_square - 11 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square - 11 * i] < 6)
            {
                validMoves.Add(_square - 11 * i);
            }

            i = 1;

            while (_board.BoardRepresentation[_square + 11 * i] == 0)
            {
                validMoves.Add(_square + 11 * i);
                i++;
            }

            if (_board.BoardRepresentation[_square + 11 * i] < 6)
            {
                validMoves.Add(_square + 11 * i);
            }

            return validMoves;
        }
    }
}