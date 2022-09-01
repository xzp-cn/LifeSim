using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorTool 
{
    [MenuItem("Assets/Txt2Wps")]
    static void OpenAssetExample()
    {
        Object obj = Selection.objects[0];
        string path=AssetDatabase.GetAssetPath(obj);
        Debug.Log(path);
        if (!EditorUtility.DisplayDialog($"Open txt", $"Open txt?", "Yes", "Cancel"))
            return;
        AssetDatabase.OpenAsset(obj.GetInstanceID());
       
    }
}
