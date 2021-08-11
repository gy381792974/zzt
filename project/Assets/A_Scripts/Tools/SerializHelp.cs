using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace EazyGF
{
    /// <summary>
    /// 序列化和反序列化
    /// </summary>
    public class SerializHelp
    {
        static SerializHelp()
        {
            key = 108;//与ab包通用
        }

        public static int GetKey()
        {
            return key;
        }
        //显示Key
        private static int key;
        /// <summary>
        /// 将对象序列化成文件并加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="classObj">需要序列化的对象</param>
        /// <param name="filePath">保存的地址</param>
        /// 默认加密
        public static void SerializeFile<T>(string filePath, T classObj) where T : class
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    MemoryStream memoryStream = new MemoryStream();
                    bf.Serialize(memoryStream, classObj);
                    byte[] newBytes = memoryStream.GetBuffer();
                    EncryptHelp.SimpleEncypt(ref newBytes, key);
                    //将内存中的缓存写入到FileStream
                    memoryStream.WriteTo(fileStream);
                    memoryStream.Close();
                    memoryStream.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.Log($"保存{classObj}数据失败");
#endif
            }
        }

        /// <summary>
        /// 将文件反序列化成对象并解密
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filePath">文件地址</param>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public static T DeserializeFileToObj<T>(string filePath, out bool IsSuccess) where T : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    EncryptHelp.SimpleEncypt(ref bytes, key);
                    memoryStream.Write(bytes, 0, bytes.Length);//写入缓存
                    memoryStream.Flush();//立即将缓存写入文件里
                    memoryStream.Position = 0; //写入完成后需要将位置重置为0！
                    if (bf.Deserialize(memoryStream) is T result)
                    {
                        IsSuccess = true;
                        return result;
                    }
                    IsSuccess = false;
                    return default;
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"读取{typeof(T)}数据失败");
#endif
                IsSuccess = false;
                return default;
            }
        }


        /// <summary>
        /// 序列化文件但是不加密
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="classObj"></param>
        public static void SerializeFileWhitoutEncrypt<T>(string filePath, T classObj) where T : class
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(fileStream, classObj);
                    //将内存中的缓存写入到FileStream
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.Log($"保存{classObj}数据失败");
#endif
            }
        }
        /// <summary>
        /// 将文件反序列化成对象但是不解密
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="filePath">文件地址</param>
        /// <param name="IsSuccess"></param>
        /// <returns></returns>
        public static T DeserializeFileToObjWhitoutEncrypt<T>(string filePath, out bool IsSuccess) where T : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    T result = bf.Deserialize(fileStream) as T;

                    fileStream.Close();
                    fileStream.Dispose();
                    if (result != null)
                    {
                        IsSuccess = true;
                        return result;
                    }
                    IsSuccess = false;
                    return default;
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.Log($"读取{typeof(T)}数据失败");
#endif
                IsSuccess = false;
                return default;
            }
        }

        public static T DeserializeFileToObjWhitByte<T>(byte []bytes, out bool IsSuccess) where T : class
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(bytes, 0, bytes.Length);//写入缓存
                   // memoryStream.Flush();//立即将缓存写入文件里
                   // memoryStream.Position = 0; //写入完成后需要将位置重置为0！
                    

                    //bf.Deserialize()

                    T result = bf.Deserialize(memoryStream) as T;
                    if (result != null)
                    {
                        IsSuccess = true;
                        return result;
                    }
                    IsSuccess = false;
                    return default;
                }
            }
            catch (Exception)
            {
#if UNITY_EDITOR
                Debug.Log($"读取{typeof(T)}数据失败");
#endif
                IsSuccess = false;
                return default;
            }
        }
    }
}
