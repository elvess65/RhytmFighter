using RhytmFighter.Assets;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core.Enums;
using RhytmFighter.Enviroment.Effects;
using RhytmFighter.UI.Bar;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public class PlayerView : AbstractBattleNPCView
    {
        private System.Action m_InternalTeleportEvent;
        private DoubleBarBehaviour m_HealthBarBehaviour;
        private iMovementStrategy m_TeleportStrategy;
        private MovementStrategyTypes m_CurrentMoveStrategyType = MovementStrategyTypes.Bezier;


        public override void Initialize(float moveSpeed)
        {
            base.Initialize(moveSpeed);

            m_TeleportStrategy = new Teleport_MovementStrategy(transform);
            m_TeleportStrategy.OnMovementFinished += MovementFinishedHandler;
        }

        #region Movement
        public override void StartMove(Vector3[] path)
        {
            switch (m_CurrentMoveStrategyType)
            {
                case MovementStrategyTypes.Bezier:
                    base.StartMove(path);
                    break;
                case MovementStrategyTypes.Teleport:
                    Teleport(path);
                    break;
            }
        }

        public void SwitchMoveStrategy(MovementStrategyTypes strategyType)
        {
            m_CurrentMoveStrategyType = strategyType;
        }

        public void FinishFocusing()
        {
            FinishRotate();
        }


        void Teleport(Vector3[] path)
        {
            //Rotate in move direction
            m_TeleportStrategy.RotateTo(Quaternion.LookRotation(path[0] - transform.position));

            //Create teleport event
            m_InternalTeleportEvent = delegate ()
            {
                m_OnInternalOtherAnimationEvent -= m_InternalTeleportEvent;

                //Start teleport
                m_TeleportStrategy.StartMove(path);

                //Show effect
                AssetsManager.GetPrefabAssets().InstantiatePrefab<AbstractVisualEffect>(AssetsManager.GetPrefabAssets().TeleportEffectPrefab,
                                                                                        DefenceSpawnParent.position, 
                                                                                        transform.rotation * Quaternion.Euler(0, 180, 0)).ScheduleHideView();
            };

            //m_OnInternalOtherAnimationEvent += m_InternalTeleportEvent;
            m_AnimationController.PlayAnimation(AnimationTypes.Teleport);
            m_InternalTeleportEvent.Invoke();
        }
        #endregion

        #region UI
        protected override void CreateHealthBar()
        {
            m_HealthBarBehaviour = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().PlayerHealthBarPrefab);
            m_HealthBarBehaviour.RectTransform.SetParent(Core.GameManager.Instance.ManagersHolder.UIManager.PlayerUIParent);
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
