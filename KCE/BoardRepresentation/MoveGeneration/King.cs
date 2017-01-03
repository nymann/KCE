using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class King : IMoveGeneration
    {
        private readonly Board _board;
        private readonly int _square;

        public King(Board board, int square)
        {
            _board = board;
            _square = square;
        }

        public List<int> MoveGeneration()
        {
            return _board.SideToMove
                ? WhiteMoveGeneration(_square)
                : BlackMoveGeneration(_square);
        }

        private List<int> BlackMoveGeneration(int square)
        {
            var validMoves = new List<int>();

            if (_board.BoardRepresentation[square + 1] == 0 || _board.BoardRepresentation[square + 1] < 6)
            {
                validMoves.Add(square + 1);
            }

            if (_board.BoardRepresentation[square - 1] == 0 || _board.BoardRepresentation[square - 1] < 6)
            {
                validMoves.Add(square - 1);
            }

            if (_board.BoardRepresentation[square + 10] == 0 || _board.BoardRepresentation[square + 10] < 6)
            {
                validMoves.Add(square + 10);
            }

            if (_board.BoardRepresentation[square - 10] == 0 || _board.BoardRepresentation[square - 10] < 6)
            {
                validMoves.Add(square - 10);
            }


            return validMoves;
        }

        private List<int> WhiteMoveGeneration(int square)
        {
            var validMoves = new List<int>();

            if (_board.BoardRepresentation[square + 1] == 0 ||
                _board.BoardRepresentation[square + 1] > 6 && _board.BoardRepresentation[square + 1] < 13)
            {
                validMoves.Add(square + 1);
            }

            if (_board.BoardRepresentation[square - 1] == 0 ||
                _board.BoardRepresentation[square - 1] > 6 && _board.BoardRepresentation[square - 1] < 13)
            {
                validMoves.Add(square - 1);
            }

            if (_board.BoardRepresentation[square - 10] == 0 ||
                _board.BoardRepresentation[square - 10] > 6 && _board.BoardRepresentation[square - 10] < 13)
            {
                validMoves.Add(square - 10);
            }

            if (_board.BoardRepresentation[square + 10] == 0 ||
                _board.BoardRepresentation[square + 10] > 6 && _board.BoardRepresentation[square + 10] < 13)
            {
                validMoves.Add(square + 10);
            }

            return validMoves;
        }
    }
}