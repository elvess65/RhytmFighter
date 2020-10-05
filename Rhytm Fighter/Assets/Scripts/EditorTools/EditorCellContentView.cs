using UnityEngine;
using UnityEditor;
using Frameworks.Grid.View.Cell;

namespace RhytmFighter.EditorTools
{
    /// <summary>
    /// Расширение для быстрого сбора компонентов, ответственных за рендереры разных типох элементов
    /// </summary>
    
    [CustomEditor(typeof(SimpleCellContentView))]
    public class EditorCellContentView : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            SimpleCellContentView cellView = (SimpleCellContentView)target;
            if (GUILayout.Button(new GUIContent("Refresh renderers", "Собрать все рендер-компоненты и назначить их соответствующим полям")))
            {
                cellView.RefreshRenderers();
            }
        }
    }

    /// <summary>
    /// Расширение для ячеек врат
    /// </summary>

    [CustomEditor(typeof(GateCellContentView))]
    public class EditorGateCellContentView : EditorCellContentView
    {
     
    }
}
