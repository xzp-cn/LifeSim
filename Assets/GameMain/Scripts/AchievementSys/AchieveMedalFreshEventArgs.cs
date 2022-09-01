using System.Collections;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;

public class AchieveMedalFreshEventArgs : GameEventArgs
{
    /// <summary>
    /// 网络连接成功事件编号。
    /// </summary>
    public static readonly int EventId = typeof(AchieveMedalFreshEventArgs).GetHashCode();
    // Start is called before the first frame update

    public AchieveMedalFreshEventArgs()
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

    public static AchieveMedalFreshEventArgs Create(object args)
    {
        AchieveMedalFreshEventArgs medalFreshEventArgs = ReferencePool.Acquire<AchieveMedalFreshEventArgs>();
        medalFreshEventArgs.UserData = args;
        return medalFreshEventArgs;
    }


    public override void Clear()
    {

    }
}