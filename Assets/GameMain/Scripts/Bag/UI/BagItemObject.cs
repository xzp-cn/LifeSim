using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

public class BagItemObject : ObjectBase
{
    public static BagItemObject Create(object target)
    {
        BagItemObject bagItemObject = ReferencePool.Acquire<BagItemObject>();
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
        BagItem bagItem = (BagItem)Target;
        bagItem.gameObject.SetActive(true);
        Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnUnspawn();
        BagItem bagItem = (BagItem)Target;
        bagItem.gameObject.SetActive(false);
        Log.Debug("回收");
    }
}