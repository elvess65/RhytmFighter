﻿using Frameworks.Grid.Data;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Objects.Model;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class CellView : MonoBehaviour
    {
        public System.Action<AbstractGridObjectModel> OnObjectDetected;

        private Abstract_CellContent m_CellContent;
        private iCellAppearanceStrategy m_CellAppearanceStrategy;

        public GridCellData CorrespondingCellData { get; private set; }
        public bool IsShowed => m_CellAppearanceStrategy.IsShowed;


        public void Initialize(GridCellData correspondingCellData, Abstract_CellContent cellContent)
        {
            //Cell data
            CorrespondingCellData = correspondingCellData;

            //Cell content
            m_CellContent = cellContent; 
            m_CellContent.transform.parent = transform;
            m_CellContent.transform.localPosition = Vector3.zero;

            //Cell appearance
            m_CellAppearanceStrategy = new ScaleUp_CellAppearanceStrategy(transform);
        }

        public void ShowCell()
        {
            if (IsShowed)
                return;

            //Show cell
            m_CellAppearanceStrategy.Show();

            //Make cell visited
            if (!CorrespondingCellData.IsVisited)
                CorrespondingCellData.IsVisited = true;

            //If cell contains object
            if (CorrespondingCellData.HasObject)
            {
                //Show object graphics
                CorrespondingCellData.GetObject().ShowView(this);

                //Notify about object detection
                OnObjectDetected?.Invoke(CorrespondingCellData.GetObject());
            }
        }

        public void HideCell()
        {
            if (!IsShowed)
                return;

            m_CellAppearanceStrategy.Hide();

            //If cell contains object
            if (CorrespondingCellData.HasObject)
            {
                //Hide object graphics
                CorrespondingCellData.GetObject().HideView();
            }
        }
    }
}
