using System.Collections.Generic;
using UnityEngine;

namespace InteractablePanel
{
    public class InteractablePanelObject : MonoBehaviour
    {
        [Header("Affected Objects")] 
        [SerializeField] private List<GameObject> affectedObjects;

        private bool _isActivated;
        
        public void Initialize()
        {
            if (!_isActivated) return;
            _isActivated = false;
            foreach (var affectedObject in affectedObjects)
            {
                var newState = affectedObject.activeSelf;
                affectedObject.SetActive(!newState);
            }
        }

        public void Interact()
        {
            if (_isActivated) return;
            _isActivated = true;
            foreach (var affectedObject in affectedObjects)
            {
                var newState = affectedObject.activeSelf;
                affectedObject.SetActive(!newState);
            }
        }
    }
}