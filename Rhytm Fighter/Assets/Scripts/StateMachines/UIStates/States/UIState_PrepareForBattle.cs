using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_PrepareForBattle : UIState_Battle
    {
        public UIState_PrepareForBattle(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, UIComponent_ActionPointsIndicator apIndicator) :
            base(buttonDefence, textBattleStatus, tickIndicator, apIndicator)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Prepare for battle";
            m_TextBattleStatus.color = Color.yellow;
            BattleManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToPrepareState();
        }
    }
}
