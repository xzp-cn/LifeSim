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
        if (clickNum > m_TreasureData.MaxNum)
        {
            Log.Debug("点击次数 " + clickNum);
            GameEntry.Entity.HideEntity(this);
            GameEntry.Entity.ShowEffect(
                new EffectData(GameEntry.Entity.GenerateSerialId(), m_TreasureData.ClickEffectId)
                {
                    Position = CachedTransform.localPosition,
                });

            int energy = (clickNum-1) * m_TreasureData.PerEnergy;
            int m_value = (int)GameEntry.DataNode.GetData<VarInt32>("Energy")+energy ;
            GameEntry.DataNode.SetData("Energy", new VarInt32() { Value =m_value });

            
        }
        else
        {
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
                count = Mathf.Clamp(m_TreasureData.MaxNum-clickNum,0,m_TreasureData.MaxNum)
            };
            GameEntry.Event.Fire(this, ModelTreasureStoreFreshEventArgs.Create(data));
            //
            //UIForm uiForm=GameEntry.UI.GetUIForm(AssetUtility.GetUIFormAsset("Life_portraitOfMan"));
            //PortraitOfManForm form = uiForm.Logic as PortraitOfManForm;
            //Transform btn=form.transform.Find("Screen_Portrait/center/center/Right/buttons/button");
            //Vector2 screenPos= Camera.main.WorldToScreenPoint(transform.position);
            //Vector3 worldPos;


            //screenPos=RectTransformUtility.WorldToScreenPoint(null,btn.position);
            //worldPos= Camera.main.ScreenToWorldPoint(screenPos);
            //GameObject go=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //go.transform.position = worldPos;
            //go.transform.localScale=Vector3.one*100;
            //go.transform.DOMove(btn.position,2);

        }
    }
}