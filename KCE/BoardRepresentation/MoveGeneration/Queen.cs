using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Queen
    {
        private readonly Board _board;
        private readonly int _square;

        public Queen(int square, Board board)
        {
            _square = square;
            _board = board;
        }

        public List<int> MoveGeneration()
        {
            var validMoves = new List<int>();

            var bishop = new Bishop(_board, _square);
            var rook = new Rook(_square, _board);

            var bishopMoves = bishop.MoveGeneration();
            var rookMoves = rook.MoveGeneration();
            
            validMoves.AddRange(bishopMoves);
            validMoves.AddRange(rookMoves);

            return validMoves;
        }
    }
}