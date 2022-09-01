using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using GameFramework.DataNode;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using XCharts;

namespace StarForce
{
    public class CareerForm : UGuiForm
    {
        /// <summary>
        /// 图表
        /// </summary>
        [SerializeField]
        private Transform m_Chart;

        /// <summary>
        /// 职业分析
        /// </summary>
        [SerializeField]
        private Transform m_CareerAnalysis;

        /// <summary>
        /// 职业建议
        /// </summary>
        [SerializeField]
        private Transform m_CareerAdvice;

        private ProcedureCareer m_ProcedureCareer = null;

        private CareerModule m_CareerModule;


        [SerializeField]
        public GameObject m_ExamPanel;
        public GameObject m_TwoGameObject;
        public GameObject m_ThreeGameObject;
        public QuesManager m_QuesManager;

        public void OnQuitButtonClick()
        {
            Log.Debug("退出软件");
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
                Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
                OnClickConfirm = delegate (object userData) { UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit); },
            });
        }

        public void OnQuesConfirmButtonClick()
        {
           bool isFinish=m_QuesManager.OnCareerConfirmBtnClick();
           if (isFinish)
           {
               m_QuesManager.OnClose(true,null);
               m_CareerModule.OnOpen();
           }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
    protected  override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            Log.Debug("初始化");

            if (m_CareerModule==null)
            {
                m_CareerModule = new CareerModule();
            }
            m_CareerModule.Init(m_Chart,m_CareerAnalysis,m_CareerAdvice);

            if (m_QuesManager == null)
            {
                m_QuesManager = new QuesManager();
                m_QuesManager.Init(m_ExamPanel.transform, m_TwoGameObject, m_ThreeGameObject);
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
    protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedureCareer = (ProcedureCareer)userData;
            if (m_ProcedureCareer == null)
            {
                Log.Warning("ProcedurePositive is invalid when open PositiveForm.");
                return;
            }
            //m_CareerModule?.OnOpen();

            m_QuesManager.OnOpen();

            GameEntry.Event.Subscribe(GameOverEventArgs.EventId,OverScene);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            m_CareerModule?.Update();

            m_QuesManager?.Update();

            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    string[] typeArr = new string[6]
            //    {
            //        "R",
            //        "I",
            //        "A",
            //        "S",
            //        "E",
            //        "C"
            //    };

            //    //
            //    foreach (var _careerType in typeArr)
            //    {
            //        string nodePath = "Career/" + _careerType + "/Score";
            //        GameEntry.DataNode.SetData<VarInt32>(nodePath,(int)Utility.Random.GetRandomDouble()*10);
            //    }
            //}
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);

            GameEntry.Event.Unsubscribe(GameOverEventArgs.EventId, OverScene);

            m_CareerModule.OnClose(isShutdown,userData);

            m_QuesManager.OnClose(isShutdown, userData);
            m_ProcedureCareer = null;
        }

        public void OverScene(object sender, GameEventArgs args)
        {
            Log.Debug("正能量完成");
            m_ProcedureCareer.GotoHome();
        }
    }
}


