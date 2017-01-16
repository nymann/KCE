using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public class King : IMoveGeneration
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;
        private readonly bool _wccks;
        private readonly bool _wccqs;
        private readonly bool _bccks;
        private readonly bool _bccqs;

        public King(int[] board, int square, bool sideToMove, bool wccks, bool wccqs, bool bccks, bool bccqs)
        {
            _board = board;
            _square = square;
            _sideToMove = sideToMove;
            _wccks = wccks;
            _wccqs = wccqs;
            _bccks = bccks;
            _bccqs = bccqs;
        }

        public King(int[] board, int[] kingSquares, bool sideToMove, bool wccks, bool wccqs, bool bccks, bool bccqs)
        {
            _board = board;
            _sideToMove = sideToMove;
            _wccks = wccks;
            _wccqs = wccqs;
            _bccks = bccks;
            _bccqs = bccqs;

            _square = sideToMove == Definitions.White ? kingSquares[1] : kingSquares[0];
        }

        public King(int[] board, int square, bool sideToMove)
        {
            _board = board;
            _square = square;
            _sideToMove = sideToMove;
            _wccks = false;
            _wccqs = false;
            _bccks = false;
            _bccqs = false;
        }

        public int[] MoveGeneration()
        {
            return _sideToMove
                ? WhiteMoveGeneration(_square)
                : BlackMoveGeneration(_square);
        }

        private int[] BlackMoveGeneration(int square)
        {
            var helper = new Helper();

            var validMoves = new int[Definitions.MaxKingMoves];
            var index = 0;

            if (_board[square + 1] == Definitions.EmptySquare || _board[square + 1] <= Definitions.WhiteKing)
            {
                validMoves[index] = square + 1;
                index++;
            }

            if (_board[square - 1] == Definitions.EmptySquare || _board[square - 1] <= Definitions.WhiteKing)
            {
                validMoves[index] = square - 1;
                index++;
            }
            if (_board[square + 9] == Definitions.EmptySquare || _board[square + 9] <= Definitions.WhiteKing)
            {
                validMoves[index] = square + 9;
                index++;
            }

            if (_board[square - 9] == Definitions.EmptySquare || _board[square - 9] <= Definitions.WhiteKing)
            {
                validMoves[index] = square - 9;
                index++;
            }

            if (_board[square + 10] == Definitions.EmptySquare || _board[square + 10] <= Definitions.WhiteKing)
            {
                validMoves[index] = square + 10;
                index++;
            }

            if (_board[square - 10] == Definitions.EmptySquare || _board[square - 10] <= Definitions.WhiteKing) {
                validMoves[index] = square - 10;
                index++;
            }

            if (_board[square - 11] == Definitions.EmptySquare || _board[square - 11] <= Definitions.WhiteKing)
            {
                validMoves[index] = square - 11;
                index++;
            }

            if (_board[square + 11] == Definitions.EmptySquare || _board[square + 11] <= Definitions.WhiteKing)
            {
                validMoves[index] = square + 11;
                index++;
            }


            if (_bccks
                && _board[92] == Definitions.EmptySquare // G8
                && _board[93] == Definitions.EmptySquare // F8
                && !helper.IsSquareAttacked(Definitions.Black, _board, 92) &&
                !helper.IsSquareAttacked(Definitions.Black, _board, 93))
            {
                validMoves[index] = 92; // E8G8, 92 = G8.
                index++;
            }

            if (_bccqs
                && _board[97] == Definitions.EmptySquare // B8
                && _board[96] == Definitions.EmptySquare // C8
                && _board[95] == Definitions.EmptySquare // D8
                && !helper.IsSquareAttacked(Definitions.Black, _board, 96) &&
                !helper.IsSquareAttacked(Definitions.Black, _board, 95))
            {
                validMoves[index] = 96; // 96 = C8.
                index++;
            }

            var finalArray = new int[index];
            for (var i = 0; i < index; i++)
            {
                finalArray[i] = validMoves[i];
            }

            return finalArray;
        }

        private int[] WhiteMoveGeneration(int square)
        {
            var helper = new Helper();
            var index = 0;
            var validMoves = new int[Definitions.MaxKingMoves];

            if (_board[square + 1] == Definitions.EmptySquare ||
                _board[square + 1] > Definitions.WhiteKing && _board[square + 1] <= Definitions.BlackKing)
            {
                validMoves[index] = square + 1;
                index++;
            }

            if (_board[square - 1] == Definitions.EmptySquare ||
                _board[square - 1] > Definitions.WhiteKing && _board[square - 1] <= Definitions.BlackKing)
            {
                validMoves[index] = square - 1;
                index++;
            }

            if (_board[square + 9] == Definitions.EmptySquare ||
                _board[square + 9] > Definitions.WhiteKing && _board[square + 9] <= Definitions.BlackKing)
            {
                validMoves[index] = square + 9;
                index++;
            }

            if (_board[square - 9] == Definitions.EmptySquare ||
                _board[square - 9] > Definitions.WhiteKing && _board[square - 9] <= Definitions.BlackKing)
            {
                validMoves[index] = square - 9;
                index++;
            }

            if (_board[square - 10] == Definitions.EmptySquare ||
                _board[square - 10] > Definitions.WhiteKing && _board[square - 10] <= Definitions.BlackKing)
            {
                validMoves[index] = square - 10;
                index++;
            }

            if (_board[square + 10] == Definitions.EmptySquare ||
                _board[square + 10] > Definitions.WhiteKing && _board[square + 10] <= Definitions.BlackKing)
            {
                validMoves[index] = square + 10;
                index++;
            }

            if (_board[square + 11] == Definitions.EmptySquare ||
                _board[square + 11] > Definitions.WhiteKing && _board[square + 11] <= Definitions.BlackKing)
            {
                validMoves[index] = square + 11;
                index++;
            }

            if (_board[square - 11] == Definitions.EmptySquare ||
                _board[square - 11] > Definitions.WhiteKing && _board[square - 11] <= Definitions.BlackKing)
            {
                validMoves[index] = square - 11;
                index++;
            }

            if (_wccks
                && _board[22] == Definitions.EmptySquare // G1
                && _board[23] == Definitions.EmptySquare // F1
                && !helper.IsSquareAttacked(Definitions.White, _board, 23) &&
                !helper.IsSquareAttacked(Definitions.White, _board, 22))
            {
                validMoves[index] = 22; // G1 = 22
                index++;
            }

            if (_wccqs
                && _board[27] == Definitions.EmptySquare // B1
                && _board[26] == Definitions.EmptySquare // C1
                && _board[25] == Definitions.EmptySquare // D1
                && !helper.IsSquareAttacked(Definitions.White, _board, 26) &&
                !helper.IsSquareAttacked(Definitions.White, _board, 25))
            {
                validMoves[index] = 26; // C1 = 26
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