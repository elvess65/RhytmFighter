using UnityEngine;

namespace RhytmFighter.UI.Bar
{
    public abstract class AbstractBarStrategy : MonoBehaviour
    {
        public abstract void SetProgress(int cur, int max);
    }
}
