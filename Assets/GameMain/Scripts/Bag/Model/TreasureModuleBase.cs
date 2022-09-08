using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.DataTable;
using StarForce;
using UnityEngine;

public abstract class TreasureModuleBase:IReference
{
    protected DRTreasure[] drTreasures;
    protected Vector3[] posArr;

    

    public virtual void Init(Vector3[] _posArr,int storyId)
    {
        posArr= _posArr;
        IDataTable<DRTreasure> dataTables = GameEntry.DataTable.GetDataTable<DRTreasure>();
        drTreasures = dataTables.GetAllDataRows();
    }

    public virtual void Clear()
    {
        drTreasures = null;
        posArr = null;
    }


}
