using System;

namespace KCE.Engine
{
    public class Ply
    {
        public int Score { get; set; } = - Definitions.INFINITE;
        private readonly int[] _board;
        private readonly int[] _hisBoard;

        private readonly int _enPas;
        /*private int _hisEnPas;*/
        private readonly bool _wccks;
        private readonly bool _wccqs;
        private readonly bool _bccks;
        private readonly bool _bccqs;
        /*private bool _hisWCCKS;
        private bool _hisWCCQS;
        private bool _hisBCCKS;
        private bool _hisBCCQS;*/
        //private Tuple<int, int> _move;

        private readonly string _algebraicPly;

        public Ply(int[] board, int[] hisBoard, /*int hisEnPas,*/ int enPas, string algebraicPly, 
            bool wccks, bool wccqs, bool bccks, bool bccqs/*, bool hisWccks, bool hisWccqs, bool hisBccks, bool hisBccqs*//*, int fromSquare, int toSquare*/)
        {
            _board = (int[]) board.Clone();
            _hisBoard = (int[]) hisBoard.Clone();
            /*_hisEnPas = hisEnPas;*/
            _enPas = enPas;
            _algebraicPly = algebraicPly;
            _wccks = wccks;
            _wccqs = wccqs;
            _bccks = bccks;
            _bccqs = bccqs;
            /*_hisWCCKS = hisWccks;
            _hisWCCQS = hisWccqs;
            _hisBCCKS = hisBccks;
            _hisBCCQS = hisBccqs;*/
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

        /*public int GetHisEnPas()
        {
            return _hisEnPas;
        }*/

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

        /*public bool GetHisWCCKS()
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
        }*/
    }
}