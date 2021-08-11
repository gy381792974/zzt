using System;
using System.IO;
using EazyGF;
using LitJson;
using UnityEngine;

[Serializable]
public class LanguageData
{
    public string[][] LanguageArray;
}

public class LanguageMgr
{
    private static LanguageData _languageData = new LanguageData();
    private static string loadLuanguageName = string.Empty;
    /// <summary>
    /// 根据手机系统语言加载不同的语言文本
    /// </summary>
    public static void InitLanguage()
    {
        SystemLanguage systemLanguage = GetPlayerLauguage(); 
        LoadLanguage(systemLanguage);
    }

    private static SystemLanguage GetPlayerLauguage()
    {
        return Application.systemLanguage;
        //return SystemLanguage.English;
    }


    private static void LoadLanguage(SystemLanguage systemLanguage)
    {
        switch (systemLanguage)
        {
            case SystemLanguage.Chinese:
            case SystemLanguage.ChineseSimplified:
                loadLuanguageName = "CN";
                break;
            case SystemLanguage.English:
                loadLuanguageName = "EN";
                break;
            default:
                loadLuanguageName = "EN";
                break;
        }

        AssetLoadMode assetLoadMode = GameMgr.Instance.LoadMode;
        switch (assetLoadMode)
        {
            case AssetLoadMode.EditorMode:
                LoadLanguageInEditor();
                break;
            case AssetLoadMode.AssetBundleMode:
                LoadLanguageInAssetBundle();
                break;
        }

    }

    //编辑器模式下加载语言包
    private static void LoadLanguageInEditor()
    {
        string languageLocalPath =
            $"{AB_ResFilePath.jsonLanguageDatasRootDir}{loadLuanguageName}{AB_ResFilePath.LanguageSuffix}";

#if UNITY_EDITOR
        try
        {
            TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(languageLocalPath);
            InitLanguagText(textAsset.text);
        }
        catch (Exception e)
        {
            Debug.Log("加载语言表失败:" + e);
        }
#endif
    }

    //ab包模式下加载语言包
    private static void LoadLanguageInAssetBundle()
    {
        try
        {
            string abName = loadLuanguageName.ToLower();
            TextAsset textAsset = AssetMgr.Instance.LoadAsset<TextAsset>(abName, $"{loadLuanguageName}.txt");
            InitLanguagText(textAsset.text);
            textAsset = null;
            AssetMgr.Instance.UnloadAsset(abName,true,true);
        }
        catch (Exception e)
        {
            Debug.Log("加载语言表失败:" + e);
        }
    }

    private static void InitLanguagText(string jsonValue)
    {
        var temp = JsonMapper.ToObject(jsonValue);
        if (temp.IsArray)
        {
            int i = 0;
            _languageData.LanguageArray = new string[temp.Count][];
            foreach (JsonData element in temp)
            {
                if (element.IsArray)
                {
                    _languageData.LanguageArray[i] = new string[element.Count];
                    int j = 0;
                    foreach (JsonData child in element)
                    {
                        if (child != null)
                        {
                            _languageData.LanguageArray[i][j++] = child.ToString();
                        }
                        else
                        {
                            _languageData.LanguageArray[i][j++] = string.Empty;
                        }
                    }
                    i++;
                }
            }
        }
    }

    ///// <summary>
    ///// 根据下标获取多语言,拆分下标
    ///// </summary>
    ///// <param name="indexStr"></param>
    ///// <param name="pObjects"></param>
    ///// <returns></returns>
    //public static string GetTranstion(string indexStr, params object[] pObjects)
    //{
    //    string[] indexArray = indexStr.Split('-');
    //    return GetTranstion(int.Parse(indexArray[0]), int.Parse(indexArray[1]), pObjects);
    //}

    /// <summary>
    ///  根据下标获取多语言,拆分数组
    /// </summary>
    /// <param name="indexStr"></param>
    /// <param name="pObjects"></param>
    /// <returns></returns>
    public static string GetTranstion(int []languageArray, params object[] pObjects)
    {
        if (languageArray == null)
        {
            Debug.LogError("数组为空");
            return "多语言错误!"; ;
        }

        if (languageArray.Length != 2)
        {
            Debug.LogError("多语言数组长度不对");
            return "多语言错误!"; ;
        }
        return GetTranstion(languageArray[0], languageArray[1], pObjects);
    }

    /// <summary>
    /// 根据下标获取多语言
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    /// <param name="pObjects"></param>
    /// <returns></returns>
    public static string GetTranstion(int index1, int index2, params object[] pObjects)
    {
        try
        {
            if (pObjects.Length == 0)
            {
                return _languageData.LanguageArray[index1 - 1][index2];
            }
            return string.Format(_languageData.LanguageArray[index1 - 1][index2], pObjects);
        }
        catch (Exception)
        {
            Debug.LogWarning($"找不到多语言下标：{index1}:{index2},或者参数错误,请检查！");
        }

        return "多语言错误!";
    }

    /// <summary>
    /// 根据下标获取多语言数组
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    /// <param name="pObjects"></param>
    /// <returns></returns>
    public static string[] GetLanguageArr(int index)
    {
        try
        {

            return _languageData.LanguageArray[index - 1];
        }
        catch (Exception)
        {
            Debug.LogError($"找不到多语言下标：{index- 1},或者参数错误,请检查！");
        }

        return null;
    }

    /// <summary>
    /// 改变语言
    /// </summary>
    /// <param name="systemLanguage"></param>
    public static void SwitchLanguage(SystemLanguage systemLanguage)
    {
        LoadLanguage(systemLanguage);
    }
}
