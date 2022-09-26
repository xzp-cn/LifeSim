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
using UnityEngine.Profiling;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class DialogModule : StoryModuleBase
{
    public Text dialogText;
    private Transform m_ImgLeftTransform;
    private Transform m_ImgRightTransform;
    GameObject uiPoolGameObject;
    // Start is called before the first frame update
    private IDataTable<DRDialog> drdialogTable;

    private int m_curId = -1;
    private int m_lastId = -100;

    private int tableIndex = 0;
    private int[] dialogIdRange;

    private Tweener tw;
    private CharacterModule m_Character;
    public int CurId//获取当前表中
    {
        get
        {
            return m_curId;
        }
    }
    DRDialog GetDialogContent(int _dialogId)
    {
        DRDialog drDialog = drdialogTable.GetDataRow(_dialogId);
        return drDialog;
    }


    int[] GetDialogIdRange(int _speakAsideId)
    {
        IDataTable<DRSpeakAside> drSceneContentTable = GameEntry.DataTable.GetDataTable<DRSpeakAside>();
        DRSpeakAside drSceneContent = drSceneContentTable.GetDataRow(_speakAsideId);
        string[] dialogArr = drSceneContent.DialogIdArr.Split('|');
        int[] idArr = new int[dialogArr.Length];
        for (int i = 0; i < dialogArr.Length; i++)
        {
            idArr[i] = int.Parse(dialogArr[i]);
        }
        return idArr;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_roleId">角色id</param>
    /// <param name="_name">角色名字</param>
    /// <param name="_pos">角色位置</param>
    public void RefreshContent(int _dialogId)
    {
        DRDialog drDialog = GetDialogContent(_dialogId);
        System.Action action = () =>
        {
            if (_dialogId==-1)
            {
                tableIndex = 1;
                Log.Debug("对话为空");
                m_ImgLeftTransform.parent.parent.gameObject.SetActive(false);
                return;
            }
            m_ImgLeftTransform.parent.parent.gameObject.SetActive(true);
            m_ImgLeftTransform.gameObject.SetActive(false);
            m_ImgRightTransform.gameObject.SetActive(false);
            //
            int _pos= drDialog.Pos;
            Transform parTransform = _pos < 0 ? m_ImgLeftTransform : m_ImgRightTransform; 
            dialogText = parTransform.Find("dialog").GetComponentInChildren<Text>();
            parTransform.gameObject.SetActive(true);
            FreshCharater(CurId);

            string _str = drDialog.Content;
            
            int roleId = drdialogTable.GetDataRow(m_curId).RoleId;
            DRRole role= m_Character.GetRoleTable(roleId);
            GameEntry.TTS.TTSStart(_str,role.SpeakerId);

            float speed = GameEntry.Setting.GetFloat(Constant.Setting.UISpeakTextSpeed);
            //speed = Mathf.Clamp(speed, 1, 10);
            float _delay = _str.Length / speed;
            dialogText.text = string.Empty;
            tw= dialogText.DOText(_str, _delay);
            tw.onComplete = () =>
            {
                DialogIdFresh();
            };

            //刷新角色

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
    void DialogIdFresh()
    {
        tableIndex++;
        //自我更新对话
        m_curId = dialogIdRange[Mathf.Clamp(tableIndex, 0, dialogIdRange.Length - 1)];
    }

    void FreshCharater(int _curId)
    {
        int roleId = drdialogTable.GetDataRow(_curId).RoleId;
        int pos = drdialogTable.GetDataRow(_curId).Pos;
        m_Character.Refresh(roleId, pos);
    }

    protected override void OnInit(IFsm<IStoryManager> fsm)
    {
        Transform _dialogCenterTransform = ((StoryModuleMgr)fsm.Owner).m_imgDialogTransform;
        m_ImgLeftTransform = _dialogCenterTransform.Find("Image_left");
        m_ImgRightTransform = _dialogCenterTransform.Find("Image_right");
        m_ImgRightTransform.gameObject.SetActive(false);
        m_ImgLeftTransform.gameObject.SetActive(false);

        drdialogTable = GameEntry.DataTable.GetDataTable<DRDialog>();

        IDataNode node = GameEntry.DataNode.GetNode("Story/StoryAside/Dialog");//清理掉
        if (node == null)//首次进入软件
        {
            tableIndex = 0;
        }
        else
        {
            int m_AsideId = fsm.GetState<AsideModule>().CurId;
            dialogIdRange = GetDialogIdRange(m_AsideId);
            int _dialogId = (int)node.GetData().GetValue();
            int findIndex=Array.FindIndex(dialogIdRange, (_value) => { return _value == _dialogId; });
            tableIndex =findIndex <0?0:findIndex;
        }

        //角色头像刷新
        m_Character =new CharacterModule();
        m_Character.Init(_dialogCenterTransform.parent);
        //
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<IStoryManager> fsm)
    {
        int m_AsideId = fsm.GetState<AsideModule>().CurId;
        dialogIdRange = GetDialogIdRange(m_AsideId);
        //
        if (dialogIdRange[0]==-1)//没有对话
        {
            m_curId = dialogIdRange[0];
            Log.Debug("没有对话");
            //m_ImgLeftTransform.parent.parent.gameObject.SetActive(true);
        }
        else//有对话
        {

            //Log.Warning(tableIndex);
            if (tableIndex==dialogIdRange.Length - 1)
            {
                tableIndex -= 1;
            }
          
            FreshCharater(dialogIdRange[tableIndex]);
            FreshCharater(dialogIdRange[tableIndex + 1]);
            m_curId = dialogIdRange[Mathf.Clamp(tableIndex, 0, dialogIdRange.Length - 1)];
        }
        
        
    }

    /// <summary>
    /// 有限状态机状态轮询时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
    /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
    protected override void OnUpdate(IFsm<IStoryManager> fsm, float elapseSeconds, float realElapseSeconds)
    {
        //Profiler.BeginSample("dialogState");
        base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        //m_GotoNextDialogDelaySeconds += elapseSeconds;
        //if (m_GotoNextDialogDelaySeconds >= GotoNexIntervalSeconds)
        //{
        //    m_GotoNextDialogDelaySeconds = 0;
        //    DialogIdFresh();
        //}


        if (tableIndex >=dialogIdRange.Length)
        {
            //跳转到旁白
            ChangeState<AsideModule>(fsm);
            Log.Debug("当前对话完成 "+CurId);
        }
        else
        {
            if (CurId != m_lastId)//
            {
                if (m_lastId != -100&&CurId !=-1)//第二次开始，有对话
                {
                    VarBoolean isAuto=GameEntry.DataNode.GetData<VarBoolean>("isAutoPlay");
                    if (isAuto)//自动播放
                    {
                        m_lastId = CurId;
                        RefreshContent(CurId);
                    }
                    else//手动播放
                    {
                       bool isClicked= fsm.GetData<VarBoolean>("Play");
                       if (isClicked)
                       {
                           fsm.SetData<VarBoolean>("Play",new VarBoolean(){Value = false});
                           m_lastId = CurId;
                           RefreshContent(CurId);
                       }
                    }
                }
                else
                {
                    m_lastId = CurId;
                    RefreshContent(CurId);
                }
            }
        }
        //Profiler.EndSample();
    }

    /// <summary>
    /// 有限状态机状态离开时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
    protected override void OnLeave(IFsm<IStoryManager> fsm, bool isShutdown)
    {
        base.OnLeave(fsm, isShutdown);
        //m_GotoNextDialogDelaySeconds = 0;
        m_Character.OnLeave();
        fsm.SetData<VarBoolean>("Play", new VarBoolean() { Value = false });
        //强制切换，重置数据
        tableIndex = 0;
        m_lastId = -100;
        m_ImgLeftTransform.parent.parent.gameObject.SetActive(false);

        tw?.Kill();
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy(IFsm<IStoryManager> fsm)
    {
        base.OnDestroy(fsm);


        m_Character.OnDestroy();
        m_Character = null;

        if (m_curId>0)
        {
            GameEntry.DataNode.SetData<VarInt32>("Story/StoryAside/Dialog", m_curId);
        }
      
    }
}
