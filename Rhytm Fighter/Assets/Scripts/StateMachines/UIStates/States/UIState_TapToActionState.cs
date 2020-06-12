using UnityEngine.UI;

namespace RhytmFighter.StateMachines.UIState
{
    public class UIState_TapToActionState : UIState_Abstract
    {
        private Text m_TextPressToContinue;

        public UIState_TapToActionState(Text textBattleStatus, Text textPressToContinue) : base(textBattleStatus)
        {
            m_TextPressToContinue = textPressToContinue;
        }

        public override void EnterState()
        {
            base.EnterState();

            //Text
            m_TextPressToContinue.gameObject.SetActive(true);
            m_TextBattleStatus.gameObject.SetActive(true);
        }
    }
}
