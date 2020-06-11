using FrameworkPackage.Utils;
using RhytmFighter.Assets;
using RhytmFighter.Persistant.Abstract;
using System.Collections.Generic;
using UnityEngine;

namespace RhytmFighter.UI.Components
{
    public class UIComponent_ActionPointsIndicator : MonoBehaviour, iUpdatable
    {
        [SerializeField] private RectTransform ItemsParent;

        private List<UIComponent_ActionPointsIndicatorItem> m_Items;


        public void Initialize(int apCount, int restoreDuration)
        {
            m_Items = new List<UIComponent_ActionPointsIndicatorItem>();
            for (int i = 0; i < apCount; i++)
            {
                UIComponent_ActionPointsIndicatorItem item = AssetsManager.GetPrefabAssets().InstantiatePrefab(AssetsManager.GetPrefabAssets().ActionPointIndicatorItemPrefab);
                item.transform.parent = ItemsParent;
                item.transform.localScale = Vector3.one;
                item.transform.localPosition = Vector3.zero;

                item.Initialize(restoreDuration);

                m_Items.Add(item);
            }
        }

        public void UseActionPoint(int curActionPoints)
        {
            m_Items[curActionPoints].StartRestoring();
        }

        public void RestoreActionPoint(int curActionPoints)
        {
            m_Items[curActionPoints].Restore();
        }

        public void PerformUpdate(float deltaTime)
        {
            for (int i = 0; i < m_Items.Count; i++)
            {
                if (m_Items[i].IsRestoring)
                    m_Items[i].PerformUpdate(deltaTime);
            }
        }
    }
}
