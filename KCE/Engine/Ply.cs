using System;

namespace KCE.Engine
{
    public class Ply
    {
        public int Score { get; set; } = - Definitions.INFINITE;
        private int[] _board;
        private int[] _hisBoard;

        private int _enPas;
        private int _hisEnPas;
        private bool _WCCKS;
        private bool _WCCQS;
        private bool _BCCKS;
        private bool _BCCQS;
        private bool _hisWCCKS;
        private bool _hisWCCQS;
        private bool _hisBCCKS;
        private bool _hisBCCQS;
        //private Tuple<int, int> _move;

        private string _algebraicPly;

        public Ply(int[] board, int[] hisBoard, int hisEnPas, int enPas, string algebraicPly, 
            bool wccks, bool wccqs, bool bccks, bool bccqs, bool hisWccks, bool hisWccqs, bool hisBccks, bool hisBccqs/*, int fromSquare, int toSquare*/)
        {
            _board = (int[]) board.Clone();
            _hisBoard = (int[]) hisBoard.Clone();
            _hisEnPas = hisEnPas;
            _enPas = enPas;
            _algebraicPly = algebraicPly;
            _WCCKS = wccks;
            _WCCQS = wccqs;
            _BCCKS = bccks;
            _BCCQS = bccqs;
            _hisWCCKS = hisWccks;
            _hisWCCQS = hisWccqs;
            _hisBCCKS = hisBccks;
            _hisBCCQS = hisBccqs;
            //_move = new Tuple<int, int>(fromSquare, toSquare);
        }

        /*public Tuple<int, int> GetMove()
        {
            return _move;
        }*/

        public int[] GetBoard()
        {
            return _board;
        }

        public int[] GetHisBoard()
        {
            return _hisBoard;
        }

        public string GetAlgebraicPly()
        {
            return _algebraicPly;
        }

        public int GetEnPas()
        {
            return _enPas;
        }

        public int GetHisEnPas()
        {
            return _hisEnPas;
        }

        public bool GetWCCKS()
        {
            return _WCCKS;
        }

        public bool GetWCCQS()
        {
            return _WCCQS;
        }

        public bool GetBCCKS()
        {
            return _BCCKS;
        }

        public bool GetBCCQS()
        {
            return _BCCQS;
        }

        public bool GetHisWCCKS()
        {
            return _hisWCCKS;
        }

        public bool GetHisWCCQS()
        {
            return _hisWCCQS;
        }

        public bool GetHisBCCKS()
        {
            return _hisBCCKS;
        }

        public bool GetHisBCCQS()
        {
            return _hisBCCQS;
        }

        public bool UpdateCastlePerms()
        {
            return _hisBCCKS != _BCCKS || _hisBCCQS != _BCCQS || _hisWCCKS != _WCCKS || _hisWCCQS != _WCCQS;
        }
    }
}