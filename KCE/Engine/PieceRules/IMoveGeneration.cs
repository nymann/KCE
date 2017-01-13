using System.Collections.Generic;

namespace KCE.Engine.PieceRules
{
    public interface IMoveGeneration
    {
        List<int> MoveGeneration();
    }
}