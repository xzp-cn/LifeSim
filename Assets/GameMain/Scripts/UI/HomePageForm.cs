using System.Collections;
using System.Collections.Generic;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class HomePageForm : UGuiForm
{
    [SerializeField]
    private GameObject m_QuitButton = null;

    private ProcedureHomePage m_ProcedureMenu = null;

    public void OnContinueButtonClick()
    {
        //GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        Log.Debug("继续游戏");
        m_ProcedureMenu.StartGame();
    }

    public void OnNewButtonClick()
    {
        //TODO 全局清理
        GameEntry.DataNode.Clear();
        

        Log.Debug("重新开始");
        //
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

        m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
    {
        m_ProcedureMenu = null;

        base.OnClose(isShutdown, userData);
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


