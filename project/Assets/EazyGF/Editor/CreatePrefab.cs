using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;
using System.Linq;

namespace EazyGF
{
    public class CreatPrefab
    {
        private static string buildNamePath = "OtherAssets/BuildConfig/";

        private static string createPath = "Assets/TTTNewgy/_CPrefab";

        [MenuItem("Tools/ FollowCamera (alt+q) &q", false, 0)]
        public static void FollowCamera()
        {
            var camera = Camera.allCameras.FirstOrDefault();
            SceneView.lastActiveSceneView.LookAt(camera.transform.position, camera.transform.rotation, 5);
        }

        //[MenuItem("Tools/ CreatPrefab2")]
        public static void CreatPrefab2()
        {
            Transform[] pfs = Selection.transforms;    //   只能在 Hierarchy 面板下多选，在其他面板 下 只能 选一个
            Debug.Log(pfs.Length);
            if (pfs.Length == 0)
            {
                return;
            }

            for (int i = 0; i < pfs.Length; i++)
            {
                PrefabUtility.SaveAsPrefabAsset(pfs[i].gameObject, createPath + pfs[i].name + ".prefab");
            }
        }


        [MenuItem("Tools/ GetTranPath")]
        public static void GetTranPath()
        {
            Transform[] pfs = Selection.transforms;    //   只能在 Hierarchy 面板下多选，在其他面板 下 只能 选一个

            Debug.Log(pfs.Length);

            if (pfs.Length != 1)
            {
                return;
            }

            Debug.LogError(pfs[0].GetPath());
        }

        [MenuItem("Tools/ PrintPreLog")]
        public static void PrintPreLog()
        {
            GetCurrentAssetDirectory();
        }

        [MenuItem("Tools/ReadFile")]
        public static string ReadStallFile()
        {
            string name = GetOtherAssetsFullPath() + "stall.txt";

            //Debug.LogError("  " + name);

            StreamReader streamReader;
            if (File.Exists(name))
            {
                streamReader = File.OpenText(name);
                List<string> list = new List<string>();
                string lineFile;

                StringBuilder sb = new StringBuilder();

                while ((lineFile = streamReader.ReadLine()) != null)
                {
                    sb.Append(lineFile);
                    sb.AppendLine();
                }

                sb = sb.Remove(sb.Length - 2, 2);

                streamReader.Close();
                streamReader.Dispose();

                //Debug.LogError(sb.ToString());

                return sb.ToString();
            }
            return null;
        }

        //[MenuItem("Tools/WriteStall1")]
        public static void WriteStall1()
        {
            WriteDictionaryConent("stall1.txt", GetCurrentAssetDirectory());
            //string name = GetOtherAssetsFullPath() + "/" + "stall.txt";

            //ReadFile("stall");

            //WriteDictionaryConent("stall");
        }

        //[MenuItem("Tools/WriteEquip1")]
        public static void WriteEquip1()
        {
            WriteDictionaryConent("equip1.txt", GetCurrentAssetDirectory());
            //string name = GetOtherAssetsFullPath() + "/" + "stall.txt";

            //ReadFile("stall");

            //WriteDictionaryConent("stall");
        }

        //[MenuItem("Tools/Writeborn1")]
        public static void Writeborn1()
        {
            WriteDictionaryConent("born1.txt", GetCurrentAssetDirectory());
            //string name = GetOtherAssetsFullPath() + "/" + "stall.txt";

            //ReadFile("stall");

            //WriteDictionaryConent("stall");
        }

        [MenuItem("Tools/DeleteCatchFile  删除本地数据")]
        public static void DeleteCatchFile()
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), $"OtherAssets/PlayerData");

            if (Directory.Exists(dir))
            {
                DirectoryInfo direction = new DirectoryInfo(dir);
                FileInfo[] files = direction.GetFiles("*");
                for (int i = 0; i < files.Length; i++)
                {
                    //忽略关联文件

                    if (files[i].Name != "MD5_File.data" && files[i].Name != "PlayerData.data")
                    {
                        RemoveFile(files[i].Name);
                    }

                    //Debug.Log("文件名:" + files[i].Name);
                    //Debug.Log("文件绝对路径:" + files[i].FullName);
                    //Debug.Log("文件所在目录:" + files[i].DirectoryName);
                }
            }

            //Debug.Log("删除成功");
        }



        public static string ReadFile(string txtName)
        {
            string name = GetOtherAssetsFullPath() + "/" + txtName + ".txt";

            Debug.LogError("  " + name);

            StreamReader streamReader;
            if (File.Exists(name))
            {
                streamReader = File.OpenText(name);
                List<string> list = new List<string>();
                string lineFile;

                StringBuilder sb = new StringBuilder();

                while ((lineFile = streamReader.ReadLine()) != null)
                {
                    sb.Append(lineFile);
                    sb.AppendLine();
                }

                sb = sb.Remove(sb.Length - 2, 2);

                streamReader.Close();
                streamReader.Dispose();

                //Debug.LogError(sb.ToString());

                return sb.ToString();
            }
            return null;
        }

        public static void WriteDictionaryConent(string name, string sb)
        {
            string dir = GetOtherAssetsFullPath() + "/";

            StreamWriter streamWriter;
            FileInfo fileInfo = new FileInfo(dir + name);

            if (File.Exists(fileInfo.FullName))
            {
                File.Delete(fileInfo.FullName);
            }

            streamWriter = fileInfo.CreateText();

            streamWriter.Write(sb);

            streamWriter.Close();
            streamWriter.Dispose();
        }

        public static void RemoveFile(string name)
        {
            string dir = Path.Combine(Directory.GetCurrentDirectory(), $"OtherAssets/PlayerData/{name}");

            FileInfo fileInfo = new FileInfo(dir);

            if (File.Exists(fileInfo.FullName))
            {
                File.Delete(fileInfo.FullName);
            }
        }

        public static string GetCurrentAssetDirectory()
        {
            string log = "";

            List<string> ss = new List<string>();

            foreach (var obj in Selection.GetFiltered<UnityEngine.Object>(SelectionMode.Assets))
            {
                var path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path))
                    continue;

                ss.Add(obj.name);
            }

            ss.Sort((a, b) =>
            {
                if(a.Substring(0,1) != b.Substring(0, 1))
                {
                    return a.Substring(0, 1).CompareTo(b.Substring(0, 1));
                }
                if (a.Length != b.Length)
                {
                    return a.Length.CompareTo(b.Length);
                }
                else
                {
                    return a.CompareTo(b);
                }

            });

            for (int i = 0; i < ss.Count; i++)
            {
                log += ss[i] + "\r\n";
            }


            Debug.LogError(log);

            return log;
        }

        public static string GetOtherAssetsFullPath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), buildNamePath);
        }


        
    }
}