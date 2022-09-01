using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPool : MonoBehaviour
{
    public List<UIStruct> m_UiStructs=new List<UIStruct>();

    public List<UITextureStruct> m_TextureStructs=new List<UITextureStruct>();
}

[System.Serializable]
public struct UIStruct
{
    public string uiName;
    public Sprite uiSprite;
}

[System.Serializable]
public struct UITextureStruct
{
    public string uiName;
    public Texture2D uiTexture2D;
}
