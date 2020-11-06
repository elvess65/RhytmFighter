using UnityEngine;

namespace RhytmFighter.Data.Models
{
    public abstract class DeserializableDataModel<T> where T : new()
    {
        public static T DeserializeData(string serializedData) => JsonUtility.FromJson<T>(serializedData);

        public abstract void ReorganizeData();
    }
}
