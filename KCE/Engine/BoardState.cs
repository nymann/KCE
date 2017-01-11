﻿using System.Collections.Generic;

namespace KCE.BoardRepresentation
{
    public class BoardState
    {
        public int[] BoardRepresentation { get; set; }
        public bool SideToMove { get; set; }
        public bool WhiteCanCastleKingSide { get; set; }
        public bool WhiteCanCastleQueenSide { get; set; }
        public bool BlackCanCastleKingSide { get; set; }
        public bool BlackCanCastleQueenSide { get; set; }
        public string LastMove { get; set; }
        public int[] KingSquares { get; set; }
        public int EnPasSquare { get; set; }
        public int FiftyMoveRule { get; set; }
        public int Ply { get; set; }
        public Ply BestPly { get; set; }
        public List<Ply> PvTable { get; set; }
        

        public BoardState(int[] boardRepresentation, bool sideToMove, int[] kingSquares, int enPasSquare, int fiftyMoveRule, bool WCCKS, bool WCCQS, bool BCCKS, bool BCCQS)
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

        public BoardState()
        {
            
        }
    }
}