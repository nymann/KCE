using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KCE.BoardRepresentation;
using KCE.Engine.Search;

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
            //new BoardRepresentation.BoardRepresentation("r3k2r/p2pqpb1/bn2pnp1/2pP4/1pN1P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq c6 0 1", 4);
            //new Program().Stuff("r1b1k2r/ppppnppp/2n2q2/2b5/3NP3/2P1B3/PP3PPP/RN1QKB1R w KQkq - 0 1");
            //Console.ReadKey();
            new UCI().Identify();
        }

        private void Stuff(string FEN)
        {
            var helper = new Engine.Helper();
            BoardState bs = helper.BoardsetupFromFen(FEN);
            var search = new KCE.Engine.Search.Search();
            var sInfo = new SearchInfo();
            search.SearchPosition(bs, sInfo);
        }
    }
}
