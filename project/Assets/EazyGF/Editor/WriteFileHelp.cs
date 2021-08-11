using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

public class WriteFileHelp :Editor
{
    /// <summary>
    /// 将语句写在指定的语句下面
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="belowContext">指定的语句</param>
    /// <param name="context">写入的内容</param>
    public static void WriteBelow(string filePath, string belowContext, string context)
    {
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        if (text_all.Contains(context)) text_all.Replace(context, string.Empty);

        int beginIndex = text_all.IndexOf(belowContext);
        if (beginIndex == -1)
        {
            Debug.LogError(filePath + "中没有找到标致" + belowContext);
            return;
        }

        int endIndex = text_all.LastIndexOf("\n", beginIndex + belowContext.Length);

        text_all = text_all.Substring(0, endIndex) + "\n" + context + "\n" + text_all.Substring(endIndex);

        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(text_all);
        streamWriter.Close();
    }

    /// <summary>
    /// 替换文本指定语句的值
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="contentStr"></param>
    /// <param name="newContextStr"></param>
    public static void Replace(string filePath, string contentStr, string newContextStr)
    {
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        int beginIndex = text_all.IndexOf(contentStr);
        if (beginIndex == -1)
        {
            Debug.LogError(filePath + "中没有找到标致" + contentStr);
            return;
        }

        text_all = text_all.Replace(contentStr, newContextStr);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(text_all);
        streamWriter.Close();

    }


    /// <summary>
    ///  替换指定语句里的值--适合复杂类型-但有瑕疵，如果是字符串和字符 请使用下面的一个
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="contentStr">查找的语句，如 int a = </param>
    /// <param name="symbo"> 区分的符号</param>
    /// <param name="newValue">新的值</param>
    /// <param name="newSymbo">新的符号</param>
    public static void ReplaceContentBySymbo(string filePath,string contentStr, string symbo, string newValue,string newSymbo=null)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError(filePath + "路径下文件不存在");
            return;
        }
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        int index = text_all.IndexOf(contentStr);
        if (index == -1)
        {
            Debug.LogError(filePath + "中没有找到标致" + contentStr);
            return;
        }

        string allOldStr=string.Empty;
        int Count = 0;
        bool isStarAdd = false;
        for (int i = index; i < text_all.Length; i++)
        {
            if (text_all[i] == symbo.ToCharArray()[0])
            {
                if (Count == 0)
                {
                    Count++;
                    isStarAdd = true;
                }
                else
                {
                    allOldStr += text_all[i];
                    break;
                }
            }

            if (isStarAdd)
            {
                allOldStr += text_all[i];
            }
        }

        allOldStr = contentStr + allOldStr;
        if (string.IsNullOrEmpty(newSymbo))
        {
            newSymbo = symbo;
        }

        string allNewStr = contentStr + newSymbo + newValue + newSymbo;

        string final= text_all.Replace(allOldStr, allNewStr);
        StreamWriter streamWriter = new StreamWriter(filePath);
        streamWriter.Write(final);
        streamWriter.Close();
        
    }

    /// <summary>
    /// 替换指定标志的内的文本的值--适应替换 字符串和 char赋值的语言，不能使用值不带符号的语句，如 int a = 100;
    /// </summary>
    /// <param name="filePath">完整文件路径</param>
    /// <param name="contentStr">查找的内容,如一整段赋值语句 string Name="Kyle"; char ss='s'; </param>  
    /// <param name="symbo">标志符号-没有符号的可以手动空格一下</param>
    /// <param name="newValue">修改的值</param>
    public static void ReplaceContextStringAndChar(string filePath, string contentStr, char symbo, string newValue)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError(filePath + "路径下文件不存在");
            return;
        }
        StreamReader streamReader = new StreamReader(filePath);
        string text_all = streamReader.ReadToEnd();
        streamReader.Close();

        //传入的参数中的值
        string contentOldValue = string.Empty;
        int index1 = contentStr.IndexOf(symbo);
        int index2 = contentStr.LastIndexOf(symbo);
        for (int i = index1 + 1; i < index2; i++)
        {
            contentOldValue += contentStr[i];
        }

        if (contentOldValue.Equals(newValue))
        {
            Debug.Log("用法错误，请保证两个值不一样！");
            return;
        }

        int index = text_all.IndexOf(contentStr);

        if (index >= 0)//在文本中找到了需要替换的语句
        {
            //替换文本
            string wishContent = contentStr.Replace(contentOldValue, newValue);

            string final = text_all.Replace(contentStr, wishContent);
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(final);
            streamWriter.Flush();
            streamWriter.Dispose();
            AssetDatabase.Refresh();
            Debug.Log($"{Path.GetFileName(filePath)}:替换成功");
        }//如果没有找到该值
        else
        {
            //这里分为两种情况
            //尝试去找相反的值--即目前想要设置成的值
            string oppContent = contentStr.Replace(contentOldValue, newValue);
            int oppIndex = text_all.IndexOf(oppContent);
            if (oppIndex >= 0)//相反的值找到了
            {
                Debug.Log($"{Path.GetFileName(filePath)}:需要设置值与该值一样，无需设置！");
            }
            else//如果连相反的值都找不到，那是真找不到了
            {
                Debug.LogError("文件:" + filePath + "---->找不到对应的语句！");
            }
        }
    }
    
    /// <summary>
    /// 在所有的文件夹中找到最下面的文件夹
    /// </summary>
    /// <param name="allDirArray"></param>
    /// <param name="IgnoreEmptyDir">只有里面有文件的才算</param>
    /// <returns></returns>
    public static List<DirectoryInfo> FindBelowDirectory(string rootDirPath,bool onlyHaveFile)
    {
        //根目录
        DirectoryInfo rootDir = new DirectoryInfo(rootDirPath);
        //搜寻所有的文件夹
        DirectoryInfo[] allDirArray = rootDir.GetDirectories("*", SearchOption.AllDirectories);

        List<DirectoryInfo> allChildDirArray = new List<DirectoryInfo>();
        //找到最底层的文件夹
        for (int i = 0; i < allDirArray.Length; i++)
        {
            DirectoryInfo chilDirectoryInfo = allDirArray[i];
            if (chilDirectoryInfo.GetDirectories().Length <= 0)//如果小于0表示该文件夹下面没有文件夹了
            {
                if (chilDirectoryInfo.GetFiles().Length > 0 || onlyHaveFile==false)//如果该文件夹下没有文件，也不需要打进AB包
                {
                    allChildDirArray.Add(chilDirectoryInfo);
                }
            }
        }
        return allChildDirArray;
    }

    /// <summary>
    /// 绝对路径转化成相对路径
    /// </summary>
    /// <param name="obsPath"></param>
    /// <returns></returns>
    public static string ObsPathToRelativePath(string obsPath)
    {
        int index = obsPath.IndexOf("Assets", StringComparison.Ordinal);
        if (index < 0)
        {
            Debug.LogError($"路径非法，无法将{obsPath}转换为本地路径！");
            return string.Empty;
        }
        string relativePath = obsPath.Substring(index, obsPath.Length - index);
        return relativePath;
    }


    /// <summary>
    /// 获取路径中指定的文件夹层级的文件夹名字
    /// </summary>
    /// <param name="fullPath"></param>
    /// <param name="folderLevel">倒数</param>
    /// <returns></returns>
    public static string GetFolderFolderNameByFolderLevel(string fullPath, int folderLevel)
    {
        if (folderLevel <= 0)
        {
            Debug.LogError("至少从1开始");
            return string.Empty;
        }

        string path = Path.Combine(fullPath, "");
        string fullRealPath = path.Replace('\\', '/');
        string[] pathArray = fullRealPath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        int pathArrayLenth = pathArray.Length;
        if (folderLevel >= pathArrayLenth - 1)
        {
            return string.Empty;
        }

        return pathArray[pathArrayLenth - folderLevel];
    }
}

