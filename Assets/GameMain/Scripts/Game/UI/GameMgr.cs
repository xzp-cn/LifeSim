using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StarForce;
using UnityEngine;

public class GameMgr : IUIModule
{
    private Transform m_GamePanelTransform;
    private System.Action callback;
    public void Init(Transform _panelTransform)
    {
        CommonButton[] commonButtons= m_GamePanelTransform.GetComponentsInChildren<CommonButton>(true);
        commonButtons[0].m_OnClick.AddListener(PuzzleOpen);
        commonButtons[1].m_OnClick.AddListener(OpenOther1);
        commonButtons[2].m_OnClick.AddListener(OpenOther2);
        callback = () =>
        {
            m_GamePanelTransform.gameObject.SetActive(false);
        };
    }

    /// <summary>
    /// 打开拼图游戏
    /// </summary>
    void PuzzleOpen()
    {
        
        GameEntry.UI.OpenUIForm(UIFormId.Life_GameForm, GameId.Puzzle);
        DOTween.PauseAll();
    }



    void OpenOther1()
    {

    }

    void OpenOther2()
    {

    }

    public void OnOpen()
    {

    }

    public void Update()
    {

    }

    public void OnClose(bool isShutdown, object userData)
    {

    }

    public void OnRecycle()
    {

    }
}
