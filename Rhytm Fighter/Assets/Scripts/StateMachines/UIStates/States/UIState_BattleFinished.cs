using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_BattleFinished : UIState_Abstract
    {
        public UIState_BattleFinished(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Victory";
            m_TextBattleStatus.color = Color.green;
            BattleManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToNormalState();
        }
    }
}
