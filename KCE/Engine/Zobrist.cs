/*using System;

namespace KCE.Engine
{
    public class Zobrist
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

        public Zobrist()
        {
            
        }

        public ulong[,] InitZobrist(BoardState bs)
        {
            var table = new ulong[13,120];
            for (var i = 0; i < 12; i++)
            {
                for (var j = 0; j < 120; j++)
                {
                    table[i, j] = NextUint64(new Random());
                }
            }

            return table;
        }

        public ulong Hash(BoardState bs)
        {
            ulong posKey = 0;

            foreach (var square in _board64)
            {
                var piece = bs.BoardRepresentation[square];
                if (piece != Definitions.EmptySquare)
                {
                    posKey ^= bs.PieceKeys[piece,square];
                }
            }

            return posKey;
        }

        public static ulong NextUint64(Random rnd)
        {
            var buffer = new byte[8];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}*/