using System;
using System.Collections.Generic;
using UI.Views;
using UnityEngine;

namespace UI
{
    public class UIManager : ManagerBase<UIManager>
    {
        [Header("UI Elements")]
        [SerializeField] private List<UIView> views;
        private Dictionary<UIViews, UIView> _views;

        protected override void Awake()
        {
            base.Awake();
            _views = new Dictionary<UIViews, UIView>();
            foreach (var view in views)
            {
                _views.Add(view.GetViewType(), view);
            }
        }

        public static void ActivateView(UIViews view)
        {
            Instance._views[view].ActivateView();
        }
        
        public static void DeactivateView(UIViews view)
        {
            Instance._views[view].DeactivateView();
        }
    }
}