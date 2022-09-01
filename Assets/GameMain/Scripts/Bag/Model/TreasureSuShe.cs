using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using GameFramework.Entity;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = StarForce.GameEntry;

public class TreasureSuShe : TreasureModuleBase
{
    public override void Init(Vector3[] _posArr)
    {
        base.Init(_posArr);
        //
        Utility.Random.SetSeed((int)Time.realtimeSinceStartup);
        IDataTable<DRTreasure> dataTables= GameEntry.DataTable.GetDataTable<DRTreasure>();
        DRTreasure[] drTreasures= dataTables.GetAllDataRows();
        DRTreasure[]shDrTreasures=Array.FindAll(drTreasures, (_dataTreasure) => { return _dataTreasure.Model.Equals("SuShe"); });
        int i = 0;
        DefaultEntityGroupHelper helpler=GameEntry.Entity.GetEntityGroup("Treasure").Helper as DefaultEntityGroupHelper;
        foreach (DRTreasure _drTreasure in shDrTreasures)
        {
            GameEntry.Entity.ShowTreasure(new TreasureData(GameEntry.Entity.GenerateSerialId(), _drTreasure.Id, Utility.Random.GetRandom(1,2))
            {
                Position =helpler.transform.InverseTransformPoint(posArr[i]),
            });
            i++;
        }
    }
}
