using GameFramework;
using System.Collections;
using System.Collections.Generic;
using GameFramework.ObjectPool;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class TestPoolItem: ObjectBase
{
    public static TestPoolItem Create(object target)
    {
        TestPoolItem hpBarItemObject = ReferencePool.Acquire<TestPoolItem>();
        hpBarItemObject.Initialize(target);
        return hpBarItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        ObjectPoolItem hpBarItem = (ObjectPoolItem)Target;
        if (hpBarItem == null)
        {
            return;
        }

        Object.Destroy(hpBarItem.gameObject);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnSpawn();
        Log.Debug("回收");
    }
}
