using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public class Knight : IMoveGeneration
    {
        private Board _board;

        public Knight(Board board)
        {
            _board = board;
        }

        public List<int> MoveGeneration()
        {
            return _board.SideToMove
                ? WhiteMoveGeneration(_board.)
        }

        private List<int> BlackMoveGeneration(int square)
        {
            
        }

        private List<int> WhiteMoveGeneration(int square)
        {
            
        }
    }
}