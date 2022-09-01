using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class BagTreasureFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(BagTreasureFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public BagTreasureFreshEventArgs()
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

    public static BagTreasureFreshEventArgs Create(TreasureBagData args)
    {
        BagTreasureFreshEventArgs bagTreasureFreshEventArgs = ReferencePool.Acquire<BagTreasureFreshEventArgs>();
        bagTreasureFreshEventArgs.UserData = (object)args;
        return bagTreasureFreshEventArgs;
    }


    public override void Clear()
    {

    }
}