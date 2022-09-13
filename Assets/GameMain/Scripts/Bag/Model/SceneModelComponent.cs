﻿using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using DG.Tweening;
using GameFramework;
using GameFramework.DataTable;
using StarForce;
using UnityEngine.UIElements;

public class SceneModelComponent : GameFrameworkComponent
{
    private Transform m_SuSheTransform;
    private Transform m_ShiTangTransform;
    private Transform m_JiaoShiTransform;
    private Transform m_TuShuGuanTransform;

    private TreasureModuleBase SusheBase;

    public List<TreasureBagData> TreasureBagDatas;

    public Dictionary<int, List<TreasureData>> treasureDic = new Dictionary<int, List<TreasureData>>();
    private int curStoryId = 0;
    protected override void Awake()
    {
        base.Awake();
    }


    void OnEnable()
    {
        
    }
    // Start is called before the first frame update    
    void Start()
    {
        m_SuSheTransform = transform.Find("SuShe");
        m_ShiTangTransform = transform.Find("ShiTang");
        m_JiaoShiTransform = transform.Find("JiaoShi");
        m_TuShuGuanTransform = transform.Find("TuShuGuan");
        //

        //Debug.LogWarning("Start");

        GameEntry.Event.Subscribe(ModelChangeEventArgs.EventId, ModelChange);
        //
        GameEntry.Event.Subscribe(ModelTreasureEventArgs.EventId, FreshData);


        GameEntry.Event.Subscribe(TaskTipEventArgs.EventId, TaskTipEntities);

        TreasureBagDatas =new List<TreasureBagData>();

        GameEntry.Setting.RemoveSetting("Treasure");

        InitStoryTaskValue();
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    GameEntry.Entity.ShowTreasure(new TreasureData(GameEntry.Entity.GenerateSerialId(),10000,3)
        //    {
        //        Position = new Vector3(0,0,4),
        //        Rotation = Quaternion.Euler(new Vector3(30,60,90))
        //    });
        //}

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameEntry.Setting.RemoveSetting("Treasure");
            GameEntry.Setting.Save();
        }
    }


    void OnDisble()
    {   
        Debug.LogWarning("OnDestroy");
        GameEntry.Event.Unsubscribe(ModelChangeEventArgs.EventId, ModelChange);
        GameEntry.Event.Unsubscribe(ModelTreasureEventArgs.EventId,FreshData);
        GameEntry.Event.Unsubscribe(TaskTipEventArgs.EventId, TaskTipEntities);
    }

    void ModelChange(object sender, GameEventArgs args)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform temp= transform.GetChild(i);
            temp.gameObject.SetActive(false);
            DOTween.Kill(temp.gameObject);
        }
        //
        ModelFreshData model = (ModelFreshData)((ModelChangeEventArgs)args).UserData;
        //model = "JiaoShi01";
        switch (model.modelName)
        {
            case "DaMen01"://大门
            case "SuShe01"://宿舍
                SetSuShe(model.storyId);
                break;  
            case "ShiTang01":
            case "ShiTang02":
            case "shiTang03"://食堂
                SetShiTang();
                break;
            case "JiaoShi01":
            case "JiaoShi02"://教室
            case "jiaoXueLou":
                SetJiaoShi();
                break;
            case "CaoChang01"://操场
                break;
            case "linYinLu01"://林荫路
                break;
            case "TuShuGuan01"://图书馆
                SetTuShuGuan();
                break;
            default:
                break;
        }
    }

    void SetJiaoShi()
    {
        m_JiaoShiTransform.gameObject.SetActive(true);
        m_JiaoShiTransform.DOLocalMove(new Vector3(1.4f, -0.6f, 10.2f), 1);
        //m_JiaoShiTransform.localPosition= new Vector3(0, -2.8f, 5.3f);

    }

    void SetShiTang()
    {
        m_ShiTangTransform.gameObject.SetActive(true);
        m_ShiTangTransform.DOLocalJump(new Vector3(-11.5f, -1.2f, 18.8f), 2, 3, 1);
        //m_ShiTangTransform.localPosition=new Vector3(-1.4f,0,-0.9f);

    } 

    void SetTuShuGuan()
    {
        m_TuShuGuanTransform.gameObject.SetActive(true);
        m_TuShuGuanTransform.DOLocalMove(new Vector3(-0.11f, -2.27f, 10.2f), 1);
        //m_TuShuGuanTransform.localPosition=new Vector3(8.8f,-2.3f,11.5f);

    }

    void SetSuShe(int _storyId)
    {
        curStoryId = _storyId;
        m_SuSheTransform.gameObject.SetActive(true);
        m_SuSheTransform.DOLocalMove(new Vector3(-5.28f, 4.33f, -5.76f), 1f).onComplete= () =>
        {
            if (SusheBase == null)
            {
                SusheBase =ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_SuSheTransform.Find("RenShengMoNi_SuShe/Postion");
            count = postionTransform.childCount;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                posArr[i] = postionTransform.GetChild(i).position;
            }
            SusheBase.Init(posArr,_storyId);
        };
        //m_SuSheTransform.localPosition=new Vector3(0,0,4.75f);

    }

    //收藏物品点击
    void FreshData(object sender,GameEventArgs args)
    {
        TreasureBagData modelTreasureData =(TreasureBagData)((ModelTreasureEventArgs)args).UserData;

        int bagID= modelTreasureData.bagId;
        TreasureBagData data=TreasureBagDatas.Find((_bData)=> { return _bData.bagId ==bagID; });
        if (data==null)
        {
            TreasureBagDatas.Add(modelTreasureData);
        }
        else
        {
            data.num = modelTreasureData.num;
        }

        GameEntry.Setting.SetObject("Treasure",TreasureBagDatas);

        GameEntry.Event.Fire(this,BagTreasureFreshEventArgs.Create(null));//背包更新

        GameEntry.Event.Fire(this,AchieveMedalFreshEventArgs.Create(null));//成就更新
        
    }

    /// <summary>
    /// 收藏物品高亮显示
    /// </summary>
    void TaskTipEntities(object sender,GameEventArgs args)
    {
        bool isOn = (VarBoolean)((TaskTipEventArgs)args).UserData;

        List<TreasureData> treastList= treasureDic[curStoryId];
        foreach (TreasureData data in treastList)
        {
            GameObject go= GameEntry.Entity.GetEntity(data.Id).gameObject;
            if (isOn)
            {
                go.OpenHighLighterFlash();
            }
            else
            {
                go.CloseHighLighter();
            }
        }

    }

    public void HideAllModels()
    {
        m_SuSheTransform.gameObject.SetActive(false);
        m_ShiTangTransform.gameObject.SetActive(false);
        m_JiaoShiTransform.gameObject.SetActive(false);
        m_TuShuGuanTransform.gameObject.SetActive(false);
    }


    void InitStoryTaskValue()
    {
        for (int i = 10000; i < 10036; i++)
        {
            GameEntry.DataNode.SetData("StoryPower/" + i, new VarBoolean()
            {
                Value = true
            });
        }
    }


    public void ResetAll()
     {
         //TODO::重置所有状态

         //清空数据
         treasureDic.Clear();
    }
}

public class TreasureBagData
{
    public int num;
    public int bagId;
    public int power;
}