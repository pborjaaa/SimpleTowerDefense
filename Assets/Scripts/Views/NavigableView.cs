using Controllers;
using UnityEngine;

namespace Views
{
    public abstract class NavigableView<TController> : MonoBehaviour where TController : ViewController
    {
        protected TController Controller;

        protected void SetController(TController controller)
        {
            Controller = controller;
        }
    }
}