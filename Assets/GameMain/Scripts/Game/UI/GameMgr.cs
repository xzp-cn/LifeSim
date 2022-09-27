using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using StarForce;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class GameMgr : IUIModule
{
    private Transform m_GamePanelTransform;
    private UnityAction callback0,callback1,callback2;
    private CommonButton[] commonButtons;

    private Action m_GameAction;
    private int energyCosume = 3;
    public void Init(Transform _panelTransform)
    {
        m_GamePanelTransform = _panelTransform;
        commonButtons= m_GamePanelTransform.transform.Find("Image_mobile/center").GetComponentsInChildren<CommonButton>(true);
    }


    void DialogOpen()
    {
        //Log.Warning("123");

        Action action = () =>
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("EnergyConsumptionLack.Title"),
                Message = GameEntry.Localization.GetString( "EnergyConsumptionLack.Message"),
                ConfirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton"),
                OnClickConfirm = delegate(object data)
                {
                    Log.Debug("关闭界面");
                }
            });
        };


        //对话框打开
        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Title = GameEntry.Localization.GetString("EnergyConsumption.Title"),
            Message = string.Format(GameEntry.Localization.GetString("EnergyConsumption.Message"),energyCosume),
            CancelText = GameEntry.Localization.GetString("Dialog.CancelButton"),
            ConfirmText = GameEntry.Localization.GetString("Dialog.ConfirmButton"),
            OnClickConfirm = delegate (object userdata)
            {
                Log.Debug("消耗能量");
                int curEnergy = GameEntry.DataNode.GetData<VarInt32>("Energy");
                int leftEnergy = curEnergy - energyCosume;
                if (leftEnergy<0)
                {
                    action?.Invoke();
                }
                else
                {
                    OnClose(false, null);

                    GameEntry.DataNode.SetData("Energy",new VarInt32(){Value = leftEnergy});
                    GameEntry.Event.Fire(this, FreshEnergyEventArgs.Create(null));

                    switch (energyCosume)
                    {
                        case 5:
                            PuzzleOpen();
                            break;
                        case 10:
                            OpenOther1();
                            break;
                        case 15:
                            OpenOther2();
                            break;
                    }
                }
            },
        });
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
        //DOTween.PauseAll();
        Log.Debug("游戏开始11");
    }

    void OpenOther2()
    {
        //DOTween.PauseAll();
        Log.Debug("游戏开始2");
    }

    public void OnOpen()
    {
        m_GamePanelTransform.gameObject.SetActive(true);

        callback0 += () => { energyCosume = 5; };
        callback0 += DialogOpen;

        callback1 += () => { energyCosume = 10; };
        callback1 += DialogOpen;

        callback2 += () => { energyCosume = 15; };
        callback2 += DialogOpen;

        commonButtons[0].m_OnClick.AddListener(callback0);
        commonButtons[1].m_OnClick.AddListener(callback1);
        commonButtons[2].m_OnClick.AddListener(callback2);
    }

    public void Update()
    {
    }

    public void OnClose(bool isShutdown, object userData)
    {
        m_GamePanelTransform.gameObject.SetActive(false);
        foreach (CommonButton comButton in commonButtons)
        {
            comButton.m_OnClick.RemoveAllListeners();
        }

        callback0 = null;
        callback1 = null;
        callback2 = null;
    }

    public void OnRecycle()
    {

    }
}
