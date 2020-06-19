using UnityEngine;

namespace Frameworks.Grid.View.Cell
{
    public abstract class Abstract_CellContentView : MonoBehaviour
    {
        public GameObject Graphics;
        public GameObject Effects;

        public MeshRenderer[] CellRenderers;
        public MeshRenderer[] ContentRenderers;


        public virtual void Initialize()
        {

        }

        public void ApplyMaterials(Material cellMaterial = null, Material contentMaterial = null)
        {
            ApplyMaterial(cellMaterial, CellRenderers);
            ApplyMaterial(contentMaterial, ContentRenderers);
        }

        void ApplyMaterial(Material material, MeshRenderer[] renderers)
        {
            if (material != null && renderers.Length > 0)
            {
                for (int i = 0; i < renderers.Length; i++)
                {
                    renderers[i].sharedMaterial = material;
                }
            }
        }
    }
}
