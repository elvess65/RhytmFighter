using Frameworks.Grid.View;
using RhytmFighter.Characters;
using RhytmFighter.Interfaces;
using RhytmFighter.Main;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.Battle
{
    public class BattleController
    {
        public System.Action OnPrepareForBattle;
        public System.Action OnBattleStarted;
        public System.Action OnBattleFinished;

        private iBattleObject m_CurrentEnemy;
        private Level.LevelController m_LevelController;
        private WaitForSeconds m_WaitBeforeDistanceAdjustement;
        private ModelMovementController m_EnemyMovementController;
        
        private Dictionary<int, iBattleObject> m_PendingEnemies;
        

        public iBattleObject Player { get; set; }

        private const float m_DELAY_BEFORE_DISTANCE_ADJUSTEMENT = 1;
        

        public BattleController(Level.LevelController levelController)
        {
            m_LevelController = levelController;
            m_PendingEnemies = new Dictionary<int, iBattleObject>();
            m_EnemyMovementController = new ModelMovementController(levelController);
            m_WaitBeforeDistanceAdjustement = new WaitForSeconds(m_DELAY_BEFORE_DISTANCE_ADJUSTEMENT);
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
            {
                OnPrepareForBattle?.Invoke();
                GameManager.Instance.StartCoroutine(DelayBeforeAdjustementCoroutine(m_DELAY_BEFORE_DISTANCE_ADJUSTEMENT));
            }
        }

        public void ProcessEnemyActions()
        {
            m_CurrentEnemy.ActionBehaviour.ExecuteAction();
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
            if (m_PendingEnemies.ContainsKey(sender.ID))
                m_PendingEnemies.Remove(sender.ID);

            //Get closest to player enemy
            //if has enemy
            //  Player.SetTarget
            //  Adjust distance
            //else 
            //  FinishBattle

            //Finish battle
            if (m_PendingEnemies.Count == 0)
                OnBattleFinished?.Invoke();
        }

        private void AdjustDistance()
        {
            //Current target - player target
            //Remove target from pending
            //Check distance between player and target
            //If distance is less than 2 cell - adjust movement
            //  OnMovementFinished - StartBattle()
            //else 
            //  StartBattle()
        }

        private void AdjustDistance(iBattleObject battleObject)
        {
            Debug.Log("Distance adjustement");

            GameObject ob = null;

            float distanceToPlayer = float.MinValue;
            float distanceToEnemy = float.MinValue;

            Vector3[] path = null;
            Frameworks.Grid.Data.GridCellData targetCell = null;
            Frameworks.Grid.Data.SquareGrid grid = m_LevelController.Model.GetCurrenRoomData().GridData;
            List<Frameworks.Grid.Data.GridCellData> coords = grid.GetCellWalkableAndVisibleNeighboursCoordInRange(battleObject.CorrespondingCell.X, battleObject.CorrespondingCell.Y, 2);

            while (coords.Count > 0 && path == null)
            {
                for (int i = 0; i < coords.Count; i++)
                {
                    Frameworks.Grid.Data.GridCellData currentCell = grid.GetCellByCoord(coords[i].X, coords[i].Y);
                    float distToPlayer = grid.GetDistanceBetweenCells(Player.CorrespondingCell, currentCell);
                    float distToEnemy = grid.GetDistanceBetweenCells(battleObject.CorrespondingCell, currentCell);
                    if (distToPlayer >= distanceToPlayer && distToEnemy >= distanceToEnemy)
                    {
                        targetCell = currentCell;
                        distanceToPlayer = distToPlayer;
                        distanceToEnemy = distToEnemy;

                        if (ob != null)
                            MonoBehaviour.Destroy(ob);

                        CellView currentView = m_LevelController.RoomViewBuilder.GetCellVisual(m_LevelController.Model.GetCurrenRoomData().ID, targetCell.X, targetCell.Y);
                        ob = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        ob.transform.position = currentView.transform.position;
                    }
                }

                path = grid.FindPath(battleObject.CorrespondingCell, targetCell);

                if (coords.Contains(targetCell))
                    coords.Remove(targetCell);
            }

            CellView view = m_LevelController.RoomViewBuilder.GetCellVisual(m_LevelController.Model.GetCurrenRoomData().ID, targetCell.X, targetCell.Y);
            GameObject.CreatePrimitive(PrimitiveType.Capsule).transform.position = view.transform.position;

            m_EnemyMovementController.SetModel(battleObject as iMovableModel);
            //m_MovementController.MoveCharacter(view);
        }

        System.Collections.IEnumerator DelayBeforeAdjustementCoroutine(float time)
        {
            yield return m_WaitBeforeDistanceAdjustement;
            AdjustDistance();
        }
    }
}
