using Frameworks.Grid.Data;

namespace RhytmFighter.Interfaces
{
    public interface iMovableModel : iMovable
    {
        GridCellData CorrespondingCell { get; }

        void MovementFinishedReverseCallback(GridCellData cellData);
    }
}
