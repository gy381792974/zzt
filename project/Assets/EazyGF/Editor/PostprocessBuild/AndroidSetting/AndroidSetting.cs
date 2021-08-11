using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

namespace JLGame
{
    /// <summary>
    /// 安卓-打成AS包时输出文件替换设置
    /// </summary>
    public class AndroidSetting
    {
        //可赢平台的Android文件
        private static string androidSettingPath = Application.dataPath + "/EazyGF/Editor/PostprocessBuild/AndroidSetting/Res";
        
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
        {
            if (buildTarget != BuildTarget.Android)
            {
                return;
            }

            if (EditorUserBuildSettings.androidBuildSystem != AndroidBuildSystem.Gradle)
            {
                return;
            }

            if (!EditorUserBuildSettings.exportAsGoogleAndroidProject)
            {
                return;
            }

            //if (!Directory.Exists(androidSettingPath))
            //{
            //    return;
            //}

            //string identifier = Application.identifier;
            //string version = Application.version;
            //string exportPath = path + "/" + Application.productName;
            string exportPath = path + "/unityLibrary";

            string targetPath;
            string sourcePath;

            //manifest
            sourcePath = androidSettingPath + "/AndroidManifest.xml";
            targetPath = exportPath + "/src/main/AndroidManifest.xml";
            //先复制再修改，保持源文件不动
            Copy(sourcePath, targetPath);


            ////buildgradle
            //sourcePath = androidSettingPath + "/build.gradle";
            //targetPath = exportPath + "/build.gradle";
            //Copy(sourcePath, targetPath);
            //WriteFileHelp.ReplaceContentBySymbo(targetPath, "versionName ", version, "'");

            //string[] versionArray = version.Split('.');
            //string versionCode = string.Empty;
            //for (int i = 0; i < versionArray.Length; i++)
            //{
            //    versionCode += versionArray[i];
            //}
            //WriteFileHelp.ReplaceContentBySymbo(targetPath, "versionCode", versionCode, " ");
            //WriteFileHelp.ReplaceContentBySymbo(targetPath, "applicationId ", identifier, "'");

            ////gradle.properties
            //sourcePath = androidSettingPath + "/gradle.properties";
            //targetPath = exportPath + "/gradle.properties";
            //Copy(sourcePath, targetPath);

            ////google-services
            //sourcePath = androidSettingPath + "/google-services.json";
            //targetPath = exportPath + "/google-services.json";
            //Copy(sourcePath, targetPath);

            //UnityPlayerActivity
            // sourcePath = androidSettingPath + "/UnityPlayerActivity.java";
            targetPath = path + "/unityLibrary/src/main/java/com/unity3d/player/";

            targetPath += "UnityPlayerActivity.java";
            //Copy(sourcePath, targetPath);
            //WriteFileHelp.ReplaceContentBySymbo(targetPath, "package", identifier, " ");
            string replaceStr =
                @"        if((getIntent().getFlags() & Intent.FLAG_ACTIVITY_BROUGHT_TO_FRONT) !=0)
                {
                     finish();return; 
                }";
            WriteFileHelp.WriteBelow(targetPath, "super.onCreate(savedInstanceState);", replaceStr);
        }
        
        public static void Copy(string sourceFilePath, string targetFilePath)
        {
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, targetFilePath, true);
            }
            else
            {
                Debug.Log("有文件不存在 " + sourceFilePath + " " + targetFilePath);
            }
        }

    }
}

