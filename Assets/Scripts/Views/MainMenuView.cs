using Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
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