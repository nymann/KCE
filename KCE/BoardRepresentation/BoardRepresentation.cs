using System;
using System.Text;

namespace KCE.BoardRepresentation
{
    public class BoardRepresentation
    {
        private readonly int[] _board =
        {
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 4, 2, 3, 5, 6, 3, 2, 4, 99,
            99, 1, 1, 1, 1, 1, 1, 1, 1, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 0, 0, 0, 0, 0, 0, 0, 0, 99,
            99, 7, 7, 7, 7, 7, 7, 7, 7, 99,
            99, 10, 8, 9, 11, 12, 9, 8, 10, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
        };

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

        public BoardRepresentation()
        {
            PrintBoard();
        }

        private void PrintBoard()
        {
            var counter = 8;
            Console.OutputEncoding = Encoding.Unicode;
            Console.WriteLine("   a   b   c   d   e   f   g   h");
            Console.WriteLine(" +---+---+---+---+---+---+---+---+");
            foreach (var square in _board64)
            {
                Console.Write(" | ");
                switch (_board[square])
                {
                    case 0:
                        // Empty Square
                        Console.Write(' ');
                        break;
                    case 1:
                        // White Pawn
                        Console.Write('\u2659');
                        break;

                    case 2:
                        // White Knight
                        Console.Write('\u2658');
                        break;

                    case 3:
                        // White Bishop
                        Console.Write('\u2657');
                        break;

                    case 4:
                        // White Rook
                        Console.Write('\u2656');
                        break;

                    case 5:
                        // White Queen
                        Console.Write('\u2655');
                        break;

                    case 6:
                        // White King
                        Console.Write('\u2654');
                        break;

                    case 7:
                        // Black Pawn
                        Console.Write('\u265f');
                        break;
                    case 8:
                        // Black Knight
                        Console.Write('\u265e');
                        break;

                    case 9:
                        // Black Bishop
                        Console.Write('\u265d');
                        break;

                    case 10:
                        // Black Rook
                        Console.Write('\u265c');
                        break;

                    case 11:
                        // Black Queen
                        Console.Write('\u265b');
                        break;

                    case 12:
                        // Black King
                        Console.Write('\u265a');
                        break;

                    default:
                        Console.WriteLine("THIS SHOULDN'T HAPPEN.");
                        break;
                }

                if (square%10 != 8) continue;
                Console.WriteLine(" | {0}", counter);
                Console.WriteLine(" +---+---+---+---+---+---+---+---+");
                counter--;
            }
        }

    }
}