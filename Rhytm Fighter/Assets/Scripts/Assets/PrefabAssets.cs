using System.Collections;
using System.Collections.Generic;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using Frameworks.Grid.View.Cell;
using UnityEngine;

namespace RhytmFighter.Assets
{
    [CreateAssetMenu(fileName = "New PrefabAssets", menuName = "Assets/PrefabLibrary", order = 101)]
    public class PrefabAssets : ScriptableObject
    {
        [Header("Cells")]
        public CellView CellView_Prefab;
        [Header(" - Cell content")]
        public Abstract_CellContent CellContent_Gate_Next_Prefab;
        public Abstract_CellContent CellContent_Gate_Parent_Prefab;

        public Abstract_CellContent[] CellContent_Normal_Prefabs;
        public Abstract_CellContent[] CellContent_Obstacle_Prefabs;

        private Dictionary<CellTypes, Abstract_CellContent[]> m_CellContentPrefabs;


        public void Initialize()
        {
            InitializeCellContentPrefabs();
        }


        //Cells
        void InitializeCellContentPrefabs()
        {
            m_CellContentPrefabs = new Dictionary<CellTypes, Abstract_CellContent[]>();
            m_CellContentPrefabs.Add(CellTypes.Normal, CellContent_Normal_Prefabs);           
            m_CellContentPrefabs.Add(CellTypes.Obstacle, CellContent_Obstacle_Prefabs);
        }

        public Abstract_CellContent GetRandomCellContent(CellTypes cellType)
        {
            if (m_CellContentPrefabs.ContainsKey(cellType))
            {
                Abstract_CellContent[] cellContents = m_CellContentPrefabs[cellType];
                int rndIndex = Random.Range(0, cellContents.Length);
                return cellContents[rndIndex];
            }

            return null;
        }
    }
}
