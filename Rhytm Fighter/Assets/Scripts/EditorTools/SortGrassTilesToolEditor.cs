using UnityEditor;
using UnityEngine;

namespace RhytmFighter.EditorTools
{
    [CustomEditor(typeof(SortGrassTilesEditorTool))]
    public class SortGrassTilesToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SortGrassTilesEditorTool tg = (SortGrassTilesEditorTool)target;
            if (GUILayout.Button("Sort"))
            {
                tg.Sort();
            }
        }
    }
}
