using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class MapLocate : IUIModule
{
    public Transform m_MapTransform;

    public Transform img_Locate;

    private MapLocateTip m_MapTip;
    private IDataTable<DRSceneContent> sceneTable;
    //初始化
    public void Init(Transform _mapTransform)
    {
        m_MapTransform = _mapTransform;
        img_Locate = m_MapTransform.Find("Panel/Image_locate");
        if (m_MapTip==null)
        {
            m_MapTip=new MapLocateTip();
            m_MapTip.Init(img_Locate);
        }
        sceneTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();

    }

    public void OnOpen()
    {
        GameEntry.Event.Subscribe(MapLocateEventArgs.EventId,FreshLocate);
        m_MapTip.OnOpen();
    }

    void FreshLocate(object sender,GameEventArgs args)
    {
        int _storyId = (VarInt32)((MapLocateEventArgs)args).UserData;
        DRSceneContent drScene= sceneTable.GetDataRow(_storyId);
        string _locateName="Location_"+ drScene.LocateName;
        Vector3 originPos=Vector3.zero;
        originPos = m_MapTransform.Find("Panel/RawImage/"+_locateName).position;
        m_MapTip.originPos = originPos;

        m_MapTip.FreshTip(drScene.LocateName);
    }
    public void Update()
    {
        m_MapTip.Update();
    }

    public void OnClose(bool isShutdown, object userData)
    {
        GameEntry.Event.Unsubscribe(MapLocateEventArgs.EventId, FreshLocate);
        m_MapTip.OnClose(isShutdown,userData);
    }
    public void OnRecycle()
    {
        m_MapTip.OnRecycle();
    }


   
}
