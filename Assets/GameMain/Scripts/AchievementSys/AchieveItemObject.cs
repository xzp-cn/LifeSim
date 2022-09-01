using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

public class AchieveItemObject : ObjectBase
{
    public static AchieveItemObject Create(object target)
    {
        AchieveItemObject bagItemObject = ReferencePool.Acquire<AchieveItemObject>();
        bagItemObject.Initialize(target);
        return bagItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        BagItem bagItem = (BagItem)Target;
        if (bagItem == null)
        {
            return;
        }
        Object.Destroy(bagItem.gameObject);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        AchieveItem achieveItem = (AchieveItem)Target;
        achieveItem.gameObject.SetActive(true);
        Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnUnspawn();
        AchieveItem achieveItem = (AchieveItem)Target;
        achieveItem.gameObject.SetActive(false);
        Log.Debug("回收");
    }
}