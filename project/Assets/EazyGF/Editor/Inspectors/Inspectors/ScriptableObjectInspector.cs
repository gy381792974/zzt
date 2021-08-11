using UnityEditor;
using UnityEngine;

namespace EazyGF
{
    [CustomEditor(typeof(ScriptableObject), true, isFallback = true)]
    [CanEditMultipleObjects]
    internal class ScriptableObjectInspector : UnityEditor.Editor
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
            DrawDefaultInspector();
        }
    }
}