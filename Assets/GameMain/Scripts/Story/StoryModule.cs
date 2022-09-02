using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using Random = UnityEngine.Random;

public class StoryModule :StoryModuleBase
{
    private int m_lastStoryId = -100;
    private int m_CurStoryId =-1;
    private int tableIndex = -1;
    private IDataTable<DRSceneContent> drSceneContentTable;
    private int[] storyIdRange;

    private IFsm<IStoryManager> m_fsm;
    private Dictionary<int, bool> storyOverDic;
    public int CurStoryId//获取当前表中
    {
        get
        {
            return m_CurStoryId;
        }
    }

    private RawImage BgImage;
    int[] GetStoryIdRange()
    {
        IDataTable<DRSceneContent> drSceneContentTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();
        DRSceneContent[] drSceneContents = drSceneContentTable.GetAllDataRows();
        int[] idRange = new int[drSceneContents.Length];
        for (int i = 0; i < drSceneContents.Length; i++)
        {
            idRange[i] = drSceneContents[i].Id;
        }
        return idRange;
    }

    /// <summary>
    /// 剧情更新
    /// </summary>
    void StoryRefresh()
    {
        tableIndex++;
    }

    /// <summary>
    /// UI刷新
    /// </summary>
    GameObject uiPoolGameObject;
    void FreshBG()
    {
        DRSceneContent drSceneContent = drSceneContentTable.GetDataRow(m_CurStoryId);

        GameEntry.Event.Fire(this,ModelChangeEventArgs.Create(drSceneContent.SceneBG));

        //剧情按钮点击    
        GameEntry.Event.Fire(this, PlotItemCallEventArgs.Create(m_CurStoryId));
        return;

        System.Action action = () =>
        {
            UIPool uiPool = uiPoolGameObject.GetComponent<UIPool>();
            UITextureStruct uiStruct = uiPool.m_TextureStructs.Find((_uiStruct) => { return _uiStruct.uiTexture2D.name.Equals(drSceneContent.SceneBG); });
            BgImage.texture = uiStruct.uiTexture2D;
            //BgImage.SetNativeSize();
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


    protected override void OnInit(IFsm<IStoryManager> fsm)
    {
        BgImage = ((StoryModuleMgr) fsm.Owner).m_plotDialogTransform.GetComponentInParent<PortraitOfManForm>().transform.Find("Screen_Portrait/bg").GetComponent<RawImage>();

        drSceneContentTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();
        storyIdRange=GetStoryIdRange();
     
        IDataNode node = GameEntry.DataNode.GetNode("Story");
        if (node == null)//首次进入软件
        {
            tableIndex = 0;
        }
        else
        {
            int storyId = (int)node.GetData().GetValue();
            m_CurStoryId = storyId;
            tableIndex = Array.FindIndex(storyIdRange, (_value) => { return _value == storyId; });
        }
        storyOverDic=new Dictionary<int, bool>();
        for (int i = 0; i < storyIdRange.Length; i++)
        {
            storyOverDic.Add(storyIdRange[i],false);
        }
        //
        m_fsm = fsm;
        //注册修改场景ID 事件
        GameEntry.Event.Subscribe(StoryEventArgs.EventId, ChangeTableIdEvent);
        GameEntry.Event.Subscribe(QuesOverEventArgs.EventId, OverExam);
        GameEntry.Event.Subscribe(StoryFreshEventArgs.EventId, FreshStory);
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override  void OnEnter(IFsm<IStoryManager> fsm)
    {
        base.OnEnter(fsm);

        Action callback = () =>
        {
            m_CurStoryId = storyIdRange[Mathf.Clamp(tableIndex, 0, storyIdRange.Length - 1)];
            //刷新背景图
            FreshBG();
            //更新地图
            GameEntry.Event.Fire(this, MapLocateEventArgs.Create(m_CurStoryId));
        };

        if (m_lastStoryId!=-100)
        {
            storyOverDic[m_CurStoryId] = true;
            //当前剧情结束，场景UI完成 刷新
            GameEntry.Event.Fire(this,PlotOverEventArgs.Create(m_CurStoryId,true));
            //
            //GameEntry.Event.Fire(this, QuesFreshEventArgs.Create(m_CurStoryId));

            GameEntry.Event.Fire(this, QuesOverEventArgs.Create(null));
            //
        }
        else
        {
            callback?.Invoke();
        }
        

    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected  override void OnUpdate(IFsm<IStoryManager> fsm, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

        if (tableIndex>=storyIdRange.Length)
        {
            //TODO。弹出对话框结束，跳转到下一个场景

            GameEntry.Event.Fire(this, StoryOverEventArgs.Create(CurStoryId));
            Log.Debug($"所有场景{CurStoryId}结束");
        }
        else
        {
            if (CurStoryId != m_lastStoryId)
            {
                m_lastStoryId = CurStoryId;
                ChangeState<AsideModule>(fsm);
            }
        }
    }

    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected  override void OnLeave(IFsm<IStoryManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected  override void OnDestroy(IFsm<IStoryManager> fsm)
    {
        base.OnDestroy(fsm);

        GameEntry.Event.Unsubscribe(StoryEventArgs.EventId, ChangeTableIdEvent);
        GameEntry.Event.Unsubscribe(QuesOverEventArgs.EventId, OverExam);
        GameEntry.Event.Unsubscribe(StoryFreshEventArgs.EventId, FreshStory);

        if (GameEntry.DataNode.Root != null)
        {
            VarInt32 varId = new VarInt32();
            varId.SetValue(m_CurStoryId);
            GameEntry.DataNode.SetData("Story",varId);
        }
        storyOverDic.Clear();   
        DOTween.KillAll();
    }

    //顶部菜单改变
    void ChangeTableIdEvent(object sender,GameEventArgs args)
    {
        int _storyId=(VarInt32)((StoryEventArgs)args).UserData;
        tableIndex = Array.FindIndex(storyIdRange, (_value) => { return _value == _storyId; });
        m_lastStoryId = -100;

        m_fsm.GetState<AsideModule>().ChangeTableIdEvent();
        //切换流程状态
        ChangeState<StoryModule>(m_fsm);
    }


    void OverExam(object sender,GameEventArgs args)
    {
        //TODO 选择 0.寄语 1.游戏
        // GameEntry.Event.Fire(JzFreshEventArgs.EventId,JzFreshEventArgs.Create(m_CurStoryId));
        //
        int x= Random.Range(0, 10000) % 2; 
        x = 0;
        if (x==0)
        {
            GameEntry.Event.Fire(JzFreshEventArgs.EventId, JzFreshEventArgs.Create(m_CurStoryId));
        }
        else
        {
            GameEntry.Event.Fire(GameShowEventArgs.EventId, GameShowEventArgs.Create(GameId.Puzzle));
        }
    }

    void FreshStory(object sender,GameEventArgs args)
    {
        Action callback = () =>
        {
            m_CurStoryId = storyIdRange[Mathf.Clamp(tableIndex, 0, storyIdRange.Length - 1)];
            //刷新背景图
            FreshBG();
            //更新地图
            GameEntry.Event.Fire(this, MapLocateEventArgs.Create(m_CurStoryId));
        };

        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Title = GameEntry.Localization.GetString("Dialog.StoryTitle"),
            Message = GameEntry.Localization.GetString("Dialog.StoryMessage"),
            OnClickConfirm = delegate (object userData)
            {
                StoryRefresh();
                callback?.Invoke();
            },
            OnClickCancel = delegate(object userData)
            {//

                int _index=tableIndex;
                _index++;
                int _CurStoryId = storyIdRange[Mathf.Clamp(_index, 0, storyIdRange.Length - 1)];
                GameEntry.Event.Fire(this,PlotItemNextFreshEventArgs.Create(_CurStoryId));
            }
        });
    }

}
