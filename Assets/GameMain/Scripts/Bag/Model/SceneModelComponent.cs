using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;
using DG.Tweening;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using StarForce;
using UnityEngine.UIElements;

public class SceneModelComponent : GameFrameworkComponent
{
    private Transform m_SuSheTransform;
    private Transform m_ShiTangTransform;
    private Transform m_JiaoShiTransform;
    private Transform m_TuShuGuanTransform;
    private Transform m_CaochangTransform;
    private Transform m_LinYinLuTransform;
    private TreasureModuleBase SusheBase;

    public List<TreasureBagData> TreasureBagDatas;

    public Dictionary<int, List<TreasureData>> treasureDic = new Dictionary<int, List<TreasureData>>();
    private int curStoryId = 10008;
    private IDataTable<DRSceneContent> m_SceneContents;
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
        m_CaochangTransform = transform.Find("CaoChang");
        m_LinYinLuTransform = transform.Find("LinYinLu");
        //
        //Debug.LogWarning("Start");

        GameEntry.Event.Subscribe(ModelChangeEventArgs.EventId, ModelChange);
        //
        GameEntry.Event.Subscribe(ModelTreasureEventArgs.EventId, FreshData);


        GameEntry.Event.Subscribe(TaskTipEventArgs.EventId, TaskTipEntities);

        TreasureBagDatas =new List<TreasureBagData>();

        GameEntry.Setting.RemoveSetting("Treasure");

        InitStoryTaskValue();

        m_SceneContents = GameEntry.DataTable.GetDataTable<DRSceneContent>();
    }

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


    void OnDisable()
    {   
        //Debug.LogWarning("OnDestroy");
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

        if (m_SceneContents==null)
        {
            m_SceneContents = GameEntry.DataTable.GetDataTable<DRSceneContent>();
        }
        //能量提示关闭
        //
        ModelFreshData model = (ModelFreshData)((ModelChangeEventArgs)args).UserData;
        Log.Debug(model.storyId+"  当前故事情节");

        //当前场景中的收藏品隐藏
        //隐藏其他收藏品
        IEntityGroup iGroup = GameEntry.Entity.GetEntityGroup("Treasure");
        foreach (IEntity m_entity in iGroup.GetAllEntities())
        {
            GameEntry.Entity.HideEntity(m_entity.Id);
        }
        //
        switch (model.modelName)
        {
            case "DaMen01"://大门
            case "SuShe01"://宿舍
                SetSuShe(model.storyId);
                break;  
            case "ShiTang01":
            case "ShiTang02":
            case "shiTang03"://食堂
                SetShiTang(model.storyId);
                break;
            case "JiaoShi01":
            case "JiaoShi02"://教室
            case "jiaoXueLou":
                SetJiaoShi(model.storyId);
                break;
            case "CaoChang01"://操场
                SetCaoChang(model.storyId);
                break;
            case "linYinLu01"://林荫路
                SetLinYInLu(model.storyId);
                break;
            case "TuShuGuan01"://图书馆
                SetTuShuGuan(model.storyId);
                break;
            default:
                break;
        }
    }

    void SetJiaoShi(int _storyId)
    {
        curStoryId = _storyId;
        m_JiaoShiTransform.gameObject.SetActive(true);
        m_JiaoShiTransform.DOLocalMove(new Vector3(1.4f, -0.6f, 10.2f), 1).onComplete = () =>
        {
            DRSceneContent drSceneContents = Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id ==_storyId;
            });

            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }

            string[] posStrs = drSceneContents.PosArr.Split('|');

            if (SusheBase == null)
            {
                SusheBase = ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_JiaoShiTransform.Find("RenShengMoNi_JiaoShi/Position");
            count = posStrs.Length;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = postionTransform.Find(posStrs[i]).position;
                posArr[i] = pos;
            }
            SusheBase.Init(posArr, _storyId);
        };

    }

    void SetShiTang(int _storyId)
    {
        curStoryId = _storyId;
        m_ShiTangTransform.gameObject.SetActive(true);
        m_ShiTangTransform.DOLocalJump(new Vector3(-11.5f, -1.2f, 18.8f), 2, 3, 1).onComplete = () =>
        {
            DRSceneContent drSceneContents = Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id == curStoryId;
            });

            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }

            string[] posStrs = drSceneContents.PosArr.Split('|');
           
            if (SusheBase == null)
            {
                SusheBase = ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_ShiTangTransform.Find("RenShengMoNi_ShiTang/Position");
            count = posStrs.Length;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = postionTransform.Find(posStrs[i]).position;
                posArr[i] = pos;
            }
            SusheBase.Init(posArr, _storyId);
        };
        //m_ShiTangTransform.localPosition=new Vector3(-1.4f,0,-0.9f);
    } 

    void SetTuShuGuan(int _storyId)
    {
        curStoryId = _storyId;
        m_TuShuGuanTransform.gameObject.SetActive(true);
        m_TuShuGuanTransform.DOLocalMove(new Vector3(-0.11f, -2.27f, 10.2f), 1).onComplete = () =>
        {
            DRSceneContent drSceneContents = Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id == curStoryId;
            });
            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }

            string[] posStrs = drSceneContents.PosArr.Split('|');
           


            if (SusheBase == null)
            {
                SusheBase = ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_TuShuGuanTransform.Find("RenShengMoNi_TuShuGuan/Position");
            count = posStrs.Length;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = postionTransform.Find(posStrs[i]).position;
                posArr[i] = pos;
            }
            SusheBase.Init(posArr, _storyId);
        };
    }

    /// <summary>
    /// 操场
    /// </summary>
    void SetCaoChang(int _storyId)
    {
        curStoryId = _storyId;
        m_CaochangTransform.gameObject.SetActive(true);
        m_CaochangTransform.DOLocalMove(new Vector3(-0.11f, -2.27f, 10.2f), 1).onComplete = () =>
        {
            DRSceneContent drSceneContents = Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id == curStoryId;
            });

            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }


            string[] posStrs = drSceneContents.PosArr.Split('|');
            
            if (SusheBase == null)
            {
                SusheBase = ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_CaochangTransform.Find("Position");
            count = posStrs.Length;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = postionTransform.Find(posStrs[i]).position;
                posArr[i] = pos;
            }
            SusheBase.Init(posArr, _storyId);
        };
    }

    /// <summary>
    /// 林荫路
    /// </summary>
    void SetLinYInLu(int _storyId)
    {
        curStoryId = _storyId;
        m_LinYinLuTransform.gameObject.SetActive(true);
        m_LinYinLuTransform.DOLocalMove(new Vector3(-0.11f, -2.27f, 10.2f), 1).onComplete = () =>
        {
            DRSceneContent drSceneContents = Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id == curStoryId;
            });

            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }

            string[] posStrs = drSceneContents.PosArr.Split('|');
            if (SusheBase == null)
            {
                SusheBase = ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_LinYinLuTransform.Find("Position");
            count = posStrs.Length;
            posArr = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = postionTransform.Find(posStrs[i]).position;
                posArr[i] = pos;
            }
            SusheBase.Init(posArr, _storyId);
        };
    }

    void SetSuShe(int _storyId)
    {
        curStoryId = _storyId;
        m_SuSheTransform.gameObject.SetActive(true);
        m_SuSheTransform.DOLocalMove(new Vector3(-5.28f, 4.33f, -5.76f), 1f).onComplete= () =>
        {
            DRSceneContent drSceneContents= Array.Find(m_SceneContents.GetAllDataRows(), (_item) => {
                return _item.Id==curStoryId;
            });
            if (string.IsNullOrEmpty(drSceneContents.PosArr))
            {
                Log.Debug($"当前章节{drSceneContents.StoryName}没有收藏品");
                return;
            }
            string[] posStrs = drSceneContents.PosArr.Split('|');

            if (SusheBase == null)
            {
                SusheBase =ReferencePool.Acquire<TreasureSuShe>();
            }
            Transform postionTransform = null;
            int count = 0;
            Vector3[] posArr = null;
            postionTransform = m_SuSheTransform.Find("RenShengMoNi_SuShe/Position");
            count = posStrs.Length;
            posArr = new Vector3[count];    
            for (int i = 0; i < count; i++)
            {
                Vector3 pos=postionTransform.Find(posStrs[i]).position;
                posArr[i] =pos;
            }
            SusheBase.Init(posArr,_storyId);
        };
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

        if (!treasureDic.ContainsKey(curStoryId))
        {
            Log.Debug("没有收藏品　"+curStoryId);
            return;
        }

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
         InitStoryTaskValue();
    }
}

public class TreasureBagData
{
    public int num;
    public int bagId;
    public int power;
}