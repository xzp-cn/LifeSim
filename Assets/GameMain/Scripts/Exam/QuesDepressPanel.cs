using System.Collections;
using System.Collections.Generic;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class QuesDepressPanel : QuesPanelBase
{
    public override void OnInit(Transform _panel, int _quesId)
    {
        base.OnInit(_panel, _quesId);
        //
        InitQues();
    }

    void InitQues()
    {
        DRExamDepression drExam = GetDataRow<DRExamDepression>(m_quesId);
        List<DepItemData> optionList = new List<DepItemData>()
        {
            new DepItemData(){ stem = drExam.OptionA,score = drExam.OptionScoreA},
            new DepItemData(){ stem = drExam.OptionB,score = drExam.OptionScoreB},
        };
        m_TitleText.text = drExam.QuestionStem;

        int index = 0;
        foreach (Toggle _toggle in m_Toggles)
        {
            QuesDepressItem option = new QuesDepressItem();
            option.OnInit(_toggle, optionList[index].stem);
            option.m_Score = optionList[index].score;
            m_QuesItemList.Add(option);

            index++;
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
        base.OnClose(isShutdown, userData);
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
        int score =-1000;
        foreach (QuesItemBase quesItem in m_QuesItemList)
        {
            if (quesItem.m_IsSelected)
            {
                score = ((QuesDepressItem)quesItem).m_Score;
                break;
            }
        }
        Log.Debug("抑郁症题" +score);
        return score;
    }


}

public class DepItemData
{
    public string stem;
    public int score;
}
