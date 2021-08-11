using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EazyGF
{
    [CustomPropertyDrawer(typeof(ButtonAttribute), true)]
    internal class ButtonAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ButtonAttribute attribute = (ButtonAttribute) this.attribute;
            return attribute.height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ButtonAttribute attribute = (ButtonAttribute) this.attribute;
            if (attribute.funcNames == null || attribute.funcNames.Length == 0)
            {
                position = EditorGUI.IndentedRect(position);
                EditorGUI.HelpBox(position, "[Button] funcNames is Not Set!", MessageType.Warning);
                return;
            }

            //看看应该显示按钮的时机
            bool disabled = false;
            if (attribute.showIfRunTime != ShowIfRunTime.All)
            {
                if (attribute.showIfRunTime == ShowIfRunTime.Playing != Application.isPlaying)
                {
                    disabled = true;
                }
            }

            using (new EditorGUI.DisabledScope(disabled))
            {
                if (attribute.funcNames.Length == 0)
                { Debug.LogError("除数为0"); return; }
                float width = position.width / attribute.funcNames.Length;
                position.width = width;
                for (int i = 0; i < attribute.funcNames.Length; i++)
                {
                    string funcName = attribute.funcNames[i];

                    if (GUI.Button(position, funcName))
                    {
                        CalledFunc(property.serializedObject.targetObject, funcName);
                    }

                    position.x += width;
                }
            }
        }

        private static void CalledFunc(Object target, string strFuncName)
        {
            MethodInfo methodInfo = target.GetType()
                .GetMethod(strFuncName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                return;
            }

            Debug.Log(target + ".<b>" + strFuncName + "()</b> Invoke");
            methodInfo.Invoke(target, null);
        }
    }
}