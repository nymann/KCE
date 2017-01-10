namespace KCE.BoardRepresentation
{
    public class Evaluate
    {
        private readonly int[] _board64 =
        {
            21, 22, 23, 24, 25, 26, 27, 28,
            31, 32, 33, 34, 35, 36, 37, 38,
            41, 42, 43, 44, 45, 46, 47, 48,
            51, 52, 53, 54, 55, 56, 57, 58,
            61, 62, 63, 64, 65, 66, 67, 68,
            71, 72, 73, 74, 75, 76, 77, 78,
            81, 82, 83, 84, 85, 86, 87, 88,
            91, 92, 93, 94, 95, 96, 97, 98
        };

        #region piece square tables

        // https://youtu.be/zSJF6jZ61w0?list=PLZ1QII7yudbc-Ky058TEaOstZHVbT-2hg
        private int[] pawnTable =
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            10, 10, 0, -10, -10, 0, 10, 10,
            5, 0, 0, 5, 5, 0, 0, 5,
            0, 0, 10, 20, 20, 10, 0, 0,
            5, 5, 5, 10, 10, 5, 5, 5,
            10, 10, 10, 20, 20, 10, 10, 10,
            20, 20, 20, 30, 30, 20, 20, 20,
            0, 0, 0, 0, 0, 0, 0, 0
        };


        private int[] knightTable =
        {
            0, -10, 0, 0, 0, 0, -10, 0,
            0, 0, 0, 5, 5, 0, 0, 0,
            0, 0, 10, 10, 10, 10, 0, 0,
            0, 0, 10, 20, 20, 10, 5, 0,
            5, 10, 15, 20, 20, 15, 10, 5,
            5, 10, 10, 20, 20, 10, 10, 5,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        };

        private int[] bishopTable =
        {
            0, 0, -10, 0, 0, -10, 0, 0,
            0, 0, 0, 10, 10, 0, 0, 0,
            0, 0, 10, 15, 15, 10, 0, 0,
            0, 10, 15, 20, 20, 15, 10, 0,
            0, 10, 15, 20, 20, 15, 10, 0,
            0, 0, 10, 15, 15, 10, 0, 0,
            0, 0, 0, 10, 10, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0
        };

        private int[] mirror64 = {
            56  ,   57  ,   58  ,   59  ,   60  ,   61  ,   62  ,   63  ,
            48  ,   49  ,   50  ,   51  ,   52  ,   53  ,   54  ,   55  ,
            40  ,   41  ,   42  ,   43  ,   44  ,   45  ,   46  ,   47  ,
            32  ,   33  ,   34  ,   35  ,   36  ,   37  ,   38  ,   39  ,
            24  ,   25  ,   26  ,   27  ,   28  ,   29  ,   30  ,   31  ,
            16  ,   17  ,   18  ,   19  ,   20  ,   21  ,   22  ,   23  ,
            8   ,   9   ,   10  ,   11  ,   12  ,   13  ,   14  ,   15  ,
            0   ,   1   ,   2   ,   3   ,   4   ,   5   ,   6   ,   7
        };

        private int[] rookTable =
        {
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            25, 25, 25, 25, 25, 25, 25, 25,
            0, 0, 5, 10, 10, 5, 0, 0
        };

        #endregion

        public Evaluate()
        {
        }

        public int EvalPosition(BoardState bs)
        {
            int score = 0;
            int counter64 = 0;
            foreach (int square in _board64)
            {
                switch (bs.BoardRepresentation[square])
                {
                    #region white

                    case Definitions.WhitePawn:
                        score += 100;
                        score += pawnTable[counter64];
                        break;
                    case Definitions.WhiteKnight:
                        score += 300;
                        score += knightTable[counter64];
                        break;
                    case Definitions.WhiteBishop:
                        score += 300;
                        score += bishopTable[counter64];
                        break;
                    case Definitions.WhiteRook:
                        score += 500;
                        score += rookTable[counter64];
                        break;
                    case Definitions.WhiteQueen:
                        score += 900;
                        break;
                    case Definitions.WhiteKing:

                        break;

                    #endregion

                    #region black
                    case Definitions.BlackPawn:
                        score -= 100;
                        score -= pawnTable[mirror64[counter64]];
                        break;
                    case Definitions.BlackKnight:
                        score -= 300;
                        score -= knightTable[mirror64[counter64]];
                        break;
                    case Definitions.BlackBishop:
                        score -= 300;
                        score -= bishopTable[mirror64[counter64]];
                        break;
                    case Definitions.BlackRook:
                        score -= 500;
                        score -= rookTable[mirror64[counter64]];
                        break;
                    case Definitions.BlackQueen:
                        score -= 900;
                        break;
                    case Definitions.BlackKing:
                        break;
                    #endregion

                    default:
                        break;
                        
                }
                counter64++;
            }

            return score;
        }
    }
}