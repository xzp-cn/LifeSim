using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class AchieveItem : MonoBehaviour
{
    public Image m_Image;
    public Text m_NumText;

    public AchieveItemGridData m_BagItemGridData;
    private GameObject uiPoolObject;
    public void FreshContent(AchieveItemGridData _bagItemGridData)
    {
        m_BagItemGridData = _bagItemGridData;
        FreshImg();
        m_NumText.text = _bagItemGridData.num.ToString();
    }

    void FreshImg()
    {
        Action action = () =>
        {
            UIPool uiPool = uiPoolObject.GetComponent<UIPool>();
            UIStruct uiStruct = uiPool.m_UiStructs.Find((_uiStruct) => { return _uiStruct.uiSprite.name.Equals(m_BagItemGridData.bagData.imageName); });
            m_Image.sprite = uiStruct.uiSprite;
        };

        if (uiPoolObject == null)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUIFormAsset("UIPrefab"), Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    uiPoolObject = (GameObject)asset;
                    Log.Info("Load 资源 '{0}' OK.", "UIPrefab");
                    action?.Invoke();
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", "UIPrefab", assetName, errorMessage);
                }));
        }
        else
        {
            action?.Invoke();
        }
    }

    public void OnClick()
    {
        GameEntry.Event.Fire(this, AchieveInfoFreshEventArgs.Create(m_BagItemGridData.bagData));
    }
}
