using System.Collections.Generic;
using UnityEngine;

namespace Input
{
    public class InputManager : ManagerBase<InputManager>
    {
        [Header("Input Handles")] 
        [SerializeField] private KeyboardInput keyboardInput;
        [SerializeField] private MouseInput mouseInput;
        
        public static readonly List<(KeyCode button, string action)> KeyboardButtons = new()
        {
            (KeyCode.A, "Move Left"),
            (KeyCode.D, "Move Right"),
            (KeyCode.W, "Move Forwards"),
            (KeyCode.S, "Move Backwards"),
            (KeyCode.Space, "Jump"),
            (KeyCode.LeftShift, "Sprint"),
            (KeyCode.LeftControl, "Crouch"),
            (KeyCode.LeftAlt, "Walk"),
            (KeyCode.E, "Interact"),
            (KeyCode.F, "InteractAlt"),
            (KeyCode.Tab, "Inventory"),
            (KeyCode.Escape, "Escape"),
            (KeyCode.Alpha1, "Item Slot 1"),
            (KeyCode.Alpha2, "Item Slot 2"),
            (KeyCode.Alpha3, "Item Slot 3"),
            (KeyCode.Alpha4, "Item Slot 4"),
            (KeyCode.Alpha5, "Item Slot 5"),
        };

        public static readonly List<(int button, string action)> MouseButtons = new()
        {
            (0, "Base"),
            (1, "Alternative"),
        };

        public void test()
        {
            
        }
    }
}