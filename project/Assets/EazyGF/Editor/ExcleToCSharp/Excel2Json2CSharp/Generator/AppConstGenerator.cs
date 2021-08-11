using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using EazyGF;
using UnityEngine;

public class AppConstGenerator 
{
    private static StringBuilder jsonBuilder = new StringBuilder(100);
    private static StringBuilder cShapeBuilder = new StringBuilder(100);
    public static bool WriteAppConstJson(DataTable mSheet , string fileName)
    {
        jsonBuilder.Clear();
        if (!WriteJsonData(mSheet))
        {
            return false;
        }
        
        //写C#文件
        cShapeBuilder.Clear();
        if (!WriteCSharpClass(mSheet, fileName))
        {
            return false;
        }

        using (FileStream fileStream = new FileStream(JsonGenerator.jsonFolderPath + "/" + fileName + "_Data.txt", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(jsonBuilder);
            }
        }
        using (FileStream fileStream = new FileStream(JsonGenerator.csharpFolderPath + "/" + fileName + "_Data.cs", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(cShapeBuilder);
            }
        }

        return true;
    }

    private static bool WriteJsonData(DataTable mSheet)
    {
        //jsonBuilder.Append('[');
        //jsonBuilder.Append('\n');
        //jsonBuilder.Append('\t');
        jsonBuilder.Append('{');
        jsonBuilder.Append('\n');
        for (int i = 0; i < ExcleGeneratorBase.rowCount; i++)
        {
            //mSheet.Rows[i][0] 字段名
            //mSheet.Rows[i][1] 字段值
            //mSheet.Rows[i][3] 字段注释

            jsonBuilder.Append('\t');
           // jsonBuilder.Append('\t');
            string fullNameWithType = mSheet.Rows[i][0].ToString();//属性名字_类型
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
            
            if (!WriteValue(jsonBuilder, mSheet, pName, typeName, i, 1))
            {
                return false;
            }
        }
        
        jsonBuilder.Append('}');

        return true;
    }

    private static bool WriteValue(StringBuilder stringBuilder, DataTable mSheet, string pName, string typeName, int index1, int index2)
    {
        string cellValue = mSheet.Rows[index1][index2].ToString();
        string LogErrorStr = "表:" + ExcleGeneratorBase.EexcleName + "-> Sheet:" + mSheet.TableName + "中的字段:" + pName + "的值:" + cellValue + "--值的类型不对。";
        stringBuilder.Append($"\"{pName}\"");
        stringBuilder.Append(':');
        //设置默认值
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

        //不是数组类型
        ExcleTypeSupportBase singleType = ExcleGeneratorBase.GetSingleType(typeName);
        if (singleType != null)
        {
            if (singleType.GetValue(cellValue, out var result))
            {
                stringBuilder.Append(result);
            }
            else
            {
                Debug.LogError(LogErrorStr);
                return false;
            }

            if (index1 != ExcleGeneratorBase.rowCount - 1)
            {
                stringBuilder.Append(',');
            }

        }
        else//数组类型
        {
            stringBuilder.Append('[');

            string[] arryValue = cellValue.Split(new[] { JsonGenerator.ArraySplitChar }, StringSplitOptions.RemoveEmptyEntries);

            ExcleTypeSupportBase arrayType = ExcleGeneratorBase.GetArrayType(typeName);
            for (int j = 0; j < arryValue.Length; j++)
            {
                if (arrayType.GetValue(arryValue[j], out var result))
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
            if (index1 != ExcleGeneratorBase.rowCount - 1)
            {
                stringBuilder.Append(',');
            }
        }
        stringBuilder.Append('\n');
        return true;
    }

    private static bool WriteCSharpClass(DataTable mSheet, string fileName)
    {
        cShapeBuilder.AppendLine("//Generator by Tools");
        cShapeBuilder.AppendLine("//Editor by YS");
        cShapeBuilder.AppendLine("using System;");
        cShapeBuilder.AppendLine("using System.Collections.Generic;");
        cShapeBuilder.AppendLine("using UnityEngine;");
        cShapeBuilder.AppendLine("using System.Numerics;\n");
        cShapeBuilder.AppendLine("public class " + fileName + "_Property");
        cShapeBuilder.AppendLine("{");
        for (int i = 0; i < ExcleGeneratorBase.rowCount; i++)
        {
            string fullNameWithType = mSheet.Rows[i][0].ToString();//属性名字_类型
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

            cShapeBuilder.AppendLine("\t/// <summary>");
            cShapeBuilder.AppendLine("\t/// " + mSheet.Rows[i][2]);//注释
            cShapeBuilder.AppendLine("\t/// </summary>");
            cShapeBuilder.Append('\t');

            ExcleTypeSupportBase singleType = ExcleGeneratorBase.GetSingleType(typeName);
            if (singleType != null)
            {
                cShapeBuilder.AppendLine($"public {singleType.defineName} " + pName + " { get; set; }");
            }
            else
            {
                ExcleTypeSupportBase arrayType = ExcleGeneratorBase.GetArrayType(typeName);
                cShapeBuilder.AppendLine($"public {arrayType.defineName}[] " + pName + " { get; set; }");
            }
        }
        cShapeBuilder.Append('}');

        cShapeBuilder.Append('\n');
        cShapeBuilder.Append('\n');
        cShapeBuilder.AppendLine("public class " + fileName + "_Data");
        cShapeBuilder.AppendLine("{");
        cShapeBuilder.AppendLine($"\tpublic static {fileName}_Property DataArray;");
        cShapeBuilder.AppendLine("}");


        return true;
    }
}
