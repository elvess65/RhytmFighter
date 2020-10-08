using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using RhytmFighter.UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_WaitNextEnemy : UIState_Battle
    {
        public UIState_WaitNextEnemy(Button buttonDefence, Text textBattleStatus, UIWidget_Tick tickIndicator, UIComponent_ActionPointsIndicator apIndicator) :
            base(buttonDefence, textBattleStatus, tickIndicator, apIndicator)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Its not finished yet";
            m_TextBattleStatus.color = Color.yellow;
            BattleManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToPrepareState();
        }
    }
}
