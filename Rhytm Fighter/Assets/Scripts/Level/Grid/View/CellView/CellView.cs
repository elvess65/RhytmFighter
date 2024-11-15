﻿using Frameworks.Grid.Data;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Animation;
using RhytmFighter.Objects.Model;
using UnityEngine;

namespace Frameworks.Grid.View
{
    public class CellView : MonoBehaviour
    {
        public System.Action<AbstractGridObjectModel> OnObjectDetected;

        public Transform ConentParent;
        public Collider ViewCollider;
        public AbstractAnimationController AnimationController;

        private Abstract_CellContentView m_CellContent;
        private iCellAppearanceStrategy m_CellAppearanceStrategy;

        public GridCellData CorrespondingCellData { get; private set; }
        public bool IsShowed => m_CellAppearanceStrategy != null && m_CellAppearanceStrategy.IsShowed;


        public void Initialize(GridCellData correspondingCellData, Abstract_CellContentView cellContent)
        {
            //Cell data
            CorrespondingCellData = correspondingCellData;

            if (cellContent == null)
                return;

            //Cell content
            m_CellContent = cellContent;
            m_CellContent.transform.parent = ConentParent;
            m_CellContent.transform.localPosition = Vector3.zero;
            m_CellContent.Initialize();

            //Cell appearance
            m_CellAppearanceStrategy = new Animation_CellAppearanceStrategy(AnimationController, ViewCollider, m_CellContent);
            m_CellAppearanceStrategy.OnShowed += CellContentShowedHandler;
        }

        public void ShowCell()
        {
            if (IsShowed)
                return;

            //Make data visited
            if (!CorrespondingCellData.IsDiscovered)
                CorrespondingCellData.IsDiscovered = true;

            //Mark data as showed (for correct pathfinding)
            CorrespondingCellData.IsShowed = true;

            if (m_CellContent == null)
                return;

            //Affect visual
            m_CellAppearanceStrategy.Show();

            //If cell contains object
            if (CorrespondingCellData.HasObject)
            {
                //Notify about object detection
                OnObjectDetected?.Invoke(CorrespondingCellData.GetObject());
            }
        }

        public void HideCell(bool hideImmdeiate)
        {
            if (!IsShowed)
                return;

            //Mark data as hided (for correct pathfinding)
            CorrespondingCellData.IsShowed = false;

            if (m_CellContent == null)
                return;

            //Affect visual
            m_CellAppearanceStrategy.Hide(hideImmdeiate);

            //If cell contains object
            if (CorrespondingCellData.HasObject)
            {
                //Hide object graphics
                CorrespondingCellData.GetObject().HideView();
            }
        }


        private void CellContentShowedHandler()
        {
            if (CorrespondingCellData.HasObject)
                CorrespondingCellData.GetObject().ShowView(this);
        }
    }
}
