﻿using System.Collections.Generic;
using RhytmFighter.Persistant.Abstract;
using RhytmFighter.UI.Widget;
using UnityEngine;

namespace RhytmFighter.UI.View
{
    /// <summary>
    /// Базовый класс для UI View
    /// </summary>
    public abstract class UIView_Abstract : MonoBehaviour, iUpdatable
    {
        public Transform Root;

        protected List<iUpdatable> m_Updatables = new List<iUpdatable>();
        protected List<UIWidget> m_Widgets = new List<UIWidget>();


        public abstract void Initialize();
 
        public virtual void PerformUpdate(float deltaTime)
        {
            if (m_Updatables != null)
            {
                for (int i = 0; i < m_Updatables.Count; i++)
                    m_Updatables[i].PerformUpdate(deltaTime);
            }
        }


        public void DisableView(bool isDisabled)
        {
            Root.gameObject.SetActive(!isDisabled);
        }

        public void DiableWidgets(bool isDisabled, bool isAnimated)
        {
            for (int i = 0; i < m_Widgets.Count; i++)
                m_Widgets[i].DisableWidget(isDisabled, isAnimated);
        }


        protected void RegisterWidget(UIWidget widget)
        {
            m_Widgets.Add(widget);
        }

        protected void RegisterUpdatable(iUpdatable iUpdatable)
        {
            m_Updatables.Add(iUpdatable);
        }
    }
}
