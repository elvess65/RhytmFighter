using RhytmFighter.Battle.Core;
using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_BattleStart : UIState_Battle
    {
        public UIState_BattleStart(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, UIComponent_ActionPointsIndicator apIndicator) : 
            base(buttonDefence, textBattleStatus, tickIndicator, apIndicator)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Fight";
            m_TextBattleStatus.color = Color.red;
            BattleManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToBattleState();
            m_TickIndicator.PlayArrowsAnimation();
        }


        protected override void ProcessTickHandler(int ticksSinceStart)
        {
            base.ProcessTickHandler(ticksSinceStart);

            m_TickIndicator.PlayArrowsAnimation();
        }
    }
}
