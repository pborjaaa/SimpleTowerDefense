using UnityEngine.SceneManagement;

namespace Controllers
{
    public class MainMenuController : ViewController
    {
        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}