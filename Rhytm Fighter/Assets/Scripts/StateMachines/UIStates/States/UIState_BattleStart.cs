using RhytmFighter.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_BattleStart : UIState_Battle
    {
        public UIState_BattleStart(Button buttonDefence, Text textBattleStatus, UIComponent_TickIndicator tickIndicator, Transform playerUIParent) : base(buttonDefence, textBattleStatus, tickIndicator, playerUIParent)
        {
        }


        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextBattleStatus.text = "Fight";
            m_TextBattleStatus.color = Color.red;
            Core.GameManager.Instance.StartCoroutine(DisableBattleStatusTextCoroutine());

            //Tick indicator
            m_TickIndicator.ToBattleState();
        }


        protected override void ProcessTickHandler(int ticksSinceStart)
        {
            base.ProcessTickHandler(ticksSinceStart);

            m_TickIndicator.PlayArrowsAnimation();
        }
    }
}
