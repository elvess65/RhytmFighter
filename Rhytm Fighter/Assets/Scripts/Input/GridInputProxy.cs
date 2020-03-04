using Frameworks.Grid.View;
using UnityEngine;

namespace RhytmFighter.Input
{
    /// <summary>
    /// Converting low level inputs to grid input
    /// </summary>
    public class GridInputProxy 
    {
        public event System.Action<CellView> OnCellInput;

        /// <summary>
        /// Get CellView from low level input
        /// </summary>
        /// <param name="mousePos">Mouse screen position</param>
        public void TryGetCellFromInput(Vector3 mousePos)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit))
            {
                CellView cellView = hit.collider.transform.parent.gameObject.GetComponent<CellView>();
                if (cellView != null)
                    OnCellInput?.Invoke(cellView);
            }
        }
    }
}
