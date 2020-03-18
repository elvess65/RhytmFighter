using RhytmFighter.Objects.Data;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractGridObjectView : MonoBehaviour
    {
        public AbstractGridObject CorrespondingGridObject { get; protected set; }


        public virtual void Show(AbstractGridObject correspondingGridObject)
        {
            CorrespondingGridObject = correspondingGridObject;
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
