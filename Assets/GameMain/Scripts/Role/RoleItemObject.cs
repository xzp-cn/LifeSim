using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class RoleItemObject : ObjectBase
{
    public static RoleItemObject Create(object target)
    {
        RoleItemObject roleItemObject = ReferencePool.Acquire<RoleItemObject>();
        roleItemObject.Initialize(target);
        return roleItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        RoleItem plotItem = (RoleItem)Target;
        if (plotItem == null)
        {
            return;
        }

        Object.Destroy(plotItem.gameObject);
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
