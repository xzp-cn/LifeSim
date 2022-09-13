using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameFramework.DataTable;
using GameFramework.Event;
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
    public void Init(Transform _taskTransform)
    {
        m_TaskTipTransform = _taskTransform as RectTransform;
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
    }

    public void OnOpen()
    {
        m_TaskTipTransform.gameObject.SetActive(true);

        GameEntry.Event.Subscribe(MapLocateEventArgs.EventId,StoryFresh);
    }

    public void Update()
    {

    }

    public void OnRecycle()
    {

    }

    public void OnClose(bool isShutdown, object userData)
    {
        m_TaskTipTransform.gameObject.SetActive(false);
        GameEntry.Event.Unsubscribe(MapLocateEventArgs.EventId, StoryFresh);
    }

}