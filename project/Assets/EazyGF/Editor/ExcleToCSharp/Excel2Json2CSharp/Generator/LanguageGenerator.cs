using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using EazyGF;
using LitJson;
using UnityEditor;
using UnityEngine;

public class LanguageGenerator
{
    private static int LanguageSplitIndexValue = int.MaxValue;//多语言表中不填写的地方的值默认下标为-1--该处不要改
    private static int LanguageStartIndex = -1;//多语言起始下标-即多语言表中用来分隔值--该处不要改


    //多语言信息字典
    private static Dictionary<string, List<object>> languageDic = new Dictionary<string, List<object>>();

    public static bool WriteLanguageData(DataTable mSheet)
    {
        languageDic.Clear();
        if (CheckTableValue(mSheet))
        {
            return WriteLanguageValue();
        }

        return false;
    }

    private static bool CheckTableValue(DataTable mSheet)
    {
        for (int i = 2; i < ExcleGeneratorBase.rowCount; i++)
        {

            for (int j = 0; j < ExcleGeneratorBase.colCount; j++)
            {

                string fullNameWithType = mSheet.Rows[1][j].ToString();//属性名字_类型
                int lastIndex = fullNameWithType.LastIndexOf('_');
                if (lastIndex < 0)
                {
                    string errorStr = "表：" + ExcleGeneratorBase.EexcleName + "--> Shee：" + mSheet.TableName + " 里面的" +
                                                    $" 字段类型不正确: {fullNameWithType}，请检查！";
                    Debug.LogError(errorStr);
                    return false;
                }

                string pName = fullNameWithType.Substring(0, lastIndex);
                string typeName = fullNameWithType.Substring(lastIndex + 1, fullNameWithType.Length - lastIndex - 1);
                if (!ExcleGeneratorBase.IsSupportType(typeName))
                {
                    string errorStr = "表：" + ExcleGeneratorBase.EexcleName + "--> Shee：" + mSheet.TableName + " 里面的：" + pName +
                                                    $" 字段类型不支持类型: {typeName}，请检查！";
                    Debug.LogError(errorStr);
                    return false;
                }

                mSheet.Columns[j].ColumnName = pName;

                WriteLanguageDataToDic(mSheet, pName, typeName, i, j);
            }
        }

        return true;
    }


    // 将多语言表中的值写入到字典中
    private static void WriteLanguageDataToDic(DataTable mSheet, string pName, string typeName, int index1, int index2)
    {
        string cellValue = mSheet.Rows[index1][index2].ToString();
        //当语言表里值为空时设置语言表的默认值--以便隔开
        if (string.IsNullOrEmpty(cellValue))
        {
            if (ExcleGeneratorBase.IsIntType(typeName))
            {
                cellValue = LanguageSplitIndexValue.ToString();
            }

            if (ExcleGeneratorBase.IsStringType(typeName))
            {
                cellValue = string.Empty;
            }
        }

        if (languageDic.TryGetValue(pName, out var objList))
        {
            objList.Add(cellValue);
        }
        else
        {
            List<object> tempList = new List<object>();
            tempList.Add(cellValue);
            languageDic.Add(pName, tempList);
        }
    }


    private static bool WriteLanguageValue()
    {
        try
        {
            if (!languageDic.ContainsKey("Index"))
            {
                Debug.LogError("多语言表需要第一列的属性名称为：Index");
                return false;
            }

            List<object> IndexArray = languageDic["Index"];
            List<int> arrCount = new List<int>();
            int count = 0;

            for (int i = 0; i < IndexArray.Count; i++)
            {
                int IndexValue = int.Parse(IndexArray[i].ToString());

                if (i == IndexArray.Count - 1)
                {
                    arrCount.Add(++count);
                }

                if (IndexValue <= LanguageStartIndex)
                {
                    if (i != 0)
                    {
                        arrCount.Add(count);
                    }

                    count = 0;
                }

                if (IndexValue != LanguageSplitIndexValue)
                {
                    count++;
                }
            }

            string[][] finalStrings = new string[arrCount.Count][];
            for (int j = 0; j < arrCount.Count; j++)
            {
                finalStrings[j] = new string[arrCount[j]];
            }

            int index0 = -1;
            int index1 = 0;

            foreach (var tempValue in languageDic)
            {
                if (tempValue.Key.ToLower().Equals("index"))
                {
                    continue;
                }


                for (int k = 0; k < tempValue.Value.Count; k++)
                {
                    int indexValue = int.Parse(IndexArray[k].ToString());
                    if (indexValue <= LanguageStartIndex)
                    {
                        index0++;
                        index1 = 0;
                    }

                    if (indexValue != LanguageSplitIndexValue)
                    {
                        string value = tempValue.Value[k].ToString();
                        finalStrings[index0][index1] = value;

                        index1++;
                    }
                    else if (indexValue == LanguageSplitIndexValue)
                    {
                        index1 = 0;
                    }
                }

                //多语言处理
                index0 = -1;
                index1 = 0;

                string json = JsonMapper.ToJson(finalStrings);
                string dataPath = Application.dataPath.RemoveString("Assets");
                string path = AB_ResFilePath.jsonLanguageDatasRootDir.CreateDirIfNotExists() + "/" + tempValue.Key + AB_ResFilePath.LanguageSuffix;
                string fullPath = Path.Combine(dataPath, path);

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
                    {
                        textWriter.Write(json);
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("转化语言表失败:" + e);
            return false;
        }

        return true;
    }
}
