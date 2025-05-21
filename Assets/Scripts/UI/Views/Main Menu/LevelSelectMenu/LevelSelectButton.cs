using UnityEngine;
using UnityEngine.UI;

namespace UI.Views.Main_Menu.LevelSelectMenu
{
    public class LevelSelectButton : MonoBehaviour
    {
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color selectedColor;

        [SerializeField] private Image image;

        [SerializeField] private GameObject lockImage;

        public void SetHighlight(bool state)
        {
            image.color = state ? selectedColor : defaultColor;
        }

        public void SetLockState(bool state)
        {
            lockImage.SetActive(state);
        }
    }
}