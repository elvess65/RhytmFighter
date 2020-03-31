using Frameworks.Grid.Data;

namespace RhytmFighter.Interfaces
{
    public interface iMovableModel : iMovable
    {
        void MovementFinishedReverseCallback(GridCellData cellData);
    }
}
