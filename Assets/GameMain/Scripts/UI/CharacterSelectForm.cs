using System.Collections;
using System.Collections.Generic;
using GameFramework.DataNode;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class CharacterSelectForm : UGuiForm
{
    private ProcedureSelect m_ProcedureSelect = null;

    [SerializeField] private Toggle m_MainToggle;

    [SerializeField] private Toggle m_StoryToggle;

    [SerializeField] private Text m_EnergyText;

    //public void GotoMainStory(bool isOn)
    //{
    //    Log.Debug("继续游戏");
    //    if (isOn)
    //    {
    //        m_ProcedureSelect.GoMainPage();
    //    }
    //}

    //public void GotoCharacterSelectStory(bool isOn)
    //{
    //    if (isOn)
    //    {
    //        m_ProcedureSelect.GoCharacter();
    //    }
    //}

    public void OnQuitButtonClick()
    {
        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
            Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
            OnClickConfirm = delegate (object userData)
            {
                UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
            },
        });
    }

    void FreshEnergy()
    {
        IDataNode dataNode = GameEntry.DataNode.GetNode("Energy");
        int energy = 0;
        if (dataNode == null)
        {
            GameEntry.DataNode.SetData("Energy", new VarInt32() { Value = energy });
        }
        else
        {
            energy = GameEntry.DataNode.GetData<VarInt32>("Energy");
        }
        m_EnergyText.text = energy.ToString();
    }

    protected override void OnInit(object userData)
    {
        base.OnInit(userData);
    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnOpen(object userData)
#else
    protected internal override void OnOpen(object userData)
#endif
    {
        base.OnOpen(userData);

        m_ProcedureSelect = (ProcedureSelect)userData;
        if (m_ProcedureSelect == null)
        {
            Log.Warning("ProcedureMenu is invalid when open MenuForm.");
            return;
        }
        FreshEnergy();
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
    }


#if UNITY_2017_3_OR_NEWER
    protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
    {
        m_ProcedureSelect = null;

        base.OnClose(isShutdown, userData);
    }
}
