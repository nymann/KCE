using System.Collections.Generic;

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

        public List<int> MoveGeneration()
        {
            var validMoves = new List<int>();

            var bishop = new Bishop(_board, _square, _sideToMove);
            var rook = new Rook(_board, _square, _sideToMove);

            var bishopMoves = bishop.MoveGeneration();
            var rookMoves = rook.MoveGeneration();

            validMoves.AddRange(bishopMoves);
            validMoves.AddRange(rookMoves);

            return validMoves;
        }
    }
}