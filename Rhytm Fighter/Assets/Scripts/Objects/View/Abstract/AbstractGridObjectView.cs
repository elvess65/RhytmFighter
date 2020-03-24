using RhytmFighter.Objects.Model;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractGridObjectView : MonoBehaviour
    {
        public AbstractGridObjectModel CorrespondingGridObject { get; protected set; }


        public virtual void Show(AbstractGridObjectModel correspondingGridObject)
        {
            CorrespondingGridObject = correspondingGridObject;
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
