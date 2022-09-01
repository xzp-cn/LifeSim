using System.Collections;
using System.Collections.Generic;
using StarForce;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

public class QuesCareerPanel : QuesPanelBase
{
    public string rightAnswer;
    public string typeCareer;
    public override void OnInit(Transform _panel, int _quesId)
    {
        base.OnInit(_panel, _quesId);
        //
        InitQues();
    }

    void InitQues()
    {
        DRExamCareer drExam = GetDataRow<DRExamCareer>(m_quesId);
        rightAnswer = drExam.RightAnswer;
        typeCareer = drExam.TypeCareer;
        List<CarrItemData> optionList = new List<CarrItemData>()
        {
            new CarrItemData(){ stem = drExam.OptionA,optionName = "A"},
            new CarrItemData(){ stem = drExam.OptionB,optionName = "B"},
        };
        m_TitleText.text =(m_quesId%70000+1)+". "+drExam.QuestionStem;

        int index = 0;
        foreach (Toggle _toggle in m_Toggles)
        {
            QuesCareerItem option = new QuesCareerItem();
            option.OnInit(_toggle, optionList[index].stem);
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
        int index = 0;
        int score = 0;
        foreach (QuesItemBase quesItem in m_QuesItemList)
        {
            if (quesItem.m_IsSelected)
            {
                break;
            }
            index++;
        }

        if (index==m_QuesItemList.Count)
        {
            score = -1000;
        }
        else
        {
            string select = index == 0 ? "A" : "B";
            if (select.Equals(rightAnswer))
            {
                score = 1;
            }
        }
        Log.Debug("职场测试选项 ： " + score);

        return score;
    }
}

public class CarrItemData
{
    public string stem;
    public string optionName;
}

