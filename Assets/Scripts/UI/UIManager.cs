using System;
using System.Collections.Generic;
using System.Linq;
using UI.Overlays;
using UI.Views;
using UnityEngine;

namespace UI
{
    public class UIManager : ManagerBase<UIManager>
    {
        [Header("UI Elements")]
        [SerializeField] private List<UIView> views;
        private Dictionary<UIViews, UIView> _views;
        [SerializeField] private List<GameObject> overlays;
        private Dictionary<OverlayType, GameObject> _overlays;

        protected override void Awake()
        {
            base.Awake();
            _views = new Dictionary<UIViews, UIView>();
            foreach (var view in views)
            {
                _views.Add(view.GetViewType(), view);
            }

            _overlays = new Dictionary<OverlayType, GameObject>();
            var overlayTypes = Enum.GetValues(typeof(OverlayType)).Cast<OverlayType>();
            foreach (var (type, obj) in overlayTypes.Zip(overlays, (type, obj) => (type, obj)))
            {
                _overlays.Add(type, obj);
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
        
        public static void ActivateOverlay(OverlayType type)
        {
            Instance._overlays[type].SetActive(true);
        }
        
        public static void DeactivateOverlay(OverlayType type)
        {
            Instance._overlays[type].SetActive(false);
        }
    }
}