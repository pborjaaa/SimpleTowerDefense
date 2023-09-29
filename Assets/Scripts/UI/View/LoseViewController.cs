using TMPro;
using UI.Controller;
using UnityEngine;
using UnityEngine.UI;
using Views;

namespace UI.View
{
    public class LoseViewController : NavigableView<LoseMenuController>
    {
        [SerializeField] private TextMeshProUGUI loseTextCount;
        [SerializeField] private Button button;
        
        private void Start()
        {
            SetController(new LoseMenuController());
            button.onClick.AddListener(Controller.NavigateToMainMenu);
        }

        public void Setup(int escapedEnemies)
        {
            loseTextCount.text = escapedEnemies.ToString();
        }
    }
}