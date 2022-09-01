using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.ObjectPool;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.Serialization;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using Object = UnityEngine.Object;

public class RoleItemMgr
{
    [SerializeField]
    private RoleItem m_RoleItemTemplate = null;

    [SerializeField]
    private Transform m_RoleInstanceRoot = null;

    [SerializeField]
    private int m_InstancePoolCapacity = 20;

    private IObjectPool<RoleItemObject> m_RoleItemObjectPool = null;
    private List<RoleItem> m_ActiveRoleItems = null;
    private Transform m_CachedTransform = null;


    public virtual void Init(Transform _par, RoleItem _roleItem)
    {
        if (_par == null)
        {
            Log.Error("You must set HP bar instance root first.");
            return;
        }

        m_RoleInstanceRoot = _par;
        m_RoleItemTemplate = _roleItem;
        m_CachedTransform = m_RoleInstanceRoot.GetComponent<Transform>();
        m_RoleItemObjectPool = GameEntry.ObjectPool.GetObjectPool<RoleItemObject>();
        if (m_RoleItemObjectPool == null)
        {
            m_RoleItemObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<RoleItemObject>("RoleItem", m_InstancePoolCapacity);
        }
        m_ActiveRoleItems = new List<RoleItem>();

    }

    /// <summary>
    /// 显示所有的故事主线
    /// </summary>
    private IDataTable<DRRole> drRoleTable;

    private GameObject uiPoolGameObject;
    public void InitRelationItems()
    {
        Action action= () =>
        {
            UIPool uiPool = uiPoolGameObject.GetComponent<UIPool>();
            //
            drRoleTable = GameEntry.DataTable.GetDataTable<DRRole>();
            DRRole[] drRoleArray = drRoleTable.GetAllDataRows();
            foreach (DRRole drRole in drRoleArray)
            {
                RoleItem roleItem = ShowPlotItem(drRole.Id);
                Sprite headSprite = uiPool.m_UiStructs.Find((_item) => { return _item.uiSprite.name == drRole.HeadImg; }).uiSprite;
                roleItem.Init(drRole.Id,headSprite,drRole.DesRelation,drRole.Personality,drRole.Name);
            }
        };

        string Asset = "UIPrefab";

        if (uiPoolGameObject == null)
        {
            GameEntry.Resource.LoadAsset(AssetUtility.GetUIFormAsset(Asset), Constant.AssetPriority.UIFormAsset, new LoadAssetCallbacks(
                (assetName, asset, duration, userData) =>
                {
                    uiPoolGameObject = (GameObject)asset;
                    Log.Info("Load 资源 '{0}' OK.", Asset);
                    action.Invoke();
                },

                (assetName, status, errorMessage, userData) =>
                {
                    Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", Asset, assetName, errorMessage);
                }));
        }
        else
        {
            action.Invoke();
        }
    }



    public RoleItem ShowPlotItem(int roleID)
    {
        RoleItem roleItem = GetActiveRoleItem(roleID);
        if (roleItem == null)
        {
            roleItem = CreateRoleItem();
            m_ActiveRoleItems.Add(roleItem);
        }

        return roleItem;
    }


    /// <summary>
    /// 隐藏 PlotItem
    /// </summary>
    /// <param name="roleItem"></param>
    private void HidePlotItem(RoleItem roleItem)
    {
       // roleItem.Reset();
        m_ActiveRoleItems.Remove(roleItem);
        m_RoleItemObjectPool.Unspawn(roleItem);

    }

    private RoleItem GetActiveRoleItem(int id)
    {

        for (int i = 0; i < m_ActiveRoleItems.Count; i++)
        {
            if (m_ActiveRoleItems[i].roleId == id)
            {
                return m_ActiveRoleItems[i];
            }
        }

        return null;
    }

    private RoleItem CreateRoleItem()
    {
        RoleItem roleItem = null;
        RoleItemObject roleItemObject = m_RoleItemObjectPool.Spawn();
        if (roleItemObject != null)
        {
            roleItem = (RoleItem)roleItemObject.Target;
        }
        else
        {
            roleItem = Object.Instantiate(m_RoleItemTemplate);
            Transform transform = roleItem.GetComponent<Transform>();
            transform.SetParent(m_RoleInstanceRoot);
            transform.localScale = Vector3.one;
            m_RoleItemObjectPool.Register(RoleItemObject.Create(roleItem), true);
        }

        return roleItem;
    }






    //周期管理
    public virtual void OnOpen()
    {
        m_RoleInstanceRoot.gameObject.SetActive(true);
        //GameEntry.Event.Subscribe(PlotItemCallEventArgs.EventId, SetCurrentStory);
        //GameEntry.Event.Subscribe(PlotOverEventArgs.EventId, SetOverStory);

    }
    public virtual void Update()
    {
    }
    public void OnClose(bool isShutdown, object userData)
    {
        m_RoleInstanceRoot.gameObject.SetActive(isShutdown);
        //GameEntry.Event.Unsubscribe(PlotItemCallEventArgs.EventId, SetCurrentStory);
        //GameEntry.Event.Unsubscribe(PlotOverEventArgs.EventId, SetOverStory);
    }

    public void OnRecycle()
    {

    }
}
