using StarForce;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.DataNode;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityGameFramework.Runtime;
using Entity = StarForce.Entity;
using GameEntry = StarForce.GameEntry;

public class Treasure : Entity
{
    [SerializeField]
    private TreasureData m_TreasureData = null;

#if UNITY_2017_3_OR_NEWER
    protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
    {
        base.OnInit(userData);
        shakeAmount = new Vector3(0, 0, 0.2f);
    }

#if UNITY_2017_3_OR_NEWER
    protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
    {
        base.OnShow(userData);

        m_TreasureData = userData as TreasureData;
        if (m_TreasureData == null)
        {
            Log.Error("Treasure data is invalid.");
            return;
        }
    }

    Vector3 shakeAmount;
#if UNITY_2017_3_OR_NEWER
    protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
    {
        base.OnUpdate(elapseSeconds, realElapseSeconds);

        if (isClicked)
        {
            if (_timer < interval)
            {
                _timer += realElapseSeconds;


                Vector3 pos = CachedTransform.localPosition;
                pos += 0.9f * shakeAmount;
                CachedTransform.localPosition = pos;
                shakeAmount *= -1;
            }
            else
            {
                isClicked = false;
            }
        }
    }

    protected override void OnHide(bool isShutdown, object userData)
    {
        base.OnHide(isShutdown, userData);
    }

    private float _timer = 0;
    private const float interval = 0.1f;
    private bool isClicked = false;
    private int clickNum = 0;
    void OnMouseDown()
    {
        isClicked = true;
        _timer = 0;
        GameEntry.Sound.PlaySound(m_TreasureData.SoundId);

        clickNum++;

        //更新能量显示
        int energy = clickNum * m_TreasureData.PerEnergy;

        IDataNode dataNode = GameEntry.DataNode.GetNode("Energy");
        int originEnergy = 0;
        if (dataNode == null)
        {
            GameEntry.DataNode.SetData("Energy", new VarInt32() { Value = originEnergy });
        }
        else
        {
            originEnergy = GameEntry.DataNode.GetData<VarInt32>("Energy");
        }

        int m_value = (int)GameEntry.DataNode.GetData<VarInt32>("Energy") + energy;
        GameEntry.DataNode.SetData("Energy", new VarInt32() { Value = m_value });
        GameEntry.Event.Fire(this, FreshEnergyEventArgs.Create(null));

        //更新背包
        GameEntry.Event.Fire(this, ModelTreasureEventArgs.Create(new TreasureBagData()
        {
            num = clickNum,
            bagId = m_TreasureData.BagId,
            power = m_TreasureData.PerEnergy
        }));

        //场景数据刷新
        TreasureEntityData data = new TreasureEntityData()
        {
            storyId = m_TreasureData.StoryId,
            typeId = m_TreasureData.TypeId,
            count = Mathf.Clamp(m_TreasureData.MaxNum - clickNum, 0, m_TreasureData.MaxNum)//剩余点击次数
        };
        GameEntry.Event.FireNow(this, ModelTreasureStoreFreshEventArgs.Create(data));


        if (clickNum >=m_TreasureData.MaxNum)
        {
            Log.Debug("点击次数 " + clickNum);
            GameEntry.Entity.HideEntity(this);
            GameEntry.Entity.ShowEffect(
                new EffectData(GameEntry.Entity.GenerateSerialId(), m_TreasureData.ClickEffectId)
                {
                    Position = CachedTransform.localPosition,
                });
        }
    }
}