using GameFramework;
using GameFramework.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTreasureStoreFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(ModelTreasureStoreFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public ModelTreasureStoreFreshEventArgs()
    {

    }
    /// <summary>
    /// 获取用户自定义数据。
    /// </summary>
    public object UserData
    {
        get;
        private set;
    }

    public override int Id
    {
        get
        {
            return EventId;

        }
    }

    public static ModelTreasureStoreFreshEventArgs Create(TreasureEntityData args)
    {
        ModelTreasureStoreFreshEventArgs modelTreasureEventArgs = ReferencePool.Acquire<ModelTreasureStoreFreshEventArgs>();
        modelTreasureEventArgs.UserData = (object)args;
        return modelTreasureEventArgs;
    }


    public override void Clear()
    {

    }
}
