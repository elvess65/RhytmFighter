using UnityEngine;

namespace RhytmFighter.Level.Scheme.Builder
{
    public abstract class AbstractSchemeBuilder 
    {
        protected Vector3 m_INIT_POSITION;

        public abstract bool HasData { get; }


        public AbstractSchemeBuilder()
        {
            Dispose();
        }

        public abstract void Dispose();

        public abstract void ShowAllAsNormal();
    }
}
