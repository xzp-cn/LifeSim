using System.Collections;
using System.Collections.Generic;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class QuesVoilencePanel : QuesPanelBase
{
    public override void OnInit(Transform _panel,int _quesId)
    {
        base.OnInit(_panel,_quesId);
        //
        InitQues();
    }

    void InitQues()
    {
        DRExamVoilence drExam = GetDataRow<DRExamVoilence>(m_quesId);
        List<VoiItemData> optionList=new List<VoiItemData>()
        {
            new VoiItemData(){ stem = drExam.OptionA, nextQuesId = drExam.NextQuesA},
            new VoiItemData(){ stem = drExam.OptionB, nextQuesId = drExam.NextQuesB},
            new VoiItemData(){ stem = drExam.OptionC, nextQuesId = drExam.NextQuesC},
        };

        //标题
        m_TitleText.text = drExam.QuestionStem;
        //

        //选项
        int index = 0;
        foreach (Toggle _toggle in m_Toggles)
        {
            QuesVoilenceItem option = new QuesVoilenceItem();
            option.OnInit(_toggle,optionList[index].stem);
            option.m_nextQuesId = optionList[index].nextQuesId;
            index++;
            m_QuesItemList.Add(option);
        }
        
    }
    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnClose(bool isShutdown, object userData)
    {
        base.OnClose(isShutdown,userData);
    }

    public override void OnRecycle()
    {
        base.OnRecycle();
    }

    /// <summary>
    /// 结果判断
    /// </summary>
    public int Settle()
    {
        int nextId = -1000;
        foreach (QuesItemBase quesItem in m_QuesItemList)
        {
            if (quesItem.m_IsSelected)
            {
                nextId = ((QuesVoilenceItem)quesItem).m_nextQuesId;
                break;
            }
        }
        Log.Debug("暴力倾向："+nextId);
        return nextId;
    }


}

public class VoiItemData
{
    public string stem;
    public int nextQuesId;
}
