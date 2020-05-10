using RhytmFighter.Assets;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core.Enums;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        private DoubleBarBehaviour m_HealthBarBehaviour;
        private iMovementStrategy m_TeleportStrategy;
        private MovementStrategyTypes m_CurrentMoveStrategyType = MovementStrategyTypes.Bezier;


        public override void Initialize(float moveSpeed)
        {
            base.Initialize(moveSpeed);

            m_TeleportStrategy = new Teleport_MovementStrategy(transform);
            m_TeleportStrategy.OnMovementFinished += MovementFinishedHandler;
        }

        public override void NotifyView_StartMove(Vector3[] path)
        {
            switch (m_CurrentMoveStrategyType)
            {
                case MovementStrategyTypes.Bezier:
                    base.NotifyView_StartMove(path);
                    break;
                case MovementStrategyTypes.Teleport:
                    Teleport(path);
                    break;
            }
        }

        public void NotifyView_SwitchMoveStrategy(MovementStrategyTypes strategyType)
        {
            m_CurrentMoveStrategyType = strategyType;
        }


        void Teleport(Vector3[] pos)
        {
            m_AnimationController.PlayAnimation(AnimationTypes.Teleport);

            //Start teleport
            m_TeleportStrategy.StartMove(pos);

            //Show effect
            GameObject teleportEffect = AssetsManager.GetPrefabAssets().InstantiateGameObject(AssetsManager.GetPrefabAssets().TeleportEffectPrefab,
                                            DefenceSpawnParent.position, transform.rotation * Quaternion.Euler(0, 180, 0));
            Destroy(teleportEffect, 2);
        }

        #region UI
        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(FindObjectOfType<Canvas>().transform);
            m_HealthBarBehaviour.RectTransform.anchoredPosition3D = Vector3.zero;
        }

        protected override void UpdateHealthBar()
        {
            m_HealthBarBehaviour.SetProgress(m_ModelAsBattleModel.HealthBehaviour.HP, m_ModelAsBattleModel.HealthBehaviour.MaxHP);
        }

        protected override void DestroyHealthBar()
        {
            UpdateHealthBar();
        }
        #endregion
    }
}
