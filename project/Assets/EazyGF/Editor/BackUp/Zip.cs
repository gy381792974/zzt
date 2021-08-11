using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;


/// <summary>
/// 压缩的方法
/// </summary>
public class Zip
{
    public void ZipFile(string []strFiles, string strZip)
    {
        ZipOutputStream outstream = new ZipOutputStream(File.Create(strZip));
        outstream.SetLevel(6);
        try
        {
            for (int i = 0; i < strFiles.Length; i++)
            {
                string folderName = Path.GetFileName(strFiles[i]);
                EditorUtility.DisplayProgressBar($"压缩:{folderName}中:", $"总进度:{i + 1} / {strFiles.Length}", (float)(i + 1) / strFiles.Length);
                zip(strFiles[i], outstream, strFiles[i]);
            }
        }
        catch (Exception)
        {
            EditorUtility.ClearProgressBar();
        }

        EditorUtility.ClearProgressBar();
        outstream.Finish();
        outstream.Close();
    }

    public void zip(string strFile, ZipOutputStream outstream, string staticFile)
    {
        if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
        {
            strFile += Path.DirectorySeparatorChar;
        }

        Crc32 crc = new Crc32();
        //获取指定目录下所有文件和子目录文件名称
        string[] filenames = Directory.GetFileSystemEntries(strFile);
        //遍历文件
        foreach (string file in filenames)
        {
            if (Directory.Exists(file))
            {
                zip(file, outstream, staticFile);
            }
            //否则，直接压缩文件
            else
            {
                //打开文件
                FileStream fs = File.OpenRead(file);
                //定义缓存区对象
                byte[] buffer = new byte[fs.Length];
                //通过字符流，读取文件
                fs.Read(buffer, 0, buffer.Length);
                //得到目录下的文件（比如:D:\Debug1\test）,test
                //string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);

                string folderName = Path.GetFileName(Directory.GetCurrentDirectory());
                int index = file.LastIndexOf(folderName, StringComparison.Ordinal);
                string foldeName = file.Substring(index, file.Length - index);
        
                
                // ZipEntry entry = new ZipEntry(tempfile);
                ZipEntry entry = new ZipEntry(foldeName);
                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                outstream.PutNextEntry(entry);
                //写文件
                outstream.Write(buffer, 0, buffer.Length);
            }
        }
    }
}
