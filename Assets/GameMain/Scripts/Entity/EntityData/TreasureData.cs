using System.Collections;
using System.Collections.Generic;
using GameFramework.DataTable;
using StarForce;
using UnityEngine;

public class TreasureData : EntityData
{
    [SerializeField]
    private string m_Name;

    [SerializeField]
    private int m_BagId = 0;

    [SerializeField]
    private string m_Model;

    [SerializeField]
    private int m_ClickEffectId = 0;

    [SerializeField]
    private int m_SoundId = 0;

    private int m_MaxNum=0;
    public TreasureData(int entityId, int typeId,int maxNum)
        : base(entityId, typeId)
    {
        IDataTable<DRTreasure> drTreasures = GameEntry.DataTable.GetDataTable<DRTreasure>();
        DRTreasure drTreasure = drTreasures.GetDataRow(TypeId);
        if (drTreasure == null)
        {
            return;
        }
        //
        m_Name = drTreasure.Name;
        m_BagId = drTreasure.BagId;
        m_Model = drTreasure.Model;
        m_ClickEffectId = drTreasure.ClickEffectId;
        m_SoundId = drTreasure.SoundId;
        m_MaxNum = maxNum;
    }

    public string Name
    {
        get
        {
            return m_Name;
        }
    }

    public int BagId
    {
        get
        {
            return m_BagId;
        }
    }

    public string Model
    {
        get
        {
            return m_Model;
        }
    }

    public int ClickEffectId
    {
        get
        {
            return m_ClickEffectId;
        }
    }

    public int SoundId
    {
        get
        {
            return m_SoundId;
        }
    }

    public int MaxNum
    {
        get
        {
            return m_MaxNum;
        }
    }
}

