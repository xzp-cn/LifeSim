using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

public class EditorTools
{
    [MenuItem("Tools/ChangeTxt2UTF8")]
    public static void ChangeEncoding()
    {
        EditorUtility.OpenFolderPanel("select path", "Assets/GameMain", "文件面s板");
        var dir = Directory.GetCurrentDirectory();
        dir= EditorUtility.OpenFolderPanel("select path", "Assets/GameMain", "DataTables");
        Log.Debug(dir);

        //Encoding.RegisterProvider]
        foreach (var f in new DirectoryInfo(dir).GetFiles("*.txt", SearchOption.AllDirectories))
        {
            //Debug.Log(Encoding.Default);
            //Debug.Log(f.FullName+"\n"+));
            Encoding encoding=TxtFileEncoder.GetEncodingFileNoBom(f.FullName);
            Debug.Log(encoding);

            StreamReader sr = new StreamReader(f.FullName, encoding);
            string strBody = sr.ReadToEnd();
            sr.Close();

            try
            {
                StreamWriter sw = new StreamWriter(f.FullName, false, new UTF8Encoding(false));
                sw.Write(strBody);
                sw.Close();
            }
            catch (Exception)
            {
                continue;
            }
        }
    }

    [MenuItem("Assets/ChangeTxt2UTF8")]
    static void ChangeTxt()
    {
        Object obj= Selection.activeObject;
        string path= AssetDatabase.GetAssetPath(obj);
        Debug.Log(path);
        StreamReader sr = new StreamReader(path, Encoding.UTF8);
        string strBody = sr.ReadToEnd();
        sr.Close();
        try
        {
            StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(strBody);
            sw.Flush();
            sw.Close();

        }
        catch (Exception)
        {
            //continue;
        }
    }


}
