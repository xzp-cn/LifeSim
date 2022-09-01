using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class RoleItem : MonoBehaviour
{
    /// <summary>
    /// 剧情id
    /// </summary>
    public int roleId = 0;
    
    public Image image_head;
    public Text nameText;

    public Text relationText;

    public Text personlityText;
    // Start is called before the first frame update

    public void Init(int _roleId,Sprite _headSprite,string _relationText,string _personlityText,string _nameText)
    {
        if (_roleId == 0)
        {
            Log.Error("Owner is invalid.");
            return;
        }

        roleId = _roleId;
        image_head.sprite = _headSprite;
        relationText.text = _relationText;
        personlityText.text = _personlityText;
        nameText.text=_nameText;
        //
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        
    }
}
