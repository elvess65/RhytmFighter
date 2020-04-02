using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        protected override Transform GetHealthBarParent()
        {
            return FindObjectOfType<Canvas>().transform;
        }

        protected override BarBehaviour GetHealthBarPrefab()
        {
            return Assets.AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab;
        }
    }
}
