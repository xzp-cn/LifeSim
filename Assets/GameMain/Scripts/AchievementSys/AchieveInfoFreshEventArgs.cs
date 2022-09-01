using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class AchieveInfoFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(AchieveInfoFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public AchieveInfoFreshEventArgs()
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

    public static AchieveInfoFreshEventArgs Create(object args)
    {
        AchieveInfoFreshEventArgs bagEventsArgs = ReferencePool.Acquire<AchieveInfoFreshEventArgs>();
        bagEventsArgs.UserData = args;
        return bagEventsArgs;
    }


    public override void Clear()
    {

    }
}