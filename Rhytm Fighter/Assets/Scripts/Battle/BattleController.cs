using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Interfaces;
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
        private bool m_ForceCameraFollowPoint;
        private Level.LevelController m_LevelController;
        private Camera.CameraController m_CameraController;
        private ModelMovementController m_EnemyMovementController;
        
        private Dictionary<int, iBattleObject> m_PendingEnemies;
        

        public iBattleObject Player { get; set; }

        private const int m_DISTANCE_ADJUSTEMENT_RANGE = 2;
        private const int m_TICKS_BEFORE_ACTIVATING_ENEMY = 1;
        private const int m_TICKS_BEFORE_FINISHING_BATTLE = 2;
        private const float m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_DISTANCE = 3;
        

        public BattleController(Level.LevelController levelController, Camera.CameraController cameraController)
        {
            m_ForceCameraFollowPoint = false;

            m_LevelController = levelController;
            m_CameraController = cameraController;

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

            //Makes camera focus battle
            if (m_ForceCameraFollowPoint)
                m_CameraController.SetFollowPoint((Player.Target.ViewPosition + Player.ViewPosition) / 2);
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


        private void EnemyDestroyedHandler(iBattleObject sender)
        {
            //Clear players target
            if (Player.Target.ID == sender.ID)
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

                m_CameraController.ClearFollowPoint();
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
            //Stop follow focus point
            if (m_ForceCameraFollowPoint)
                m_ForceCameraFollowPoint = false;

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
            //Start follow focus point
            m_ForceCameraFollowPoint = true;

            //Remove target from pending
            if (m_PendingEnemies.ContainsKey(enemy.ID))
                m_PendingEnemies.Remove(enemy.ID);

            //Check distance between player and target
            SquareGrid curentGrid = m_LevelController.Model.GetCurrenRoomData().GridData;
            float distanceBetweenTargetAndEnemy = curentGrid.GetDistanceBetweenCells(Player.CorrespondingCell, enemy.CorrespondingCell);

            //If distance is less than treshold - adjust movement or start battle
            if (distanceBetweenTargetAndEnemy < m_TRESHHOLD_BETWEEN_PLAYER_AND_ENEMY_TO_ADJUST_DISTANCE)
                AdjustDistanceForObject(enemy);
            else
                OnBattleStarted?.Invoke();
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
                m_EnemyMovementController.SetModel(battleObject as iMovableModel);
                m_EnemyMovementController.MoveCharacter(view);
            }
            else
                OnBattleStarted?.Invoke();
        }

        private void EnemyAdjustementMovementFinished(GridCellData cell)
        {
            m_EnemyMovementController.OnMovementFinished -= EnemyAdjustementMovementFinished;
            OnBattleStarted?.Invoke();
        }
    }
}
