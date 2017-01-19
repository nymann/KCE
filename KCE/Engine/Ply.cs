using System;

namespace KCE.Engine
{
    public class Ply
    {
        public int Score { get; set; } = - Definitions.INFINITE;
        private readonly int[] _board;
        private readonly int[] _hisBoard;

        private readonly int _enPas = 99;
        private readonly bool _wccks;
        private readonly bool _wccqs;
        private readonly bool _bccks;
        private readonly bool _bccqs;
        private readonly int[] _fromToSquare;
        public string Promotion { get; set; } = "";

        //private readonly string _algebraicPly;

        public Ply(int[] board, int[] hisBoard, int enPas, /*string algebraicPly,*/ 
            bool wccks, bool wccqs, bool bccks, bool bccqs, int[] fromToSquare)
        {
            _board = (int[]) board.Clone();
            _hisBoard = (int[]) hisBoard.Clone();
            _enPas = enPas;
            //_algebraicPly = algebraicPly;
            _wccks = wccks;
            _wccqs = wccqs;
            _bccks = bccks;
            _bccqs = bccqs;
            _fromToSquare = fromToSquare;
        }

        public Ply()
        {

        }

        public int[] GetBoard()
        {
            return _board;
        }

        public int[] GetHisBoard()
        {
            return _hisBoard;
        }

        public int[] GetFromToSquare()
        {
            return _fromToSquare;
        }

        public int GetEnPas()
        {
            return _enPas;
        }

        public bool GetWCCKS()
        {
            return _wccks;
        }

        public bool GetWCCQS()
        {
            return _wccqs;
        }

        public bool GetBCCKS()
        {
            return _bccks;
        }

        public bool GetBCCQS()
        {
            return _bccqs;
        }
    }
}