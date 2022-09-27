using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Resource;
using StarForce;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class BagItem : MonoBehaviour
{
    public Image m_Image;
    public Text m_NumText;  
    public Text m_NameText;

    public BagItemGridData m_BagItemGridData;
    private GameObject uiPoolObject;
    private BagInfoData m_BagInfoData;

    private void Start()
    {
        m_BagInfoData=new BagInfoData();
    }

    public void FreshContent(BagItemGridData _bagItemGridData)
    {
        m_BagItemGridData = _bagItemGridData;
        FreshImg();
        m_NumText.text = _bagItemGridData.num.ToString();
        m_NameText.text = _bagItemGridData.bagData.name;
    }

    void FreshImg()
    {
        Action action = () =>
        {
            UIPool uiPool = uiPoolObject.GetComponent<UIPool>();
            UIStruct uiStruct = uiPool.m_UiStructs.Find((_uiStruct) => { return _uiStruct.uiSprite.name.Equals(m_BagItemGridData.bagData.imageName); });
            m_Image.sprite = uiStruct.uiSprite;
        };

        if (uiPoolObject==null)
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

    public void OnHover()
    {
        Log.Debug("OnHover");
        m_BagInfoData.bagData = m_BagItemGridData.bagData;
        m_BagInfoData.isHover = true;
        GameEntry.Event.Fire(this, BagInfoFreshEventArgs.Create(m_BagInfoData));
    }

    public void OnHoverExit()
    {
        m_BagInfoData.bagData = m_BagItemGridData.bagData;
        m_BagInfoData.isHover = false;
        GameEntry.Event.Fire(this, BagInfoFreshEventArgs.Create(m_BagInfoData));
    }
}


public class BagInfoData
{
    public BagItemData bagData;
    public bool isHover;
}