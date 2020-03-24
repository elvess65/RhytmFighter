using RhytmFighter.Objects.Model;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractGridObjectView : MonoBehaviour
    {
        public AbstractGridObjectModel CorrespondingModel { get; protected set; }


        public virtual void Show(AbstractGridObjectModel correspondingModel)
        {
            CorrespondingModel = correspondingModel;
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
