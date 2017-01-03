namespace KCE.BoardRepresentation
{
    public class Board
    {
        public int[] BoardRepresentation { get; }
        public bool SideToMove { get; set; }

        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }

        public int[] KingSquares { get; set; }

        public int EnPasSquare { get; set; }

        public int FiftyMoveRule { get; set; }

        public Board(int[] boardRepresentation, bool sideToMove, int[] kingSquares, int enPasSquare, int fiftyMoveRule, bool WCCKS, bool WCCQS, bool BCCKS, bool BCCQS)
        {
            BoardRepresentation = boardRepresentation;
            SideToMove = sideToMove;
            KingSquares = kingSquares;
            EnPasSquare = enPasSquare;
            FiftyMoveRule = fiftyMoveRule;

            WhiteCanCastleKingSide = WCCKS;
            WhiteCanCastleQueenSide = WCCQS;
            BlackCanCastleKingSide = BCCKS;
            BlackCanCastleQueenSide = BCCQS;
        }
    }
}