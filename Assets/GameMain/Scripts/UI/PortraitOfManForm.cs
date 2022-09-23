using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

namespace StarForce
{
    public class PortraitOfManForm : UGuiForm
    {
        [SerializeField] private Sprite playSprite;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Image pauseImage;

        [SerializeField]
        private PlotItem m_PlotItem;

        [SerializeField]
        private Transform m_MapTransform;

        [SerializeField]
        private Transform m_PlotParent;
        private ProcedurePortraitOfMan m_ProcedurePortraitOfMan = null;

        [SerializeField]
        private Transform PlotDialogTransform;

        [SerializeField]
        private RoleItem m_RoleItem;

        /// <summary>
        /// 滚动条模块
        /// </summary>
        private IUIModule PlotItemMgr = null;

        /// <summary>
        /// 对话模块
        /// </summary>
        private StoryModuleMgr StoryModuleMgr = null;


        [SerializeField]
        private Transform m_RoleParent;
        private RoleItemMgr m_RoleItemMgr = null;

        //定位
        public MapLocate m_MapLocate;

        [SerializeField]
        private BagItem m_BagItem;
        [SerializeField] 
        private Transform m_bagPanel;
        private BagPanelMgr m_BagPanelMgr;

        //成就系统
        [SerializeField]
        private AchieveItem m_AchieveItem;
        [SerializeField]
        private Transform m_achievePanel;
        private AchieveMgr m_AchievePanelMgr;

        /// <summary>
        /// 界面关闭
        /// </summary>
        [SerializeField]
        private Transform m_GamePanel;
        private GameMgr m_GameMgr;

        /// <summary>
        /// 测试题
        /// </summary>
        //[SerializeField]
        //public GameObject m_ExamPanel;
        //public GameObject m_TwoGameObject;
        //public GameObject m_ThreeGameObject;
        //public QuesManager m_QuesManager;

        /// <summary>
        /// 大学生寄语
        /// </summary>
        public Transform m_JzTransform;
        public JzMgr m_JzMgr;

        public TaskMgr m_TaskMgr;
        public Transform m_TaskTransform;
        public GameObject tipBar;

        public void SetPlayMode()
        {
            bool has= GameEntry.Fsm.HasFsm<IStoryManager>();
            if (has)
            {
                IFsm<IStoryManager> iStoryManagerFsm= GameEntry.Fsm.GetFsm<IStoryManager>();

                iStoryManagerFsm.SetData("Play",new VarBoolean(){Value = true});
            }
        }

        public void PauseOrPlay()
        {
            if (pauseImage.sprite.name.Equals(playSprite.name))
            {
                pauseImage.sprite = pauseSprite;//切换为手动模式
                GameEntry.DataNode.SetData("isAutoPlay",new VarBoolean()
                {
                    Value = false
                });
            }
            else
            {
                pauseImage.sprite = playSprite;//切换为自动模式
                GameEntry.DataNode.SetData("isAutoPlay", new VarBoolean()
                {
                    Value = true
                });
            }

            return;
            //if (pauseImage.sprite == playSprite)
            //{
            //    pauseImage.sprite = pauseSprite;//暂停
            //    GameEntry.Base.PauseGame();
            //    GameEntry.TTS.StopPlay();
            //    DOTween.PauseAll();
            //}
            //else
            //{
            //    pauseImage.sprite = playSprite;//播放
            //    GameEntry.Base.ResumeGame();
            //    GameEntry.TTS.Resume();
            //    DOTween.RestartAll();
            //}
        }

        public void OnBackButtonClick()
        {
            Log.Debug("返回到初始场景");
            m_ProcedurePortraitOfMan.GotoMenu();
        }

        public void OnSettingButtonClick()
        {
            Log.Debug("设置点击");
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm, this);
        }

        public void OnNewButtonClick()
        {
            //m_ProcedureMenu.StartGame();
        }
        public void OnQuesConfirmButtonClick()
        {
            //m_QuesManager?.OnConfirmClick();
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

        public void OnBagBtnClick()
        {
            Log.Debug("背包打开");
            m_BagPanelMgr.OnOpen();
        }
        public void OnBagCloseBtnClick()
        {
            Log.Debug("背包关闭");
            m_BagPanelMgr.CloseBagPanel();
        }

        public void OnAchieveBtnClick()
        {
            Log.Debug("成就系统打开");
            m_AchievePanelMgr.Show();
        }
        public void OnAchieveCloseBtnClick()
        {
            Log.Debug("成就系统关闭");
            m_AchievePanelMgr.CloseBagPanel();
        }

        //卷轴关闭
        public void OnMessagePanelClose()
        {
            Log.Debug("消息面吧关闭");
            m_JzMgr.Close();
        }

        /// <summary>
        /// 游戏界面关闭
        /// </summary>
        public void OnGameBtnClick()
        {
            Log.Debug("游戏界面关闭");
            m_GameMgr.OnOpen();
        }
        public void OnGameBtnCloseClick()
        {
            Log.Debug("游戏界面关闭");
            m_GameMgr.OnClose(false,null);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected  override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            GameEntry.DataNode.SetData("isAutoPlay", new VarBoolean()
            {
                Value = true
            });

            if (StoryModuleMgr == null)
            {
                StoryModuleMgr = new StoryModuleMgr(PlotDialogTransform);
            }

            if (m_RoleItemMgr == null)
            {
                m_RoleItemMgr = new RoleItemMgr();
                m_RoleItemMgr.Init(m_RoleParent, m_RoleItem);

                m_RoleItemMgr.InitRelationItems();
            }

            if (m_MapLocate == null)
            {
                m_MapLocate = new MapLocate();
                m_MapLocate.Init(m_MapTransform);
            }

            //if (m_QuesManager == null)
            //{
            //    m_QuesManager = new QuesManager();
            //    m_QuesManager.Init(m_ExamPanel.transform, m_TwoGameObject, m_ThreeGameObject);
            //}

            if (m_BagPanelMgr == null)
            {
                m_BagPanelMgr = new BagPanelMgr();
                m_BagPanelMgr.Init(m_bagPanel, m_BagItem);
            }

            if (m_AchievePanelMgr == null)
            {
                m_AchievePanelMgr = new AchieveMgr();
                m_AchievePanelMgr.Init(m_achievePanel, m_AchieveItem);
            }

            if (m_JzMgr==null)
            {
                m_JzMgr=new JzMgr();
                m_JzMgr.Init(m_JzTransform);
            }

            if (m_GameMgr == null)
            {
                m_GameMgr = new GameMgr();
                m_GameMgr.Init(m_GamePanel);
            }

            if (m_TaskMgr==null)
            {
                m_TaskMgr=new TaskMgr();
                m_TaskMgr.Init(m_TaskTransform,PlotDialogTransform.parent.Find("TipBar"),tipBar);
            }
            
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedurePortraitOfMan = (ProcedurePortraitOfMan)userData;
            if (m_ProcedurePortraitOfMan == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            m_RoleItemMgr.OnOpen();

            //PlotItemMgr.OnOpen();

            m_MapLocate.OnOpen();

            //m_QuesManager.OnOpen();

            m_AchievePanelMgr.OnOpen();

            m_JzMgr.OnOpen();

            m_TaskMgr.OnOpen();

            StoryModuleMgr.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(),
                new StoryModuleBase[]
                {
                    new StoryModule(),
                    new AsideModule(),
                    new DialogModule()
                });
            //Log.Error("启动");
            StoryModuleMgr.StartProcedure<StoryModule>();

            GameEntry.Event.Subscribe(StoryOverEventArgs.EventId, OverScene);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            //PlotItemMgr?.Update();

            m_RoleItemMgr?.Update();

            m_MapLocate?.Update();

            //m_QuesManager?.Update();
            m_TaskMgr.Update();

            //if (Input.GetKeyDown(KeyCode.E))
            //{
            //    GameEntry.Event.Fire(this, QuesFreshEventArgs.Create(10000));
            //}

            //if (Input.GetKeyDown(KeyCode.F))
            //{
            //    GameEntry.Event.Fire(this, QuesFreshEventArgs.Create(10001));
            //}
            if (Input.GetKeyDown(KeyCode.H))
            {
                GameEntry.Event.Fire(this, GameShowEventArgs.Create(GameId.Puzzle));
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                GameEntry.Event.Fire(this, StoryOverEventArgs.Create(10047));
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            Log.Debug("关闭当前PortraitOfManForm" + isShutdown.ToString()+userData);
            base.OnClose(isShutdown, userData);

            //PlotItemMgr.OnClose(isShutdown, userData);
            m_RoleItemMgr.OnClose(isShutdown, userData);
            StoryModuleMgr.OnClose(isShutdown, userData);
            m_MapLocate.OnClose(isShutdown, userData);
           // m_QuesManager.OnClose(isShutdown, userData);
            m_BagPanelMgr.OnClose(isShutdown,userData);
            m_AchievePanelMgr.OnClose(isShutdown, userData);
            m_JzMgr.OnClose(isShutdown, userData);
            m_GameMgr.OnClose(isShutdown,userData);

            m_TaskMgr.OnClose(isShutdown,userData);

            GameEntry.Event.Unsubscribe(StoryOverEventArgs.EventId, OverScene);
            GameEntry.TTS.StopPlay();

        }

        

        /// <summary>
        /// 页面结束
        /// </summary>
        public void OverScene(object sender, GameEventArgs args)
        {
            int id = (VarInt32)((StoryOverEventArgs)args).UserData;
            Log.Debug("故事ID =" + id);
            m_ProcedurePortraitOfMan.GotoPositive();
            m_ProcedurePortraitOfMan = null;
        }

    }
}

