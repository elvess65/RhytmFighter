using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_WaitNextEnemy : UIState_Battle
    {
        public UIState_WaitNextEnemy(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Its not finished yet";
            m_TextBattleStatus.color = Color.yellow;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToPrepareState();
        }
    }
}
