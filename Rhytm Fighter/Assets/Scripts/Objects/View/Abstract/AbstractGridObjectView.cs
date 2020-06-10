using RhytmFighter.Objects.Model;
using UnityEngine;

namespace RhytmFighter.Objects.View
{
    public abstract class AbstractGridObjectView : MonoBehaviour
    {
        public System.Action OnViewShowed;

        public AbstractGridObjectModel CorrespondingModel { get; protected set; }


        public virtual void ShowView(AbstractGridObjectModel correspondingModel)
        {
            CorrespondingModel = correspondingModel;
            OnViewShowed?.Invoke();
        }

        public virtual void HideView()
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 2);
        }
    }
}
