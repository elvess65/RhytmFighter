using UnityEngine;

namespace RhytmFighter.Level.Scheme.View
{
    /// <summary>
    /// Base scheme view
    /// </summary>
    public abstract class AbstractSchemeView : MonoBehaviour
    {
        protected readonly Color NORMAL_VIEW_COLOR = Color.white;
        protected readonly Color CURRENT_VIEW_COLOR = Color.green;
        

        private Material m_Mat;


        public virtual void ShowAsNormal() => ApplyColorToMaterial(NORMAL_VIEW_COLOR);

        public void ShowAsCurrent() => ApplyColorToMaterial(CURRENT_VIEW_COLOR); 


        protected void ApplyColorToMaterial(Color color) => m_Mat.color = color;

        protected void Initialize(string name)
        {
            InitializeRenderer();
            ShowAsNormal();
            SetName(name);
        }


        void InitializeRenderer()
        {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            m_Mat = new Material(Shader.Find("Unlit/Color"));
            mr.sharedMaterial = m_Mat;
        }

        void SetName(string name) => gameObject.name = name;
    }
}
