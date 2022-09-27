using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using StarForce;
using UnityEngine;
using UnityGameFramework.Runtime;

public class GridItemObject :ObjectBase
{
    public static GridItemObject Create(object target)
    {
        GridItemObject gridItemObject = ReferencePool.Acquire<GridItemObject>();
        gridItemObject.Initialize(target);
        return gridItemObject;
    }

    protected override void Release(bool isShutdown)
    {
        GameObject gridUiItem = (GameObject)Target;
        if (gridUiItem == null)
        {
            return;
        }
        Object.Destroy(gridUiItem);
    }

    protected override void OnSpawn()
    {
        base.OnSpawn();
        (Target as GameObject).SetActive(true);
        //Log.Debug("生成对象事件");
    }

    protected override void OnUnspawn()
    {
        base.OnSpawn();
        (Target as GameObject).SetActive(false);
        //Log.Debug("回收");
    }
}
