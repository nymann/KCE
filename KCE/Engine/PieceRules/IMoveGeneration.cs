using System.Collections.Generic;

namespace KCE.BoardRepresentation.PieceRules
{
    public interface IMoveGeneration
    {
        List<int> MoveGeneration();
    }
}