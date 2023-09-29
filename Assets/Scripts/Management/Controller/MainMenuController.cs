using Controllers;
using UnityEngine.SceneManagement;

namespace Management.Controller
{
    public class MainMenuController : ViewController
    {
        public void OnStartButtonClicked()
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}