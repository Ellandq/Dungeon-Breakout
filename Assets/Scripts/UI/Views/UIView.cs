using UnityEngine;

namespace UI.Views
{
    public abstract class UIView : MonoBehaviour
    {
        [Header("View Info")] 
        [SerializeField] private UIViews viewType;
        
        public virtual void ActivateView(bool instant = true)
        {
            gameObject.SetActive(true);
        }
        
        public virtual void DeactivateView()
        {
            gameObject.SetActive(false);
        }

        public UIViews GetViewType()
        {
            return viewType;
        }
    }
}