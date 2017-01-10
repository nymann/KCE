using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCE
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://www.rocechess.ch/perft.html

            // Startposition
            /*new BoardRepresentation.BoardRepresentation(
                "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");*/

            // Good testposition
            /*new BoardRepresentation.BoardRepresentation(
                "r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");*/

            // Discover promotion bugs
            /*new BoardRepresentation.BoardRepresentation(
                "n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1");*/

            // Depth 2 bug found, r3k3/1K6/8/8/8/8/8/8 w q - 0 1 ;D1 4 ;D2 49 ;D3 243 ;D4 3991 ;D5 20780 ;D6 367724
            // Expected value 49, actual 50.

            new BoardRepresentation.BoardRepresentation("r3k3/1K6/8/8/8/8/8/8 w q - 0 1");
            Console.ReadKey();
        }
    }
}
