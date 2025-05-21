using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace InteractablePanel
{
    public class InteractablePanelObject : MonoBehaviour
    {
        [Header("Light Colors")]
        [SerializeField] private Color lightActivated;
        [SerializeField] private Color lightDeactivated;
        
        [Header("Affected Objects")] 
        [SerializeField] private List<GameObject> affectedObjects;
        [SerializeField] private Light2D spotlight;

        private bool _isActivated;
        
        public void Initialize()
        {
            spotlight.color = lightDeactivated;
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
            spotlight.color = lightActivated;
            _isActivated = true;
            foreach (var affectedObject in affectedObjects)
            {
                var newState = affectedObject.activeSelf;
                affectedObject.SetActive(!newState);
            }
        }
    }
}