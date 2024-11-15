﻿using Frameworks.Grid.Data;
using Frameworks.Grid.View;
using RhytmFighter.Assets;
using RhytmFighter.Battle.Action;
using RhytmFighter.Battle.AI;
using RhytmFighter.Battle.Health;
using RhytmFighter.Persistant.Enums;
using RhytmFighter.Objects.View;
using RhytmFighter.Battle.AI.Abstract;

namespace RhytmFighter.Objects.Model
{
    public class StandardEnemyNPCModel : AbstractBattleNPCModel
    {
        private int m_ViewID;

        public StandardEnemyNPCModel(int id, int viewID, GridCellData correspondingCell,
            float moveSpeed, iBattleActionBehaviour actionBehaviour, iHealthBehaviour healthBehaviour,
            AITypes aiType, int experianceForDestroy) : 
            base(id, correspondingCell, moveSpeed, actionBehaviour, healthBehaviour, true)
        {
            AI = GetAI(aiType);
            m_ViewID = viewID;
            ExperianceForDestroy = experianceForDestroy;
        }

        protected override AbstractGridObjectView CreateView(CellView cellView) 
        {
            return AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().StandartEnemyNPCViewPrefab, cellView.transform.position);
        }

        #region AI
        AbstractAI GetAI(AITypes type)
        {
            AbstractAI result = null;

            switch (type)
            {
                case AITypes.Simple:
                    result = new SimpleAI(this);
                    break;
                case AITypes.SimpleDefencible:
                    result = new SimpleDefencibleAI(this);
                    break;
            }

            return result;
        }
        #endregion
    }
}
