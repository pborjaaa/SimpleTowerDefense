using UI.Controller;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace UI.View
{
    public class WinViewController : NavigableView<WinMenuController>
    {
        [SerializeField] private Button button;
        
        private void Start()
        {
            SetController(new WinMenuController());
            button.onClick.AddListener(Controller.NavigateToMainMenu);
        }
    }
}