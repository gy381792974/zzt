using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EazyGF
{
    internal class ButtonExAttributeDrawer
    {
        private readonly Object target;
        private readonly List<MethodInfo> buttonMethods;
        private readonly List<ButtonExAttribute> buttonAttributes;

        public ButtonExAttributeDrawer(Object target)
        {
            if (!target)
            {
                return;
            }

            this.target = target;
            MethodInfo[] methodInfos = target.GetType().GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (methodInfos.Length <= 0)
            {
                return;
            }

            buttonMethods = new List<MethodInfo>();
            buttonAttributes = new List<ButtonExAttribute>();
            for (int i = 0; i < methodInfos.Length; i++)
            {
                MethodInfo buttonMethod = methodInfos[i];
                if (Attribute.IsDefined(buttonMethod, typeof(ButtonExAttribute), true))
                {
                    buttonMethods.Add(buttonMethod);
                    ButtonExAttribute[] exAttributes = buttonMethod.GetCustomAttributes(typeof(ButtonExAttribute), true) as ButtonExAttribute[];
                    buttonAttributes.Add(exAttributes[0]);
                }
            }
        }

        public void OnInspectorGUI()
        {
            if (buttonMethods.Count == 0)
            {
                return;
            }

            for (int index = 0; index < buttonMethods.Count; index++)
            {
                MethodInfo methodInfo = buttonMethods[index];
                ButtonExAttribute buttonExAttribute = buttonAttributes[index];
                bool disabled = false;
                if (buttonExAttribute.showIfRunTime != ShowIfRunTime.All)
                {
                    if (buttonExAttribute.showIfRunTime == ShowIfRunTime.Playing != Application.isPlaying)
                    {
                        disabled = true;
                    }
                }

                using (new EditorGUI.DisabledScope(disabled))
                {
                    string name = buttonExAttribute.txtButtonName ?? methodInfo.Name;
                    if (GUILayout.Button(name))
                    {
                        methodInfo.Invoke(target, null);
                    }
                }
            }
        }
    }
}