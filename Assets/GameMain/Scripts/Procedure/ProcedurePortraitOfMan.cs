using System.Collections.Generic;
using GameFramework.Event;
using GameFramework.UI;
using UnityEngine;
using UnityGameFramework.Runtime;
using OpenUIFormSuccessEventArgs = UnityGameFramework.Runtime.OpenUIFormSuccessEventArgs;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace StarForce
{
    public class ProcedurePortraitOfMan : ProcedureBase
    {
        private bool m_GotoMenu = false;
        private bool m_GotoPositive = false;
        //private ProcedureHomePage m_ProcedureMenu = null;
        private PortraitOfManForm m_PortraitOfManForm;

        public override bool UseNativeDialog
        {
            get { return false; }
        }

        public void GotoMenu()
        {
           m_GotoMenu = true;
        }

        public void GotoPositive()
        {
            m_GotoPositive = true;
        }

        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);

            //m_Games.Add(GameMode.Survival, new SurvivalGame());
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
            //IUIGroup group= GameEntry.UI.GetUIForm(UIFormId.Life_portraitOfMan).UIForm.UIGroup;
            //group.Pause = true;
            //m_Games.Clear();
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_GotoMenu = false;
            m_GotoPositive = false;          

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            GameEntry.UI.OpenUIForm(UIFormId.Life_portraitOfMan, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_PortraitOfManForm != null)
            {
                m_PortraitOfManForm.Close(true);
                m_PortraitOfManForm = null;
            }

            m_GotoPositive = false;
            m_GotoMenu =false;
            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_GotoMenu)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.HomePage"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }

            if (m_GotoPositive)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.CareerChoice"));//职业选择
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

            m_PortraitOfManForm = (PortraitOfManForm)ne.UIForm.Logic;
        }



    }
}
