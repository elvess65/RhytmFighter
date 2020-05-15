using RhytmFighter.UI.Abstract;
using UnityEngine;

namespace RhytmFighter.UI.Bar
{
    public class SingleBarBehaviour : AbstractUIObject
    {
        [SerializeField] private AbstractBarStrategy BarStrategy;

        public void SetProgress(int cur, int max)
        {
            BarStrategy.SetProgress(cur, max);
        }
    }
}
