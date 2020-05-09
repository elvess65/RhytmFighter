using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Characters.Movement;
using RhytmFighter.Core;
using RhytmFighter.Core.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class BattleController : iUpdatable
    {
        public System.Action OnPrepareForBattle;
        public System.Action OnEnemyDestroyed;
        public System.Action OnBattleStarted;
        public System.Action OnBattleFinished;

        private int m_TargetTick;
        private Level.LevelController m_LevelController;
        private CameraSystem.CameraController m_CameraController;
        private ModelMovementController m_EnemyMovementController;
        private PlayerCharacterController m_PlayerCharacterController;

        private Dictionary<int, iBattleObject> m_PendingEnemies;
        
        public iBattleObject Player { get; set; }

        private const int m_DISTANCE_ADJUSTEMENT_RANGE = 4;     //Max range (in cells) to find adjust disatnce cell
        private const int m_TICKS_BEFORE_ACTIVATING_ENEMY = 1;  //Delay (in ticks) after first enemy found and start battle
        private const int m_TICKS_BEFORE_FINISHING_BATTLE = 2;  //Delay (in ticks) after last enemy destroyed and finish battle
        private const float m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_DISTANCE = 5;    //Min distance between cells to start adjust distance 
        private const float m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_ROTATION = 5;    //Min rotation between player and enemy to adjust rotation


        public BattleController(Level.LevelController levelController, CameraSystem.CameraController cameraController, PlayerCharacterController playerCharacterController)
        {
            m_LevelController = levelController;
            m_CameraController = cameraController;
            m_PlayerCharacterController = playerCharacterController;

            m_PendingEnemies = new Dictionary<int, iBattleObject>();
            m_EnemyMovementController = new ModelMovementController(levelController);

            OnPrepareForBattle += PrepareForBattleEventHandler;
            OnEnemyDestroyed += TryActivateNextEnemy;
        }

        public void AddEnemy(iBattleObject battleObject)
        {
            //Add enemy to pending list
            if (!m_PendingEnemies.ContainsKey(battleObject.ID))
            {
                m_PendingEnemies.Add(battleObject.ID, battleObject);

                battleObject.Target = Player;
                battleObject.OnDestroyed += EnemyDestroyedHandler;
            }

            //Start battle with adding the first enemy
            if (m_PendingEnemies.Count == 1)
                OnPrepareForBattle?.Invoke();
        }

        public void ProcessEnemyActions(int currentTick)
        {
            Player.Target.AI.ExecuteAction(currentTick);
        }

        public void PerformUpdate(float deltaTime)
        {
            m_EnemyMovementController?.PerformUpdate(deltaTime);
            Player?.Target?.AI?.PerformUpdate(deltaTime);
        }

        public iBattleObject GetClosestEnemy(iBattleObject relativeToBattleObject)
        {
            iBattleObject result = null;
            float closestSqrDistToEnemy = float.MaxValue;

            //If relativeObject already has target set init closestDist to sqr dist to it
            if (relativeToBattleObject.Target != null)
            {
                closestSqrDistToEnemy = (relativeToBattleObject.ViewPosition - relativeToBattleObject.Target.ViewPosition).sqrMagnitude;
                result = relativeToBattleObject.Target;
            }

            foreach(iBattleObject enemy in m_PendingEnemies.Values)
            {
                float sqrDistToEnemy = (relativeToBattleObject.ViewPosition - enemy.ViewPosition).sqrMagnitude;
                if (sqrDistToEnemy < closestSqrDistToEnemy)
                {
                    closestSqrDistToEnemy = sqrDistToEnemy;
                    result = enemy;
                }
            }

            return result;
        }

        
        private void EnemyDestroyedHandler(iBattleObject enemy)
        {
            //Clear players target
            if (Player.Target.ID == enemy.ID)
                Player.Target = null;

            OnEnemyDestroyed?.Invoke();
        }

        private void PrepareForBattleEventHandler()
        {
            m_TargetTick = Rhytm.RhytmController.GetInstance().CurrentTick + m_TICKS_BEFORE_ACTIVATING_ENEMY;
            Rhytm.RhytmController.GetInstance().OnTick += ActivateFirstEnemyOnTick;
        }


        private void FinishBattleOnTick(int currentTick)
        {
            if (currentTick >= m_TargetTick)
            {
                Rhytm.RhytmController.GetInstance().OnTick -= FinishBattleOnTick;

                m_CameraController.ActivateCamera(CameraTypes.Main);
                m_CameraController.SubscribeForBlendingFinishedEvent(() => m_CameraController.PeekMemberFromTargetGroup());

                OnBattleFinished?.Invoke();
            }
        }

        private void ActivateFirstEnemyOnTick(int currentTick)
        {
            if (currentTick >= m_TargetTick)
            {
                Rhytm.RhytmController.GetInstance().OnTick -= ActivateFirstEnemyOnTick;
                TryActivateNextEnemy();
            }
        }


        private void TryActivateNextEnemy()
        {
            //Get closest enemy to player
            iBattleObject closestEnemy = GetClosestEnemy(Player);

            //If enemy exists
            if (closestEnemy != null)
            {
                Player.Target = closestEnemy;
                ActivateEnemy(closestEnemy);
            }
            else
            {
                m_TargetTick = Rhytm.RhytmController.GetInstance().CurrentTick + m_TICKS_BEFORE_FINISHING_BATTLE;
                Rhytm.RhytmController.GetInstance().OnTick += FinishBattleOnTick;
            }
        }

        private void ActivateEnemy(iBattleObject enemy)
        {
            //Start focusing player
            m_PlayerCharacterController.Focus(enemy);

            //Push enemy to camera target group
            m_CameraController.PeekMemberFromTargetGroup();

            //Remove target from pending
            if (m_PendingEnemies.ContainsKey(enemy.ID))
                m_PendingEnemies.Remove(enemy.ID);

            //Sset model for movement
            m_EnemyMovementController.SetModel(enemy as iMovableModel);

            //Check distance between player and target
            SquareGrid curentGrid = m_LevelController.Model.GetCurrenRoomData().GridData;
            float distanceBetweenPlayerAndEnemy = curentGrid.GetDistanceBetweenCells(Player.CorrespondingCell, enemy.CorrespondingCell);

            //If distance is less than treshold - adjust movement or start battle
            if (distanceBetweenPlayerAndEnemy < m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_DISTANCE)
                AdjustDistanceForObject(enemy);
            else
            {
                CheckRotationToPlayer(enemy);
                FocusBattleCamera(enemy.ViewTransform);
            }
        }


        private void AdjustDistanceForObject(iBattleObject battleObject)
        {
            float distanceToEnemy = float.MinValue;
            float distanceToPlayer = float.MinValue;

            GridCellData adjustedCell = null;
            Vector3[] pathToAdjustedCell = null;
            SquareGrid currentGrid = m_LevelController.Model.GetCurrenRoomData().GridData;
            List<GridCellData> neighbourCells = currentGrid.GetWalkableAndVisibleCellsInRange(battleObject.CorrespondingCell.X,
                                                                                              battleObject.CorrespondingCell.Y,
                                                                                              m_DISTANCE_ADJUSTEMENT_RANGE);

            while (neighbourCells.Count > 0 && pathToAdjustedCell == null)
            {
                for (int i = 0; i < neighbourCells.Count; i++)
                {
                    GridCellData currentCell = neighbourCells[i];

                    float distToPlayer = currentGrid.GetDistanceBetweenCells(Player.CorrespondingCell, currentCell);
                    float distToEnemy = currentGrid.GetDistanceBetweenCells(battleObject.CorrespondingCell, currentCell);

                    //Find the most distant cell from player and enemy
                    if (distToPlayer >= distanceToPlayer && distToEnemy >= distanceToEnemy)
                    {
                        adjustedCell = currentCell;
                        distanceToPlayer = distToPlayer;
                        distanceToEnemy = distToEnemy;
                    }
                }

                //Try to find path to the adjusted cell
                pathToAdjustedCell = currentGrid.FindPath(battleObject.CorrespondingCell, adjustedCell);

                //Remove adjusted cell from list
                if (neighbourCells.Contains(adjustedCell))
                    neighbourCells.Remove(adjustedCell);
            }

            if (adjustedCell != null)
            {
                CellView view = m_LevelController.RoomViewBuilder.GetCellVisual(m_LevelController.Model.GetCurrenRoomData().ID,
                                                                                adjustedCell.X,
                                                                                adjustedCell.Y);

                m_EnemyMovementController.OnMovementFinished += EnemyAdjustementMovementFinished;
                m_EnemyMovementController.MoveCharacter(view);

                FocusBattleCamera(view.transform);
            }
            else
            {
                CheckRotationToPlayer(battleObject);
                FocusBattleCamera(battleObject.ViewTransform);
            }
        }

        private void EnemyAdjustementMovementFinished(GridCellData cell)
        {
            m_EnemyMovementController.OnMovementFinished -= EnemyAdjustementMovementFinished;
            CheckRotationToPlayer(Player.Target);
        }

        private void CheckRotationToPlayer(iBattleObject battleObject)
        {
            Vector3 dirToPlayer = Player.ViewPosition - Player.Target.ViewPosition;

            if (Vector3.Angle(dirToPlayer, Player.Target.ViewForwardDir) > m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_ROTATION)
            {
                m_EnemyMovementController.RotateCharacter(Quaternion.LookRotation(dirToPlayer));
                m_EnemyMovementController.OnRotationFinished += RotationToPlayerFinished;
            }
            else 
                StartBattle();
        }

        private void RotationToPlayerFinished()
        {
            m_EnemyMovementController.OnRotationFinished -= RotationToPlayerFinished;
            StartBattle();
        }


        private void StartBattle()
        {
            //Stop focusing
            m_PlayerCharacterController.StopFocusing();

            OnBattleStarted?.Invoke();
        }

        private void FocusBattleCamera(Transform target)
        {
            m_CameraController.PushMemberToTargetGroup(target, 1.25f);

            Quaternion targetCameraRotation = Quaternion.LookRotation(target.position - Player.ViewPosition);
            Vector3 cameraEuler = GameManager.Instance.CamerasHolder.VCamBattle.transform.localEulerAngles;
            cameraEuler.y = targetCameraRotation.eulerAngles.y + m_CameraController.GetNoiseForBattleCamera();
            targetCameraRotation.eulerAngles = cameraEuler;

            GameManager.Instance.CamerasHolder.VCamBattle.transform.localEulerAngles = targetCameraRotation.eulerAngles;
            m_CameraController.ActivateCamera(CameraTypes.Battle);
        }

    }
}
