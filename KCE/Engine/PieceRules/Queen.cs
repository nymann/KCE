using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace KCE.Engine.PieceRules
{
    public class Queen
    {
        private readonly int[] _board;
        private readonly int _square;
        private readonly bool _sideToMove;

        public Queen(int[] board, int square, bool sideToMove)
        {
            _square = square;
            _board = board;
            _sideToMove = sideToMove;
        }

        public int[] MoveGeneration()
        {
            var bishop = new Bishop(_board, _square, _sideToMove);
            var rook = new Rook(_board, _square, _sideToMove);

            var bishopMoves = bishop.MoveGeneration();
            var rookMoves = rook.MoveGeneration();

            var validMoves = bishopMoves.Concat(rookMoves).ToArray();

            return validMoves;
        }
    }
}