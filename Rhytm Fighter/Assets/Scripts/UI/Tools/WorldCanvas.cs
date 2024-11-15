﻿using RhytmFighter.Battle.Core;
using UnityEngine;

namespace RhytmFighter.UI.Tools
{
    public class WorldCanvas : MonoBehaviour
    {
        private Canvas m_Canvas;

        private void Start()
        {
            m_Canvas = GetComponent<Canvas>();
            m_Canvas.worldCamera = BattleManager.Instance.CamerasHolder.WorldCamera;
        }
    }
}
