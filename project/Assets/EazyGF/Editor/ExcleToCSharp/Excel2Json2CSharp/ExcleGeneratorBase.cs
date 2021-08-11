using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using EazyGF;
using UnityEditor;

public enum GeneratorType
{
    Json,
    Language,
    AppConst
}

public class ExcleGeneratorBase
{
    //当前的表名
    public static string EexcleName = string.Empty;
    //横向有多少个格子
    public static int colCount = 0;
    //竖向有多少个格子
    public static int rowCount = 0;
    

    //Excle表支持的类型
    private static readonly ExcleTypeSupportBase[] ExcleTypeSupportTypeArray =
    {
        new ExcleIntType("int","i","ia","iam"),
        new ExcleLongType("long","l","la","lam"),
        new ExcleFloatType("float","f","fa","fam"),
        new ExcleDoubleType("double","d","da","dam"),
        new ExcleBigIntegerType("BigInteger","b","ba","bam"),
        new ExcleStringType("string","s","sa","sam"),
    };


    [MenuItem("ExcleTools/生成Excle--Json数据 &F1", false, 999)]
    public static void StartGeneratorData()
    {
        ExcleBackUp.BackUpExcle(true);
        //本地Excle文件夹位置
        string excleFolderParh =EditorFilePath.Instance.GetLocalExclePath();

        //删除已经生成的json文件
        DeleteJsonData();

        if (ReadAllFiles(excleFolderParh))
        {
            //写入加载Json数据的文件
            JsonDataLoadGenerator.WriteJsonDataLoadFile();

            Debug.Log("生成json数据成功！");
            AssetDatabase.Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("", "出错了", "确定");
        }
    }


    //删除已经生成的json文件
    private static void DeleteJsonData()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(JsonGenerator.jsonFolderPath);
        FileInfo[] fileInfo = directoryInfo.GetFiles();
        for (int i = fileInfo.Length - 1; i > 0; i--)
        {
            fileInfo[i].Delete();
        }
    }

    /// <summary>
    /// 读取所有的Excle表
    /// </summary>
    public static bool ReadAllFiles(string _excelPath)
    {
        DirectoryInfo folder = new DirectoryInfo(_excelPath);
        if (!folder.Exists)
        {
            Debug.LogError("文件夹路径错误！不存在：" + _excelPath);
            return false;
        }

        if (folder.GetFiles().Length <= 0)
        {
            Debug.LogError("该文件夹下无任何数据表！" + _excelPath);
            return false;
        }

        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;

        //生成管理数据类
        ControllerClassParmList.Clear();

        for (int index = 0; index < length; index++)
        {
            if (files[index].Name.Contains("$"))
            {
                continue;
            }
            CheckGenControllClass(files[index]);
        }

        for (int index = 0; index < length; index++)
        {
            if (files[index].Name.Contains("$"))
            {
                continue;
            }

            //生成数据类
            if (!GenSingeFileJson(files[index]))
            {
                return false;
            }
        }
        
        AssetDatabase.Refresh();
        return true;
    }

    public class ControllerClassParm_Editor
    {
        public int count;//个数
        public List<int> indexList;//下划线最后的下标
    }
    public static Dictionary<string, ControllerClassParm_Editor> ControllerClassParmList=new Dictionary<string, ControllerClassParm_Editor>();

    private static void CheckGenControllClass(FileSystemInfo files)
    {
        if (files.Name.EndsWith(".xlsx"))
        {
            string tempFilePath = string.Empty;
            string childPath = files.FullName;
            childPath = childPath.Replace('\\', '/');
            EexcleName = files.Name;
            FileStream mStream;
            try
            {
                mStream = File.Open(childPath, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                //如果第一次打开失败，说明该文件已经被打开，复制该文件后再次尝试打开
                try
                {
                    tempFilePath = EditorFilePath.Instance.GetTempFileRootDirPath() + "/";
                    tempFilePath += files.Name;
                    tempFilePath = tempFilePath.Replace('\\', '/');

                    File.Copy(childPath, tempFilePath, true);
                    mStream = File.Open(tempFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    Debug.LogError("打开:" + EexcleName + "-->失败，请关闭该文件!" + e);
                    return;
                }
            }

            IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
            DataSet mResultSet = mExcelReader.AsDataSet();

            if (mResultSet == null)
            {
                Debug.LogError($"文件{mExcelReader.Name}似乎有点问题！");
                return ;
            }

            //重新构建一个DataSet
            DataSet m_newDataSet = new DataSet();
            DataTable m_newTable = new DataTable();
            m_newDataSet.Tables.Add(m_newTable);

            for (int i = 0; i < mResultSet.Tables.Count; i++)
            {
                DataTable mSheet = mResultSet.Tables[i];
                string sheetName = mSheet.TableName;
                if (GetFrontNameAndIndex(sheetName, out string frontName, out int nameIndex))
                {
                    if (ControllerClassParmList.TryGetValue(frontName, out var tempControoler))
                    {
                        tempControoler.count += 1;
                        tempControoler.indexList.Add(nameIndex);
                    }
                    else
                    {
                        tempControoler = new ControllerClassParm_Editor();
                        tempControoler.count = 1;
                        tempControoler.indexList = new List<int>();
                        tempControoler.indexList.Add(nameIndex);
                        ControllerClassParmList.Add(frontName, tempControoler);
                    }

                    tempControoler.indexList.Sort();
                }
            }
        }
    }

    public static bool GetFrontNameAndIndex(string sheetName,out string frontName,out int nameIndexStr)
    {
        frontName = string.Empty;
        nameIndexStr = -1;
        int lastIndex = sheetName.LastIndexOf("_");
        if (lastIndex == -1)
        {
            return false;
        }

        string lastIndexStr= sheetName.Substring(lastIndex + 1, sheetName.Length - lastIndex - 1);
        frontName = sheetName.Substring(0, lastIndex);
        if (int.TryParse(lastIndexStr, out int tempValue))
        {
            nameIndexStr = tempValue;
            return true;
        }

        return false;
    }


    private static bool GenSingeFileJson(FileSystemInfo files)
    {
        //临时文件名称
        string tempFilePath = string.Empty;
        if (files.Name.EndsWith(".xlsx"))
        {
            StringBuilder CsharpBuilder = new StringBuilder();
            string childPath = files.FullName;
            childPath = childPath.Replace('\\', '/');
            EexcleName = files.Name;
            FileStream mStream;
            try
            {
                mStream = File.Open(childPath, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                //如果第一次打开失败，说明该文件已经被打开，复制该文件后再次尝试打开
                try
                {
                    tempFilePath =EditorFilePath.Instance.GetTempFileRootDirPath()+"/";
                    tempFilePath += files.Name;
                    tempFilePath = tempFilePath.Replace('\\', '/');

                    File.Copy(childPath, tempFilePath, true);
                    mStream = File.Open(tempFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                }
                catch (Exception e)
                {
                    Debug.LogError("打开:" + EexcleName + "-->失败，请关闭该文件!" + e);
                    return false;
                }
            }

            IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
            DataSet mResultSet = mExcelReader.AsDataSet();

            if (mResultSet == null)
            {
                Debug.LogError($"文件{mExcelReader.Name}似乎有点问题！");
                return false;
            }

            //重新构建一个DataSet
            DataSet m_newDataSet = new DataSet();
            DataTable m_newTable = new DataTable();
            m_newDataSet.Tables.Add(m_newTable);
            


            for (int i = 0; i < mResultSet.Tables.Count; i++)
            {
                CsharpBuilder.Clear();
                //默认读取第一个数据表
                DataTable mSheet = mResultSet.Tables[i];
                //如果sheet的名字有以下字段则不生成
                if (mSheet.TableName.ToLower().Contains("sheet") || mSheet.TableName.ToLower().Contains("debug"))
                {
                    continue;
                }
                
                //新构建的DataSet设置table名字
                string fileName = m_newTable.TableName = mSheet.TableName;

                //判断数据表内是否存在数据
                if (mSheet.Rows.Count < 1)
                {
                    Debug.LogError($"文件{mSheet.TableName}数据太少！");
                    return false;
                }
                
                //根据表名设置需要生成的数据类型
                GeneratorType generatorType= GetGeneratorType(fileName);
                
                //找到该表真正的长度
                FindTableRealLenth(mSheet, generatorType);

                //是否有相同的属性名字--常量表的数据排列方式不一致，需要剔除
                if (generatorType!=GeneratorType.AppConst && CheckIsHaveSameKey(mSheet))
                {
                    return false;
                }

                switch (generatorType)
                {
                    case GeneratorType.Json://生成Json文件
                        if (!JsonGenerator.WriteJson(mSheet, fileName))
                        {
                            return false;
                        }
                        break;
                    case GeneratorType.Language://写语言表
                        if (!LanguageGenerator.WriteLanguageData(mSheet))
                        {
                            return false;
                        }
                        break;
                    case GeneratorType.AppConst://写常量表
                        if (!AppConstGenerator.WriteAppConstJson(mSheet, fileName))
                        {
                            return false;
                        }
                        break;
                }
            }
        }
        //删除临时文件
        if (!string.IsNullOrEmpty(tempFilePath))
        {
            if (File.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
        return true;
    }



    private static GeneratorType GetGeneratorType(string fileName)
    {
        switch (fileName.ToLower())
        {
            case "language":
                return GeneratorType.Language;
            case "appconst":
                return GeneratorType.AppConst;
            default:
                return GeneratorType.Json;
        }
    }

    //找到表的真正长度
    private static void FindTableRealLenth(DataTable mSheet, GeneratorType generatorType)
    {
        rowCount = 0;
        colCount = 0;
        try
        {
            while (true)
            {
                if (string.IsNullOrEmpty(mSheet.Rows[rowCount][0].ToString()))
                {
                    //横向最多可以空一格，为多语言表特殊处理
                    int maxLine = 0;
                    if (generatorType == GeneratorType.Language)
                    {
                        maxLine = 1;
                    }
                    if (string.IsNullOrEmpty(mSheet.Rows[rowCount + maxLine][0].ToString()))
                    {
                        break;
                    }
                }
                rowCount++;
            }
        }
        catch (Exception)
        {
           //
        }
        try
        {
            while (true)
            {
                if (string.IsNullOrEmpty(mSheet.Rows[0][colCount].ToString()))
                {
                    break;
                }
                colCount++;
            }
        }
        catch (Exception)
        {
            //
        }
    }


    //检查是否有相同属性名称（策划容易在此处复制粘贴）
    private static bool CheckIsHaveSameKey(DataTable mSheet)
    {
        List<string> nameList = new List<string>();
        for (int i = 0; i < colCount; i++)
        {
            string FullNameWithType = mSheet.Rows[1][i].ToString();//属性名字_类型
            int lastIndex = FullNameWithType.LastIndexOf('_');
            if (lastIndex < 0)
            {
                string errorStr = "表：" + EexcleName + "--> Shee：" + mSheet.TableName + " 里面的" +
                             $" 字段类型不正确: {FullNameWithType}，请检查！";
                Debug.LogError(errorStr);
                return false;
            }

            string pName = FullNameWithType.Substring(0, lastIndex);//属性名字
            string typeName = FullNameWithType.Substring(lastIndex + 1, FullNameWithType.Length - lastIndex - 1);

            //检查是否是支持类型
            if (!IsSupportType(typeName))
            {
                string errorStr = "表：" + EexcleName + "--> Shee：" + mSheet.TableName + " 里面的：" + pName +
                                  $" 字段类型不支持类型: {typeName}，请检查！";
                Debug.LogError(errorStr);
                return false;
            }

            //如果是数组类型则不参与相同字段检查
            if (GetSingleType(typeName) == null)
            {
                continue;
            }

            //检查是否是
            if (nameList.Contains(pName))
            {
                string errorStr = "表：" + EexcleName + "--> Shee：" + mSheet.TableName + " 里面的：" + pName + "有相同字段!!";
                Debug.LogError(errorStr);
                return true;
            }
            nameList.Add(pName);
        }
        return false;
    }

    /// <summary>
    /// 是否是支持的类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsSupportType(string type)
    {
        string lowTypeName = type.ToLower();
        for (int i = 0; i < ExcleTypeSupportTypeArray.Length; i++)
        {
            if (ExcleTypeSupportTypeArray[i].defineName.ToLower().Equals(lowTypeName)
                || ExcleTypeSupportTypeArray[i].singleShortName.ToLower().Equals(lowTypeName)
                || ExcleTypeSupportTypeArray[i].arrayShortName.ToLower().Equals(lowTypeName)
                || ExcleTypeSupportTypeArray[i].arrayMultipShort.ToLower().Equals(lowTypeName))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 获取当个类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ExcleTypeSupportBase GetSingleType(string type)
    {
        string lowTypeName = type.ToLower();
        for (int i = 0; i < ExcleTypeSupportTypeArray.Length; i++)
        {
            if (ExcleTypeSupportTypeArray[i].singleShortName.ToLower().Equals(lowTypeName))
            {
                return ExcleTypeSupportTypeArray[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 获取数组类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ExcleTypeSupportBase GetArrayType(string type)
    {
        string lowTypeName = type.ToLower();
        for (int i = 0; i < ExcleTypeSupportTypeArray.Length; i++)
        {
            if (ExcleTypeSupportTypeArray[i].arrayShortName.ToLower().Equals(lowTypeName))
            {
                return ExcleTypeSupportTypeArray[i];
            }
        }

        return null;
    }

    /// <summary>
    /// 获取联合数组类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static ExcleTypeSupportBase GetArrayMultipType(string type)
    {
        string lowTypeName = type.ToLower();
        for (int i = 0; i < ExcleTypeSupportTypeArray.Length; i++)
        {
            if (ExcleTypeSupportTypeArray[i].arrayMultipShort.ToLower().Equals(lowTypeName))
            {
                return ExcleTypeSupportTypeArray[i];
            }
        }

        return null;
    }

    public static bool IsIntType(string typeName)
    {
        string lowTypeName = typeName.ToLower();
        if (lowTypeName.Equals("int") || lowTypeName.Equals("i"))
        {
            return true;
        }

        return false;
    }

    public static bool IsStringType(string typeName)
    {
        string lowTypeName = typeName.ToLower();
        if (lowTypeName.Equals("string") || lowTypeName.Equals("s"))
        {
            return true;
        }

        return false;
    }
}

