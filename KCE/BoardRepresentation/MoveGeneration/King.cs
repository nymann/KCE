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

            if (_board.BlackCanCastleKingSide
                && _board.BoardRepresentation[92] == Definitions.EmptySquare // G8
                && _board.BoardRepresentation[93] == Definitions.EmptySquare) // F8
            {
                validMoves.Add(92); // E8G8, 92 = G8.
            }

            if (_board.WhiteCanCastleQueenSide 
                && _board.BoardRepresentation[97] == Definitions.EmptySquare // B8
                && _board.BoardRepresentation[96] == Definitions.EmptySquare // C8
                && _board.BoardRepresentation[95] == Definitions.EmptySquare) // D8
            {
                // add black queenside castling. (e8c8)
                validMoves.Add(96); // 96 = C8.
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

            if (_board.WhiteCanCastleKingSide
                && _board.BoardRepresentation[22] == Definitions.EmptySquare // G1
                && _board.BoardRepresentation[23] == Definitions.EmptySquare) // F1
            {
                // add white kingside castling. (e1g1)
                validMoves.Add(22); // G1 = 22
            }

            if (_board.WhiteCanCastleQueenSide
                && _board.BoardRepresentation[27] == Definitions.EmptySquare // B1
                && _board.BoardRepresentation[26] == Definitions.EmptySquare // C1
                && _board.BoardRepresentation[25] == Definitions.EmptySquare) // D1
            {
                // add white queenside castling. (e1c1)
                validMoves.Add(26); // C1 = 26
            }

            return validMoves;
        }
    }
}