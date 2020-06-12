using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_BattleFinished : UIState_Abstract
    {
        private UIComponent_TickIndicator m_TickIndicator;

        public UIState_BattleFinished(Text textBattleStatus, UIComponent_TickIndicator tickIndicator) : 
            base(textBattleStatus)
        {
            m_TickIndicator = tickIndicator;
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
