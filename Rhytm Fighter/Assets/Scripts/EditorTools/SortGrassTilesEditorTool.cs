using Frameworks.Grid.View.Cell;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RhytmFighter.EditorTools
{
    public class SortGrassTilesEditorTool : MonoBehaviour
    {
        public Abstract_CellContentView ContentViewPrefab;

        public void Sort()
        {
            Dictionary<int, GameObject> m_SortedParents = new Dictionary<int, GameObject>();

            //Sort tiles
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform tile = transform.GetChild(i);
                MeshFilter tileMF = tile.GetComponent<MeshFilter>();
                if (tileMF == null)
                    continue;

                string meshName = tileMF.sharedMesh.name;
                int meshHash = meshName.GetHashCode();

                if (m_SortedParents.ContainsKey(meshHash))
                {
                    tile.parent = m_SortedParents[meshHash].transform;
                    i--;
                }
                else
                {
                    GameObject tileParent = new GameObject();
                    tileParent.name = meshName;
                    tileParent.transform.parent = transform;

                    tile.parent = tileParent.transform;
                    m_SortedParents[meshHash] = tileParent;
                    i--;
                }
            }

            int initPos = 0;
            int groupIndex = 0;
            GameObject groupParent;
            foreach (GameObject tilesParent in m_SortedParents.Values)
            {
                //Group
                groupParent = new GameObject();
                groupParent.name = "Group " + groupIndex;

                //Sort between tiles and obstacles
                List<Transform> tiles = new List<Transform>();
                List<Transform> obstacles = new List<Transform>();
                for (int i = 0; i < tilesParent.transform.childCount; i++)
                {
                    Transform tile = tilesParent.transform.GetChild(i);

                    if (IsObstacle(tile))
                        obstacles.Add(tile);
                    else
                        tiles.Add(tile);
                }

                int tileIndex = 0;
                int localGroupIndex = 0;

                //Obstacles
                GameObject groupObstacles = new GameObject();
                groupObstacles.name = $"Group {groupIndex} obstacles";
                groupObstacles.transform.parent = groupParent.transform;

                foreach (Transform tile in obstacles)
                {
                    //Position tile
                    tile.transform.localPosition = new Vector3(initPos, 0, localGroupIndex++ * 3);

                    //Create content view
                    Abstract_CellContentView contentView = PrefabUtility.InstantiatePrefab(ContentViewPrefab) as Abstract_CellContentView;
                    contentView.Graphics.transform.GetChild(0).gameObject.SetActive(false);
                    contentView.transform.parent = groupObstacles.transform;
                    contentView.transform.position = tile.transform.position;
                    contentView.gameObject.name = $"CellContent_Obstacle_Group_{groupIndex}_Tile_{tileIndex++}";

                    //Position tile
                    tile.transform.parent = contentView.Graphics.transform;
                    tile.transform.localScale = tile.transform.localScale * 0.5f;
                    tile.transform.localPosition = tile.transform.localPosition - new Vector3(0, 0.5f, 0);

                    //Create prefab
                    string dirPath = $"Assets/TestTiles/{groupParent.name}/{groupObstacles.name}";
                    string localPath = $"{dirPath}/{contentView.name}.prefab";

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
                    {
                        //Create dialog to ask if User is sure they want to overwrite existing prefab
                        if (EditorUtility.DisplayDialog("Are you sure?",
                                "The prefab already exists. Do you want to overwrite it?",
                                "Yes",
                                "No"))
                        //If the user presses the yes button, create the Prefab
                        {
                            CreatePrefab(contentView.gameObject, localPath);
                        }
                    }
                    //If the name doesn't exist, create the new Prefab
                    else
                    {
                        Debug.Log(contentView.gameObject.name + " is not a prefab, will convert");
                        CreatePrefab(contentView.gameObject, localPath);
                    }
                }

                tileIndex = 0;
                localGroupIndex++;

                GameObject groupTiles = new GameObject();
                groupTiles.name = $"Group {groupIndex} tiles";
                groupTiles.transform.parent = groupParent.transform;

                foreach (Transform tile in tiles)
                {
                    //Position tile
                    tile.transform.localPosition = new Vector3(initPos, 0, localGroupIndex++ * 3);

                    //Create content view
                    Abstract_CellContentView contentView = PrefabUtility.InstantiatePrefab(ContentViewPrefab) as Abstract_CellContentView;
                    contentView.Graphics.transform.GetChild(0).gameObject.SetActive(false);
                    contentView.transform.parent = groupTiles.transform;
                    contentView.transform.position = tile.transform.position;
                    contentView.gameObject.name = $"CellContent_Normal_Group_{groupIndex}_Tile_{tileIndex++}";

                    //Position tile
                    tile.transform.parent = contentView.Graphics.transform;
                    tile.transform.localScale = tile.transform.localScale * 0.5f;
                    tile.transform.localPosition = tile.transform.localPosition - new Vector3(0, 0.5f, 0);

                    //Set the path as within the Assets folder, and name it as the GameObject's name with the .prefab format
                    string dirPath = $"Assets/TestTiles/{groupParent.name}/{groupTiles.name}";
                    string localPath = $"{dirPath}/{contentView.name}.prefab";

                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);

                    if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
                    {
                        //Create dialog to ask if User is sure they want to overwrite existing prefab
                        if (EditorUtility.DisplayDialog("Are you sure?",
                                "The prefab already exists. Do you want to overwrite it?",
                                "Yes",
                                "No"))
                        //If the user presses the yes button, create the Prefab
                        {
                            CreatePrefab(contentView.gameObject, localPath);
                        }
                    }
                    //If the name doesn't exist, create the new Prefab
                    else
                    {
                        Debug.Log(contentView.gameObject.name + " is not a prefab, will convert");
                        CreatePrefab(contentView.gameObject, localPath);
                    }
                }

                initPos += 10;
                groupIndex++;
            }
        }

        void CreatePrefab(GameObject obj, string localPath)
        {
            Debug.Log("Create prefab " + obj + " " + localPath);

            PrefabUtility.SaveAsPrefabAsset(obj, localPath, out bool success);
            Debug.Log(success);
        }


        private bool IsObstacle(Transform tile)
        {
            return tile.FindDeepChild("Tree") != null;
        }
    }

    public static class TransformExtension
    {
        public static Transform FindDeepChild(this Transform aParent, string aName)
        {
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(aParent);
            while (queue.Count > 0)
            {
                var c = queue.Dequeue();
                if (c.name.Contains(aName))
                    return c;
                foreach (Transform t in c)
                    queue.Enqueue(t);
            }
            return null;
        }
    }
}
