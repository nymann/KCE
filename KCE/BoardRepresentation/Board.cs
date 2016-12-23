namespace KCE.BoardRepresentation
{
    public class Board
    {
        public int[] BoardRepresentation { get; }
        public bool SideToMove { get; }
        public int[] KingSquares { get; }

        public int EnPasSquare { get; }

        public int FiftyMoveRule { get; }

        public Board(int[] boardRepresentation, bool sideToMove, int[] kingSquares, int enPasSquare, int fiftyMoveRule)
        {
            BoardRepresentation = boardRepresentation;
            SideToMove = sideToMove;
            KingSquares = kingSquares;
            EnPasSquare = enPasSquare;
            FiftyMoveRule = fiftyMoveRule;
        }




    }
}