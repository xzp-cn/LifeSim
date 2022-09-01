using StarForce;
using System.Collections;
using System.Collections.Generic;
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
        shakeAmount=new Vector3(0,0, 0.2f);
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
            if (_timer<interval)
            {
                _timer += realElapseSeconds;


                Vector3 pos = CachedTransform.localPosition;
                pos +=0.9f*shakeAmount;
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
        base.OnHide(isShutdown,userData);
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
        if (clickNum>m_TreasureData.MaxNum)
        {
            Log.Debug("点击次数 "+clickNum);
            GameEntry.Entity.HideEntity(this);
            GameEntry.Entity.ShowEffect(
                new EffectData(GameEntry.Entity.GenerateSerialId(), m_TreasureData.ClickEffectId)
                {
                    Position = CachedTransform.localPosition,
                });
        }
        else
        {
            GameEntry.Event.Fire(this,ModelTreasureEventArgs.Create(new TreasureBagData()
            {
                num = clickNum,
                bagId = m_TreasureData.BagId
            }));
        }
    }
}