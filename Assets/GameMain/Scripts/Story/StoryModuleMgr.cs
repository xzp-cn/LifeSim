using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFramework.DataTable;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;
using GameFramework.Fsm;
using GameFramework.Procedure;
using System;
using DG.Tweening;
using GameFramework.Event;
using JetBrains.Annotations;
namespace StarForce
{
    public class StoryModuleMgr:IStoryManager
    {
        public Transform m_plotDialogTransform;

        public Transform m_imgAsidegTransform;

        public Transform m_DialogTransform;

        public Transform m_imgDialogTransform;

        private IFsmManager m_FsmManager;
        private IFsm<IStoryManager> m_ProcedureFsm;

        public StoryModuleBase CurrentProcedure =>(StoryModuleBase)m_ProcedureFsm.CurrentState;

        public float CurrentProcedureTime => m_ProcedureFsm.CurrentStateTime;

        public StoryModuleMgr(Transform _plot)
        {
            m_plotDialogTransform = _plot;
            m_imgAsidegTransform= m_plotDialogTransform.Find("aside");
            m_DialogTransform = m_plotDialogTransform.Find("dialog");
            m_imgDialogTransform= m_plotDialogTransform.Find("dialog/dialog_center");
            m_DialogTransform.gameObject.SetActive(false);
        
        }

        DRDialog GetDialogContent(int _dialogId)
        {
            IDataTable<DRDialog> drdialogTable = GameEntry.DataTable.GetDataTable<DRDialog>();
            DRDialog drDialog = drdialogTable.GetDataRow(_dialogId);
            return drDialog;
        }


        public void Initialize(IFsmManager fsmManager, params StoryModuleBase[] procedures)
        {
            if (fsmManager == null)
            {
                throw new GameFrameworkException("FSM manager is invalid.");
            }

            m_FsmManager = fsmManager;
            m_ProcedureFsm = m_FsmManager.CreateFsm(this, procedures);
        }

        public void StartProcedure<T>() where T : StoryModuleBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start<T>();
        }

        public void StartProcedure(Type storyBaseType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            m_ProcedureFsm.Start(storyBaseType);
        }

        public bool HasProcedure<T>() where T : StoryModuleBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState<T>();
        }

        public bool HasProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return m_ProcedureFsm.HasState(procedureType);
        }

        public StoryModuleBase GetProcedure<T>() where T : StoryModuleBase
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }
            return m_ProcedureFsm.GetState<T>();
        }

        public StoryModuleBase GetProcedure(Type procedureType)
        {
            if (m_ProcedureFsm == null)
            {
                throw new GameFrameworkException("You must initialize procedure first.");
            }

            return (StoryModuleBase)m_ProcedureFsm.GetState(procedureType);
        }


        public void OnRecycle()
        {
            Log.Error("状态机回收");
            //GameEntry.Fsm.DestroyFsm(m_ProcedureFsm);
        }

        public void OnClose(bool isShutdown, object userData)
        {
            DOTween.KillAll();
            //GameEntry.Event.Unsubscribe(StoryEventArgs.EventId, StoryRefresh);
            bool isDes= GameEntry.Fsm.DestroyFsm(m_ProcedureFsm);
            Log.Debug("销毁状态机 "+isDes);
        }

    }

}
