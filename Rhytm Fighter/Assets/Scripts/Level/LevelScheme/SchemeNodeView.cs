using RhytmFighter.Level.Data;
using UnityEngine;

namespace RhytmFighter.Level.Scheme
{
    /// <summary>
    /// Scheme view for node visualization
    /// </summary>
    public class SchemeNodeView : MonoBehaviour
    {
        public LevelNodeData NodeData { get; private set; }

        public void Initialize(LevelNodeData nodeData)
        {
            NodeData = nodeData;
            gameObject.name += $"_{NodeData.ID}";
        }

        public void AddConnectionRenderer(Vector3 pos)
        {
            LineRenderer lineRenderer = CreateConnectionLineRenderer();

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, pos);
        }


        LineRenderer CreateConnectionLineRenderer()
        {
            GameObject lineRendererHolder = new GameObject();
            lineRendererHolder.transform.parent = transform;
            lineRendererHolder.transform.localPosition = Vector3.zero;

            LineRenderer lineRenderer = lineRendererHolder.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;

            return lineRenderer;
        }
    }
}
