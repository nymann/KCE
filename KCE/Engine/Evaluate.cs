using System;
using KCE.Engine;

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

        private int[] pawnTable = {
            0, 0, 0, 0, 0, 0, 0, 0,
            10, 10, 0, -10, -10, 0, 10, 10,
            0, 5, 0, 5, 5, 0, 5, 0,
            0, 0, 10, 20, 20, 10, 0, 0,
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

        private int[] rookTable =
        {
            1, 0, 5, 10, 10, 5, 0, 1,
            25, 25, 25, 25, 25, 25, 25, 25,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0,
            0, 0, 5, 10, 10, 5, 0, 0
        };

        #endregion

        public Evaluate()
        {
        }

        public int EvalPosition(BoardState bs)
        {
            int whitePieces;
            int blackPieces;

            int whiteBishopPair = 0;
            int blackBishopPair = 0;

            int score = 0;

            for (int square = 0; square < 64; square++)
            {
                switch (bs.BoardRepresentation[_board64[square]])
                {
                    #region white

                    case Definitions.WhitePawn:
                        score += 100;
                        score += pawnTable[square];
                        break;
                    case Definitions.WhiteKnight:
                        score += 300;
                        score += knightTable[square];
                        break;
                    case Definitions.WhiteBishop:
                        score += 300;
                        score += bishopTable[square];
                        whiteBishopPair++;
                        break;
                    case Definitions.WhiteRook:
                        score += 500;
                        score += rookTable[square];
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
                        score -= pawnTable[mirror64[square]];
                        break;
                    case Definitions.BlackKnight:
                        score -= 300;
                        score -= knightTable[mirror64[square]];
                        break;
                    case Definitions.BlackBishop:
                        score -= 300;
                        score -= bishopTable[mirror64[square]];
                        blackBishopPair++;
                        break;
                    case Definitions.BlackRook:
                        score -= 500;
                        score -= rookTable[mirror64[square]];
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
            }

            if (blackBishopPair == 2)
            {
                // technically he could've promoted a pawn to a bishop and now have 2 same color bishops, but we'll not handle that.
                score -= 5;
            }

            if (whiteBishopPair == 2)
            {
                score += 5;
            }


            // Mobility, disabled for now.
            //int mobility = bs.LegalMovesCount / 10;
            //new Helper().PrintBoardWhitePerspective(bs.BoardRepresentation);
            if (bs.SideToMove == Definitions.White)
            {
                
                /*Console.WriteLine("Side: White, Score: {0}, Mobility: {1}, Total: {2}.", score, mobility, score + mobility);*/
                return score /*+ mobility*/;
            }
            /*Console.WriteLine("Side: Black, Score: {0}, Mobility: {1}, Total {2}.", -score, mobility, -score - mobility);*/
            return -score /*- mobility*/;
        }
    }
}