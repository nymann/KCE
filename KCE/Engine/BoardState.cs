namespace KCE.Engine
{
    public class BoardState
    {
        //public List<Tuple<int, int>> Moves { get; set; }
        public int[] BoardRepresentation { get; set; }
        public bool SideToMove { get; set; }
        public bool Wcks { get; set; }
        public bool Wcqs { get; set; }
        public bool Bcks { get; set; }
        public bool Bcqs { get; set; }
        public bool HisWcks { get; set; }
        public bool HisWcqs { get; set; }
        public bool HisBcks { get; set; }
        public bool HisBcqs { get; set; }
        public int HisEnPas { get; set; }

        public int[] KingSquares { get; set; }
        public int EnPasSquare { get; set; }
        public int FiftyMoveRule { get; set; }
        public int Ply { get; set; }
        public Ply BestPly { get; set; }
        public Ply BestPlyAtLowerDepth { get; set; } = null;
        public bool HaveSearched { get; set; } = false;
        public bool EndGame { get; set; } = false;
        //public int LegalMovesCount { get; set; } = 0;

        public BoardState(int[] boardRepresentation, bool sideToMove, int[] kingSquares, int enPasSquare, int fiftyMoveRule, bool wcks, bool wcqs, bool bcks, bool bcqs)
        {
            BoardRepresentation = boardRepresentation;
            SideToMove = sideToMove;
            KingSquares = kingSquares;
            EnPasSquare = enPasSquare;
            FiftyMoveRule = fiftyMoveRule;
            this.Wcks = wcks;
            this.Wcqs = wcqs;
            this.Bcks = bcks;
            this.Bcqs = bcqs;
            //Moves = new List<Tuple<int, int>>();
        }

        public BoardState()
        {
            
        }
    }
}