using System.Collections.Generic;

namespace KCE.BoardRepresentation.MoveGeneration
{
    public interface IMoveGeneration
    {
        List<int> MoveGeneration();
    }
}