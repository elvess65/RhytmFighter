using System.Collections.Generic;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Objects.View;
using UnityEngine;

namespace RhytmFighter.Assets
{
    [CreateAssetMenu(fileName = "New PrefabAssets", menuName = "Assets/PrefabLibrary", order = 101)]
    public class PrefabAssets : ScriptableObject
    {
        [Header("Cells")]
        public CellView CellView_Prefab;
        [Header(" - Cell content")]
        public Abstract_CellContentView CellContent_Gate_Next_Prefab;
        public Abstract_CellContentView CellContent_Gate_Parent_Prefab;

        public Abstract_CellContentView[] CellContent_Normal_Prefabs;
        public Abstract_CellContentView[] CellContent_Obstacle_Prefabs;

        [Header("Objects")]
        public AbstractGridObjectView PlayerViewPrefab;
        public AbstractGridObjectView StandartItemViewPrefab;
        public AbstractGridObjectView StandartEnemyNPCViewPrefab;

        private Dictionary<CellTypes, Abstract_CellContentView[]> m_CellContentPrefabs;


        public void Initialize()
        {
            InitializeCellContentPrefabs();
        }


        public T InstantiatePrefab<T>(T source) where T : MonoBehaviour
        {
            return InstantiatePrefab(source, new Vector3(1000, 1000, 1000), Quaternion.identity);
        }

        public T InstantiatePrefab<T>(T source, Vector3 pos) where T : MonoBehaviour
        {
            return InstantiatePrefab(source, pos, Quaternion.identity);
        }

        public T InstantiatePrefab<T>(T source, Vector3 pos, Quaternion rotation) where T : MonoBehaviour
        {
            return Instantiate(source, pos, rotation) as T;
        }


        //Cells
        void InitializeCellContentPrefabs()
        {
            m_CellContentPrefabs = new Dictionary<CellTypes, Abstract_CellContentView[]>();
            m_CellContentPrefabs.Add(CellTypes.Normal, CellContent_Normal_Prefabs);           
            m_CellContentPrefabs.Add(CellTypes.Obstacle, CellContent_Obstacle_Prefabs);
        }

        public Abstract_CellContentView GetRandomCellContent(CellTypes cellType)
        {
            if (m_CellContentPrefabs.ContainsKey(cellType))
            {
                Abstract_CellContentView[] cellContents = m_CellContentPrefabs[cellType];
                int rndIndex = Random.Range(0, cellContents.Length);
                return cellContents[rndIndex];
            }

            return null;
        }
    }
}
