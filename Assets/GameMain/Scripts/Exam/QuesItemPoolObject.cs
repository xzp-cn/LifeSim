using GameFramework;
using GameFramework.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

public class QuesItemPoolObject : ObjectBase
{
    public static QuesItemPoolObject Create(object target)
    {
        QuesItemPoolObject quesItemObject = ReferencePool.Acquire<QuesItemPoolObject>();
        quesItemObject.Initialize(target);
        return quesItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        GameObject quesItem = (GameObject)Target;
        if (quesItem == null)
        {
            return;
        }

        Object.Destroy(quesItem.gameObject);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        ((GameObject)Target).SetActive(true);
        Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnSpawn();
        ((GameObject)Target).SetActive(false);
        Log.Debug("回收");
    }
}
