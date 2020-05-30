using UnityEngine;

namespace RhytmFighter.Assets.Abstract
{
    public abstract class PrefabAssets : ScriptableObject
    {
        public virtual void Initialize()
        {
        }


        public T InstantiatePrefab<T>(T source) where T : MonoBehaviour
        {
            return InstantiatePrefab(source, new Vector3(1000, 1000, 1000), Quaternion.identity);
        }

        public T InstantiatePrefab<T>(T source, Vector3 pos) where T : MonoBehaviour
        {
            return InstantiatePrefab(source, pos, Quaternion.identity);
        }

        public T InstantiatePrefab<T>(T source, Vector3 pos, Quaternion rotation) where T : MonoBehaviour
        {
            return Instantiate(source, pos, rotation) as T;
        }


        public GameObject InstantiateGameObject(GameObject source)
        {
            return Instantiate(source, new Vector3(1000, 1000, 1000), Quaternion.identity);
        }

        public GameObject InstantiateGameObject(GameObject source, Vector3 pos)
        {
            return Instantiate(source, pos, Quaternion.identity);
        }

        public GameObject InstantiateGameObject(GameObject source, Vector3 pos, Quaternion rotation)
        {
            return Instantiate(source, pos, rotation);
        }
    }
}
