using System;
using UnityEngine;

namespace UI.Views
{
    public abstract class UIView : MonoBehaviour
    {
        [Header("View Info")] 
        [SerializeField] private UIViews viewType;
        
        public virtual void ActivateView()
        {
            throw new NotImplementedException();
        }
        
        public virtual void DeactivateView()
        {
            throw new NotImplementedException();
        }

        public UIViews GetViewType()
        {
            return viewType;
        }
    }
}