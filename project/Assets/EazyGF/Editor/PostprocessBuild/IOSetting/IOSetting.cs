using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using ChillyRoom.UnityEditor.iOS.Xcode;
using UnityEngine;

namespace JLGame
{
    /// <summary>
    /// IOS打包XCode设置
    /// </summary>
    public class IOSetting
    {
        [PostProcessBuild(100)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
            {
                return;
            }
            //XCode 项目路径
            string projectPath = pathToBuiltProject;

            PBXProject pbxProject = new PBXProject();
            //XCode 配置路径
            string configFilePath = PBXProject.GetPBXProjectPath(projectPath);

            pbxProject.ReadFromFile(configFilePath);
            string targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());

            //通用设置
            XCodeCommonSetting(pbxProject, targetGuid);
            //framework-静态库
            AddFrameWork(pbxProject, targetGuid);
            //framework-动态库
            AddDynamicFrameworks(ref pbxProject, targetGuid);
            //capability设置
            SetCapabilities(configFilePath);
            //App名字本地化
            AddLocalization(pbxProject, projectPath);
            //修改WriteUnityAppController
            WriteUnityAppController(projectPath);
            //写入文件
            pbxProject.WriteToFile(configFilePath);
            //plist文件
            PlistSetting(projectPath);
        }


        private static void XCodeCommonSetting(PBXProject pbxProject, string targetGuid)
        {
            //string debug = pbxProject.BuildConfigByName(targetGuid, "Debug");
            //string release = pbxProject.BuildConfigByName(targetGuid, "Release");
            //pbxProject.AddBuildPropertyForConfig(debug, "CODE_SIGN_RESOURCE_RULES_PATH",
            //    "$(SDKROOT)/ResourceRules.plist");
            //pbxProject.AddBuildPropertyForConfig(release, "CODE_SIGN_RESOURCE_RULES_PATH",
            //    "$(SDKROOT)/ResourceRules.plist");
            //关闭 ENABLE_BITCODE
            pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");

            pbxProject.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");

            pbxProject.SetBuildProperty(targetGuid, "ARCHS", "$(ARCHS_STANDARD)");
        }

        private static void WriteUnityAppController(string projectPath)
        {
           // string filePath = projectPath + "/Classes/UnityAppController.mm";
            //针对猎豹SDK的设置-新版IS的SDK不需要
            // WriteFileHelp.WriteBelow(filePath,"#include <sys/sysctl.h>", "#include <AppLovinSDK/ALSdk.h>");
             //WriteFileHelp.WriteBelow(filePath, "[KeyboardDelegate Initialize];", "[ALSdk initializeSdk];");
        }

        private static void AddFrameWork(PBXProject pbxProject, string targetGuid)
        {
            // 第三方库，放到Unity工程的Plungins/IOS目录下会自动引用
            //Bugly
            //pbxProject.AddFrameworkToProject(targetGuid, "JavaScriptCore.framework", true);
            //pbxProject.AddFrameworkToProject(targetGuid, "libz.tbd", true);
            //pbxProject.AddFrameworkToProject(targetGuid, "libc++.tbd", true);
        }

        /// <summary>
        /// 动态库
        /// </summary>
        /// <param name="project"></param>
        /// <param name="target"></param>
        private static void AddDynamicFrameworks(ref PBXProject project, string target)
        {
            //const string defaultLocationInProj = "Frameworks/Plugins/iOS";
            //const string coreFrameworkName = "EXAMPLE.framework";

            //string relativeCoreFrameworkPath = Path.Combine(defaultLocationInProj, coreFrameworkName);
            //project.AddDynamicFrameworkToProject(target, relativeCoreFrameworkPath);

            //Debug.Log("Dynamic Frameworks added to Embedded binaries.");
        }


        private static void SetCapabilities(string configPath)
        {
            //var capManager = new ProjectCapabilityManager(configPath, "JLGame.entitlements", PBXProject.GetUnityTargetName());
            //// 推送
            //capManager.AddPushNotifications(true);
            ////内购
            //capManager.AddInAppPurchase();

            //capManager.WriteToFile();

            // proj.SetSystemCapabilities(target, "com.apple.Push", "1");
        }

        private static void PlistSetting(string projectPath)
        {
            string plistPath = projectPath + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDic = plist.root;
            //必选项 不设置这项，提交时会显示缺少合规证明
            rootDic.SetBoolean("ITSAppUsesNonExemptEncryption", false);

            //新版IOS上传到Appstore这个key会报错
            if (rootDic.values.ContainsKey("UIApplicationExitsOnSuspend"))
            {
                if (rootDic.values.Remove("UIApplicationExitsOnSuspend"))
                {
                    Debug.Log("移除..................UIApplicationExitsOnSuspend");
                }
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }

        /// <summary>
        ///可选项 App名字本地化 如果选择该项，需要设置对应的App名字
        /// </summary>
        /// <param name="pbxProject"></param>
        private static void AddLocalization(PBXProject project, string projectPath)
        {
            //string localPath = Path.Combine(Application.dataPath, "JLGame/SDK/IOS/IOS_AppNameLanguage");
            //NativeLocale.AddLocalizedStringsIOS(project, projectPath, localPath);
        }


        public static void CopyDirectory(string srcPath, string destPath)
        {
            try
            {
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)     //判断是否文件夹
                    {
                        if (!Directory.Exists(Path.Combine(destPath, i.Name)))
                        {
                            Directory.CreateDirectory(Path.Combine(destPath, i.Name));   //目标目录下不存在此文件夹即创建子文件夹
                        }
                        CopyDirectory(i.FullName, Path.Combine(destPath, i.Name));    //递归调用复制子文件夹
                    }
                    else
                    {
                        if (!i.FullName.EndsWith(".meta"))//剔除掉meta文件
                            File.Copy(i.FullName, Path.Combine(destPath, i.Name), true);      //不是文件夹即复制文件，true表示可以覆盖同名文件
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

    }
}

