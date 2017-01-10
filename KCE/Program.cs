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

            // Current bug.
            //new BoardRepresentation.BoardRepresentation("r3k2r/p2pqpb1/bn2pnp1/2pP4/1pN1P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq c6 0 1", 1);
            //new BoardRepresentation.BoardRepresentation("r3k2r/p2pqpb1/bnP1pnp1/8/1pN1P3/2N2Q1p/PPPBBPPP/R3K2R b KQkq - 0 1", 1);
            new BoardRepresentation.BoardRepresentation("r3k2r/p2pqpb1/bn2pnp1/2pP4/1pN1P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq c6 0 1", 2);
            Console.ReadKey();
        }
    }
}
