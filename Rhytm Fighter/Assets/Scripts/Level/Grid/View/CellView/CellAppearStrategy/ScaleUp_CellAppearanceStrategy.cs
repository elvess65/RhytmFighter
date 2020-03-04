using System;
using UnityEngine;

namespace Frameworks.Grid.View.Cell
{
    public class ScaleUp_CellAppearanceStrategy : iCellAppearanceStrategy
    {
        public event Action<CellView> OnShowed;
        public event Action<CellView> OnHided;

        public bool IsShowed => m_IsShowed;

        private bool m_IsShowed = false;
        private Transform m_ControlledObject;
        

        public ScaleUp_CellAppearanceStrategy(Transform controlledObject)
        {
            m_ControlledObject = controlledObject;
        }

        public void Show()
        {
            m_ControlledObject.gameObject.SetActive(true);
            m_IsShowed = true;
        }

        public void Hide()
        {
            m_ControlledObject.gameObject.SetActive(false);
            m_IsShowed = false;
        }
    }
}
