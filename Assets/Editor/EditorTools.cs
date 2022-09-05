using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

public class EditorTools
{
    [MenuItem("Tools/ChangeTxt2UTF8")]
    public static void ChangeEncoding()
    {
       // EditorUtility.OpenFolderPanel("select path", "Assets/GameMain", "文件面板");
        var dir = Directory.GetCurrentDirectory();
        dir= EditorUtility.OpenFolderPanel("select path", "Assets/GameMain", "DataTables");
        Log.Debug(dir);
        foreach (var f in new DirectoryInfo(dir).GetFiles("*.txt", SearchOption.AllDirectories))
        {
            Debug.Log(f.FullName);
            var s = File.ReadAllText(f.FullName, Encoding.Default);
            try
            {
                File.WriteAllText(f.FullName, s, new UTF8Encoding(false));
            }
            catch (Exception)
            {
                continue;
            }
        }
    }
}
