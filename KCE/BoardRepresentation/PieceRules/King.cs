using System.Collections.Generic;

namespace KCE.BoardRepresentation.PieceRules
{
    public class King : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;
        private readonly bool _WCCKS;
        private readonly bool _WCCQS;
        private readonly bool _BCCKS;
        private readonly bool _BCCQS;

        public King(int[] board, int square, bool sideToMove, bool wccks, bool wccqs, bool bccks, bool bccqs)
        {
            _board = board;
            _square = square;
            _sideToMove = sideToMove;
            _WCCKS = wccks;
            _WCCQS = wccqs;
            _BCCKS = bccks;
            _BCCQS = bccqs;
        }

        public List<int> MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration(_square)
                : BlackMoveGeneration(_square);
        }

        private List<int> BlackMoveGeneration(int square)
        {
            Helper helper = new Helper();

            var validMoves = new List<int>();

            if (_board[square + 1] == 0 || _board[square + 1] < 6)
            {
                validMoves.Add(square + 1);
            }

            if (_board[square - 1] == 0 || _board[square - 1] < 6)
            {
                validMoves.Add(square - 1);
            }

            if (_board[square + 9] == 0 || _board[square + 9] < 6)
            {
                validMoves.Add(square + 9);
            }

            if (_board[square - 9] == 0 || _board[square - 9] < 6)
            {
                validMoves.Add(square - 9);
            }

            if (_board[square + 10] == 0 || _board[square + 10] < 6)
            {
                validMoves.Add(square + 10);
            }

            if (_board[square - 10] == 0 || _board[square - 10] < 6)
            {
                validMoves.Add(square - 10);
            }

            if (_board[square - 11] == 0 || _board[square - 11] < 6)
            {
                validMoves.Add(square - 11);
            }

            if (_board[square + 11] == 0 || _board[square + 11] < 6)
            {
                validMoves.Add(square + 11);
            }

            
            if (_BCCKS
                && _board[92] == Definitions.EmptySquare // G8
                && _board[93] == Definitions.EmptySquare // F8
                && !helper.IsSquareAttacked(Definitions.Black, _board, 92) && !helper.IsSquareAttacked(Definitions.Black, _board, 93))
            {
                validMoves.Add(92); // E8G8, 92 = G8.
            }

            if (_BCCQS
                && _board[97] == Definitions.EmptySquare // B8
                && _board[96] == Definitions.EmptySquare // C8
                && _board[95] == Definitions.EmptySquare // D8
                && !helper.IsSquareAttacked(Definitions.Black, _board, 96) && !helper.IsSquareAttacked(Definitions.Black, _board, 95))
            {
                // add black queenside castling. (e8c8)
                validMoves.Add(96); // 96 = C8.
            }
            

            return validMoves;
        }

        private List<int> WhiteMoveGeneration(int square)
        {
            Helper helper = new Helper();

            var validMoves = new List<int>();

            if (_board[square + 1] == 0 ||
                _board[square + 1] > 6 && _board[square + 1] < 13)
            {
                validMoves.Add(square + 1);
            }

            if (_board[square - 1] == 0 ||
                _board[square - 1] > 6 && _board[square - 1] < 13)
            {
                validMoves.Add(square - 1);
            }

            if (_board[square + 9] == 0 ||
                _board[square + 9] > 6 && _board[square + 9] < 93)
            {
                validMoves.Add(square + 9);
            }

            if (_board[square - 9] == 0 ||
                _board[square - 9] > 6 && _board[square - 9] < 93)
            {
                validMoves.Add(square - 9);
            }

            if (_board[square - 10] == 0 ||
                _board[square - 10] > 6 && _board[square - 10] < 13)
            {
                validMoves.Add(square - 10);
            }

            if (_board[square + 10] == 0 ||
                _board[square + 10] > 6 && _board[square + 10] < 13)
            {
                validMoves.Add(square + 10);
            }

            if (_board[square + 11] == 0 ||
                _board[square + 11] > 6 && _board[square + 11] < 13)
            {
                validMoves.Add(square + 11);
            }

            if (_board[square - 11] == 0 ||
                _board[square - 11] > 6 && _board[square - 11] < 13)
            {
                validMoves.Add(square - 11);
            }

            if (_WCCKS
                && _board[22] == Definitions.EmptySquare // G1
                && _board[23] == Definitions.EmptySquare // F1
                && !helper.IsSquareAttacked(Definitions.White, _board, 23) && !helper.IsSquareAttacked(Definitions.White, _board, 22))
            {
                // add white kingside castling. (e1g1)
                validMoves.Add(22); // G1 = 22
            }

            if (_WCCQS
                && _board[27] == Definitions.EmptySquare // B1
                && _board[26] == Definitions.EmptySquare // C1
                && _board[25] == Definitions.EmptySquare // D1
                && !helper.IsSquareAttacked(Definitions.White, _board, 26) && !helper.IsSquareAttacked(Definitions.White, _board, 25))
            {
                // add white queenside castling. (e1c1)
                validMoves.Add(26); // C1 = 26
            }

            return validMoves;
        }
    }
}