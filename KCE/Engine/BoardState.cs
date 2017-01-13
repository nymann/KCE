namespace KCE.Engine
{
    public class BoardState
    {
        public int[] BoardRepresentation { get; set; }
        public bool SideToMove { get; set; }
        public bool WCKS { get; set; }
        public bool WCQS { get; set; }
        public bool BCKS { get; set; }
        public bool BCQS { get; set; }
        public int[] KingSquares { get; set; }
        public int EnPasSquare { get; set; }
        public int FiftyMoveRule { get; set; }
        public int Ply { get; set; }
        public Ply BestPly { get; set; }
        public Ply BestPlyAtLowerDepth { get; set; } = null;
        public bool HaveSearched { get; set; } = false;
        public bool EndGame { get; set; } = false;
        //public int LegalMovesCount { get; set; } = 0;

        public BoardState(int[] boardRepresentation, bool sideToMove, int[] kingSquares, int enPasSquare, int fiftyMoveRule, bool WCKS, bool WCQS, bool BCKS, bool BCQS)
        {
            BoardRepresentation = boardRepresentation;
            SideToMove = sideToMove;
            KingSquares = kingSquares;
            EnPasSquare = enPasSquare;
            FiftyMoveRule = fiftyMoveRule;
            this.WCKS = WCKS;
            this.WCQS = WCQS;
            this.BCKS = BCKS;
            this.BCQS = BCQS;
        }

        public BoardState()
        {
            
        }
    }
}