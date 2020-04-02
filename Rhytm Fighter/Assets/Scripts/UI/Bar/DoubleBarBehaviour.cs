using UnityEngine;

namespace RhytmFighter.UI.Bar
{
    public class DoubleBarBehaviour : SingleBarBehaviour
    {
        [SerializeField] private AbstractBarStrategy SecondBarStrategy;

        public void SetProgress_SecondBar(int cur, int max)
        {
            SecondBarStrategy.SetProgress(cur, max);
        }
    }
}
