namespace Frameworks.Grid.View.Cell
{
    public interface iCellAppearanceStrategy
    {
        event System.Action<CellView> OnShowed;
        event System.Action<CellView> OnHided;

        bool IsShowed { get; }

        void Show();
        void Hide(bool hideImmdeiate);        
    }
}
