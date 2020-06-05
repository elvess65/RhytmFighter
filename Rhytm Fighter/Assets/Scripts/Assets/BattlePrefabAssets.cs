using System.Collections.Generic;
using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using Frameworks.Grid.View.Cell;
using RhytmFighter.Assets.Abstract;
using RhytmFighter.Battle.Command.View;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.Objects.View;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Assets
{
    [CreateAssetMenu(fileName = "New PrefabAssets", menuName = "Assets/PrefabLibrary", order = 101)]
    public class BattlePrefabAssets : PrefabAssets
    {
        [Header("Cells")]
        public CellView CellView_Prefab;
        [Header(" - Cell content")]
        public Abstract_CellContentView CellContent_Gate_Next_Prefab;
        public Abstract_CellContentView CellContent_Gate_Parent_Prefab;

        public Abstract_CellContentView[] CellContent_Normal_Prefabs;
        public Abstract_CellContentView[] CellContent_Obstacle_Prefabs;
        public Abstract_CellContentView[] CellContent_Normal_Decorated_Prefabs;

        [Header("Objects")]
        public AbstractGridObjectView PlayerViewPrefab;
        public AbstractGridObjectView StandartItemViewPrefab;
        public AbstractGridObjectView StandartEnemyNPCViewPrefab;

        [Header("UI")]
        public SingleBarBehaviour EnemyHealthBarPrefab;
        public DoubleBarBehaviour PlayerHealthBarPrefab;

        [Header("Projectile")]
        public AbstractProjectileView ProjectilePrefab;
        public AbstractVisualEffect ProjectileImpactEffectPrefab;

        [Header("Defence")]
        public AbstractDefenceView DefencePrefab;
        public AbstractVisualEffect DefenceImpactEffectPrefab;

        [Header("Enviroment")]
        public AbstractVisualEffect PointerPrefab;
        public AbstractVisualEffect TeleportEffectPrefab;
        public AbstractVisualEffect DestroyEffectPrefab;
        public AbstractVisualEffect HealEffectPrefab;


        public override void Initialize()
        {
            base.Initialize();
        }

        public Abstract_CellContentView GetRandomCellContent(CellTypes cellType, bool getDecorated)
        {
            switch(cellType)
            {
                case CellTypes.Normal:
                    if (getDecorated)
                        return CellContent_Normal_Decorated_Prefabs[Random.Range(0, CellContent_Normal_Decorated_Prefabs.Length)];

                    return CellContent_Normal_Prefabs[Random.Range(0, CellContent_Normal_Prefabs.Length)];

                case CellTypes.Obstacle:
                    return CellContent_Obstacle_Prefabs[Random.Range(0, CellContent_Obstacle_Prefabs.Length)];
            }

            return null;
        }
    }
}
