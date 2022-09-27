using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class GameForm : UGuiForm
{
    public Transform m_PuzzleTr;
    private PuzzleMgr m_PuzzleMgr;

    public GameObject gridItem;
    public GameObject pieceItem;
    public EdgeStruct m_EdgeStruct;

    GameId gameId;

    public void PlayUISound(int soundId)
    {
        GameEntry.Sound.PlaySound(soundId);
    }


    public void OnFinishBtnClick()
    {
        bool isFinish= m_PuzzleMgr.Settle();
        Log.Debug("isFinish "+isFinish.ToString());
        if (isFinish)
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("PuzzleTip.Title"),
                Message = GameEntry.Localization.GetString("PuzzleTip.Message"),
                CancelText = GameEntry.Localization.GetString("Dialog.CancelButton"),
                ConfirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton"),
                OnClickConfirm = delegate (object userdata)
                {
                    Log.Debug("未完成");
                    Close();
                    m_PuzzleMgr.OnClose(false, null);
                    DOTween.PlayAll();//
                },
            });
        }
        else
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("PuzzleTip.Title"),
                Message = GameEntry.Localization.GetString("PuzzleTip.ErrorMessage"),
                CancelText = GameEntry.Localization.GetString("Dialog.CancelButton"),
                ConfirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton"),
                OnClickConfirm = delegate (object userdata)
                {
                    Log.Debug("未完成");
                },
            });
        }
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);

        gameId= (GameId)userData;

        if (m_PuzzleMgr == null)
        {
            m_PuzzleMgr = new PuzzleMgr();
            m_PuzzleMgr.Init(m_PuzzleTr, gridItem, pieceItem, m_EdgeStruct);
        }

    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnOpen(object userData)
#else
    protected internal override void OnOpen(object userData)
#endif
    {
        base.OnOpen(userData);
     
        if (gameId == GameId.Puzzle)
        {
            m_PuzzleMgr.OnOpen();
        }
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            GameEntry.UI.CloseUIForm(this);
        }
        m_PuzzleMgr?.Update();
    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
    {
        base.OnClose(isShutdown, userData);
    }
}