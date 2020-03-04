using RhytmFighter.Level.Data;
using UnityEngine;

namespace RhytmFighter.Level.Scheme.View
{
    /// <summary>
    /// Scheme view for node visualization
    /// </summary>
    public class SchemeNodeView : AbstractSchemeView
    {
        public static Color LEFT_NODE_COLOR = Color.yellow;
        public static Color RIGHT_NODE_COLOR = Color.blue;
        public static Color PARENT_NODE_COLOR = Color.cyan;

        public LevelNodeData NodeData { get; private set; }

        private const float m_LINE_RENDERER_WIDTH = 0.05f;


        public void Initialize(LevelNodeData nodeData)
        {
            NodeData = nodeData;

            Initialize($"Node {NodeData.ID}");
        }

        public void AddConnectionRenderer(Vector3 pos, Color color, Vector3 offset)
        {
            LineRenderer lineRenderer = CreateConnectionLineRenderer(color);

            lineRenderer.SetPosition(0, transform.position + offset);
            lineRenderer.SetPosition(1, pos + offset);
        }

        public void ShowAsLinkedNode(bool leftNode) => ApplyColorToMaterial(leftNode ? LEFT_NODE_COLOR : RIGHT_NODE_COLOR);

        public void ShowAsParentNode() => ApplyColorToMaterial(PARENT_NODE_COLOR);


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
