using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class BagInfoFreshEventArgs :GameEventArgs
{
/// <summary>
/// 
/// </summary>
public static readonly int EventId = typeof(BagInfoFreshEventArgs).GetHashCode();
// Start is called before the first frame update

public BagInfoFreshEventArgs()
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

public static BagInfoFreshEventArgs Create(object args)
{
    BagInfoFreshEventArgs bagEventsArgs = ReferencePool.Acquire<BagInfoFreshEventArgs>();
    bagEventsArgs.UserData = args;
    return bagEventsArgs;
}


public override void Clear()
{

}
}
