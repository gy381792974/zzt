using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EazyGF;
using UnityEditor;
using UnityEngine;

public class JsonDataLoadGenerator : Editor
{
    //加载文件
    public static void WriteJsonDataLoadFile()
    {
        string gameLoadFolder = EditorFilePath.Instance.GetGeneratedFolderRootPath(); 
        gameLoadFolder.CreateDirIfNotExists();

        //生成的文件类名
        string gameLoadFileName = "LocalJsonDataLoader";
        DirectoryInfo directoryInfo = new DirectoryInfo(AB_ResFilePath.jsonGameDatasRootDir);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.txt");

        StringBuilder gameLoadBuilder = new StringBuilder();
        gameLoadBuilder.AppendLine("using System.IO;");
        gameLoadBuilder.AppendLine("using LitJson;");
        gameLoadBuilder.AppendLine("using UnityEngine;");
        gameLoadBuilder.AppendLine();
        gameLoadBuilder.AppendLine("//自动生成,不要在该文件里写任何代码");
        gameLoadBuilder.AppendLine($"public class {gameLoadFileName}");
        gameLoadBuilder.AppendLine("{");
        gameLoadBuilder.AppendLine("\tprivate string filePath;");
        gameLoadBuilder.AppendLine("\tprivate string jsonValue;");
        gameLoadBuilder.AppendLine("\tprivate TextAsset textAsset;");
        gameLoadBuilder.AppendLine("\tpublic void LoadInEditor()");
        gameLoadBuilder.AppendLine("\t{");
        for (int i = 0; i < fileInfos.Length; i++)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfos[i].FullName);
            int lastIndex = fileNameWithoutExtension.LastIndexOf('_');
            string fileNameWithoutData = fileNameWithoutExtension.Substring(0, lastIndex);
            
            gameLoadBuilder.AppendLine($"\t\t//加载 {fileNameWithoutExtension}");
            gameLoadBuilder.Append("\t\tfilePath =");
            gameLoadBuilder.Append("$\"{AB_ResFilePath.jsonGameDatasRootDir}/");
            gameLoadBuilder.Append($"{fileInfos[i].Name}\";");
            gameLoadBuilder.AppendLine();
            gameLoadBuilder.AppendLine("\t\tjsonValue = File.ReadAllText(filePath);");
            gameLoadBuilder.AppendLine(
                $"\t\t{fileNameWithoutExtension}.DataArray = JsonMapper.ToObject<{fileNameWithoutData}{GetProperty(fileNameWithoutData)}>(jsonValue);");
            if (!fileNameWithoutExtension.ToLower().Contains("appconst"))
            {
                gameLoadBuilder.AppendLine($"\t\t{fileNameWithoutExtension}.Set{fileNameWithoutData}DataLenth();");
            }
           
            gameLoadBuilder.AppendLine();

            if (i == fileInfos.Length - 1)
            {
                gameLoadBuilder.AppendLine("\t\tjsonValue = null;");
                gameLoadBuilder.AppendLine("\t\tfilePath = null;");
            }
        }
        //第一个函数结束
        gameLoadBuilder.AppendLine("\t}");
        //开始写第二个函数
        gameLoadBuilder.AppendLine("\tpublic void LoadInAssetBundle()");
        gameLoadBuilder.AppendLine("\t{");

        for (int i = 0; i < fileInfos.Length; i++)
        {
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileInfos[i].FullName);
            int lastIndex = fileNameWithoutExtension.LastIndexOf('_');
            string fileNameWithoutData = fileNameWithoutExtension.Substring(0, lastIndex);
            string abName = fileNameWithoutExtension.ToLower();
            gameLoadBuilder.AppendLine($"\t\ttextAsset = AssetMgr.Instance.LoadAsset<TextAsset>(\"{abName}\",\"{fileNameWithoutExtension}\");");
            gameLoadBuilder.AppendLine(
                $"\t\t{fileNameWithoutExtension}.DataArray = JsonMapper.ToObject<{fileNameWithoutData}{GetProperty(fileNameWithoutData)}>(textAsset.text);");
            if (!fileNameWithoutExtension.ToLower().Contains("appconst"))
            {
                gameLoadBuilder.AppendLine($"\t\t{fileNameWithoutExtension}.Set{fileNameWithoutData}DataLenth();");
            }
               
            gameLoadBuilder.AppendLine($"\t\tAssetMgr.Instance.UnloadAsset(\"{abName}\",true,true);");
            if (i != fileInfos.Length - 1)
            {
                gameLoadBuilder.AppendLine();
            }
        }
        gameLoadBuilder.AppendLine("\t\ttextAsset = null;");
        gameLoadBuilder.AppendLine("\t}");

        //全部结束
        gameLoadBuilder.AppendLine("}");

        using (FileStream fileStream = new FileStream(gameLoadFolder + "/" + gameLoadFileName + ".cs", FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                textWriter.Write(gameLoadBuilder);
            }
        }
    }

    private static string GetProperty(string fileNameWithoutData)
    {
        bool isAppCount = fileNameWithoutData.ToLower().Equals("appconst");
        string propertyEnd = isAppCount ? "_Property" : "_Property[]";
        return propertyEnd;
    }
}
