using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public class PlotItemObject : ObjectBase
    {
        public static PlotItemObject Create(object target)
        {
            PlotItemObject plotItemObject = ReferencePool.Acquire<PlotItemObject>();
            plotItemObject.Initialize(target);
            return plotItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            PlotItem plotItem = (PlotItem)Target;
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

}
