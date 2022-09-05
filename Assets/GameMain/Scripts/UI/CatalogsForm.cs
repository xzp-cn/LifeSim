﻿using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class CatalogsForm : UGuiForm
{
    private ProcedureCataLog m_ProcedureCataLog = null;

    [SerializeField]
    private PlotItem m_PlotItem;

    [SerializeField]
    private Transform m_PlotParent;
    /// <summary>
    /// 滚动条模块
    /// </summary>
    private IUIModule PlotItemMgr = null;

    public void BackHome()
    {
        m_ProcedureCataLog.BackHome();
    }

    void GotoStory(object sender, GameEventArgs args)
    {
        m_ProcedureCataLog.GotoStory();
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

        m_ProcedureCataLog = (ProcedureCataLog)userData;
        if (m_ProcedureCataLog == null)
        {
            Log.Warning("ProcedureMenu is invalid when open MenuForm.");
            return;
        }
        
        GameEntry.Event.Subscribe(StoryEventArgs.EventId, GotoStory);

        PlotItemMgr.OnOpen();

    }

    protected  override void OnUpdate(float elapseSeconds, float realElapseSeconds)
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
        base.OnClose(isShutdown, userData);

        m_ProcedureCataLog = null;

        GameEntry.Event.Unsubscribe(StoryEventArgs.EventId, GotoStory);

        PlotItemMgr.OnClose(isShutdown, userData);
    }

}
