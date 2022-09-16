using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Fsm;
using GameFramework.Resource;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using  UnityEngine.UI;
public class PosDialogModule : StoryModuleBase
{
    public Text dialogText;
    private Transform m_ImgLeftTransform;
    private Transform m_ImgRightTransform;
    GameObject uiPoolGameObject;
    // Start is called before the first frame update
    private IDataTable<DRPosDialog> drdialogTable;

    private int m_curId = -1;
    private int m_lastId = -100;

    private int tableIndex = 0;
    private int[] dialogIdRange;

    private CharacterModule m_Character;
    public int CurId//获取当前表中
    {
        get
        {
            return m_curId;
        }
    }
    DRPosDialog GetDialogContent(int _dialogId)
    {
        DRPosDialog drDialog = drdialogTable.GetDataRow(_dialogId);
        return drDialog;
    }


    int[] GetDialogIdRange(int _speakAsideId)
    {
        IDataTable<DRPosSpeakAside> drSceneContentTable = GameEntry.DataTable.GetDataTable<DRPosSpeakAside>();
        DRPosSpeakAside drSceneContent = drSceneContentTable.GetDataRow(_speakAsideId);
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
        DRPosDialog drDialog = GetDialogContent(_dialogId);
        System.Action action = () =>
        {
            if (_dialogId == -1)
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
            int _pos = drDialog.Pos;
            Transform parTransform = _pos < 0 ? m_ImgLeftTransform : m_ImgRightTransform;
            dialogText = parTransform.Find("dialog").GetComponentInChildren<Text>();
            parTransform.gameObject.SetActive(true);
            FreshCharater(CurId);

            string _str = drDialog.Content;

            int roleId = drdialogTable.GetDataRow(m_curId).RoleId;
            DRRole role = m_Character.GetRoleTable(roleId);
            GameEntry.TTS.TTSStart(_str, role.SpeakerId);

            float speed = GameEntry.Setting.GetFloat(Constant.Setting.UISpeakTextSpeed);
            //speed = Mathf.Clamp(speed, 1, 10);
            float _delay = _str.Length / speed;
            dialogText.text = string.Empty;
            dialogText.DOText(_str, _delay).onComplete = () =>
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
        Transform _dialogCenterTransform = ((PositiveStoryMgr)fsm.Owner).m_imgDialogTransform;
        m_ImgLeftTransform = _dialogCenterTransform.Find("Image_left");
        m_ImgRightTransform = _dialogCenterTransform.Find("Image_right");
        m_ImgRightTransform.gameObject.SetActive(false);
        m_ImgLeftTransform.gameObject.SetActive(false);

        drdialogTable = GameEntry.DataTable.GetDataTable<DRPosDialog>();

        tableIndex = 0;

        //角色头像刷新
        m_Character = new CharacterModule();
        m_Character.Init(_dialogCenterTransform.parent);
        //
    }

    /// <summary>
    /// 有限状态机状态进入时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnEnter(IFsm<IStoryManager> fsm)
    {
        int m_AsideId = fsm.GetState<PosAsideModule>().CurId;
        dialogIdRange = GetDialogIdRange(m_AsideId);
        //
        if (dialogIdRange[0] == -1)//没有对话
        {
            m_curId = dialogIdRange[0];
            Log.Debug("没有对话");
        }
        else//有对话
        {

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
        if (tableIndex >= dialogIdRange.Length)
        {
            //跳转到旁白
            ChangeState<PosAsideModule>(fsm);
            Log.Debug("当前对话完成 " + CurId);
        }
        else
        {
            if (CurId != m_lastId)
            {
                m_lastId = CurId;
                RefreshContent(CurId);
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

        DOTween.KillAll();
        //强制切换，重置数据
        tableIndex = 0;
        m_lastId = -100;
        m_ImgLeftTransform.parent.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// 有限状态机状态销毁时调用。
    /// </summary>
    /// <param name="fsm">有限状态机引用。</param>
    protected override void OnDestroy(IFsm<IStoryManager> fsm)
    {
        base.OnDestroy(fsm);

        DOTween.KillAll();

        m_Character.OnDestroy();
        m_Character = null;
    }
}

