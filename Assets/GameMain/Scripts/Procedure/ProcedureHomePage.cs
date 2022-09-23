using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace StarForce {

    public class ProcedureHomePage : ProcedureBase
    {
        private bool m_StartGame = false;
        /// <summary>
        /// 返回选择
        /// </summary>
        private bool m_BackSelect = false;
        private HomePageForm m_HomeForm = null;

        private int? serialId_HomeForm;
        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void StartGame()
        {
            m_StartGame = true;
        }
        public void BackSelect()
        {
            m_BackSelect = true;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_StartGame = false;
            m_BackSelect = false;
            serialId_HomeForm= GameEntry.UI.OpenUIForm(UIFormId.Life_HomePage, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown) {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_HomeForm != null)
            {
                m_HomeForm.Close(isShutdown);
                m_HomeForm = null;
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StartGame)
            {
                Log.Debug("跳转到下一个场景");
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.PortraitOfMan"));
                
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            else if (m_BackSelect)
            {
                Log.Debug("跳转到下一个场景");
                //return;
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.SceneSelect"));
                ChangeState<ProcedureChangeScene>(procedureOwner);//
            }
        }

        protected  override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }


        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            m_HomeForm = (HomePageForm)ne.UIForm.Logic;
        }
    }
}

