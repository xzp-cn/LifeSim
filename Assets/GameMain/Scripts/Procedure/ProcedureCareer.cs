using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedureCareer : ProcedureBase
    {
        private bool m_GotoHome = false;
        private CareerForm m_CareerForm;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        /// <summary>
        /// 返回首页
        /// </summary>
        public void GotoHome()
        {
            m_GotoHome = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoHome = false;

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.Life_CareerForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_CareerForm != null)
            {
                m_CareerForm.Close(isShutdown);
                m_CareerForm = null;
            }

            m_GotoHome = false;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_GotoHome)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.HomePage"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            m_CareerForm = (CareerForm)ne.UIForm.Logic;
        }
    }
}

