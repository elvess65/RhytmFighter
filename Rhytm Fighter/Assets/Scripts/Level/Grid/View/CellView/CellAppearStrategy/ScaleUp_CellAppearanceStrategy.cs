using System;
using UnityEngine;

namespace Frameworks.Grid.View.Cell
{
    public class ScaleUp_CellAppearanceStrategy : iCellAppearanceStrategy
    {
        public event Action<CellView> OnShowed;
        public event Action<CellView> OnHided;

        public bool IsShowed { get; private set; }

        private Transform m_ControlledObject;
        

        public ScaleUp_CellAppearanceStrategy(Transform controlledObject)
        {
            m_ControlledObject = controlledObject;
            IsShowed = true;
        }

        public void Show()
        {
            m_ControlledObject.gameObject.SetActive(true);
            IsShowed = true;
        }

        public void Hide()
        {
            m_ControlledObject.gameObject.SetActive(false);
            IsShowed = false;
        }
    }
}
