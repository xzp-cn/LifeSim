using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework;
using GameFramework.DataNode;
using GameFramework.Event;
using GameFramework.Fsm;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class PositiveForm : UGuiForm
    {

        [SerializeField] private Sprite playSprite;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Image pauseImage;

        [SerializeField]
        private Transform m_InforPanel;

        [SerializeField]
        private Transform m_plottDialog;

        private ProcedurePositive m_ProcedurePositive = null;
        /// <summary>   
        /// 对话模块
        /// </summary>
        private PositiveStoryMgr StoryModuleMgr = null;

        public void OnBackButtonClick()
        {
            Log.Debug("返回到初始场景");
            //m_ProcedurePositive.GotoMenu();
        }


        public void OnSettingButtonClick()
        {
            Log.Debug("设置点击");
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm, this);
        }

        public void PauseOrPlay()
        {
            if (pauseImage.sprite == playSprite)
            {
                pauseImage.sprite = pauseSprite;//暂停
                GameEntry.Base.PauseGame();
            }
            else
            {
                pauseImage.sprite = playSprite;//播放
                GameEntry.Base.ResumeGame();
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
           
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
    protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_ProcedurePositive = (ProcedurePositive)userData;
            if (m_ProcedurePositive == null)
            {
                Log.Warning("ProcedurePositive is invalid when open PositiveForm.");
                return;
            }
            
            OpenDialog();

            GameEntry.Event.Subscribe(StoryOverEventArgs.EventId, OverScene);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.P))
            {
                GameEntry.Event.Fire(StoryOverEventArgs.EventId, StoryOverEventArgs.Create(null));
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                string[] typeArr = new string[6]
                {
                    "R",
                    "I",
                    "A",
                    "S",
                    "E",
                    "C"
                };

                //
                foreach (var _careerType in typeArr)
                {
                    string nodePath = "Career/" + _careerType + "/Score";
                    GameEntry.DataNode.SetData<VarInt32>(nodePath, Utility.Random.GetRandom(1278,4567811)%10);
                }

            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(bool isShutdown, object userData)
#else
        protected internal override void OnClose(bool isShutdown, object userData)
#endif
        {
            base.OnClose(isShutdown, userData);
            m_ProcedurePositive = null;
            if (StoryModuleMgr != null)
            {
                StoryModuleMgr.OnClose(isShutdown, userData);
            }

            GameEntry.Event.Unsubscribe(StoryOverEventArgs.EventId, OverScene);

        }


        /// <summary>
        /// 打开对话
        /// </summary>
        void OpenDialog()
        {
            bool isDepress=CheckDepressScore();
            bool isVoil = CheckVoilScore();

            string posTitle = "Depress.Title";
            string posMessage = "Depress.Message";
            bool isHealthy = !(isDepress||isVoil);
            
            isHealthy = true;
            if (isHealthy)
            { 
                posTitle = "Healthy.Title";
                posMessage = "Healthy.Message";
                Log.Debug("健康");

                //
                GameEntry.UI.OpenDialog(new DialogParams()
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString(posTitle),
                    Message = GameEntry.Localization.GetString(posMessage),
                    OnClickConfirm = delegate (object userData)
                    {
                        //TODO 直接跳转到职场分析
                        GameEntry.Event.Fire(this, StoryOverEventArgs.Create(null));
                    },
                });
            }
            else
            {
                //if (isDepress && isVoil)
                //{
                //    posTitle = "VoilenceDepress.Title";
                //    posMessage = "VoilenceDepress.Message";
                //}
                //else
                //{
                //    if (isDepress)
                //    {
                //        posTitle = "Depress.Title";
                //        posMessage = "Depress.Message";
                //    }
                //    else if (isVoil)
                //    {
                //        posTitle = "Voilence.Title";
                //        posMessage = "Voilence.Message";
                //    }
                //}

                Log.Debug("正能量剧情。");
                if (StoryModuleMgr == null)
                {
                    StoryModuleMgr = new PositiveStoryMgr(m_plottDialog);
                }

                StoryModuleMgr.Initialize(GameFrameworkEntry.GetModule<IFsmManager>(),
                    new StoryModuleBase[]
                    {
                        new PosStoryModule(),
                        new PosAsideModule(),
                        new PosDialogModule(),
                    });
                StoryModuleMgr.StartProcedure<PosStoryModule>();
                m_InforPanel.DOLocalMove(new Vector3(-674, 358, 0), 0.2f);
            }
        }

        /// <summary>
        /// 计分系统
        /// </summary>
        bool CheckDepressScore()
        {
            int dpresScore = 12;//抑郁症分数
            IDataNode node= GameEntry.DataNode.GetNode("Depress/Score");
            if (node!=null)
            {
                dpresScore = GameEntry.DataNode.GetData<VarInt32>("Depress/Score");
            }
            Log.Debug("抑郁症分数");

            Text textDes=m_InforPanel.Find("exponent/depress/TextDes").GetComponent<Text>();
            Text textScore = m_InforPanel.Find("exponent/depress/TextScore").GetComponent<Text>();
            textScore.text = dpresScore.ToString();
            if (dpresScore>=0&&dpresScore<5)
            {
                textDes.text = "没有抑郁症";
            }
            else if (dpresScore >= 5 && dpresScore <12)
            {
                textDes.text="有抑郁情绪";
            }
            else if (dpresScore >= 12 && dpresScore < 21)
            {
                textDes.text = "有轻度抑郁症";
            }
            else if (dpresScore >= 21 && dpresScore < 25)
            {
                textDes.text = "有中度抑郁症";
            }
            else if (dpresScore >= 25 && dpresScore <= 30)
            {
                textDes.text = "有严重抑郁症";
            }
            Log.Debug(textDes.text);
            return dpresScore>=5;
        }

        bool CheckVoilScore()
        {
            int voilScore = -200;//抑郁症分数
            IDataNode node = GameEntry.DataNode.GetNode("Voilence/Value");
            if (node != null)
            {   
                voilScore = GameEntry.DataNode.GetData<VarInt32>("Voilence/Value");
            }
            Log.Debug("暴力倾向分数");

            Text textDes = m_InforPanel.Find("exponent/voilence/TextDes").GetComponent<Text>();
            Text textScore = m_InforPanel.Find("exponent/voilence/TextScore").GetComponent<Text>();
            textScore.text = voilScore.ToString();
            if (voilScore==-100)
            {
                textDes.text = "没有暴力倾向";
            }
            else if (voilScore == -200)
            {
                textDes.text = "偶尔会有暴力冲动";
            }
            else if (voilScore == -300)
            {
                textDes.text = "暴力指数比较高";
            }
            Log.Debug(textDes.text);
            return voilScore <=-200;
        }
        /// <summary>
        /// 页面结束
        /// </summary>
        public void OverScene(object sender, GameEventArgs args)
        {
            Log.Debug("正能量完成");
            m_ProcedurePositive.GotoCareer();
        }
    }
}