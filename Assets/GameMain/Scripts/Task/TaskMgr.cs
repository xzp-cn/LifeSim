﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.ObjectPool;
using StarForce;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class TaskMgr : IUIModule
{
    private RectTransform m_TaskTipTransform;
    private Toggle m_toggleTip;
    private Toggle m_toggleUp;
    private IDataTable<DRSceneContent> m_DrSceneContents;
    private Text m_Text;


    //
    private Transform tipBarPar;
    private GameObject origin;
    private IObjectPool<TipBarItemObject> m_TipObjectPool;

    public void Init(Transform _taskTransform,Transform _tipBarPar,GameObject _origin)
    {
        m_TaskTipTransform = _taskTransform as RectTransform;
        tipBarPar = _tipBarPar;
        origin = _origin;

        m_toggleTip = m_TaskTipTransform.Find("Image/Toggle_tip").GetComponent<Toggle>();
        m_toggleUp = m_TaskTipTransform.Find("Toggle_up").GetComponent<Toggle>();

        UpDown();

        m_toggleTip.onValueChanged.AddListener((isOn) =>
        {
            GameEntry.Event.Fire(this,TaskTipEventArgs.Create(isOn));
        });

        m_toggleUp.isOn = false;

        m_Text = m_TaskTipTransform.GetComponentInChildren<ContentSizeFitter>().GetComponentInChildren<Text>();
        m_DrSceneContents = GameEntry.DataTable.GetDataTable<DRSceneContent>();

        bool exist = GameEntry.ObjectPool.HasObjectPool<TipBarItemObject>("tipBar");
        if (!exist)
        {
            m_TipObjectPool = GameEntry.ObjectPool.CreateSingleSpawnObjectPool<TipBarItemObject>("tipBar",10);//
        }
        else
        {
            m_TipObjectPool= GameEntry.ObjectPool.GetObjectPool<TipBarItemObject>("tipBar");
        }
    }

    //上下
    void UpDown()
    {
        m_toggleUp.onValueChanged.AddListener((isOn) =>
        {
            float m = 0;
            if (isOn)//上
            {
                DOTween.To(
                    () =>
                    {
                        return m;
                    },
                    (t) =>
                    {
                        m_TaskTipTransform.anchoredPosition = new Vector2(0, t);
                    },
                    162,
                    1
                );
            }
            else
            {
                m = 162;
                DOTween.To(
                    () => { return m; },
                    (t) => { m_TaskTipTransform.anchoredPosition = new Vector2(0, t); },
                    0,
                    1
                );
            }
        });
    }

    void StoryFresh(object sender,GameEventArgs args)
    {
        int _storyId = (VarInt32)((MapLocateEventArgs)args).UserData;
        Log.Debug(_storyId);//
        DRSceneContent drSceneContent= m_DrSceneContents.GetDataRow(_storyId);
        string content = drSceneContent.TaskTip;
        if (!string.IsNullOrEmpty(drSceneContent.TaskTip))
        {
            m_Text.text = content;
        }
        else
        {
            m_toggleUp.isOn = true;
        }
         
        TipBar tipBar=GetTipBarGameObject();
        string _content = drSceneContent.StoryName+", "+drSceneContent.StorySummary;
        tipBar.Show(_content,3);
    }

    public void OnOpen()
    {
        m_TaskTipTransform.gameObject.SetActive(true);

        GameEntry.Event.Subscribe(MapLocateEventArgs.EventId,StoryFresh);
        GameEntry.Event.Subscribe(ModelTreasureEventArgs.EventId,ShowTip);
    }

    public void Update()
    {

    }

    public void OnRecycle()
    {

    }

    public void OnClose(bool isShutdown, object userData)
    {
        //提示面板
        m_TaskTipTransform.gameObject.SetActive(false);
        GameEntry.Event.Unsubscribe(MapLocateEventArgs.EventId, StoryFresh);
        GameEntry.Event.Unsubscribe(ModelTreasureEventArgs.EventId, ShowTip);
    }


    void ShowTip(object sender,GameEventArgs args)
    {
        TreasureBagData modelTreasureData = (TreasureBagData)((ModelTreasureEventArgs)args).UserData;
        TipBar tipBar=GetTipBarGameObject();
        string content=  $"完成任务，能量 + {modelTreasureData.power}";
        tipBar.Show(content,2);
    }


    //
    TipBar GetTipBarGameObject()
    {
        TipBarItemObject tipBarItemObject=  m_TipObjectPool.Spawn();
        if (tipBarItemObject!=null)
        {
            return (TipBar) tipBarItemObject.Target;
        }
        else
        {
            GameObject go= GameObject.Instantiate(origin);
            go.transform.SetParent(tipBarPar);
            TipBar tipBar= go.GetComponent<TipBar>();
            m_TipObjectPool.Register(TipBarItemObject.Create(tipBar),true);
            return tipBar;
        }
    }

}