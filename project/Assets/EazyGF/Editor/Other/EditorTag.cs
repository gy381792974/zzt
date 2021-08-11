using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTag : Editor
{
    [MenuItem("GameTools/生成Tag")]
    public static void AddTag()
    {
        //打开标签管理器
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        //找到标签属性
        SerializedProperty tagsProp = tagManager.FindProperty("tags");

        tagsProp.ClearArray();
        tagManager.ApplyModifiedProperties();

        
        if (TagMgr.Instance == null)
        {
            Debug.LogError("找不到 TagMgr");
            return;
        }
        string[] tagArray = TagMgr.Instance.ConverTagToArray();

        for (int i = 0; i < tagArray.Length; i++)
        {
            // 在标签属性数组中指定的索引处插入一个空元素。
            tagsProp.InsertArrayElementAtIndex(i);
            //得到数组中指定索引处的元素
            SerializedProperty sp = tagsProp.GetArrayElementAtIndex(i);
            //给标签属性数组中刚才插入的空元素赋值
            sp.stringValue = tagArray[i];
            //添加完之后应用一下
            tagManager.ApplyModifiedProperties();
        }

    }

}
