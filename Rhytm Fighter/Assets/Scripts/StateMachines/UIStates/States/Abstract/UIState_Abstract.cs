using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.StateMachines.UIState
{
    public abstract class UIState_Abstract : AbstractState
    {
        protected List<Transform> m_ControlledObjects;
    }
}
