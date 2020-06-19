using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Material))]
[CanEditMultipleObjects]
public class MaterialCopyEditor : Editor
{
    private Material m_Current;
    private Material m_SourceMaterial;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if (m_Current == null)
            m_Current = (Material)target;

        GUILayout.BeginHorizontal();
            GUILayout.Label("Copy from Material:");
            m_SourceMaterial = EditorGUILayout.ObjectField(m_SourceMaterial, typeof(Material), false) as Material;
        GUILayout.EndHorizontal();

        if (m_SourceMaterial != null && m_SourceMaterial.name == m_Current.name)
            m_SourceMaterial = null;

        if (m_SourceMaterial != null)
        {
            if (GUILayout.Button("Copy Colors"))
                CopyColorsHandler();

            if (GUILayout.Button("Copy Fog"))
                CopyFogHandler();

            if (GUILayout.Button("Copy All"))
                CopyAllHandler();
        }
    }

    private void CopyColorsHandler()
    {
        foreach (object mat in Selection.objects)
        {
            Material material = mat as Material;
            if (material != null)
                CopyColors(m_SourceMaterial, material);
        }

        m_SourceMaterial = null;
    }

    private void CopyFogHandler()
    {
        foreach (object mat in Selection.objects)
        {
            Material material = mat as Material;
            if (material != null)
                CopyFog(m_SourceMaterial, material);
        }

        m_SourceMaterial = null;
    }

    private void CopyAllHandler()
    {
        foreach (object mat in Selection.objects)
        {
            Material material = mat as Material;
            if (material != null)
                CopyAll(m_SourceMaterial, material);
        }

        m_SourceMaterial = null;
    }


    private void CopyColors(Material source, Material target)
    {
        Debug.Log($"Copy Colors from {source} to {target}");
    }

    private void CopyFog(Material source, Material target)
    {
        Debug.Log($"Copy Fog from {source} to {target}");
    }

    private void CopyAll(Material source, Material target)
    {
        Debug.Log($"Copy All from {source} to {target}");
    }
}
