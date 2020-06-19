using UnityEngine;

namespace RhytmFighter.Persistant.Helpers
{
    public static class HelpersCollection
    {
        public static bool IsInRandomRange(int percentRange)
        {
            int rndValue = Random.Range(0, 101);
            return rndValue > 100 - percentRange;
        }
    }
}
