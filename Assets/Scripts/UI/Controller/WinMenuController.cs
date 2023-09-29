using Controllers;
using UnityEngine.SceneManagement;

namespace UI.Controller
{
    public class WinMenuController : ViewController
    {
        public void NavigateToMainMenu()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}