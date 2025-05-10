using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Input
{
    public class KeyboardInput :  UserInput<KeyCode>
    {
        private void Awake()
        {
            ButtonStates = new Dictionary<KeyCode, bool>();
            ButtonAssignment = InputManager.KeyboardButtons
                .Select(n => new KeyValuePair<string, KeyCode>(n.action, n.button))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            UpdateButtonStateDictionary();
            UpdateEventDictionaries(InputManager.KeyboardButtons);
        }
        
        private void Update ()
        {
            foreach (var button in ButtonAssignment)
            {
                var previousState = ButtonStates[button.Value];

                ButtonStates[button.Value] = UnityEngine.Input.GetKey(button.Value);

                switch (ButtonStates[button.Value])
                {
                    case true when !previousState:
                        OnInputAction[button.Key]?.Invoke(ButtonState.Down);
                        break;
                    case false when previousState:
                        OnInputAction[button.Key]?.Invoke(ButtonState.Up);
                        break;
                }
            }
        } 
    }
}