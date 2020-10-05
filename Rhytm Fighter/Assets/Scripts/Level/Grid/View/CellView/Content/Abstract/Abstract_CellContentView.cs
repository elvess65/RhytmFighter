using System.Collections.Generic;
using UnityEngine;
using static RhytmFighter.Enviroment.Presets.BattleEnviromentPreset;

namespace Frameworks.Grid.View.Cell
{
    public abstract class Abstract_CellContentView : MonoBehaviour
    {
        public GameObject Graphics;
        public GameObject Effects;
        public CellRendererTypeContainer[] TypeRenderers;


        public virtual void Initialize()
        {
        }

        public void ApplyMaterials(Material cellMaterial, ContentMaterial[] contentMaterials)
        {
            foreach (CellRendererTypeContainer typeContainer in TypeRenderers)
            {
                //Apply material to ground
                if (typeContainer.RendererType == RhytmFighter.Persistant.Enums.ContentRendererTypes.Cell)
                {
                    typeContainer.ApplyMaterialToRenderers(cellMaterial);
                }
                //Apply materials to content
                else
                {
                    foreach (ContentMaterial contentMaterial in contentMaterials)
                    {
                        if (contentMaterial.Type == typeContainer.RendererType)
                        {
                            typeContainer.ApplyMaterialToRenderers(contentMaterial.MaterialSource);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Вызывается из редактора
        /// </summary>
        public void RefreshRenderers()
        {
            TypeRenderers = GetComponentsInChildren<CellRendererTypeContainer>();

            foreach (CellRendererTypeContainer renderer in TypeRenderers)
            {
                renderer.LoadRenderers();
            }
        }
    }
}
