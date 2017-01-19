using System.Collections.Generic;

namespace KCE.Engine
{
    public static class Definitions
    {
        public static Dictionary<string, int> AlgebraicToIndex = new Dictionary<string, int>()
        {
            {"h1", 21}, {"h2", 31}, {"h3", 41}, {"h4", 51}, {"h5", 61}, {"h6", 71}, {"h7", 81}, {"h8", 91},
            {"g1", 22}, {"g2", 32}, {"g3", 42}, {"g4", 52}, {"g5", 62}, {"g6", 72}, {"g7", 82}, {"g8", 92},
            {"f1", 23}, {"f2", 33}, {"f3", 43}, {"f4", 53}, {"f5", 63}, {"f6", 73}, {"f7", 83}, {"f8", 93},
            {"e1", 24}, {"e2", 34}, {"e3", 44}, {"e4", 54}, {"e5", 64}, {"e6", 74}, {"e7", 84}, {"e8", 94},
            {"d1", 25}, {"d2", 35}, {"d3", 45}, {"d4", 55}, {"d5", 65}, {"d6", 75}, {"d7", 85}, {"d8", 95},
            {"c1", 26}, {"c2", 36}, {"c3", 46}, {"c4", 56}, {"c5", 66}, {"c6", 76}, {"c7", 86}, {"c8", 96},
            {"b1", 27}, {"b2", 37}, {"b3", 47}, {"b4", 57}, {"b5", 67}, {"b6", 77}, {"b7", 87}, {"b8", 97},
            {"a1", 28}, {"a2", 38}, {"a3", 48}, {"a4", 58}, {"a5", 68}, {"a6", 78}, {"a7", 88}, {"a8", 98}
         };

        public static Dictionary<int, string> IndexToAlgebraic = new Dictionary<int, string>()
        {
            {21, "h1"}, {31, "h2"}, {41, "h3"}, {51, "h4"}, {61, "h5"}, {71, "h6"}, {81, "h7"}, {91, "h8"},
            {22, "g1"}, {32, "g2"}, {42, "g3"}, {52, "g4"}, {62, "g5"}, {72, "g6"}, {82, "g7"}, {92, "g8"},
            {23, "f1"}, {33, "f2"}, {43, "f3"}, {53, "f4"}, {63, "f5"}, {73, "f6"}, {83, "f7"}, {93, "f8"},
            {24, "e1"}, {34, "e2"}, {44, "e3"}, {54, "e4"}, {64, "e5"}, {74, "e6"}, {84, "e7"}, {94, "e8"},
            {25, "d1"}, {35, "d2"}, {45, "d3"}, {55, "d4"}, {65, "d5"}, {75, "d6"}, {85, "d7"}, {95, "d8"},
            {26, "c1"}, {36, "c2"}, {46, "c3"}, {56, "c4"}, {66, "c5"}, {76, "c6"}, {86, "c7"}, {96, "c8"},
            {27, "b1"}, {37, "b2"}, {47, "b3"}, {57, "b4"}, {67, "b5"}, {77, "b6"}, {87, "b7"}, {97, "b8"},
            {28, "a1"}, {38, "a2"}, {48, "a3"}, {58, "a4"}, {68, "a5"}, {78, "a6"}, {88, "a7"}, {98, "a8"}
        };

        public const int EmptySquare = 0;
        public const int WhitePawn = 1;
        public const int WhiteKnight = 2;
        public const int WhiteBishop = 3;
        public const int WhiteRook = 4;
        public const int WhiteQueen = 5;
        public const int WhiteKing = 6;
        public const int BlackPawn = 7;
        public const int BlackKnight = 8;
        public const int BlackBishop = 9;
        public const int BlackRook = 10;
        public const int BlackQueen = 11;
        public const int BlackKing = 12;
        public const bool Black = false;
        public const bool White = true;
        public const bool BlackToMove = false;
        public const bool WhiteToMove = true;
        public const int NoEnPassantSquare = -1;
        public const int BCCKS = 0;
        public const int BCCQS = 1;
        public const int WCCKS = 2;
        public const int WCCQS = 3;
        public const int INFINITE = 30000;
        public const int MATE = 29000;
        public const int MAXDEPTH = 6;
        public const string STDSETUP = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        public const long StdTimePrPly = 5000;
        public const long InfiniteTime = long.MaxValue;
        public const int Stopped = 28000;

        public const int MaxQueenMoves = 27;
        public const int MaxRookMoves = 14;
        public const int MaxBishopMoves = 13;
        public const int MaxPawnMoves = 4;
        public const int MaxKnightMoves = 8;
        public const int MaxKingMoves = 9;

        public const int QuitCode = 2;
    }
}