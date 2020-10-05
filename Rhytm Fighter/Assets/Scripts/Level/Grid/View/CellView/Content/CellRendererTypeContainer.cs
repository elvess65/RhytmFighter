using RhytmFighter.Persistant.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace Frameworks.Grid.View.Cell
{
    /// <summary>
    /// Компонент для сохранения типа рендерера и упрощения сбора рендереров в редакторе
    /// </summary>
    public class CellRendererTypeContainer : MonoBehaviour
    {
        [Tooltip("Тип рендерера")]
        public ContentRendererTypes RendererType;

        [Tooltip("Для наглядности. Заполняется автоматически")]
        public List<MeshRenderer> ContentRenderers;

        /// <summary>
        /// Вызывается из редактора
        /// </summary>
        public void LoadRenderers()
        {
            ContentRenderers = new List<MeshRenderer>();

            if (RendererType != ContentRendererTypes.Cell)
            {
                MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer renderer in childRenderers)
                    ContentRenderers.Add(renderer);
            }
            else
                ContentRenderers.Add(GetComponent<MeshRenderer>());
        }

        public void ApplyMaterialToRenderers(Material material)
        {
            foreach (MeshRenderer renderer in ContentRenderers)
            {
                renderer.sharedMaterial = material;
            }
        }
    }
}
