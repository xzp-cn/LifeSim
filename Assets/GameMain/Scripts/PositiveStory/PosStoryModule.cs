using System;
using System.Collections;
using System.Collections.Generic;
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

public class PosStoryModule : StoryModuleBase
{
    private int m_lastStoryId = -100;
    private int m_CurStoryId = -1;
    private int tableIndex = -1;
    private IDataTable<DRPosSceneContent> drSceneContentTable;
    private int[] storyIdRange;

    private IFsm<IStoryManager> m_fsm;
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
        IDataTable<DRPosSceneContent> drSceneContentTable = GameEntry.DataTable.GetDataTable<DRPosSceneContent>();
        DRPosSceneContent[] drSceneContents = drSceneContentTable.GetAllDataRows();
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
        DRPosSceneContent drSceneContent = drSceneContentTable.GetDataRow(m_CurStoryId);
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

        //设置当前剧情
        //GameEntry.Event.Fire(this, PlotItemCallEventArgs.Create(m_CurStoryId));
    }


    protected override void OnInit(IFsm<IStoryManager> fsm)
    {
        BgImage = ((PositiveStoryMgr)fsm.Owner).m_plotDialogTransform.GetComponentInParent<PositiveForm>().transform.Find("Screen_Portrait/bg").GetComponent<RawImage>();

        drSceneContentTable = GameEntry.DataTable.GetDataTable<DRPosSceneContent>();
        storyIdRange = GetStoryIdRange();
        tableIndex = 0;
        m_fsm = fsm;
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<IStoryManager> fsm)
    {
        base.OnEnter(fsm);

        Action callback = () =>
        {
            m_CurStoryId = storyIdRange[Mathf.Clamp(tableIndex, 0, storyIdRange.Length - 1)];
            //刷新背景图
            FreshBG();
        };

        if (m_lastStoryId != -100)
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Dialog.StoryTitle"),
                Message = GameEntry.Localization.GetString("Dialog.StoryMessage"),
                OnClickConfirm = delegate (object userData)
                {
                    StoryRefresh();
                    callback?.Invoke();
                },
            });

        }
        callback?.Invoke();
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
        
        if (tableIndex >= storyIdRange.Length)
        {
            //TODO。弹出对话框结束，跳转到下一个场景
            GameEntry.Event.Fire(this,StoryOverEventArgs.Create(CurStoryId));
            Log.Debug($"所有场景{CurStoryId}结束");
        }
        else
        {
            if (CurStoryId != m_lastStoryId)
            {
                m_lastStoryId = CurStoryId;
                ChangeState<PosAsideModule>(fsm);
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
        base.OnLeave(fsm, isShutdown);
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy(IFsm<IStoryManager> fsm)
    {
        base.OnDestroy(fsm);

        DOTween.KillAll();
    }
}
