using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class JsonGenerator
{
    public static readonly string csharpFolderPath = EditorFilePath.Instance.GetJsonClassesFullPath();
    public static readonly string jsonFolderPath = AB_ResFilePath.jsonGameDatasRootDir;

    public static char ArraySplitChar = '_';//数组的隔开符号

    private static StringBuilder jsonBuilder = new StringBuilder(100);
    private static StringBuilder cShapeBuilder = new StringBuilder(100);
    private static StringBuilder cShapeControllerBuilder = new StringBuilder(100);
    public static bool WriteJson(DataTable mSheet,string fileName)
    {
        //写Json文件
        jsonBuilder.Clear();
        cShapeBuilder.Clear();
        cShapeControllerBuilder.Clear();

        if (!WriteJsonData(mSheet))
        {
            return false;
        }
        
        if (!WriteCSharpClass(mSheet, fileName))
        {
            return false;
        }
        //写C#文件
        jsonFolderPath.CreateDirIfNotExists();
        AssetDatabase.Refresh();
        //生成json文件
        using (FileStream fileStream = new FileStream(jsonFolderPath + "/" + fileName + "_Data.txt", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(jsonBuilder);
            }
        }

        //生成C#文件
        using (FileStream fileStream = new FileStream(csharpFolderPath + "/" + fileName + "_Data.cs", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(cShapeBuilder);
            }
        }

        bool isSpec= ExcleGeneratorBase.GetFrontNameAndIndex(fileName, out string frontName, out int nameIndex);
        if (!isSpec)
        {
            frontName = fileName;
        }
        if (string.IsNullOrEmpty(frontName))
        {
            frontName = fileName;
        }
        //生成C#数据管理文件
        using (FileStream fileStream = new FileStream(csharpFolderPath + "/" + frontName + "_DataBase.cs", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(cShapeControllerBuilder);
            }
        }

        return true;
    }

    
    private static List<string> RecordJsonArrayList=new List<string>(10);
    private static bool WriteJsonData(DataTable mSheet)
    {
        jsonBuilder.Append('[');
        jsonBuilder.Append('\n');
        for (int i = 2; i < ExcleGeneratorBase.rowCount; i++)
        {
            RecordJsonArrayList.Clear();
            jsonBuilder.Append('\t');
            jsonBuilder.Append('{');
           
            for (int j = 0; j < ExcleGeneratorBase.colCount; j++)
            {
                if (j == 0)
                {
                    jsonBuilder.Append('\n');
                }
                jsonBuilder.Append('\t');
                jsonBuilder.Append('\t');
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

                //mSheet.Columns[j].ColumnName = pName;
               
                if (!WriteValue(jsonBuilder, mSheet, pName, typeName, i, j))
                {
                    return false;
                }
            }
            jsonBuilder.Append('\t');
            jsonBuilder.Append('}');
            if (i != ExcleGeneratorBase.rowCount - 1)
            {
                jsonBuilder.Append(',');
            }
            jsonBuilder.Append('\n');
        }

        jsonBuilder.Append(']');
        return true;
    }

    //获取Excle表格单个格子的值
    private static string GetCellValue(string cellValue,string typeName)
    {
        //当语言表里值为空时设置语言表的默认值--以便隔开
        if (string.IsNullOrEmpty(cellValue))
        {
            if (ExcleGeneratorBase.IsStringType(typeName))
            {
                cellValue = string.Empty;
            }
            else
            {
                cellValue = "0";
            }
        }

        return cellValue;
    }

    private static bool WriteValue(StringBuilder stringBuilder, DataTable mSheet, string pName, string typeName, int index1, int index2)
    {
        string cellValue = GetCellValue(mSheet.Rows[index1][index2].ToString(), typeName);

        string LogErrorStr = ExcleGeneratorBase.EexcleName + "-> Sheet:" + mSheet.TableName + "中的字段:" + pName + "的值:" + cellValue + "--值的类型不对。";

        //单个类型
        ExcleTypeSupportBase singleType = ExcleGeneratorBase.GetSingleType(typeName);
        //分开数组类型
        ExcleTypeSupportBase arrayType = ExcleGeneratorBase.GetArrayType(typeName);
        //联合数组类型
        ExcleTypeSupportBase multipArrayType = ExcleGeneratorBase.GetArrayMultipType(typeName);

        if (singleType != null)//非数组类型
        {
            stringBuilder.Append($"\"{pName}\"");
            stringBuilder.Append(':');

            if (singleType.GetValue(cellValue, out var result))
            {
                stringBuilder.Append(result);
            }
            else
            {
                Debug.LogError(LogErrorStr);
                return false;
            }

            if (index2 != ExcleGeneratorBase.colCount - 1)
            {
                stringBuilder.Append(',');
            }
        }
        else if (multipArrayType != null)//数组类型-联合数组类型
        {
            stringBuilder.Append($"\"{pName}\"");
            stringBuilder.Append(':');
            stringBuilder.Append('[');

            string[] arryValue = cellValue.Split(new[] { ArraySplitChar }, StringSplitOptions.RemoveEmptyEntries);
            
            for (int j = 0; j < arryValue.Length; j++)
            {
                if (multipArrayType.GetValue(arryValue[j], out var result))
                {
                    stringBuilder.Append(result);
                }
                else
                {
                    Debug.LogError(LogErrorStr);
                    return false;
                }

                if (j != arryValue.Length - 1)
                {
                    stringBuilder.Append(',');
                }
            }

            stringBuilder.Append(']');
            if (index2 != ExcleGeneratorBase.colCount - 1)
            {
                stringBuilder.Append(',');
            }
        }
        else if(arrayType != null) //数组类型-非联合数组
        {
            int count = 0;
            //找到相同命名的数组长度
            for (int i = 0; i < ExcleGeneratorBase.colCount; i++)
            {
                string fullNameWithType = mSheet.Rows[1][i].ToString();//属性名字_类型
                int lastIndex = fullNameWithType.LastIndexOf('_');
                if (lastIndex < 0)
                {
                    string errorStr = "表：" + ExcleGeneratorBase.EexcleName + "--> Shee：" + mSheet.TableName + " 里面的" +
                                      $" 字段类型不正确: {fullNameWithType}，请检查！";
                    Debug.LogError(errorStr);
                    return false;
                }

                string sameName = fullNameWithType.Substring(0, lastIndex);
                if (sameName.Equals(pName))
                {
                    count++;
                }
            }

            //保证只写入一次json名字
            if (!RecordJsonArrayList.Contains(pName))
            {
                RecordJsonArrayList.Add(pName);

                stringBuilder.Append($"\"{pName}\"");
                stringBuilder.Append(':');
                stringBuilder.Append('[');
                int arrayLenth = index2 + count;
                for (int i = index2; i < arrayLenth; i++)
                {
                    cellValue = GetCellValue(mSheet.Rows[index1][i].ToString(), typeName);
                    if (arrayType.GetValue(cellValue, out var result))
                    {
                        stringBuilder.Append(result);
                    }
                    else
                    {
                        Debug.LogError(LogErrorStr);
                        return false;
                    }
                    if (i != arrayLenth - 1)
                    {
                        stringBuilder.Append(',');
                    }
                }
                stringBuilder.Append(']');

                if (index2 != ExcleGeneratorBase.colCount - 1 && index2 != ExcleGeneratorBase.colCount - count)
                {
                    stringBuilder.Append(',');
                }
            }
        }
        return true;
    }


    private static List<string> RecordCshapeList=new List<string>(5);
    private static bool WriteCSharpClass(DataTable mSheet,string fileName)
    {
       bool isSpec= ExcleGeneratorBase.GetFrontNameAndIndex(fileName, out string frontName, out int nameIndex);
        if (!isSpec)
        {
            frontName = fileName;
        }

        RecordCshapeList.Clear();
        for (int i = 0; i < ExcleGeneratorBase.colCount; i++)
        {
            string fullNameWithType = mSheet.Rows[1][i].ToString();//属性名字_类型
            if (!RecordCshapeList.Contains(fullNameWithType))
            {
                RecordCshapeList.Add(fullNameWithType);
            }
        }
        //生成属性类
        cShapeBuilder.AppendLine("//Generator by Tools");
        cShapeBuilder.AppendLine("//Editor by YS");
        cShapeBuilder.AppendLine("using System;");
        cShapeBuilder.AppendLine("using System.Collections.Generic;");
        cShapeBuilder.AppendLine("using UnityEngine;");
        cShapeBuilder.AppendLine("using System.Numerics;\n");
        cShapeBuilder.AppendLine("public class " + fileName + $"_Property : {frontName}_PropertyBase");
        cShapeBuilder.AppendLine("{");
        cShapeBuilder.AppendLine();

        cShapeBuilder.Append('}');
        cShapeBuilder.Append('\n');
        cShapeBuilder.Append('\n');

        //生成获取数据类
        cShapeBuilder.AppendLine("public class " + fileName + "_Data");
        cShapeBuilder.AppendLine("{");
        cShapeBuilder.AppendLine("\t//对象数组");
        cShapeBuilder.AppendLine($"\tpublic static {fileName}_Property[] DataArray;");

        cShapeBuilder.AppendLine("\t//对象数组长度");
        cShapeBuilder.AppendLine($"\tpublic static int ArrayLenth;");
        cShapeBuilder.AppendLine($"\tpublic static void Set{fileName}DataLenth()");
        cShapeBuilder.AppendLine("\t{");
        cShapeBuilder.AppendLine($"\t\t ArrayLenth = DataArray.Length;");
        cShapeBuilder.AppendLine("\t}");

       
        //检查第一列是否是ID
        string id = mSheet.Rows[1][0].ToString().Split('_')[0];
        cShapeBuilder.AppendLine();
        if (id.ToLower().Contains("id"))
        {
            //如果是ID，则根据ID遍历
            cShapeBuilder.AppendLine("\t//通过ID获取数据");
            cShapeBuilder.AppendLine($"\tpublic static {fileName}_Property Get{fileName}_DataByID(int _id)");
            cShapeBuilder.AppendLine("\t{");
            cShapeBuilder.AppendLine($"\t\tfor (int i = 0; i < ArrayLenth; i++)");
            cShapeBuilder.AppendLine("\t\t{");
            cShapeBuilder.AppendLine($"\t\t\tif ( DataArray[i].{id} == _id )");
            cShapeBuilder.AppendLine("\t\t\t{");
            cShapeBuilder.AppendLine("\t\t\t\treturn DataArray[i];");
            cShapeBuilder.AppendLine("\t\t\t}");
            cShapeBuilder.AppendLine("\t\t}");
            cShapeBuilder.AppendLine("\t\tDebug.LogError(\"DataArray中没有该ID：\"+_id);");
            cShapeBuilder.AppendLine("\t\treturn null;");
            cShapeBuilder.AppendLine("\t}");
        }
        
            cShapeBuilder.AppendLine();
            cShapeBuilder.AppendLine("\t//通过下标获取数据");
            cShapeBuilder.AppendLine($"\tpublic static {fileName}_Property Get{fileName}_DataByIndex(int _index)");
            cShapeBuilder.AppendLine("\t{");
            cShapeBuilder.AppendLine("\t\tif (_index < 0 || _index >= ArrayLenth)");
            cShapeBuilder.AppendLine("\t\t{");
            cShapeBuilder.AppendLine("\t\t\tDebug.LogError(\"DataArray下标越界：\"+_index);");
            cShapeBuilder.AppendLine("\t\t\treturn DataArray[0];");
            cShapeBuilder.AppendLine("\t\t}");
            cShapeBuilder.AppendLine("\t\treturn DataArray[_index];");
            cShapeBuilder.AppendLine("\t}");
        

        cShapeBuilder.AppendLine("}");

        //生成管理类
        cShapeControllerBuilder.AppendLine("//Generator by Tools");
        cShapeControllerBuilder.AppendLine("//Editor by YS");
        cShapeControllerBuilder.AppendLine("using System;");
        cShapeControllerBuilder.AppendLine("using System.Collections.Generic;");
        cShapeControllerBuilder.AppendLine("using UnityEngine;");
        cShapeControllerBuilder.AppendLine("using EazyGF;");
        cShapeControllerBuilder.AppendLine("using System.Numerics;\n");

        //1.获取数据类

        //需要特殊生成

        cShapeControllerBuilder.AppendLine($"public class {frontName}_DataBase");
        cShapeControllerBuilder.AppendLine("{");
        if (ExcleGeneratorBase.ControllerClassParmList.TryGetValue(frontName, out var tempValue))
        {
            //通过ID拿数据
            if (id.ToLower().Contains("id"))
            {
                cShapeControllerBuilder.AppendLine("\t//通过ID拿数据");
                cShapeControllerBuilder.AppendLine($"\tpublic static {frontName}_PropertyBase GetPropertyByID(int id, int chapterID=-1)");
                cShapeControllerBuilder.AppendLine("\t{");
                cShapeControllerBuilder.AppendLine("\t\tif (chapterID == -1)");
                cShapeControllerBuilder.AppendLine("\t\t{");
                cShapeControllerBuilder.AppendLine("\t\t\tchapterID = PlayerDataMgr.g_playerData.curChapterID;");
                cShapeControllerBuilder.AppendLine("\t\t}");
                cShapeControllerBuilder.AppendLine();

                for (int i = 0; i < tempValue.indexList.Count; i++)
                {
                    int index = tempValue.indexList[i];
                    cShapeControllerBuilder.AppendLine($"\t\tif (chapterID == {index})");
                    cShapeControllerBuilder.AppendLine("\t\t{");
                    cShapeControllerBuilder.AppendLine($"\t\t\treturn {frontName}_{index}_Data.Get{frontName}_{index}_DataByID(id);");
                    cShapeControllerBuilder.AppendLine("\t\t}");
                }
                cShapeControllerBuilder.AppendLine();
                cShapeControllerBuilder.AppendLine("\t\tDebug.LogError(\"找不到数据\");");
                cShapeControllerBuilder.AppendLine("\t\treturn null;");
                cShapeControllerBuilder.AppendLine("\t}");
                cShapeControllerBuilder.AppendLine();
            }
            
            //通过下标拿数据
            cShapeControllerBuilder.AppendLine("\t//通过下标拿数据");
            cShapeControllerBuilder.AppendLine($"\tpublic static {frontName}_PropertyBase GetPropertyByIndex(int index, int chapterID=-1)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine("\t\tif (chapterID == -1)");
            cShapeControllerBuilder.AppendLine("\t\t{");
            cShapeControllerBuilder.AppendLine("\t\t\tchapterID = PlayerDataMgr.g_playerData.curChapterID;");
            cShapeControllerBuilder.AppendLine("\t\t}");
            cShapeControllerBuilder.AppendLine();
            
            for (int i = 0; i < tempValue.indexList.Count; i++)
            {
                int index = tempValue.indexList[i];
                cShapeControllerBuilder.AppendLine($"\t\tif (chapterID == {index})");
                cShapeControllerBuilder.AppendLine("\t\t{");
                cShapeControllerBuilder.AppendLine($"\t\t\treturn {frontName}_{index}_Data.Get{frontName}_{index}_DataByIndex(index);");
                cShapeControllerBuilder.AppendLine("\t\t}");
            }
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t\tDebug.LogError(\"找不到数据\");");
            cShapeControllerBuilder.AppendLine("\t\treturn null;");
            cShapeControllerBuilder.AppendLine("\t}");

            //获取数组长度
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t//获取数组长度");
            cShapeControllerBuilder.AppendLine($"\tpublic static int GetArrayLenth(int chapterID=-1)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine("\t\tif (chapterID == -1)");
            cShapeControllerBuilder.AppendLine("\t\t{");
            cShapeControllerBuilder.AppendLine("\t\t\tchapterID = PlayerDataMgr.g_playerData.curChapterID;");
            cShapeControllerBuilder.AppendLine("\t\t}");
            cShapeControllerBuilder.AppendLine();

            for (int i = 0; i < tempValue.indexList.Count; i++)
            {
                int index = tempValue.indexList[i];
                cShapeControllerBuilder.AppendLine($"\t\tif (chapterID == {index})");
                cShapeControllerBuilder.AppendLine("\t\t{");
                cShapeControllerBuilder.AppendLine($"\t\t\treturn {frontName}_{index}_Data.ArrayLenth;");
                cShapeControllerBuilder.AppendLine("\t\t}");
            }
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t\tDebug.LogError(\"找不到数据\");");
            cShapeControllerBuilder.AppendLine("\t\treturn 0;");
            cShapeControllerBuilder.AppendLine("\t}");

            //获取数组
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t//获取数组");
            cShapeControllerBuilder.AppendLine($"\tpublic static {frontName}_PropertyBase[] GetArray(int index, int chapterID=-1)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine("\t\tif (chapterID == -1)");
            cShapeControllerBuilder.AppendLine("\t\t{");
            cShapeControllerBuilder.AppendLine("\t\t\tchapterID = PlayerDataMgr.g_playerData.curChapterID;");
            cShapeControllerBuilder.AppendLine("\t\t}");
            cShapeControllerBuilder.AppendLine();

            for (int i = 0; i < tempValue.indexList.Count; i++)
            {
                int index = tempValue.indexList[i];
                cShapeControllerBuilder.AppendLine($"\t\tif (chapterID == {index})");
                cShapeControllerBuilder.AppendLine("\t\t{");
                cShapeControllerBuilder.AppendLine($"\t\t\treturn {frontName}_{index}_Data.DataArray;");
                cShapeControllerBuilder.AppendLine("\t\t}");
            }
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t\tDebug.LogError(\"找不到数据\");");
            cShapeControllerBuilder.AppendLine("\t\treturn null;");
            cShapeControllerBuilder.AppendLine("\t}");
        }
        else//正常生成
        {
            if (id.ToLower().Contains("id"))
            {
                cShapeControllerBuilder.AppendLine("\t//通过ID拿数据");
                cShapeControllerBuilder.AppendLine($"\tpublic static {fileName}_Property GetPropertyByID(int id)");
                cShapeControllerBuilder.AppendLine("\t{");
                cShapeControllerBuilder.AppendLine($"\t\treturn {fileName}_Data.Get{fileName}_DataByID(id);");
                cShapeControllerBuilder.AppendLine("\t}");
                cShapeControllerBuilder.AppendLine();
            }

            cShapeControllerBuilder.AppendLine("\t//通过下标拿数据");
            cShapeControllerBuilder.AppendLine($"\tpublic static {fileName}_Property GetPropertyByIndex(int index)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine($"\t\treturn {fileName}_Data.Get{fileName}_DataByIndex(index);");
            cShapeControllerBuilder.AppendLine("\t}");

            //获取数组长度
            cShapeControllerBuilder.AppendLine();
            cShapeControllerBuilder.AppendLine("\t//获取数组长度");
            cShapeControllerBuilder.AppendLine($"\tpublic static int GetArrayLenth(int chapterID=-1)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine($"\t\treturn {fileName}_Data.ArrayLenth;");
            cShapeControllerBuilder.AppendLine("\t}");
            cShapeControllerBuilder.AppendLine();

            //获取数组
            cShapeControllerBuilder.AppendLine("\t//获取数组");
            cShapeControllerBuilder.AppendLine($"\tpublic static {fileName}_PropertyBase[] GetArray(int index)");
            cShapeControllerBuilder.AppendLine("\t{");
            cShapeControllerBuilder.AppendLine($"\t\treturn {fileName}_Data.DataArray;");
            cShapeControllerBuilder.AppendLine("\t}");
        }


        cShapeControllerBuilder.AppendLine("}");

        //2.生成属性类
        cShapeControllerBuilder.AppendLine();
        cShapeControllerBuilder.AppendLine("public class " + frontName + "_PropertyBase");
        cShapeControllerBuilder.AppendLine("{");

        for (int i = 0; i < ExcleGeneratorBase.colCount; i++)
        {
            string fullNameWithType = mSheet.Rows[1][i].ToString();//属性名字_类型

            //保证只写入一次
            if (RecordCshapeList.Contains(fullNameWithType))
            {
                RecordCshapeList.Remove(fullNameWithType);
            }
            else
            {
                continue;
            }

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


            cShapeControllerBuilder.AppendLine("\t/// <summary>");
            cShapeControllerBuilder.AppendLine("\t/// " + mSheet.Rows[0][i]);//注释
            cShapeControllerBuilder.AppendLine("\t/// </summary>");
            cShapeControllerBuilder.Append('\t');


            ExcleTypeSupportBase singleType = ExcleGeneratorBase.GetSingleType(typeName);
            if (singleType != null)
            {
                cShapeControllerBuilder.AppendLine($"public {singleType.defineName} " + pName + " { get; set; }");
            }
            else
            {
                ExcleTypeSupportBase arrayType = ExcleGeneratorBase.GetArrayType(typeName);
                ExcleTypeSupportBase mArrayType = ExcleGeneratorBase.GetArrayMultipType(typeName);
                string defineName = arrayType != null ? arrayType.defineName : mArrayType != null ? mArrayType.defineName : string.Empty;
                cShapeControllerBuilder.AppendLine($"public {defineName}[] " + pName + " { get; set; }");
            }
        }
        cShapeControllerBuilder.AppendLine("}");

        return true;
    }
}
