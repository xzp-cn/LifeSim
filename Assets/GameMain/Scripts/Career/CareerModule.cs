using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.DataNode;
using UnityEngine;
using UnityGameFramework.Runtime;
using XCharts;
using GameEntry = StarForce.GameEntry;
using System.Linq;
using DG.Tweening;
using GameFramework;
using GameFramework.DataTable;
using StarForce;
using UnityEngine.UI;

public class CareerModule : IUIModule
{ 
    private Transform m_RadarTransform;
    private Transform m_CareerAnalysis;
    private Transform m_CareerAdvice;
    private RadarChart m_RadarChart;
    private IDataTable<DRCareerAnalysis> m_CareerAnalysesTable;
    private IDataTable<DRCareerRelation> m_CareerRelationTable;
    private IDataTable<DRCareerMessage> m_CareerMessageTable;

    private Vector2 m_bottomPos;
    private Vector2 m_analysisPos;

    private Text m_textTitle;
    public void Init(params Transform[] _transArr)
    {
        m_RadarTransform = _transArr[0];
        m_CareerAnalysis = _transArr[1];
        m_CareerAdvice = _transArr[2];
        if (m_CareerAnalysesTable==null)
        {
            m_CareerAnalysesTable = GameEntry.DataTable.GetDataTable<DRCareerAnalysis>();
        }

        if (m_CareerRelationTable==null)
        {
            m_CareerRelationTable = GameEntry.DataTable.GetDataTable<DRCareerRelation>();
        }
        
        if (m_CareerMessageTable == null)
        {
            m_CareerMessageTable = GameEntry.DataTable.GetDataTable<DRCareerMessage>();
        }

        RectTransform bottom = m_CareerAdvice.GetComponentInParent<ScrollRect>().transform.parent as RectTransform;
        m_bottomPos = bottom.anchoredPosition;

        RectTransform rt = m_RadarTransform.parent.parent.Find("Image_Analysis") as RectTransform;
        m_analysisPos= rt.anchoredPosition;

       m_textTitle= rt.Find("Text_title").GetComponent<Text>();
        
    }
    public void OnOpen()
    {
        m_RadarChart=m_RadarTransform.GetComponent<RadarChart>();
        OpenChart();
    }

    private Dictionary<string,double> scoreDic;
    void OpenChart()
    {
        Func<string, VarInt32> func = (_careerType) =>
        {
            VarInt32 cScore = 0;
            string nodePath = "Career/" + _careerType + "/Score";
            IDataNode dataNode = GameEntry.DataNode.GetNode(nodePath);
            if (dataNode == null)
            {
                cScore = 7;
                Log.Debug("没有分数");
            }
            else
            {
                cScore = GameEntry.DataNode.GetData<VarInt32>(nodePath);
            }
            return cScore;
        };
        List<double> scoreList = new List<double>();
        if (scoreDic==null)
        {
            scoreDic=new Dictionary<string, double>();
        }
        scoreDic.Clear();
        string[] typeArr = new string[6]
        {
            "R",
            "I",
            "A",
            "S",
            "E",
            "C"
        };
        for (int j = 0; j < typeArr.Length; j++)
        {
            double score=System.Convert.ToDouble(func(typeArr[j]));
            score /= 10;
            scoreList.Add(score);
            scoreDic.Add(typeArr[j],score);
        }

        //radar图
        Series ss = m_RadarChart.series;
        Serie serie = ss.GetSerie(0);
        serie.type = SerieType.Radar;
        serie.showDataDimension = 6;
        for (int j = 0; j < serie.dataCount; j++)
        {
            serie.RemoveData(j);
        }
        serie.AddData(
            scoreList,
            "分数"
        );
        Func<string, DRCareerAnalysis> function = (_key) =>
        {
            DRCareerAnalysis drCareerAnalysis = null;
            drCareerAnalysis = m_CareerAnalysesTable.GetDataRow((_dataRow) => { return _dataRow.CareerType == _key; });
            return drCareerAnalysis;
        };

        Radar radar = m_RadarChart.GetRadar(0);
        List<Radar.Indicator> indacatotList = radar.indicatorList;
        int i = 0;
        foreach (Radar.Indicator _indactor in indacatotList)
        {
            DRCareerAnalysis drCareerAnalysis= function(typeArr[i]);
            _indactor.name =drCareerAnalysis.CareerDes+$"({drCareerAnalysis.CareerType})";
            i++;
        }


        //跳转后
        float x=0;
        RectTransform rt=m_RadarTransform.parent.parent.Find("Arrow") as RectTransform;
        Rect rect = rt.rect;
        DOTween.To(
            () => { return x;},
            (_x) => { rect.width = _x;},
            567.2f,
            2f
        ).onComplete= () =>
        {
            rt.sizeDelta = new Vector2(rect.width,rect.height);
            AnalysisScore();
        };
    }


    /// <summary>
    /// 分析分数
    /// </summary>
    void AnalysisScore()
    {
        var pairResult = from pair in scoreDic orderby pair.Value descending select pair;
        KeyValuePair<string,double>[] pairArr= pairResult.ToArray();

        Func<string, DRCareerAnalysis> func = (_key) =>
        {
            DRCareerAnalysis drCareerAnalysis = null;
            drCareerAnalysis = m_CareerAnalysesTable.GetDataRow((_dataRow) => { return _dataRow.CareerType == _key; });
            return drCareerAnalysis;
        };


        //键值对
        int k = 1;
        foreach (KeyValuePair<string,double> _keyValuePair in pairArr)
        {

            if (k >= 4)
            {
                break;
            }

            DRCareerAnalysis drCareerAnalysis= func(_keyValuePair.Key);
            Transform careerItem= m_CareerAnalysis.Find(k.ToString());

            Text _Text = careerItem.Find("Type/Text").GetComponent<Text>();
            _Text.text = drCareerAnalysis.CareerDes;
            //
            Text commonText= careerItem.Find("Common/Content").GetComponent<Text>();
            commonText.text =drCareerAnalysis.Common;
            //
            Text socialText = careerItem.Find("Social/Content").GetComponent<Text>();
            socialText.text = drCareerAnalysis.Personality;
            //
            Text suggestionText = careerItem.Find("Suggestion/Content").GetComponent<Text>();
            suggestionText.text = drCareerAnalysis.Suggestion;
            k++;
        }

        string data0 = pairArr[0].Key;
        string data1 = pairArr[1].Key;
        string data2 = pairArr[2].Key;
        string careerStr = data0 + data1 + data2;
        Log.Debug(careerStr+" 分析分数");
        Text relationText = m_CareerAnalysis.Find("4/relation/Content").GetComponent<Text>();
        try
        {
            DRCareerRelation relation = m_CareerRelationTable.GetDataRow((_dataRow) => { return _dataRow.CareerCombine == careerStr; });
            relationText.text = relation.Description;
        }
        catch (Exception e)
        {
            relationText.text =string.Empty;
            Log.Error("没有找到对应的职业组合");
            Log.Error(e.Message);
        }
      
        m_textTitle.text = "职场分析" +$"{careerStr}";

        RectTransform rt = m_RadarTransform.parent.parent.Find("Image_Analysis") as RectTransform;
        //跳转后
        float x = rt.anchoredPosition.x;
        DOTween.To(
            () => { return x; },
            (_x) =>
            {
                rt.anchoredPosition = new Vector2(_x,0);
            },
            -80f,
            2f
        ).onComplete = () =>
        { 
            WorkplaceMessage(); 
        };

    }

    /// <summary>
    /// 职场寄语
    /// </summary>
    void WorkplaceMessage()
    {
        DRCareerMessage[] drCareerMessages= m_CareerMessageTable.GetAllDataRows();

        //随机选择一个职场寄语
        int index=Utility.Random.GetRandom(0,drCareerMessages.Length-1);
        DRCareerMessage drMessage=drCareerMessages[index];
        string title= drMessage.Title;
        string content = drMessage.Content;
        Text titleText= m_CareerAdvice.GetComponent<Text>();
        titleText.text = title;
        Text textContent=m_CareerAdvice.transform.parent.Find("Text_content").GetComponent<Text>();
        textContent.text = content;

        RectTransform bottom=m_CareerAdvice.GetComponentInParent<HorizontalLayoutGroup>().transform.parent as RectTransform;
        float y = bottom.anchoredPosition.y;
        DOTween.To(
            () => { return y; },
            (_y) =>
            {
                bottom.anchoredPosition = new Vector2(80, _y);
                y = _y;
            },
            30f,
            2f
        ).onComplete = () =>
        {
           Log.Debug("职场寄语 "+y);
           GameOver();
        };
    }

    void GameOver()
    {
        float y = 0;
        DOTween.To(
            () => { return y; },
            (_y) =>
            {
                y = _y;
            },
            30f,
            5f
        ).onComplete = () =>
        {
            Log.Debug("游戏结束 " + y);
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("GameOver.Title"),
                Message = GameEntry.Localization.GetString("GameOver.Message"),
                OnClickConfirm = delegate (object userData) { GameEntry.Event.Fire(this,GameOverEventArgs.Create(null)); },
            });
        };
    }


    public void Update()
    {

    }

    public void OnRecycle()
    {

    }

    public void OnClose(bool isShutdown, object userData)
    {
        Transform bottom = m_CareerAdvice;
        Log.Debug(bottom.name);

        RectTransform rt = m_RadarTransform.parent.parent.Find("Image_Analysis") as RectTransform;
        rt.anchoredPosition3D = m_analysisPos;
        DOTween.KillAll();
    }
}
