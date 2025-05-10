using System.Collections.Generic;
using System.Linq;

namespace Input
{
    public class MouseInput : UserInput<int>
    {
        private void Awake()
        {
            ButtonStates = new Dictionary<int, bool>();
            ButtonAssignment = InputManager.MouseButtons
                .Select(n => new KeyValuePair<string, int>(n.action, n.button))
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            UpdateButtonStateDictionary();
            UpdateEventDictionaries(InputManager.MouseButtons);
        }
        
        private void Update ()
        {
            foreach (var button in ButtonAssignment)
            {
                var previousState = ButtonStates[button.Value];

                ButtonStates[button.Value] = UnityEngine.Input.GetMouseButton(button.Value);

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