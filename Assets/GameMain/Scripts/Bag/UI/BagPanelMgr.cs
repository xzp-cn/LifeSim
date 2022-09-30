using System;
using System.Collections;
using System.Collections.Generic;
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
using RectTransform = UnityEngine.RectTransform;

public class BagPanelMgr : IUIModule
{ 
    BagItem m_bagItem;
    private Transform m_BagPanelTransform;
    private Transform m_BagInfoPanelTransform;
    private IObjectPool<BagItemObject> m_BagItemObjectPool;
    private Transform par;
    private List<BagItem> bagItemList;
    public Toggle[] toggleArr;

    private int curType = -1;
    public void Init(Transform _bagPanelTr,BagItem _bagItem)
    {
        m_bagItem = _bagItem;
        m_BagPanelTransform = _bagPanelTr;
        m_BagItemObjectPool = GameEntry.ObjectPool.GetObjectPool<BagItemObject>("BagItemPool");
        if (m_BagItemObjectPool == null)
        {
            m_BagItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<BagItemObject>("BagItemPool", 30);
        }

        par = m_BagPanelTransform.Find("Image_mobile").GetComponentInChildren<GridLayoutGroup>().transform;
        m_BagInfoPanelTransform = m_BagPanelTransform.Find("InfoPanel");

       bagItemList=new List<BagItem>();

       toggleArr = m_BagPanelTransform.Find("Image_mobile/center/Btns").GetComponentsInChildren<Toggle>(true);
       RegisterButton();
    }

    void RegisterButton()
    {
        toggleArr[0].onValueChanged.AddListener((_isOn) =>
        {
            if (_isOn)
            {
                Log.Debug("显示全部");
                curType = -1;
                //显示背包
                ShowBag();
            }
           
        });

        toggleArr[1].onValueChanged.AddListener((_isOn) =>
        {
            if (_isOn)
            {
                Log.Debug("显示分类一");
                curType = 0;
                //显示背包
                ShowBag();
            }
           
        });

        toggleArr[2].onValueChanged.AddListener((_isOn) =>
        {
            if (_isOn)
            {
                Log.Debug("显示分类二");
                curType = 1;
                //显示背包
                ShowBag();
            }
        });

        toggleArr[3].onValueChanged.AddListener((_isOn) =>
        {
            if (_isOn)
            {
                Log.Debug("显示分类二");
                curType = 2;
                //显示背包
                ShowBag();
            }
        });

        toggleArr[4].onValueChanged.AddListener((_isOn) =>
        {
            if (_isOn)
            {
                Log.Debug("显示分类二");
                curType = 3;
                //显示背包
                ShowBag();
            }
        });


    }


    void ShowBag()
    {
        if (curType==-1)
        {
            foreach (BagItem _bagItem in bagItemList)
            {
                _bagItem.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (BagItem _bagItem in bagItemList)
            {
                _bagItem.gameObject.SetActive(false);
            }

            List<BagItem> _bagItems = bagItemList.FindAll((_item) => { return _item.m_BagItemGridData.bagData.type == curType; });
            foreach (BagItem _bagtItem in _bagItems)
            {
                _bagtItem.gameObject.SetActive(true);
            }
        }
    }
    public BagItem GetBagItemPoolObj()
    {
        BagItem bagItem = null;
        BagItemObject bagItemObject = m_BagItemObjectPool.Spawn();
        if (bagItemObject != null)
        {
            bagItem = (BagItem)bagItemObject.Target;
        }
        else
        {
            bagItem = Object.Instantiate(m_bagItem);
            Transform _transform = bagItem.GetComponent<Transform>();
            _transform.SetParent(par);
            _transform.localScale = Vector3.one;////
            m_BagItemObjectPool.Register(BagItemObject.Create(bagItem), true);
        }
        return bagItem;
    }
    public void RecycleBagItemPoolObj()
    {
        for (int i = bagItemList.Count - 1; i >= 0; i--)
        {
            m_BagItemObjectPool.Unspawn(bagItemList[i]);
        }
        bagItemList.Clear();
    }

    public void OnOpen()
    {
        GameEntry.Event.Subscribe(BagInfoFreshEventArgs.EventId, FreshInfo);
        GameEntry.Event.Subscribe(BagTreasureFreshEventArgs.EventId,FreshTreasureShow);
        m_BagPanelTransform.gameObject.SetActive(true);
        curType = -1;
        FreshTreasureShow(null,null);
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
            Log.Debug("当前背包中没有任何内容");
            return;
        }
        List<TreasureBagData> treasureBagDatas = GameEntry.Setting.GetObject<List<TreasureBagData>>("Treasure");

        List<BagItemGridData> bagItemGridDatas = new List<BagItemGridData>();
        IDataTable<DRBag> bagTable = GameEntry.DataTable.GetDataTable<DRBag>();
        DRBag[] drBags = bagTable.GetAllDataRows();
        for (int i = 0; i < treasureBagDatas.Count; i++)
        {
            TreasureBagData bagData = treasureBagDatas[i];
            DRBag drBag = Array.Find(drBags, (_bagData) => { return _bagData.Id == bagData.bagId; });
            BagItemGridData bagItemGrid = bagItemGridDatas.Find((_data) => { return _data.bagData.name.Equals(drBag.Name); });
            if (bagItemGrid == null)
            {
                bagItemGrid = new BagItemGridData();
                bagItemGrid.bagData = new BagItemData()
                {
                    name = drBag.Name,
                    imageName = drBag.ImageName,
                    inforText = drBag.Content,
                    type = drBag.TypeId
                };
                bagItemGridDatas.Add(bagItemGrid);
            }
            bagItemGrid.num += bagData.num;
            //
        }
        //
        bagItemList.Clear();
        foreach (BagItemGridData _bagItemGridData in bagItemGridDatas)
        {
            BagItem bagItem = GetBagItemPoolObj();
            bagItem.FreshContent(_bagItemGridData);
            bagItemList.Add(bagItem);
        }
        ShowBag();
    }

    private void FreshInfo(object sender, GameEventArgs e)
    {
        
        BagInfoFreshEventArgs bagInfoFreshEventArgs = ((BagInfoFreshEventArgs)e);
        BagInfoData bagInfoData=(BagInfoData)bagInfoFreshEventArgs.UserData;

        if (!bagInfoData.isHover)
        {
            m_BagInfoPanelTransform.gameObject.SetActive(false);
            return;
        }

        m_BagInfoPanelTransform.gameObject.SetActive(true);
        BagItemData bagItemData = bagInfoData.bagData;
        BagItem _bagItem=sender as BagItem;
        RectTransform rt= m_BagInfoPanelTransform as RectTransform;
        Vector3 pos= _bagItem.transform.position + new Vector3(100, -100, 0) + new Vector3(rt.rect.size.x/2,-rt.rect.size.y/2);
        m_BagInfoPanelTransform.position = pos;
        //
        Text text_title=m_BagInfoPanelTransform.Find("top/Text_title").GetComponent<Text>();
        Text Text_attr = m_BagInfoPanelTransform.Find("middle/Text_attr").GetComponent<Text>();
        Text Text_info = m_BagInfoPanelTransform.GetComponentInChildren<ContentSizeFitter>(true).GetComponentInChildren<Text>(true );
        text_title.text= bagItemData.name;
        //Text_attr.text = bagItemData.inforText;
        Text_info.text = bagItemData.inforText;

    }

    public void Update()
    {
    }

    public void OnClose(bool isShutdown, object userData)
    {
        //CloseBagPanel();
        m_BagPanelTransform.gameObject.SetActive(false);
        if (m_BagInfoPanelTransform.gameObject.activeSelf)
        {
            CloseBagPanel();
        }
    }
    public void OnRecycle()
    {
        RecycleBagItemPoolObj();
    }

    public void CloseBagPanel()
    {
        OnRecycle();
        GameEntry.Event.Unsubscribe(BagInfoFreshEventArgs.EventId, FreshInfo);
        GameEntry.Event.Unsubscribe(BagTreasureFreshEventArgs.EventId, FreshTreasureShow);
    }

}
