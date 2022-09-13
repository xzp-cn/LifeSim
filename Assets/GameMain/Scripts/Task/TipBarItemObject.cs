using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

public class TipBarItemObject : ObjectBase
{
    public static TipBarItemObject Create(TipBar target)
    {
        TipBarItemObject tipBarItemObject = ReferencePool.Acquire<TipBarItemObject>();
        tipBarItemObject.Initialize(target);
        return tipBarItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        TipBar itemTarget = (TipBar)Target;
        if (itemTarget == null)
        {
            return;
        }
        Object.Destroy(itemTarget.gameObject);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        TipBar item = (TipBar)Target;
        item.gameObject.SetActive(true);
        Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnUnspawn();
        TipBar item = (TipBar)Target;
        item.gameObject.SetActive(false);
        item.ResetVal();
        Log.Debug("回收");
    }
}
