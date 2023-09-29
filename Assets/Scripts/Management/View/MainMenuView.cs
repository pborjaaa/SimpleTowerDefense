using Management.Controller;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace Management.View
{
    public class MainMenuView : NavigableView<MainMenuController>
    {
        [SerializeField] private Button startButton;

        private void Start()
        {
            SetController(new MainMenuController());
            startButton.onClick.AddListener(OnStartButtonClicked);
        }

        private void OnStartButtonClicked() 
        {
            Controller.OnStartButtonClicked();
        }
    }
}