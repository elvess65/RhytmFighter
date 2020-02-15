using UnityEngine;

namespace RhytmFighter.Data
{
    public class AbstractData<T> where T : new()
    {
        public static T DeserializeData(string serializedData) => JsonUtility.FromJson<T>(serializedData);
    }
}
