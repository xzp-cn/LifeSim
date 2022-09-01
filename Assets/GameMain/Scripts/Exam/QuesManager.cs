using System.Collections;
using System.Collections.Generic;
using GameFramework.DataNode;
using GameFramework.DataTable;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using  StarForce;
using UnityEngine.UI;
using GameEntry = StarForce.GameEntry;


public class QuesManager : IUIModule
{
    /// <summary>
    /// 暴力倾向
    /// </summary>
    private QuesVoilencePanel m_QuesVoilencePanel;

    /// <summary>
    /// 职业测试
    /// </summary>
    private List<QuesPanelBase> m_CareerPanelList;

    /// <summary>
    /// 抑郁症测试
    /// </summary>
    private List<QuesPanelBase> m_DepressPanelList;

    /// <summary>
    /// 初始化
    /// </summary>
    public int m_curStoryId;

    /// <summary>
    /// 初始化
    /// </summary>
    private IDataTable<DRSceneContent> sceneContentTable;

    /// <summary>
    /// panel池
    /// </summary>
    private QuesItemPool m_QuesItemPool;

    private int m_voilenceId;

    private Transform m_ExamPanelTransform;
    public void Init(Transform _quesPar,GameObject twoItemTemplate, GameObject threeItemTemplate)
    {
        m_CareerPanelList=new List<QuesPanelBase>();
        m_DepressPanelList=new List<QuesPanelBase>();
        //
        m_ExamPanelTransform = _quesPar;

        Transform itemPar = m_ExamPanelTransform.GetComponentInChildren<ContentSizeFitter>(true).transform;
        m_QuesItemPool= new QuesItemPool(itemPar, twoItemTemplate,threeItemTemplate);

        sceneContentTable = GameEntry.DataTable.GetDataTable<DRSceneContent>();

    } 
    void OpenExam(object sender,GameEventArgs args)
    {
        int storyId = (VarInt32)((QuesFreshEventArgs)args).UserData;
        m_curStoryId = storyId;
        Fresh();
  
    }
    
    public void Fresh()
    {
        // int[] IdArr,int[] voilenceIdArr,int[] careerIdArr

        DRSceneContent sceneContent = sceneContentTable.GetDataRow(m_curStoryId);
        //抑郁症
        string _dpressArr = sceneContent.ExamDepressArr;
        string _voilenceArr = sceneContent.ExamVoilenceArr;
        string _careeArr = sceneContent.ExamCareerArr;
        if (string.IsNullOrEmpty(_dpressArr)&&string.IsNullOrEmpty(_voilenceArr)&&string.IsNullOrEmpty(_careeArr))
        {
            Log.Debug("没有试题");
            GameEntry.Event.Fire(this, QuesOverEventArgs.Create(m_curStoryId));
            return;
        }

        m_DepressPanelList.Clear();
        m_CareerPanelList.Clear();

        int[] IdArr = null;
        ExamParse(_dpressArr,out IdArr);
        if (IdArr!=null)
        {
            DepressSpawn(IdArr);
        }

        //暴力
        m_voilenceId = 0;
        if (!string.IsNullOrEmpty(_voilenceArr))
        {
            m_voilenceId = int.Parse(_voilenceArr);
        }

        if (m_voilenceId>0)//结束判断
        {
            VoilenceSpawn(m_voilenceId);
        }
        

        //职业测试
        IdArr = null;
        ExamParse(_careeArr, out IdArr);
        if (IdArr != null)
        {
            CareerSpawn(IdArr);
        }

        m_ExamPanelTransform.gameObject.SetActive(true);
        RectTransform rt= m_ExamPanelTransform.Find("Image_exam") as RectTransform;
        rt.anchoredPosition3D=Vector3.zero;
    }

  
    void ExamParse(string _str,out int[] arr)
    {
        arr = null;
        if (!string.IsNullOrEmpty(_str))
        {
            string[] strArr = _str.Split('|');
            arr = new int[strArr.Length];
            for (int i = 0; i < strArr.Length; i++)
            {
                arr[i] = int.Parse(strArr[i]);
            }
        }
    }

    void DepressSpawn(int[] _idArr)
    {      
        int index = 0;
        foreach (int _id in _idArr)
        {
            GameObject go= m_QuesItemPool.GetTwoPoolObj();
            QuesPanelBase depressPanelBase= new QuesDepressPanel(); 
            depressPanelBase.OnInit(go.transform,_id);
            m_DepressPanelList.Add(depressPanelBase);
        }
    }

    void VoilenceSpawn(int _id)
    {
        GameObject go = m_QuesItemPool.GetThreePoolObj();
        m_QuesVoilencePanel = new QuesVoilencePanel();
        m_QuesVoilencePanel.OnInit(go.transform, _id);
    }
    

    void CareerSpawn(int[] _idArr)
    {
        foreach (int _id in _idArr)
        {
            GameObject go = m_QuesItemPool.GetTwoPoolObj();
            QuesPanelBase quesCareerPanel = new QuesCareerPanel();
            quesCareerPanel.OnInit(go.transform, _id);
            m_CareerPanelList.Add(quesCareerPanel);
        }
    }

    public void OnOpen()
    {
        //GameEntry.Event.Subscribe(QuesFreshEventArgs.EventId, OpenCareerExam);
      OpenCareerExam();
    }
    public void Update()
    {

    }

    public void OnClose(bool isShutdown, object userData)
    {
       // GameEntry.Event.Unsubscribe(QuesFreshEventArgs.EventId, OpenCareerExam);

        OnRecycle();
    }

    public void OnRecycle()
    {
        m_QuesItemPool.UnspawnAllObj();
        m_CareerPanelList.Clear();
        m_DepressPanelList.Clear();
        m_QuesVoilencePanel = null;
        m_ExamPanelTransform.gameObject.SetActive(false);

    }

    public void OnConfirmClick()
    {
        bool _allOver = CheckVoilOver()&&CheckDepreOver()&&CheckCareerOver();
        if (!_allOver)
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Exam.Tip"),
                Message = GameEntry.Localization.GetString("Exam.Message"),
                OnClickConfirm = delegate (object userData)
                {
                    Log.Debug("测试题确认");
                },
            });
            return;
        }


        if (m_QuesVoilencePanel!=null)
        {
            m_voilenceId = ((QuesVoilencePanel)m_QuesVoilencePanel).Settle();
            if (m_voilenceId < 0)//题目做完
            {
                GameEntry.DataNode.SetData("Voilence/Value", new VarInt32() { Value = m_voilenceId });
            }
        }
        else
        {
            Log.Debug("没有暴力测试题");
        }


        IDataNode dataNode = null;
        //抑郁症:计分
        if (m_DepressPanelList.Count!=0)
        {
            int dScore = 0;
            dataNode = GameEntry.DataNode.GetNode("Depress");
            if (dataNode == null)
            {
                dScore = 0;
            }
            else
            {
                dScore = GameEntry.DataNode.GetData<VarInt32>("Depress/Score");
            }
            foreach (QuesDepressPanel _panel in m_DepressPanelList)
            {
                dScore += _panel.Settle();
            }
            GameEntry.DataNode.SetData("Depress/Score", new VarInt32() { Value = dScore });
        }
        else
        {
            Log.Debug("没有抑郁症测试题");
        }
        

        //职场测试

        if (m_CareerPanelList.Count!=0)
        {
            foreach (QuesCareerPanel _panel in m_CareerPanelList)
            {
                int cScore = 0;
                string nodePath = "Career/" + _panel.typeCareer + "/Score";
                dataNode = GameEntry.DataNode.GetNode("Career");
                if (dataNode == null)
                {
                    cScore = 0;
                }
                else
                {
                    dataNode = GameEntry.DataNode.GetNode("Career/" + _panel.typeCareer);
                    if (dataNode == null)
                    {
                        cScore = 0;
                    }
                    else
                    {
                        cScore = (VarInt32)GameEntry.DataNode.GetData(nodePath).GetValue();
                    }

                }
                cScore += _panel.Settle();
                GameEntry.DataNode.SetData(nodePath, new VarInt32() { Value = cScore });
            }
        }
        else
        {
            Log.Debug("没有职业生涯测试题");
        }
        


        //释放资源
        OnRecycle();

        //下个剧情
        GameEntry.Event.Fire(this,QuesOverEventArgs.Create(m_curStoryId));
    }

    bool CheckVoilOver()
    {
        if (m_QuesVoilencePanel==null)
        {
            Log.Debug("没有暴力题目");
            return true;
        }
        m_voilenceId = ((QuesVoilencePanel)m_QuesVoilencePanel).Settle();
        return m_voilenceId != -1000;
    }

    bool CheckDepreOver()
    {
        if (m_DepressPanelList.Count == 0)
        {
            Log.Debug("没有抑郁症题目");
            return true;
        }

        bool _allOver = true;
        foreach (QuesDepressPanel _panel in m_DepressPanelList)
        {
            if (_panel.Settle() < 0)
            {
                _allOver = false;
                break;
            }
        }

        return _allOver;
    }

    bool CheckCareerOver()
    {
        if (m_CareerPanelList.Count == 0)
        {
            Log.Debug("没有职业题目");
            return true;
        }
        bool _allOver = true;
        foreach (QuesCareerPanel _panel in m_CareerPanelList)
        {
            if (_panel.Settle() < 0)
            {
                _allOver = false;
                break;
            }
        }

        return _allOver;
    }


    //
    public void OpenCareerExam()
    {
        //int storyId = (VarInt32)((QuesFreshEventArgs)args).UserData;
        //m_curStoryId = storyId;

        DRExamCareer[] drExamCareer=GameEntry.DataTable.GetDataTable<DRExamCareer>().GetAllDataRows();

        m_CareerPanelList.Clear();
        int[] IdArr = new int[drExamCareer.Length];
        for (int i = 0; i < IdArr.Length; i++)
        {
            IdArr[i] = drExamCareer[i].Id;
        }
        CareerSpawn(IdArr);
    }


    /// <summary>
    /// 职场测试确认点击
    /// </summary>
     public bool OnCareerConfirmBtnClick()
    {

        bool _allOver = CheckCareerOver();
        if (!_allOver)
        {
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 1,
                Title = GameEntry.Localization.GetString("Exam.Tip"),
                Message = GameEntry.Localization.GetString("Exam.Message"),
                OnClickConfirm = delegate (object userData)
                {
                    Log.Debug("测试题确认");
                },
            });
            return false;
        }


        //职场测试
        IDataNode dataNode = null;
        if (m_CareerPanelList.Count != 0)
        {
            foreach (QuesCareerPanel _panel in m_CareerPanelList)
            {
                int cScore = 0;
                string nodePath = "Career/" + _panel.typeCareer + "/Score";
                dataNode = GameEntry.DataNode.GetNode("Career");
                if (dataNode == null)
                {
                    cScore = 0;
                }
                else
                {
                    dataNode = GameEntry.DataNode.GetNode("Career/" + _panel.typeCareer);
                    if (dataNode == null)
                    {
                        cScore = 0;
                    }
                    else
                    {
                        cScore = (VarInt32)GameEntry.DataNode.GetData(nodePath).GetValue();
                    }

                }
                cScore += _panel.Settle();
                GameEntry.DataNode.SetData(nodePath, new VarInt32() { Value = cScore });
            }
        }
        else
        {
            Log.Debug("没有职业生涯测试题");
        }

        return true;
    }
}
