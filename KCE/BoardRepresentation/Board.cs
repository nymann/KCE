using System;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace KCE.BoardRepresentation
{
    public class Board
    {
        #region globals

        private bool K = true; // Can white castle kingside?
        private bool Q = true; // Can white castle queenside?
        private bool k = true; // Can black castle kingside?
        private bool q = true; // Can black castle queenside?

        #region boards

        private string[] _algebraicBoard =
        {
            "A8", "B8", "C8", "D8", "E8", "F8", "G8", "H8",
            "A7", "B7", "C7", "D7", "E7", "F7", "G7", "H7",
            "A6", "B6", "C6", "D6", "E6", "F6", "G6", "H6",
            "A5", "B5", "C5", "D5", "E5", "F5", "G5", "H5",
            "A4", "B4", "C4", "D4", "E4", "F4", "G4", "H4",
            "A3", "B3", "C3", "D3", "E3", "F3", "G3", "H3",
            "A2", "B2", "C2", "D2", "E2", "F2", "G2", "H2",
            "A1", "B1", "C1", "D1", "E1", "F1", "G1", "H1"
        };

        private int[] _board =
        {
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 10, 8, 9, 11, 12, 9, 8, 10, 99,
            99, 7, 7, 7, 7, 7, 7, 7, 7, 99,
            99, 00, 00, 00, 00, 00, 00, 00, 00, 99,
            99, 00, 00, 00, 00, 00, 00, 00, 00, 99,
            99, 00, 00, 00, 00, 00, 00, 00, 00, 99,
            99, 00, 00, 00, 00, 00, 00, 00, 00, 99,
            99, 1, 1, 1, 1, 1, 1, 1, 1, 99,
            99, 4, 2, 3, 5, 6, 3, 2, 4, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
            99, 99, 99, 99, 99, 99, 99, 99, 99, 99,
        };

        private int[] _board64 =
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

        #endregion

        #endregion

        //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -

        public Board(string fen)
        {
        }

        public Board()
        {
        }

        private void PrintBoard()
        {
            Console.OutputEncoding = Encoding.Unicode;

            var counter = 8;
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
                        Console.Write('\u265f');
                        break;

                    case 2:
                        // White Knight
                        Console.Write('\u265e');
                        break;

                    case 3:
                        // White Bishop
                        Console.Write('\u265d');
                        break;

                    case 4:
                        // White Rook
                        Console.Write('\u265c');
                        break;

                    case 5:
                        // White Queen
                        Console.Write('\u265b');
                        break;

                    case 6:
                        // White King
                        Console.Write('\u265a');
                        break;


                    case 7:
                        // Black Pawn
                        Console.Write('\u2659');
                        break;

                    case 8:
                        // Black Knight
                        Console.Write('\u2658');
                        break;

                    case 9:
                        // Black Bishop
                        Console.Write('\u2657');
                        break;

                    case 10:
                        // Black Rook
                        Console.Write('\u2656');
                        break;

                    case 11:
                        // Black Queen
                        Console.Write('\u2655');
                        break;

                    case 12:
                        // Black King
                        Console.Write('\u2654');
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

        private int[] IndexFromLongAlgebraicNotation(string ply)
        {
            ply = ply.ToUpper();
            var plyFrom = 100 - (ply[1] - '0')*10 + (ply[0] - 64);
            var plyTo = 100 - (ply[3] - '0')*10 + (ply[2] - 64);
            return new[] {plyFrom, plyTo};
        }

        private string LongAlgebraicNotationFromIndex(int plyFrom, int plyTo)
        {
            return _algebraicBoard[plyFrom] + _algebraicBoard[plyTo];
        }

        /*private void SetupBoardFromFEN(string fen)
        {
            for (int i = 0; i < fen.Length; i++)
            {
                var c = fen[i];
                if (c - '0' > 8 || c - '0' <= 0)
                {
                    for (int j = 0; j < c - '0'; j++)
                    {
                        _board[_board64[j]] = 0;
                        i++;
                    }
                }
                else if (c == 'p')
                {
                    _board[_board64[i]] = 7;
                }
                else if (c == '')
                {
                    
                }

                
            }
        }*/
    }
}