using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using GameFramework.Event;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class TreasureSuShe : TreasureModuleBase
{
    public TreasureSuShe()
    {
        GameEntry.Event.Subscribe(ModelTreasureStoreFreshEventArgs.EventId,FreshData);
    }

    private Dictionary<int, List<TreasureData>> treasureDic = null;
    public override void Init(Vector3[] _posArr,int storyId)
    {
        base.Init(_posArr,storyId);

        treasureDic = GameEntry.SceneModel.treasureDic;
        //
        Utility.Random.SetSeed((int)Time.realtimeSinceStartup);
        DRTreasure[] shDrTreasures=Array.FindAll(drTreasures, (_dataTreasure) => { return _dataTreasure.Model.Equals("SuShe")&&_dataTreasure.StroyID.Equals(storyId); });
        DefaultEntityGroupHelper helpler=GameEntry.Entity.GetEntityGroup("Treasure").Helper as DefaultEntityGroupHelper;
        List<TreasureData> treasureDatas=new List<TreasureData>();
        if (treasureDic.ContainsKey(storyId))
        {
            treasureDatas= treasureDic[storyId];
        }
        else
        {
            int i = 0;
            foreach (DRTreasure _drTreasure in shDrTreasures)
            {
                TreasureData data= new TreasureData(GameEntry.Entity.GenerateSerialId(), _drTreasure.Id, Utility.Random.GetRandom(1, 2),_drTreasure.StroyID)
                {
                    Position = helpler.transform.InverseTransformPoint(posArr[i++])
                };
                treasureDatas.Add(data);
            }
            treasureDic.Add(storyId,treasureDatas);
        }
        

        //显示当前场景中的收藏品
        foreach (TreasureData _drTreasure in treasureDatas)
        {
            GameEntry.Entity.ShowTreasure(_drTreasure);
        }
    }

    /// <summary>
    /// 更新场景中的收藏品数据
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    void FreshData(object sender,GameEventArgs args)
    {
        TreasureEntityData treasureData=(TreasureEntityData)((ModelTreasureStoreFreshEventArgs)args).UserData;
        if (treasureDic.ContainsKey(treasureData.storyId))
        {
            List<TreasureData> treasureDataList=treasureDic[treasureData.storyId];
            if (treasureDataList.Count!=0)
            {
                TreasureData data= treasureDataList.Find((_data) => { return _data.TypeId == treasureData.typeId; });
                if (data!=null)
                {
                    if (treasureData.count==0)
                    {
                        Log.Debug("移除数据");
                        treasureDataList.Remove(data);

                        //更新当前剧情任务
                        bool has = false;
                        if (!treasureDic.ContainsKey(treasureData.storyId))
                        {
                            has = true;
                        }
                        else 
                        {
                            has = treasureDic[treasureData.storyId].Count != 0;
                        }
                        GameEntry.DataNode.SetData("StoryPower/" + treasureData.storyId, new VarBoolean()
                        {
                            Value = has
                        });
                    }
                    else
                    {
                        data.MaxNum = treasureData.count;
                    }
                }
            }
        }
    }

    public override void Clear()
    {
        GameEntry.Event.Unsubscribe(ModelTreasureStoreFreshEventArgs.EventId,FreshData);
    }

}


public class TreasureEntityData
{
    public int storyId;
    public int typeId;
    public int count;
}