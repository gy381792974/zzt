using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EazyGF
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
    internal class ShowIfAttributeDrawer : PropertyDrawer
    {
        private bool isShow = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attribute = (ShowIfAttribute)this.attribute;
            isShow = CheckShowTargets(property, attribute);
            return isShow ? base.GetPropertyHeight(property, label) : -2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //ShowIfAttribute attribute = (ShowIfAttribute)this.attribute;
            if (isShow)
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        private const string strArrayKeyword = ".Array.data[";

        private static object GetTargetObjectForArray(SerializedProperty property, string targetName)
        {
            string strPath = property.propertyPath;

            //数组名格式：xxxx.Array.data[0].yyyy
            //判断数组名
            if (strPath.Contains(strArrayKeyword))
            {
                int start = strPath.IndexOf(strArrayKeyword, StringComparison.Ordinal) + 12;
                int end = strPath.IndexOf("].", StringComparison.Ordinal);
                if (end < 0)
                {
                    return null;
                }

                int length = end - start;
                int index = -1;

                //得到数组索引
                if (!int.TryParse(strPath.Substring(start, length), out index))
                {
                    return null;
                }

                //得到数组变量名
                string strArray = strPath.Substring(0, start - 12);
                if (!targetName.Equals(strArray))
                {
                    return null;
                }

                //得到数组变量值
                FieldInfo fieldInfo = property.serializedObject.targetObject.GetType().GetField(strArray);
                object obj = fieldInfo.GetValue(property.serializedObject.targetObject);

                //取元素值
                IList list = (IList)obj;
                if (list != null)
                {
                    return list[index];
                }

                Array array = (Array)obj;
                if (array != null)
                {
                    return array.GetValue(index);
                }
            }

            return null;
        }

        public static bool CheckFieldOrProperty(object targetObject, ShowIfAttribute.Target target)
        {
            Type targetObjectType = targetObject.GetType();
            FieldInfo fieldInfo = targetObjectType.GetField(target.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null)
            {
                return CheckIsUnityObject(fieldInfo.GetValue(targetObject)) == target.show;
            }

            PropertyInfo propertyInfo = targetObjectType.GetProperty(target.name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                return CheckIsUnityObject(propertyInfo.GetValue(targetObject, null)) == target.show;
            }

            return true;
        }

        private static bool CheckIsUnityObject(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is Object)
            {
                Object obj = value as Object;
                return obj;
            }

            return (bool)value;
        }

        private static bool CheckShowTarget(SerializedProperty property, ShowIfAttribute.Target target)
        {
            if (target == null)
            {
                return true;
            }

            object obj = GetTargetObjectForArray(property, target.name) ?? property.serializedObject.targetObject;
            return CheckFieldOrProperty(obj, target);
        }

        private static bool CheckShowTargets(SerializedProperty property, ShowIfAttribute orAttribute)
        {
            bool res = true;
            if (orAttribute.mode == ShowIfAttribute.Mode.And)
            {
                for (int i = 0; i < orAttribute.targets.Length; i++)
                {
                    res &= CheckShowTarget(property, orAttribute.targets[i]);
                    if (!res)
                    {
                        return false;
                    }
                }

                return res;
            }

            res = false;
            for (int i = 0; i < orAttribute.targets.Length; i++)
            {
                res |= CheckShowTarget(property, orAttribute.targets[i]);
                if (res)
                {
                    return true;
                }
            }

            return res;
        }
    }
}