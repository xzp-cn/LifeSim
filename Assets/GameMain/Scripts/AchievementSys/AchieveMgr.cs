using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using Object = UnityEngine.Object;

public class AchieveMgr : IUIModule
{
    AchieveItem m_achieveItem;
    private Transform m_AchivevePanelTransform;
    private Transform m_AchieveInfoPanelTransform;
    private IObjectPool<AchieveItemObject> m_AchieveItemObjectPool;
    private Transform par;
    private List<AchieveItem> achieveItemList;
    private IDataTable<DRAchievementSystem> m_AchievementSystems;
    private List<AchieveItemGridData> achieveItemGridDatas;

    public void Init(Transform _achievePanelTr, AchieveItem _achieveItem)
    {
        m_achieveItem = _achieveItem;
        m_AchivevePanelTransform = _achievePanelTr;
        m_AchieveItemObjectPool = GameEntry.ObjectPool.GetObjectPool<AchieveItemObject>("AchieveItemPool");
        if (m_AchieveItemObjectPool == null)
        {
            m_AchieveItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<AchieveItemObject>("AchieveItemPool", 10);
        }

        par = m_AchivevePanelTransform.Find("Image_mobile").GetComponentInChildren<GridLayoutGroup>().transform;
        m_AchieveInfoPanelTransform = m_AchivevePanelTransform.Find("Image_mobile/center/Achieve");
        achieveItemList = new List<AchieveItem>();

        m_AchievementSystems = GameEntry.DataTable.GetDataTable<DRAchievementSystem>();
        achieveItemGridDatas = new List<AchieveItemGridData>();

    }

    public AchieveItem GetAchieveItemPoolObj()
    {
        AchieveItem achieveItem = null;
        AchieveItemObject achieveItemObject = m_AchieveItemObjectPool.Spawn();
        if (achieveItemObject != null)
        {
            achieveItem = (AchieveItem)achieveItemObject.Target;
        }
        else
        {
            achieveItem = Object.Instantiate(m_achieveItem);
            Transform _transform = achieveItem.GetComponent<Transform>();
            _transform.SetParent(par);
            _transform.localScale = Vector3.one;
            m_AchieveItemObjectPool.Register(AchieveItemObject.Create(achieveItem), true);
        }
        return achieveItem;
    }

    public void RecycleBagItemPoolObj()
    {
        for (int i = achieveItemList.Count - 1; i >= 0; i--)
        {
            m_AchieveItemObjectPool.Unspawn(achieveItemList[i]);
        }
        achieveItemList.Clear();
    }

    public void OnOpen()
    {
        GameEntry.Event.Subscribe(AchieveInfoFreshEventArgs.EventId, FreshInfo);
        GameEntry.Event.Subscribe(AchieveMedalFreshEventArgs.EventId, FreshTreasureShow);

    }

    public void Show()
    {
        m_AchivevePanelTransform.gameObject.SetActive(true);
        FreshTreasureShow(null, null);
    }
    /// <summary>
    /// 刷新事件
    /// </summary>
    void FreshTreasureShow(object sender, GameEventArgs e)
    {
        RecycleBagItemPoolObj();
        //
        if (!GameEntry.Setting.HasSetting("Treasure"))
        {
            Log.Warning("当前背包中没有任何内容");
            return;
        }
        List<TreasureBagData> treasureBagDatas = GameEntry.Setting.GetObject<List<TreasureBagData>>("Treasure");

        Func<string, int> callBack = (str) =>
         {
             string[] strArr = str.Split('|');
             int n = -1;
             for (int i = 0; i < strArr.Length; i++)
             {
                 TreasureBagData treasureBagData = treasureBagDatas.Find((item) => { return item.bagId == int.Parse(strArr[i]); });
                 if (treasureBagData == null)
                 {
                     n = -1;
                     break;
                 }
                 else
                 {
                     if (n == -1)
                     {
                         n = treasureBagData.num;
                     }
                     else
                     {
                         n = Mathf.Min(n, treasureBagData.num);
                     }
                 }
             }
             return n;
         };


        DRAchievementSystem[] drAchievementSystems = m_AchievementSystems.GetAllDataRows();


        foreach (DRAchievementSystem _drAchievementSystem in drAchievementSystems)
        {
            string treasureArr = _drAchievementSystem.TreasureArr;
            int n = callBack(treasureArr);
            if (n == -1)
            {
                Log.Debug($"{treasureArr} 没有勋章");
                continue;
            }
            else
            {
                AchieveItemGridData achieveItemGridData = achieveItemGridDatas.Find((_item) => { return _item.bagData.name == _item.bagData.name; });
                if (achieveItemGridData == null)
                {
                    achieveItemGridData = new AchieveItemGridData();
                    achieveItemGridData.bagData = new AchieveItemData()
                    {
                        name = _drAchievementSystem.Name,
                        imageName = _drAchievementSystem.ImageName,
                        inforText = _drAchievementSystem.Content,
                        treasureArr = treasureArr
                    };
                    achieveItemGridData.num = n;
                    achieveItemGridDatas.Add(achieveItemGridData);
                }
                else
                {
                    achieveItemGridData.num += n;
                }

                //数据刷新
                string[] strArr = treasureArr.Split('|');
                for (int i = 0; i < strArr.Length; i++)
                {
                    int id = int.Parse(strArr[i]);
                    TreasureBagData data = treasureBagDatas.Find((_bagData) => { return _bagData.bagId == id; });
                    data.num -= n;
                    if (data.num == 0)
                    {
                        treasureBagDatas.Remove(data);
                    }
                }

                //
                GameEntry.UI.OpenDialog(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("Dialog.AchieveTitle"),
                    UserData = _drAchievementSystem.ImageName,
                });
            }
        }
        //
        achieveItemList.Clear();
        foreach (AchieveItemGridData _bagItemGridData in achieveItemGridDatas)
        {
            AchieveItem achieveItem = GetAchieveItemPoolObj();
            achieveItem.FreshContent(_bagItemGridData);
            achieveItemList.Add(achieveItem);
        }

        GameEntry.Setting.SetObject<List<TreasureBagData>>("Treasure", treasureBagDatas);
    }

    private void FreshInfo(object sender, GameEventArgs e)
    {
        AchieveInfoFreshEventArgs achieveInfoFreshEventArgs = ((AchieveInfoFreshEventArgs)e);
        AchieveItemData bagItemData = (AchieveItemData)achieveInfoFreshEventArgs.UserData;

        m_AchieveInfoPanelTransform.gameObject.SetActive(true);
        
        Image image_achieve = m_AchieveInfoPanelTransform.Find("Top/Image_achieve").GetComponent<Image>();

        FreshImage(bagItemData.imageName,image_achieve);

        Transform contentTransform=m_AchieveInfoPanelTransform.Find("center/Scroll View").GetComponentInChildren<HorizontalLayoutGroup>().transform;
        Image[] imgs= contentTransform.GetComponentsInChildren<Image>();
        DRBag[] drBags=GameEntry.DataTable.GetDataTable<DRBag>().GetAllDataRows();
        foreach (Image img in imgs)
        {
            img.gameObject.SetActive(false);
        }

        string[] treasures=bagItemData.treasureArr.Split('|');
        int index=0;
        foreach (string _treasure in treasures)
        {
            int _id = int.Parse(_treasure);
            DRBag drBag=Array.Find(drBags, (_drbag) => { return _drbag.Id == _id; });
            string imageName=drBag.ImageName;
            Image img=contentTransform.GetChild(index++).GetComponent<Image>();
            FreshImage(imageName,img);
            img.gameObject.SetActive(true);
        }

        //Text Text_attr = m_AchieveInfoPanelTransform.Find("middle/Text_attr").GetComponent<Text>();
        Text contenText = m_AchieveInfoPanelTransform.Find("bottom/content")
            .GetComponentInChildren<Text>();
        contenText.text = bagItemData.inforText;
    }
    GameObject uiPoolObject = null;

    void FreshImage(string _imageName,Image img)
    {
        Action action = () =>
        {
            UIPool uiPool = uiPoolObject.GetComponent<UIPool>();
            UIStruct uiStruct = uiPool.m_UiStructs.Find((_uiStruct) => { return _uiStruct.uiSprite.name.Equals(_imageName); });
            img.sprite=uiStruct.uiSprite;
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

    public void OnClose(bool isShutdown, object userData)
    {
        m_AchivevePanelTransform.gameObject.SetActive(false);
        //if (m_AchieveInfoPanelTransform.gameObject.activeSelf)
        //{
        //    CloseBagPanel();
        //}
        //Log.Warning("移除事件");
        GameEntry.Event.Unsubscribe(AchieveInfoFreshEventArgs.EventId, FreshInfo);
        GameEntry.Event.Unsubscribe(AchieveMedalFreshEventArgs.EventId, FreshTreasureShow);
    }
    public void OnRecycle()
    {
        RecycleBagItemPoolObj();
    }

    public void CloseBagPanel()
    {
        m_AchivevePanelTransform.gameObject.SetActive(false);
        OnRecycle();
    }

    public void Update()
    {
        
    }


    
}
