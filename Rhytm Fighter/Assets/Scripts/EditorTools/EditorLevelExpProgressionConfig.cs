using RhytmFighter.Data;
using UnityEditor;
using UnityEngine;

namespace RhytmFighter.EditorTools
{
    /// <summary>
    /// Расширение для инверсии основной кривой в прогрессии опыт-уровень
    /// </summary>

    [CustomEditor(typeof(LevelingProgressionConfig))]
    public class EditorLevelExpProgressionConfig : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            LevelingProgressionConfig progressionConfig = (LevelingProgressionConfig)target;
            if (GUILayout.Button(new GUIContent("Inverse curve", "Собрать все рендер-компоненты и назначить их соответствующим полям")))
            {
                progressionConfig.CreateInvertedCurve();
            }
        }
    }
}
