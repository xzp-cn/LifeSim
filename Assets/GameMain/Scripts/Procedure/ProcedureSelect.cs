using GameFramework.Event;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
namespace StarForce
{
    public class ProcedureSelect : ProcedureBase
    {
        private StoryMode m_StoryMode;
        private SceneSelectForm m_SelectForm = null;

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }

        public void GoMainPage()
        {
            m_StoryMode = StoryMode.MainStory;
        }

        public void GoCharacter()
        {
            m_StoryMode = StoryMode.CharacterSelect;
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            m_StoryMode = StoryMode.None;

            GameEntry.UI.OpenUIForm(UIFormId.life_SelectForm, this);
        }

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);

            if (m_SelectForm != null)
            {
                m_SelectForm.Close(isShutdown);
                m_SelectForm = null;//
            }
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (m_StoryMode==StoryMode.MainStory)
            {
                Log.Debug("跳转到下一个场景");
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.HomePage"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            else if (m_StoryMode==StoryMode.CharacterSelect)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.CharacterSelect"));
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }

        protected override void OnDestroy(ProcedureOwner procedureOwner)
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
            m_SelectForm = (SceneSelectForm)ne.UIForm.Logic;
        }
    }
}

[System.Serializable]
public enum StoryMode//
{
    None,
    /// <summary>
    /// 主线选择
    /// </summary>
    MainStory,
    /// <summary>
    /// 角色选择
    /// </summary>
    CharacterSelect,
}