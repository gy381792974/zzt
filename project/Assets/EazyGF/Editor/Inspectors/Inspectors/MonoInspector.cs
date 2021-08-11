using UnityEditor;
using UnityEngine;

namespace EazyGF
{
    [CustomEditor(typeof(MonoBehaviour), true, isFallback = true)]
    [CanEditMultipleObjects]
    internal class MonoInspector : UnityEditor.Editor
    {
        private ButtonExAttributeDrawer buttonExAttributeDrawer;

        public override void OnInspectorGUI()
        {
            if (!target)
            {
                return;
            }

            if (buttonExAttributeDrawer == null)
            {
                buttonExAttributeDrawer = new ButtonExAttributeDrawer(target);
            }

            buttonExAttributeDrawer.OnInspectorGUI();
            base.OnInspectorGUI();
        }
    }
}