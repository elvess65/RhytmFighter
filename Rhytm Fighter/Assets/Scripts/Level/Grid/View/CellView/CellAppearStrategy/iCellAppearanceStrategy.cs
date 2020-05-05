namespace Frameworks.Grid.View.Cell
{
    public interface iCellAppearanceStrategy
    {
        event System.Action OnShowed;
        event System.Action OnHided;

        bool IsShowed { get; }

        void Show();
        void Hide(bool hideImmdeiate);        
    }
}
