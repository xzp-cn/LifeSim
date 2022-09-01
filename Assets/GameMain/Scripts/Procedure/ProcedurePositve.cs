using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedurePositive : ProcedureBase
    {
        private bool m_GotoCareer = false;
        private PositiveForm m_PositiveForm;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        public void GotoCareer()
        {
            m_GotoCareer = true;
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

            m_GotoCareer = false;

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.Life_PositiveForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_PositiveForm != null)
            {
                m_PositiveForm.Close(isShutdown);
                m_PositiveForm = null;
            }

            m_GotoCareer = false;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_GotoCareer)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.CareerChoice"));
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
            m_PositiveForm = (PositiveForm)ne.UIForm.Logic;
        }
    }
}
