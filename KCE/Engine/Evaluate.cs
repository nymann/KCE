namespace KCE.Engine
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

        private bool endGame = false;

        // As seen on.
        // https://chessprogramming.wikispaces.com/Simplified+evaluation+function
        private readonly int[] _pawnTable =
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5, 5, 10, 25, 25, 10, 5, 5,
            0, 0, 0, 20, 20, 0, 0, 0,
            5, -5, -10, 0, 0, -10, -5, 5,
            5, 10, 10, -20, -20, 10, 10, 5,
            0, 0, 0, 0, 0, 0, 0, 0
        };


        private readonly int[] _knightTable =
        {
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -30, 0, 10, 15, 15, 10, 0, -30,
            -30, 5, 15, 20, 20, 15, 5, -30,
            -30, 0, 15, 20, 20, 15, 0, -30,
            -30, 5, 10, 15, 15, 10, 5, -30,
            -40, -20, 0, 5, 5, 0, -20, -40,
            -50, -40, -30, -30, -30, -30, -40, -50,
        };

        private readonly int[] _mirror64 =
        {
            56, 57, 58, 59, 60, 61, 62, 63,
            48, 49, 50, 51, 52, 53, 54, 55,
            40, 41, 42, 43, 44, 45, 46, 47,
            32, 33, 34, 35, 36, 37, 38, 39,
            24, 25, 26, 27, 28, 29, 30, 31,
            16, 17, 18, 19, 20, 21, 22, 23,
            8, 9, 10, 11, 12, 13, 14, 15,
            0, 1, 2, 3, 4, 5, 6, 7
        };

        private readonly int[] _bishopTable =
        {
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 10, 10, 5, 0, -10,
            -10, 5, 5, 10, 10, 5, 5, -10,
            -10, 0, 10, 10, 10, 10, 0, -10,
            -10, 10, 10, 10, 10, 10, 10, -10,
            -10, 5, 0, 0, 0, 0, 5, -10,
            -20, -10, -10, -10, -10, -10, -10, -20,
        };

        private readonly int[] _rookTable =
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            5, 10, 10, 10, 10, 10, 10, 5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            0, 0, 0, 5, 5, 0, 0, 0
        };

        private readonly int[] _queenTable =
        {
            -20, -10, -10, -5, -5, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 5, 5, 5, 0, -10,
            -5, 0, 5, 5, 5, 5, 0, -5,
            0, 0, 5, 5, 5, 5, 0, -5,
            -10, 5, 5, 5, 5, 5, 0, -10,
            -10, 0, 5, 0, 0, 0, 0, -10,
            -20, -10, -10, -5, -5, -10, -10, -20
        };

        private readonly int[] _kingMiddleGameTabe =
        {
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -20, -30, -30, -40, -40, -30, -30, -20,
            -10, -20, -20, -20, -20, -20, -20, -10,
            20, 20, 0, 0, 0, 0, 20, 20,
            20, 30, 10, 0, 0, 10, 30, 20
        };

        private readonly int[] _kingEndGameTable =
        {
            -50, -40, -30, -20, -20, -30, -40, -50,
            -30, -20, -10, 0, 0, -10, -20, -30,
            -30, -10, 20, 30, 30, 20, -10, -30,
            -30, -10, 30, 40, 40, 30, -10, -30,
            -30, -10, 30, 40, 40, 30, -10, -30,
            -30, -10, 20, 30, 30, 20, -10, -30,
            -30, -30, 0, 0, 0, 0, -30, -30,
            -50, -30, -30, -30, -30, -30, -30, -50
        };

        #endregion

        public Evaluate()
        {
        }

        public int EvalPosition(BoardState bs)
        {
            //int wP = 0;
            var wN = 0;
            var wB = 0;
            var wR = 0;
            var wQ = 0;
            //int bP = 0;
            var bN = 0;
            var bB = 0;
            var bR = 0;
            var bQ = 0;

            var whiteKingIndex = 0;
            var blackKingIndex = 0;

            var score = 0;
            var index = 0;
            foreach (var square in _board64)
            {
                switch (bs.BoardRepresentation[square])
                {
                        #region white

                    case Definitions.WhitePawn:
                        score += 100;
                        score += _pawnTable[_mirror64[index]];
                        //wP++;
                        break;
                    case Definitions.WhiteKnight:
                        score += 300;
                        score += _knightTable[_mirror64[index]];
                        wN++;
                        break;
                    case Definitions.WhiteBishop:
                        score += 300;
                        score += _bishopTable[_mirror64[index]];
                        wB++;
                        break;
                    case Definitions.WhiteRook:
                        score += 500;
                        score += _rookTable[_mirror64[index]];
                        wR++;
                        break;
                    case Definitions.WhiteQueen:
                        score += 900;
                        score += _queenTable[_mirror64[index]];
                        wQ++;
                        break;
                    case Definitions.WhiteKing:
                        whiteKingIndex = index;
                        break;

                        #endregion

                        #region black

                    case Definitions.BlackPawn:
                        score -= 100;
                        score -= _pawnTable[index];
                        //bP++;
                        break;
                    case Definitions.BlackKnight:
                        score -= 300;
                        score -= _knightTable[index];
                        bN++;
                        break;
                    case Definitions.BlackBishop:
                        score -= 300;
                        score -= _bishopTable[index];
                        bB++;
                        break;
                    case Definitions.BlackRook:
                        score -= 500;
                        score -= _rookTable[index];
                        bR++;
                        break;
                    case Definitions.BlackQueen:
                        score -= 900;
                        score -= _queenTable[index];
                        bQ++;
                        break;
                    case Definitions.BlackKing:
                        blackKingIndex = index;
                        break;

                        #endregion

                    default:
                        break;
                }

                index++;
            }


            if (bB == 2)
                score -= 5;

            if (wB == 2)
                score += 5;

            endGame = EndGame(wN, wB, wR, wQ, bN, bB, bR, bQ);

            if (endGame)
            {
                score += _kingEndGameTable[_mirror64[whiteKingIndex]];
                score -= _kingEndGameTable[blackKingIndex];
            }

            else
            {
                // king safety
                score += KingSafety(bs.KingSquares, bs.BoardRepresentation);
                score += _kingMiddleGameTabe[_mirror64[whiteKingIndex]];
                score -= _kingMiddleGameTabe[blackKingIndex];
            }

            // Mobility, disabled for now.
            //int mobility = bs.LegalMovesCount / 10;
            //new Helper().PrintBoardWhitePerspective(bs.BoardRepresentation);
            if (bs.SideToMove == Definitions.White)
                return score /*+ mobility*/;
            /*Console.WriteLine("Side: Black, Score: {0}, Mobility: {1}, Total {2}.", -score, mobility, -score - mobility);*/
            return -score /*- mobility*/;
        }


        // https://chessprogramming.wikispaces.com/Tapered+Eval
        private int TaperedEval( /*int wP,*/ int wN, int wB, int wR, int wQ, /*int bP,*/ int bN, int bB, int bR, int bQ)
        {
            //var pawnPhase = 0;
            var knightPhase = 1;
            var bishopPhase = 1;
            var rookPhase = 2;
            var queenPhase = 4;
            var totalPhase = /*pawnPhase * 16 +*/ knightPhase*4 + bishopPhase*4 + rookPhase*4 + queenPhase*2;
            var phase = totalPhase;

            //phase -= wP * pawnPhase;
            phase -= wN*knightPhase;
            phase -= wB*bishopPhase;
            phase -= wR*rookPhase;
            phase -= wQ*queenPhase;

            //phase -= bP * pawnPhase;
            phase -= bN*knightPhase;
            phase -= bB*bishopPhase;
            phase -= bR*rookPhase;
            phase -= bQ*queenPhase;

            phase = (phase*256 + totalPhase/2)/totalPhase;

            var eval = (100*(256 - phase) + 300*phase)/256;

            return eval;
        }

        private bool EndGame(int wN, int wB, int wR, int wQ, int bN, int bB, int bR, int bQ)
        {
            return TaperedEval(wN, wB, wR, wQ, bN, bB, bR, bQ) > 230;
        }

        private int KingSafety(int[] kingSquares, int[] board)
        {
            int score = 0;
            if (board[kingSquares[1] + 9] == Definitions.WhitePawn)
            {
                score += 10;
            }
            if (board[kingSquares[1] + 10] == Definitions.WhitePawn)
            {
                score += 10;
            }
            if (board[kingSquares[1] + 11] == Definitions.WhitePawn)
            {
                score -= 10;
            }

            if (board[kingSquares[0] - 9] == Definitions.WhitePawn)
            {
                score -= 10;
            }
            if (board[kingSquares[0] - 10] == Definitions.WhitePawn)
            {
                score -= 10;
            }
            if (board[kingSquares[0] - 11] == Definitions.WhitePawn)
            {
                score -= 10;
            }
            return score;
        }
    }
}