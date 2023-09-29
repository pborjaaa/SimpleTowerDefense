using Controllers;
using UnityEngine.SceneManagement;

namespace UI.Controller
{
    public class LoseMenuController : ViewController
    {
        public void NavigateToMainMenu()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}