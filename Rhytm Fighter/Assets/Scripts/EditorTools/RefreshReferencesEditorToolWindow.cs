#if UNITY_EDITOR
using Frameworks.Grid.View.Cell;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace RhytmFighter.EditorTools
{
    public class RefreshReferencesEditorToolWindow : EditorWindow
    {
        private bool m_Log = true;

        [MenuItem("Custom Tools/Refresh Tile References")]
        static void Init()
        {
            RefreshReferencesEditorToolWindow window = (RefreshReferencesEditorToolWindow)EditorWindow.GetWindow(typeof(RefreshReferencesEditorToolWindow), true, "Refresh Tile Reference");
            window.Show();
        }

        void OnGUI()
        {
            if (GUILayout.Button("Get all prefabs"))
            {
                ClearLog();

                int totalPrefabsProcessed = 0;
                List<string> assetsFolders = new List<string>();
                for (int i = 0; i < 6; i++)
                    assetsFolders.Add("Assets/Prefabs/Grid/CellContent/Group " + i);

                string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab", assetsFolders.ToArray());

                foreach (string guid in prefabGUIDs)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                    Abstract_CellContentView contentView = prefab.GetComponent<Abstract_CellContentView>();
                    if (contentView == null)
                        continue;

                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat("Prefab: {0}. Path: {1}", prefab, path);
                    strBuilder.Append("\n");

                    int mrIndex = 0;
                    MeshRenderer[] meshRenderers = prefab.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer mr in meshRenderers)
                    {
                        switch(mrIndex)
                        {
                            case 0:
                                strBuilder.Append(" - Cell Renderer: " + mr.gameObject.name);
                                break;
                            default:
                                strBuilder.Append(" - Conent Renderer: " + mr.gameObject.name);
                                break;
                        }

                        mrIndex++;
                    }

                    totalPrefabsProcessed++;

                    if (m_Log)
                        strBuilder.ToString();
                }

                Debug.Log("Operation finished. Total prefabs being processed: " + totalPrefabsProcessed);

                //foreach (KeyValuePair<CustomText, DeployUpdates> update in textsToModify)
                //{
                //    ApplyTextStyle(update.Key, update.Value.style);
                //    ApplyTextColor(update.Key, update.Value.color);
                //    EditorUtility.SetDirty(update.Key);
                //}

                //AssetDatabase.SaveAssets();
                //AssetDatabase.Refresh();
            }

            m_Log = EditorGUILayout.Toggle("Show Log", m_Log);

            //    GUILayout.Label("Base Settings", EditorStyles.boldLabel);
            //myString = EditorGUILayout.TextField("Text Field", myString);

            //groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
            //myBool = EditorGUILayout.Toggle("Toggle", myBool);
            //myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
            //EditorGUILayout.EndToggleGroup();
        }

        public void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}
#endif