using System.Collections;
using System.Collections.Generic;
using GameFramework.DataNode;
using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class HomePageForm : UGuiForm
{
    [SerializeField]
    private GameObject m_QuitButton = null;

    private ProcedureHomePage m_ProcedureMenu = null;

    [SerializeField]
    private PlotItem m_PlotItem;

    [SerializeField]
    private Transform m_PlotParent;

    [SerializeField] private Text m_EnergyText;

    /// <summary>
    /// 滚动条模块
    /// </summary>
    private IUIModule PlotItemMgr = null;

    public void OnNewButtonClick()
    {
        //TODO 全局清理
        GameEntry.DataNode.Clear();
        GameEntry.SceneModel.ResetAll();
        
        Log.Debug("重新开始");
        //
        //m_ProcedureMenu.StartGame();
    }

    void GotoStory(object sender, GameEventArgs args)
    {
        Log.Debug("继续游戏");
        m_ProcedureMenu.StartGame();
    }

    public void OnQuitButtonClick()
    {
        GameEntry.UI.OpenDialog(new DialogParams()
        {
            Mode = 2,
            Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
            Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
            OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
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
        if (PlotItemMgr == null)
        {
            PlotItemMgr = new PlotItemMgr();
            ((PlotItemMgr)PlotItemMgr).Init(m_PlotParent, m_PlotItem);
            ((PlotItemMgr)PlotItemMgr).InitStoryItems();
        }
    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnOpen(object userData)
#else
    protected internal override void OnOpen(object userData)
#endif
    {
        base.OnOpen(userData);

        m_ProcedureMenu = (ProcedureHomePage)userData;
        if (m_ProcedureMenu == null)
        {
            Log.Warning("ProcedureMenu is invalid when open MenuForm.");
            return;
        }

        PlotItemMgr.OnOpen();

        FreshEnergy();

        m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);

        GameEntry.Event.Subscribe(StoryEventArgs.EventId, GotoStory);
    }

    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);
        PlotItemMgr.Update();
    }


#if UNITY_2017_3_OR_NEWER
    protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
    {
        m_ProcedureMenu = null;

        base.OnClose(isShutdown, userData);

        PlotItemMgr.OnClose(isShutdown, userData);

        GameEntry.Event.Unsubscribe(StoryEventArgs.EventId, GotoStory);
    }


    //protected override void OnResume()
    //{
    //    Visible = true;
    //    StopAllCoroutines();
    //}

    //protected override void OnPause()
    //{
    //    //TODO 弹出框出现时是否遮挡背面
    //    base.OnPause();
    //}
    
    //void OnDisable()
    //{
    //    Debug.Log("关闭 ");
    //}

}


