using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class ModelTreasureEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(ModelTreasureEventArgs).GetHashCode();
    // Start is called before the first frame update

    public ModelTreasureEventArgs()
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

    public static ModelTreasureEventArgs Create(TreasureBagData args)
    {
        ModelTreasureEventArgs modelTreasureEventArgs = ReferencePool.Acquire<ModelTreasureEventArgs>();
        modelTreasureEventArgs.UserData = (object)args;
        return modelTreasureEventArgs;
    }


    public override void Clear()
    {

    }
}
