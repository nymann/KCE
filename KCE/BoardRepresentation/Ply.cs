namespace KCE.BoardRepresentation
{
    public class Ply
    {
        private int[] _board;
        private int[] _hisBoard;

        private int _enPas;
        private int _hisEnPas;

        private string _algebraicPly;

        public Ply(int[] board, int[] hisBoard, int hisEnPas, int enPas, string algebraicPly)
        {
            _board = board;
            _hisBoard = hisBoard;
            _hisEnPas = hisEnPas;
            _enPas = enPas;
            _algebraicPly = algebraicPly;
        }

        public int[] GetBoard()
        {
            return _board;
        }

        public string GetAlgebraicPly()
        {
            return _algebraicPly;
        }
    }
}