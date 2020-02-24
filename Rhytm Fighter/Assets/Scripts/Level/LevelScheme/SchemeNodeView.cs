using RhytmFighter.Level.Data;
using UnityEngine;

namespace RhytmFighter.Level.Scheme
{
    /// <summary>
    /// Scheme view for node visualization
    /// </summary>
    public class SchemeNodeView : MonoBehaviour
    {
        public static Color LEFT_NODE_COLOR = Color.yellow;
        public static Color RIGHT_NODE_COLOR = Color.blue;
        public static Color PARENT_NODE_COLOR = Color.cyan;
        public static Color CURRENT_NODE_COLOR = Color.green;
        public static Color NORMAL_NODE_COLOR = Color.white;

        public LevelNodeData NodeData { get; private set; }

        private Material m_Mat;

        private const float m_LINE_RENDERER_WIDTH = 0.05f;


        public void Initialize(LevelNodeData nodeData)
        {
            NodeData = nodeData;
            gameObject.name += $"_{NodeData.ID}";

            MeshRenderer mr = GetComponent<MeshRenderer>();
            m_Mat = new Material(Shader.Find("Unlit/Color"));
            mr.sharedMaterial = m_Mat;

            ShowAsNormalNode();
        }

        public void AddConnectionRenderer(Vector3 pos, Color color, Vector3 offset)
        {
            LineRenderer lineRenderer = CreateConnectionLineRenderer(color);

            lineRenderer.SetPosition(0, transform.position + offset);
            lineRenderer.SetPosition(1, pos + offset);
        }

        public void ShowAsCurrentNode() { m_Mat.color = CURRENT_NODE_COLOR; Debug.Log("Show node as current " + m_Mat.color); }

        public void ShowAsLinkedNode(bool leftNode) => m_Mat.color = leftNode ? LEFT_NODE_COLOR : RIGHT_NODE_COLOR;

        public void ShowAsParentNode() => m_Mat.color = PARENT_NODE_COLOR;

        public void ShowAsNormalNode() => m_Mat.color = NORMAL_NODE_COLOR;


        LineRenderer CreateConnectionLineRenderer(Color color)
        {
            GameObject lineRendererHolder = new GameObject();
            lineRendererHolder.transform.parent = transform;
            lineRendererHolder.transform.localPosition = Vector3.zero;

            LineRenderer lineRenderer = lineRendererHolder.AddComponent<LineRenderer>();
            lineRenderer.sharedMaterial = new Material(Shader.Find("Mobile/Particles/Alpha Blended"));
            lineRenderer.startColor = color;
            lineRenderer.endColor = Color.white;
            lineRenderer.startWidth = m_LINE_RENDERER_WIDTH;
            lineRenderer.endWidth = m_LINE_RENDERER_WIDTH;

            return lineRenderer;
        }
    }
}
