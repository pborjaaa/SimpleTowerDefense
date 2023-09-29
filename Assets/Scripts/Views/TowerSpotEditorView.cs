using UnityEditor;
using UnityEngine;
namespace Views
{
    public class TowerSpotEditorView : MonoBehaviour
    {
        public float range;

        private void OnDrawGizmos()
        {
            Handles.color = new Color(0f, 1f, 0f, 1);
            Handles.DrawSolidDisc(transform.position, Vector3.forward, range);
        }
    }
}