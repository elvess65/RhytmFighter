using Frameworks.Grid.Data;

namespace RhytmFighter.Characters.Movement
{
    public interface iMovableModel : iMovable
    {
        GridCellData CorrespondingCell { get; }

        void MovementFinishedReverseCallback(GridCellData cellData);
    }
}
