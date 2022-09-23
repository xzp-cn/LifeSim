using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using DG.Tweening;

public class AsideModule :StoryModuleBase
{
    public Text Text_asideTitle;
    public Text contenText;

    private Tweener m_Tweener;

    private int tableIndex = 0;
    private int m_curId = -1;
    private int m_lastId = -100;
    private IDataTable<DRSpeakAside> drSpeakAsideTable;


    private int[] speakAsideRange;
    public int CurId//获取当前表中
    {
        get
        {
            return m_curId;
        }
    }

    /// <summary>
    /// 旁白显示
    /// </summary>
    public void RefreshAside(string _content,Action callback)
    {
        Text_asideTitle.transform.parent.gameObject.SetActive(true);

        float speed=GameEntry.Setting.GetFloat(Constant.Setting.UIAsideTextSpeed,1);
       // speed= Mathf.Clamp(speed, 1, 10);
        float _delay = _content.Length / speed;
        contenText.text = string.Empty;
        m_Tweener = contenText.DOText(_content, _delay);
        m_Tweener.onComplete= () =>
        {
            callback?.Invoke();
            callback = null;
        };
    }


    int[] GetSpeakAsideIdRange(int _storyId)
    {
        IDataTable<DRSceneContent> drSceneContentTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();
        DRSceneContent drSceneContent = drSceneContentTable.GetDataRow(_storyId);
        string[] speakAsideArr = drSceneContent.SpeakAsideIdArray.Split('|');
        int[] idRange=new int[speakAsideArr.Length];
        for (int i = 0; i < speakAsideArr.Length; i++)
        {
            idRange[i]=int.Parse(speakAsideArr[i]);
        }
       
        return idRange;
    }

    DRSpeakAside GetSpeakAsideContent(int _speakId)
    {
        DRSpeakAside drSpeakAside = drSpeakAsideTable.GetDataRow(_speakId);
        return drSpeakAside;
    }

    void AsideFresh()
    {
        tableIndex++;
    }


    protected override void OnInit(IFsm<IStoryManager> fsm)
    {
        Transform _asideTransform = ((StoryModuleMgr)fsm.Owner).m_imgAsidegTransform;
        contenText = _asideTransform.GetComponentInChildren<ContentSizeFitter>().transform.Find("Text_asideContent").GetComponent<Text>();
        Text_asideTitle = _asideTransform.Find("Text_asideTitle").GetComponent<Text>();

        drSpeakAsideTable = GameEntry.DataTable.GetDataTable<DRSpeakAside>();

        IDataNode node = GameEntry.DataNode.GetNode("Story/StoryAside");//获取数据，继续
        if (node == null)//首次进入软件
        {
            tableIndex = 0;
        }
        else
        {
            int asideId = node.GetData<VarInt32>();
            int _storyId = fsm.GetState<StoryModule>().CurStoryId;
            Log.Debug($"场景id={_storyId}");
            speakAsideRange = GetSpeakAsideIdRange(_storyId);
            m_curId = asideId;
            tableIndex = Array.FindIndex(speakAsideRange, (_value) => { return _value == asideId; });
        }

    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<IStoryManager> fsm)
    {
        if (m_lastId!=-100)
        {
            
            AsideFresh();
        }
        int _storyId = fsm.GetState<StoryModule>().CurStoryId;
        Log.Debug($"场景id={_storyId}");
        speakAsideRange = GetSpeakAsideIdRange(_storyId);

      
        m_curId = speakAsideRange[Mathf.Clamp(tableIndex, 0, speakAsideRange.Length - 1)];
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate(IFsm<IStoryManager> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);


        if (tableIndex >=speakAsideRange.Length)
        {
            tableIndex = 0;
            m_lastId = -100;
            ChangeState<StoryModule>(fsm);
            Log.Debug($"当前旁白{CurId}结束");
        }
        else
        {
            if (CurId != m_lastId)//文本更新
            {
                VarBoolean isAuto = GameEntry.DataNode.GetData<VarBoolean>("isAutoPlay");
                if (isAuto)//自动播放
                {
                    m_lastId = CurId;

                    RefreshAside(GetSpeakAsideContent(CurId).Content, () =>
                    {
                        Text_asideTitle.transform.parent.gameObject.SetActive(false);
                        ChangeState<DialogModule>(fsm);
                    });//刷新旁白
                }
                else//手动播放
                {
                    bool isClicked = fsm.GetData<VarBoolean>("Play");
                    if (isClicked)
                    {
                        fsm.SetData<VarBoolean>("Play", new VarBoolean() { Value = false });
                        RefreshAside(GetSpeakAsideContent(CurId).Content, () =>
                        {
                            Text_asideTitle.transform.parent.gameObject.SetActive(false);
                            ChangeState<DialogModule>(fsm);
                        });//刷新旁白
                    }
                }


              
              
            }
        }
    }
    
    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected override void OnLeave(IFsm<IStoryManager> fsm, bool isShutdown)
    {
        fsm.SetData<VarBoolean>("Play", new VarBoolean() { Value = false });
        base.OnLeave(fsm,isShutdown);
        m_Tweener.Kill();
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy(IFsm<IStoryManager> fsm)
    {
        base.OnDestroy(fsm);
        if (GameEntry.DataNode.Root != null)
        {
            VarInt32 varId=new VarInt32();
            varId.SetValue(m_curId);
            GameEntry.DataNode.SetData("Story/StoryAside",varId);
        }

        //注册修改场景ID 事件
        //GameEntry.Event.Unsubscribe(AsideEventArgs.EventId, ChangeTableIdEvent);
    }

    public void ChangeTableIdEvent()
    {
        //强制切换到主剧情
        //重置数据
        tableIndex = 0;
        m_lastId = -100;
    }

}
